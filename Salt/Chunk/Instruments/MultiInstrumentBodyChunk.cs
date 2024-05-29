using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk.Instruments;

public sealed record MultiInstrumentBodyChunk : ModelChunk, IAddressable {
	public MultiInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.MUIB, nameof(Atom));
	}

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.MUIT, ChunkId.MUIB, ChunkId.MUIB,
	];
}
