using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record ListCountChunk : BaseChunk {
	public ListCountChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.LCNT, nameof(Atom));

		if (Atom.Length < 4) {
			Count = 0;
			return;
		}

		Count = reader.Read<int>();
	}

	public int Count { get; }

	public override string ToString() =>  $"{nameof(ListCountChunk)} {{ {Count} }}";
}
