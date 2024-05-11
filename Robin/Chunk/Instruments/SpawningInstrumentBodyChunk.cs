using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record SpawningInstrumentBodyChunk : ModelChunk, IAddressable {
	public SpawningInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SPIB, nameof(Atom));
		MaximumSpawnPolyphony = reader.Read<int>();
		SpawnCount = reader.Read<int>();
		SpawnTime = reader.Read<Range<float>>();

		if (soundBank.Format.FileVersion < 104) {
			LegacySpawnTime = reader.Read<Range<float>>();
		}

		if (soundBank.Format.FileVersion < 57) {
			return;
		}

		SpawnPolyphonyLimitBehavior = reader.Read<PolyphonyLimitBehavior>();

		if (soundBank.Format.FileVersion < 94) {
			return;
		}

		SpawnRate = reader.Read<float>();
	}

	public int MaximumSpawnPolyphony { get; set; }
	public PolyphonyLimitBehavior SpawnPolyphonyLimitBehavior { get; set; }
	public int SpawnCount { get; set; }
	public float SpawnRate { get; set; }
	public Range<float> SpawnTime { get; set; }
	public Range<float> LegacySpawnTime { get; set; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.SPIT, ChunkId.SPIS, ChunkId.SPIB,
	];
}
