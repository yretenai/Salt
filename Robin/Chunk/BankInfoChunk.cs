using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record BankInfoChunk : ModelChunk, IAddressable {
	public BankInfoChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.BNKI, nameof(Atom));

		if (soundBank.Format.FileVersion < 55) {
			return;
		}

		Hash = reader.Read<ulong>();

		if (soundBank.Format.FileVersion < 65) {
			return;
		}

		TopLevelEventCount = reader.Read<uint>();

		if (soundBank.Format.FileVersion < 77) {
			return;
		}

		ExportFlags = reader.Read<uint>();
	}

	public ulong Hash { get; }
	public uint TopLevelEventCount { get; }
	public uint ExportFlags { get; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.BNKI];
}
