using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record BankInfoChunk : BaseChunk {
	public BankInfoChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.BNKI, nameof(Atom));

		if (reader.Length < 16) {
			return;
		}

		Id = reader.Read<Guid>();

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

	public Guid Id { get; }
	public ulong Hash { get; }
	public uint TopLevelEventCount { get; }
	public uint ExportFlags { get; }
}
