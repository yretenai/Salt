using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record HashChunk : BaseChunk {
	public HashChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.HASH, nameof(Atom));

		if (Atom.Length < 2) {
			return;
		}

		HashTable = reader.ReadElementArray<PackedKeyValue<Guid, uint>>().ToArray().ToDictionary(x => x.Key, x => x.Value);
	}

	public Dictionary<Guid, uint> HashTable { get; } = [];

	public override string ToString() => $"{nameof(HashChunk)} {{ Count = {HashTable.Count} }}";
}
