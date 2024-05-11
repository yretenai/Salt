using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public abstract record PlaylistInstrumentChunk(RIFFAtom Atom, FEVSoundBank Bank) : InstrumentChunk(Atom, Bank), IAddressable {
	public PlaylistChunk? PlaylistBody { get; private set; }

	public new static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PLIT, ChunkId.PLST, ChunkId.MUIT, ChunkId.MUIB, ChunkId.MUIS, ChunkId.SPIB, ChunkId.SPIS, ChunkId.SPIT];

	protected void ProcessPlaylistChunk(FEVReader reader) {
		if (!TryReadChunk<PlaylistChunk>(reader, Bank, out var playlistBody)) {
			return;
		}

		PlaylistBody = playlistBody;

		ProcessInstrumentChunk(reader);
	}
}
