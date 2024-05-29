using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record SoundHeaderChunk : BaseChunk, IAddressable {
	public SoundHeaderChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SNDH, nameof(Atom));

		if (Atom.Length < 2) {
			return;
		}

		OffsetTable = reader.ReadElementArray<PackedKeyValue<int, int>>().ToArray();
	}

	public Memory<PackedKeyValue<int, int>> OffsetTable { get; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.SNDH];
}
