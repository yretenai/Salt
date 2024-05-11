using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public record PlaylistChunk : BaseChunk, IAddressable {
	public PlaylistChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PLST, nameof(Atom));
	}

	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PLST];
}
