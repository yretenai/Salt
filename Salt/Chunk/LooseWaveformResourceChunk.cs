using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record LooseWaveformResourceChunk : ModelChunk, IAddressable {
	public LooseWaveformResourceChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.LWAV, nameof(Atom));

		if (reader.Length < 18) {
			Path = string.Empty;
			return;
		}

		var resource = reader.Slice();
		Path = resource.ReadString();

		if (soundBank.Format.FileVersion > 69) {
			LoadingMethod = resource.Read<WaveformLoadingMethod>();
		}

		if (soundBank.Format.FileVersion > 98) {
			Fingerprint = resource.Read<uint>();
		}
	}

	public string Path { get; set; }
	public WaveformLoadingMethod LoadingMethod { get; set; }
	public uint Fingerprint { get; set; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.LWVS, ChunkId.LWAV];
}
