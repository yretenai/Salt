using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public abstract record PlaylistInstrumentChunk(RIFFAtom Atom, FEVSoundBank Bank) : InstrumentChunk(Atom, Bank), IAddressable {
	public PlaylistInstrumentBodyChunk? PlaylistBody { get; private set; }

	public new static ReadOnlySpan<ChunkId> ListTypes => PlaylistInstrumentBodyChunk.ListTypes;

	protected void ProcessPlaylistChunk(FEVReader reader) {
		if (!TryReadChunk<PlaylistInstrumentBodyChunk>(reader, Bank, out var playlistBody)) {
			return;
		}

		PlaylistBody = playlistBody;

		ProcessInstrumentChunk(reader);
	}
}
