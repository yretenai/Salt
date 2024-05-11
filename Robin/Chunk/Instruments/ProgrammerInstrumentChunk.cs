using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record ProgrammerInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public ProgrammerInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<ProgrammerInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		ProgrammerBody = body;

		ProcessInstrumentChunk(reader);
	}

	public ProgrammerInstrumentBodyChunk ProgrammerBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => ProgrammerInstrumentBodyChunk.ListTypes;

	public override Guid Id => ProgrammerBody.Id;
}
