using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record EventInstrumentBodyChunk : ModelChunk, IAddressable {
	public EventInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.EVIB, nameof(Atom));
		Event = reader.Read<GuidRef<EventChunk>>();
		SnapshotIntensity = reader.Read<float>();
		ParameterStubs = reader.ReadElementArray<EventParameterStub>().ToArray();
	}

	public GuidRef<EventChunk> Event { get; }
	public ReadOnlyMemory<EventParameterStub> ParameterStubs { get; }
	public float SnapshotIntensity { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.EVIT, ChunkId.EVTS, ChunkId.EVTB,
	];
}
