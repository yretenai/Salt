using Robin.FEV.Models;

namespace Robin.FEV.Chunk.Abstract;

public interface IAddressable {
	public static abstract ReadOnlySpan<ChunkId> ListTypes { get; }
}
