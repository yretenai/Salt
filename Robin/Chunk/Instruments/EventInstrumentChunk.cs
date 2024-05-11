using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record EventInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public EventInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<EventInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		EventBody = body;

		ProcessInstrumentChunk(reader);
	}

	public EventInstrumentBodyChunk EventBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => EventInstrumentBodyChunk.ListTypes;

	public override Guid Id => EventBody.Id;
}
