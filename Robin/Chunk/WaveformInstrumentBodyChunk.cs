using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record WaveformInstrumentBodyChunk : ModelChunk, IAddressable {
	public WaveformInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.WAIB, nameof(Atom));

		if (soundBank.Format.FileVersion < 70) {
			LegacyLoadingMode = reader.Read<WaveformLoadingMethod>();
			Resource = Id;
			return;
		}

		Resource = reader.Read<GuidRef<WaveformResourceChunk>>();
	}

	public WaveformLoadingMethod LegacyLoadingMode { get; set; }
	public GuidRef<WaveformResourceChunk> Resource { get; set; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.WAIT, ChunkId.WAIS, ChunkId.WAIB,
	];
}
