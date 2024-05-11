using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public record PlaylistChunk : BaseChunk, IAddressable {
	public PlaylistChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PLST, nameof(Atom));

		PlayMode = reader.Read<PlaylistPlayMode>();
		SelectionMode = reader.Read<PlaylistSelectionMode>();
		Entries = reader.ReadElementArray<PlaylistEntry>().ToArray();

		if (soundBank.Format.FileVersion is > 100 and < 104) {
			IsGlobalSequential = reader.Read<bool>();
		} else {
			IsGlobalSequential = PlayMode == PlaylistPlayMode.GlobalSequential;
		}
	}

	public bool IsGlobalSequential { get; }
	public PlaylistPlayMode PlayMode { get; }
	public PlaylistSelectionMode SelectionMode { get; }

	public ReadOnlyMemory<PlaylistEntry> Entries { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PLST];
}
