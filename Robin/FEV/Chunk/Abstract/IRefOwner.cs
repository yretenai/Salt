using Robin.FEV.Models;

namespace Robin.FEV.Chunk.Abstract;

public interface IRefOwner {
	public static abstract ReadOnlySpan<ChunkId> ListTypes { get; }
	public Guid Id { get; }
}
