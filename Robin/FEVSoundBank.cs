using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using DragonLib;
using Robin.Chunk;
using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin;

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

	public bool TryGetChunk<T>([MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IAddressable => TryGetChunk(T.ListTypes, out chunk);

	public bool TryGetChunk<T>(Guid id, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IHasId, IAddressable {
		if (TryGetChunks<T>(out var chunks)) {
			chunk = chunks.FirstOrDefault(x => x.Id == id);
			return chunk != null;
		}

		chunk = null;
		return false;
	}

	public bool TryGetChunk<T>(GuidRef<T> id, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IHasId, IAddressable => TryGetChunk(id.Id, out chunk);

	public bool TryGetChunks<T>([MaybeNullWhen(false)] out List<T> chunks) where T : BaseChunk, IAddressable {
		if (TryGetChunk<ListChunk>(T.ListTypes, out var listChunk)) {
			chunks = listChunk.Chunks.OfType<T>().ToList();
			return true;
		}

		if (TryGetChunk<T>(out var chunk)) {
			chunks = [chunk];
			return true;
		}

		chunks = null;
		return false;
	}

	private bool TryGetChunk<T>(ReadOnlySpan<ChunkId> listTypes, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk {
		foreach (var chunkId in listTypes) {
			if (InnerTryGetChunk(chunkId, out chunk)) {
				return true;
			}

			if (Assets != null && Assets.InnerTryGetChunk(chunkId, out chunk)) {
				return true;
			}

			if (Strings != null && Strings.InnerTryGetChunk(chunkId, out chunk)) {
				return true;
			}
		}

		if (Master != null && Master.TryGetChunk(listTypes, out chunk)) {
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
