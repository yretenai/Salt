using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record WaveformInstrumentChunk : InstrumentChunk, IHasId, IAddressable {
	public WaveformInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<WaveformInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		WaveformBody = body;

		ProcessInstrumentChunk(reader);
	}

	public WaveformInstrumentBodyChunk WaveformBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => WaveformInstrumentBodyChunk.ListTypes;

	public override Guid Id => WaveformBody.Id;
}
