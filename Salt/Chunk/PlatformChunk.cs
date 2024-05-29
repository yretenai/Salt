using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record PlatformChunk : ModelChunk, IAddressable {
	public PlatformChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) { }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PLAT];
}
