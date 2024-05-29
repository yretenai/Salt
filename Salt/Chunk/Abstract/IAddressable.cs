using Salt.Models;

namespace Salt.Chunk.Abstract;

public interface IAddressable {
	public static abstract ReadOnlySpan<ChunkId> ListTypes { get; }
}
