using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record CommandInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public CommandInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<CommandInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		CommandBody = body;

		ProcessInstrumentChunk(reader);
	}

	public CommandInstrumentBodyChunk CommandBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => CommandInstrumentBodyChunk.ListTypes;

	public override Guid Id => CommandBody.Id;
}
