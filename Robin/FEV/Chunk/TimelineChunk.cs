using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record TimelineChunk : BaseChunk, IHasId, IAddressable, IControllerOwner {
	public TimelineChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (!TryReadChunk<TimelineBodyChunk>(reader, soundBank, out var timelineBody)) {
			throw new InvalidDataException();
		}

		TimelineBody = timelineBody;
		TransitionZones = ReadChunk(reader, soundBank);
		Controllers = ReadChunk(reader, soundBank);
	}

	public TimelineBodyChunk TimelineBody { get; }
	public BaseChunk? TransitionZones { get; } // TRNS
	public static ReadOnlySpan<ChunkId> ListTypes => TimelineBodyChunk.ListTypes;
	public BaseChunk? Controllers { get; } // CTRO
	public Guid Id => TimelineBody.Id;

	public override string ToString() => $"{nameof(TimelineChunk)} {{ Body = {TimelineBody}, TransitionZones = {TransitionZones}, Controllers = {Controllers} }}";
}
