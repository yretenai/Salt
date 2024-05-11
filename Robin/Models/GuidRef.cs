using System.Diagnostics.CodeAnalysis;
using Robin.Chunk.Abstract;

namespace Robin.Models;

public record struct GuidRef<T> where T : BaseChunk, IHasId, IAddressable {
	public Guid Id { get; set; }

	public bool TryGetChunk(FEVSoundBank soundBank, [MaybeNullWhen(false)] out T chunk) => soundBank.TryGetChunk(Id, out chunk);

	public static implicit operator Guid(GuidRef<T> value) => value.Id;

	public static implicit operator GuidRef<T>(Guid value) => new() {
		Id = value,
	};
}
