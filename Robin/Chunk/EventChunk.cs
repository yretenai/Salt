using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record EventChunk : PropertyOwnerChunk, IHasId, IAddressable {
	public EventChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (soundBank.Format.FileVersion < 49) {
			EventBody = new EventBodyChunk(reader, atom with {
				Id = ChunkId.EVTB,
			}, soundBank);
		} else {
			if (!TryReadChunk<EventBodyChunk>(reader, soundBank, out var eventBody)) {
				throw new InvalidDataException();
			}

			EventBody = eventBody;

			ProcessPropertyChunk(reader);
		}
	}

	public EventBodyChunk EventBody { get; }
	public new static ReadOnlySpan<ChunkId> ListTypes => EventBodyChunk.ListTypes;
	public override Guid Id => EventBody.Id;
}
