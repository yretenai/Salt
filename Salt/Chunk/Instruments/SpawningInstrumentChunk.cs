using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk.Instruments;

public sealed record SpawningInstrumentChunk : PlaylistInstrumentChunk, IHasId, IAddressable {
	public SpawningInstrumentChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<SpawningInstrumentBodyChunk>(reader, soundBank, out var body)) {
			throw new InvalidDataException();
		}

		SpawningBody = body;

		ProcessPlaylistChunk(reader);
	}

	public SpawningInstrumentBodyChunk SpawningBody { get; }

	public new static ReadOnlySpan<ChunkId> ListTypes => SpawningInstrumentBodyChunk.ListTypes;

	public override Guid Id => SpawningBody.Id;
}
