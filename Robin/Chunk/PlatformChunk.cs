using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record PlatformChunk : ModelChunk, IAddressable {
	public PlatformChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) { }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.PLAT];
}
