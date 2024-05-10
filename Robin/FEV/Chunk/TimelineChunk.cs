using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record TimelineChunk : ControllerOwnerChunk, IHasId, IAddressable {
	public TimelineChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<TimelineBodyChunk>(reader, soundBank, out var timelineBody)) {
			throw new InvalidDataException();
		}

		TimelineBody = timelineBody;
		TransitionZones = ReadChunk(reader, soundBank);
		ProcessControllerChunk(reader);
	}

	public TimelineBodyChunk TimelineBody { get; }
	public BaseChunk? TransitionZones { get; } // TRNS
	public static ReadOnlySpan<ChunkId> ListTypes => TimelineBodyChunk.ListTypes;
	public override Guid Id => TimelineBody.Id;
}
