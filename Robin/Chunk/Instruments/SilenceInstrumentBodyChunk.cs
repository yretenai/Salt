using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record SilenceInstrumentBodyChunk : ModelChunk, IAddressable {
	public SilenceInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SNLB, nameof(Atom));
	}

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.SNLI, ChunkId.SNLS, ChunkId.SNLB,
	];
}
