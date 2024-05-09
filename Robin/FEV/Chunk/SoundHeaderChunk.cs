using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record SoundHeaderChunk : BaseChunk {
	public SoundHeaderChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SNDH, nameof(Atom));

		if (Atom.Length < 2) {
			return;
		}

		OffsetTable = reader.ReadElementArray<PackedKeyValue<int, int>>().ToArray();
	}

	public Memory<PackedKeyValue<int, int>> OffsetTable { get; }

	public override string ToString() => $"{nameof(SoundHeaderChunk)} {{ Count = {OffsetTable.Length} }}";
}
