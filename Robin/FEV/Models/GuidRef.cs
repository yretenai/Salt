using System.Diagnostics.CodeAnalysis;
using Robin.FEV.Chunk;
using Robin.FEV.Chunk.Abstract;

namespace Robin.FEV.Models;

public record struct GuidRef<T> where T : BaseChunk, IRefOwner {
	public Guid Id { get; set; }

	public bool TryGetChunk(FEVSoundBank soundBank, [MaybeNullWhen(false)] out T chunk) {
		foreach (var chunkId in T.ListTypes) {
			if ((soundBank.TryGetChunk<ListChunk>(chunkId, out var list) && list.TryGetChunk(Id, out chunk)) ||
			    (soundBank.Assets?.TryGetChunk<ListChunk>(chunkId, out list) == true && list.TryGetChunk(Id, out chunk)) ||
			    (soundBank.Strings?.TryGetChunk<ListChunk>(chunkId, out list) == true && list.TryGetChunk(Id, out chunk)) ||
			    (soundBank.Owner?.TryGetChunk<ListChunk>(chunkId, out list) == true && list.TryGetChunk(Id, out chunk))) {
				return true;
			}
		}

		chunk = null;
		return false;
	}
}
