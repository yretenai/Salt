using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using DragonLib;
using Salt.Chunk;
using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt;

public sealed class FEVSoundBank {
	public FEVSoundBank(Stream stream, FEVSoundBank? masterBank = null) {
		var header = new RIFFAtom();
		stream.ReadExactly(new Span<RIFFAtom>(ref header).AsBytes());
		ArgumentOutOfRangeException.ThrowIfNotEqual((uint) header.Id, (uint) ChunkId.RIFF, nameof(header));
		var fmt = (ChunkId) 0;
		stream.ReadExactly(new Span<ChunkId>(ref fmt).AsBytes());
		ArgumentOutOfRangeException.ThrowIfNotEqual((uint) fmt, (uint) ChunkId.FEV, nameof(fmt));

		var shift = (int) stream.Position;
		using var buffer = MemoryPool<byte>.Shared.Rent(header.Length - 4);
		var block = buffer.Memory[..(header.Length - 4)];
		stream.ReadExactly(block.Span);

		var reader = new FEVReader(block);

		if (!BaseChunk.TryReadChunk<FormatChunk>(reader, this, out var format)) {
			throw new InvalidDataException("missing FMT chunk");
		}

		Format = format;

		if (!BaseChunk.TryReadChunk<ProjectChunk>(reader, this, out var projectChunk)) {
			throw new InvalidDataException("missing PROJ chunk");
		}

		Chunks = projectChunk.Chunks;

		if (TryGetChunk<SoundHeaderChunk>(out var soundHeaderChunk)) {
			var atom = reader.Peek<RIFFAtom>();
			if (atom is { Id: ChunkId.SND, Length: > 0 }) {
				EmbeddedSoundBanks = new SoundChunk(block, soundHeaderChunk, shift, atom, this);
			}
		}

		Master = masterBank;
		if (TryGetChunk<PlatformChunk>(out var platformChunk)) {
			Id = platformChunk.Id;
		}
	}

	public FormatChunk Format { get; }
	public List<BaseChunk> Chunks { get; }
	public SoundChunk? EmbeddedSoundBanks { get; }
	public FEVSoundBank? Master { get; }
	public FEVSoundBank? Strings { get; set; }
	public FEVSoundBank? Assets { get; set; }
	public Guid Id { get; }

	public static bool TryLoadBank(string path, FEVSoundBank? owner, [MaybeNullWhen(false)] out FEVSoundBank soundBank) {
		soundBank = null;
		if (!File.Exists(path)) {
			return false;
		}

		using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
			soundBank = new FEVSoundBank(stream, owner);
		}

		var stringsPath = Path.ChangeExtension(path, ".strings.bank");
		if (File.Exists(stringsPath)) {
			using var stream = new FileStream(stringsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			soundBank.Strings = new FEVSoundBank(stream);
		}

		var assetsPath = Path.ChangeExtension(path, ".assets.bank");
		if (File.Exists(assetsPath)) {
			using var stream = new FileStream(assetsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			soundBank.Assets = new FEVSoundBank(stream);
		}

		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private bool InnerTryGetChunk<T>(ChunkId chunkId, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk {
		foreach (var potentialChunk in Chunks) {
			if (potentialChunk.IsFunctionallyEmpty) {
				continue;
			}

			if (potentialChunk.ChunkId == chunkId) {
				switch (potentialChunk) {
					case T targetChunk:
						chunk = targetChunk;
						return true;
					case ListChunk { Body: T bodyTargetChunk }:
						chunk = bodyTargetChunk;
						return true;
				}
			}
		}

		chunk = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public bool TryGetChunk<T>([MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IAddressable {
		foreach (var chunkId in T.ListTypes) {
			if (TryGetChunk<T>(chunkId, out var potentialChunk)) {
				chunk = potentialChunk;
				return true;
			}
		}

		chunk = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public bool TryGetChunk<T>(Guid id, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IHasId, IAddressable {
		if (TryGetChunks<T>(out var chunks)) {
			chunk = chunks.FirstOrDefault(x => x.Id == id);
			return chunk != null;
		}

		chunk = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public bool TryGetChunk<T>(GuidRef<T> id, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IHasId, IAddressable => TryGetChunk(id.Id, out chunk);

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public bool TryGetChunks<T>(out List<T> chunks) where T : BaseChunk, IAddressable {
		chunks = [];
		foreach (var chunkId in T.ListTypes) {
			if (TryGetChunk<ListChunk>(chunkId, out var listChunk)) {
				chunks.AddRange(listChunk.Chunks.OfType<T>().ToList());
			}

			if (TryGetChunk<T>(out var chunk)) {
				chunks.Add(chunk);
			}
		}

		if (chunks.Count > 0) {
			return true;
		}

		chunks = [];
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private bool TryGetChunk<T>(ChunkId chunkId, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk {
		if (InnerTryGetChunk(chunkId, out chunk)) {
			return true;
		}

		if (Assets != null && Assets.InnerTryGetChunk(chunkId, out chunk)) {
			return true;
		}

		if (Strings != null && Strings.InnerTryGetChunk(chunkId, out chunk)) {
			return true;
		}

		if (Master != null && Master.TryGetChunk(chunkId, out chunk)) {
			return true;
		}

		chunk = null;
		return false;
	}

	public bool LookupGuid(string path, out Guid guid) {
		if (TryGetChunk<StringDataChunk>(out var stdt) && stdt.ToReverseDictionary().TryGetValue(path, out guid)) {
			return true;
		}

		guid = default;
		return false;
	}

	public string GetGuidString(Guid guid) {
		if (!TryGetChunk<StringDataChunk>(out var stdt) || !stdt.ToDictionary().TryGetValue(guid, out var path)) {
			return guid.ToString("B");
		}

		return path;
	}

	public string GetPathString(Guid guid) {
		if (!TryGetChunk<StringDataChunk>(out var stdt) || !stdt.ToDictionary().TryGetValue(guid, out var path)) {
			return guid.ToString("D");
		}

		var colonIndex = path.IndexOf(':', StringComparison.Ordinal);
		if (colonIndex > -1) {
			path = path[(colonIndex + 2)..];
		}

		return path.SanitizeTraversal();
	}

	public string DumpGUIDs() {
		if (!TryGetChunk<StringDataChunk>(out var stdt) || stdt.IsFunctionallyEmpty) {
			return string.Empty;
		}

		var sb = new StringBuilder();
		foreach (var (guid, name) in stdt.ToDictionary()) {
			sb.Append(guid.ToString("B"));
			sb.Append(" = ");
			sb.AppendLine(name);
		}

		return sb.ToString();
	}
}
