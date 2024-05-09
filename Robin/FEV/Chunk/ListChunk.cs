using System.Diagnostics.CodeAnalysis;
using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record ListChunk : BaseChunk {
	public ListChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.LIST, nameof(Atom));

		if (Atom.Length < 4) {
			ListId = ChunkId.LIST;
			return;
		}

		ListId = reader.Read<ChunkId>();

		if (Atom.Length == 4) {
			return;
		}

		if (reader.Peek<ChunkId>() == ChunkId.LCNT) {
			if (ReadChunk(reader, soundBank) is not ListCountChunk listCount) {
				return;
			}

			for (var i = 0; i < listCount.Count; ++i) {
				Chunks.Add(ReadChunk(reader, soundBank));
			}
		} else {
			Body = ReadChunk(reader.Slice(atom.Length - 4), new RIFFAtom {
				Id = ListId,
				Length = Atom.Length - 4,
			}, soundBank);

			Chunks.Add(Body);
		}
	}

	public ChunkId ListId { get; }
	public List<BaseChunk> Chunks { get; } = [];
	public BaseChunk? Body { get; }
	public override ChunkId ChunkId => ListId;

	public override bool IsFunctionallyEmpty => base.IsFunctionallyEmpty ||
	                                            ((Chunks.Count == 0 || Chunks.All(x => x.IsFunctionallyEmpty)) &&
	                                             (Body == null || Body.IsFunctionallyEmpty));

	public override string ToString() => $"{nameof(ListChunk)} {{ Type = {ListId:G}, Count = {Chunks.Count}, First = {Body ?? Chunks.FirstOrDefault()} }}";

	public bool TryGetChunk<T>(Guid id, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk, IRefOwner {
		chunk = Chunks.OfType<T>().FirstOrDefault(x => x.Id == id);
		return chunk != null;
	}
}
