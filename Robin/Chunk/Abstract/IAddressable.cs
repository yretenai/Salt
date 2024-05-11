using Robin.Models;

namespace Robin.Chunk.Abstract;

public interface IAddressable {
	public static abstract ReadOnlySpan<ChunkId> ListTypes { get; }
}
