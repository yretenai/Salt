using System.Diagnostics.CodeAnalysis;
using Robin.FEV.Chunk;
using Robin.FEV.Chunk.Abstract;

namespace Robin.FEV.Models;

public record struct GuidRef<T> where T : BaseChunk, IRefOwner {
	public Guid Id { get; set; }

	public bool TryGetChunk(FEVSoundBank soundBank, [MaybeNullWhen(false)] out T chunk) {
		foreach (var chunkId in T.ListTypes) {
			if (soundBank.TryGetChunk<ListChunk>(chunkId, out var list)) {
				return list.TryGetChunk(Id, out chunk);
			}
		}

		chunk = null;
		return false;
	}
}
