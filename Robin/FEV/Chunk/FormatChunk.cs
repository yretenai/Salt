using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record FormatChunk : BaseChunk {
	public FormatChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.FMT, nameof(Atom));

		if (Atom.Length < 8) {
			return;
		}

		FileVersion = reader.Read<int>();
		CompatibleVersion = reader.Read<int>();
	}

	public int FileVersion { get; }
	public int CompatibleVersion { get; }
}
