using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record PropertyChunk : BaseChunk {
	public PropertyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PRPS, nameof(Atom));
		Index = reader.Read<int>();
		Method = reader.Read<ushort>();
		Type = reader.Read<ushort>();
		Controllers = reader.ReadElementArray<GuidRef<ControllerOwnerChunk>>().ToArray();
		Modulators = reader.ReadElementArray<Guid>().ToArray();
	}

	public int Index { get; }
	public ushort Method { get; }
	public ushort Type { get; }
	public ReadOnlyMemory<GuidRef<ControllerOwnerChunk>> Controllers { get; }
	public ReadOnlyMemory<Guid> Modulators { get; }
}
