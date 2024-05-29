using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record PropertyChunk : BaseChunk {
	public PropertyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PROP, nameof(Atom));
		Index = reader.Read<int>();
		Method = reader.Read<ushort>();
		Type = reader.Read<ushort>();
		Mapping = reader.Read<Guid>();
		Controllers = reader.ReadElementArray<GuidRef<ControllerOwnerChunk>>().ToArray();
		Modulators = reader.ReadElementArray<Guid>().ToArray();
	}

	public int Index { get; }
	public ushort Method { get; }
	public ushort Type { get; }
	public Guid Mapping { get; }
	public ReadOnlyMemory<GuidRef<ControllerOwnerChunk>> Controllers { get; }
	public ReadOnlyMemory<Guid> Modulators { get; }
}
