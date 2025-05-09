using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record ProjectChunk : BaseChunk, IAddressable {
	public ProjectChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PROJ, nameof(Atom));

		while (reader.Position < reader.Length) {
			if (reader.Length - reader.Position < 8) {
				break;
			}

			var peek = reader.Peek<RIFFAtom>();
			var next = peek.Length + 8 + reader.Position;
			var chunk = ReadChunk(reader, soundBank);
			Chunks.Add(chunk);
			reader.Position = next;
		}
	}

	public List<BaseChunk> Chunks { get; } = [];
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PROJ];
}
