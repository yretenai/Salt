using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record FormatChunk : BaseChunk, IAddressable {
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
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.FMT];
}
