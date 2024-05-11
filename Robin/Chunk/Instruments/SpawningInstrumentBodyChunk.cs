using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record SpawningInstrumentBodyChunk : ModelChunk, IAddressable {
	public SpawningInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SPIB, nameof(Atom));
	}

	public int MaximumSpawnPolyphony { get; set; }
	public int SpawnPolyphonyLimitBehavior { get; set; }
	public int SpawnCount { get; set; }
	public float SpawnRate { get; set; }
	public Range<float> SpawnTime { get; set; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.SPIT, ChunkId.SPIS, ChunkId.SPIB,
	];
}
