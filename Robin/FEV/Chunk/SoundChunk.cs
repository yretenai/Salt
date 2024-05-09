using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record SoundChunk : BaseChunk {
	public SoundChunk(ReadOnlyMemory<byte> buffer, SoundHeaderChunk headerChunk, int shift, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.SND, nameof(Atom));

		foreach (var (offset, length) in headerChunk.OffsetTable.Span) {
			SoundBanks.Add(buffer.Slice(offset - shift, length).ToArray());
		}
	}

	public List<Memory<byte>> SoundBanks { get; set; } = [];
}
