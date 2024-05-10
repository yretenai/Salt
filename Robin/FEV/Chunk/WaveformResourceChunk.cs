using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record WaveformResourceChunk : ModelChunk, IAddressable {
	public WaveformResourceChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.WAV, nameof(Atom));

		if (reader.Length < 24) {
			return;
		}

		var resource = reader.Slice();
		SoundBankIndex = resource.Read<int>();
		SampleIndex = resource.Read<int>();
		if (soundBank.Format.FileVersion > 69) {
			LoadingMethod = resource.Read<WaveformLoadingMethod>();
		}
	}

	public int SoundBankIndex { get; set; }
	public int SampleIndex { get; set; }
	public WaveformLoadingMethod LoadingMethod { get; set; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.WAV, ChunkId.WAVS];
}
