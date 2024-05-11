using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record EffectInstrumentBodyChunk : ModelChunk, IAddressable {
	public EffectInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.EFIB, nameof(Atom));
		Target = reader.Read<Guid>();
	}

	public Guid Target { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.EFIT, ChunkId.EFIS, ChunkId.EFIB,
	];
}
