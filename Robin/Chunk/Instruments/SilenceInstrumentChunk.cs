using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record SilenceInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public SilenceInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<SilenceInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		SilenceBody = body;

		ProcessInstrumentChunk(reader);
	}

	public SilenceInstrumentBodyChunk SilenceBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => SilenceInstrumentBodyChunk.ListTypes;

	public override Guid Id => SilenceBody.Id;
}
