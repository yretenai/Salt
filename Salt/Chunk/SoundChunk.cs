using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record SoundChunk : BaseChunk, IAddressable {
	public SoundChunk(ReadOnlyMemory<byte> buffer, SoundHeaderChunk headerChunk, int shift, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SND, nameof(Atom));

		foreach (var (offset, length) in headerChunk.OffsetTable.Span) {
			if (FsbLoader.TryLoadFsbFromByteArray(buffer.Slice(offset - shift, length).ToArray(), out var fsb)) {
				SoundBanks.Add(fsb!);
			}
		}
	}

	public List<FmodSoundBank> SoundBanks { get; set; } = [];
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.SND];
}
