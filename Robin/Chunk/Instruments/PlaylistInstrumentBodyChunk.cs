using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record PlaylistInstrumentBodyChunk : BaseChunk, IAddressable {
	public PlaylistInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PLIT, nameof(Atom));

		if (!TryReadChunk<PlaylistChunk>(reader, Bank, out var playlist)) {
			return;
		}

		Playlist = playlist;
	}

	public PlaylistChunk? Playlist { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.PLIT, ChunkId.PLST,
	];
}
