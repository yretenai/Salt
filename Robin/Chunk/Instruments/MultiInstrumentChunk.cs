using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record MultiInstrumentChunk : PlaylistInstrumentChunk, IHasId, IAddressable {
	public MultiInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<MultiInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		MultiBody = body;

		ProcessPlaylistChunk(reader);
	}

	public MultiInstrumentBodyChunk MultiBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => MultiInstrumentBodyChunk.ListTypes;

	public override Guid Id => MultiBody.Id;
}
