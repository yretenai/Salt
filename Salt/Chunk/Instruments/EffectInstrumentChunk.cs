using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk.Instruments;

public sealed record EffectInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public EffectInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<EffectInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		EffectBody = body;

		ProcessInstrumentChunk(reader);
	}

	public EffectInstrumentBodyChunk EffectBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => EffectInstrumentBodyChunk.ListTypes;

	public override Guid Id => EffectBody.Id;
}
