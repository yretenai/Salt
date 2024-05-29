using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record ListCountChunk : BaseChunk, IAddressable {
	public ListCountChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.LCNT, nameof(Atom));

		if (Atom.Length < 4) {
			Count = 0;
			return;
		}

		Count = reader.Read<int>();
	}

	public int Count { get; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.LCNT];
}
