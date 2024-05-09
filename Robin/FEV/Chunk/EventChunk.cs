using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record EventChunk : BaseChunk, IRefOwner {
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
			Properties = ReadChunk(reader, soundBank);
		}
	}

	public EventBodyChunk EventBody { get; }
	public BaseChunk? Properties { get; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.EVTS, ChunkId.EVNT, ChunkId.EVTB];
	public Guid Id => EventBody.Id;

	public override string ToString() => $"{nameof(EventChunk)} {{ Body = {EventBody}, Properties = {Properties} }}";
}
