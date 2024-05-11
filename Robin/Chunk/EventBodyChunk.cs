using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record EventBodyChunk : ModelChunk, IAddressable {
	public EventBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.EVTB, nameof(Atom));

		if (Atom.Length < 0x40) {
			return;
		}

		SnapshotId = reader.Read<Guid>();
		Timeline = reader.Read<GuidRef<TimelineChunk>>();
		InputBus = reader.Read<Guid>();
		MasterTrack = reader.Read<Guid>();

		MaximumPolyphony = reader.Read<int>();
		Priority = reader.Read<int>();
		PolyphonyLimitBehavior = reader.Read<byte>();
		SchedulingMode = reader.Read<int>();

		ParameterLayouts = reader.ReadElementArray<Guid>().ToArray();

		if (soundBank.Format.FileVersion < 60) {
			reader.SkipElementArray(); // doesn't exist anymore??
		}

		var userFloatPropertyCount = reader.ReadSize();
		if (userFloatPropertyCount > 0) {
			for (var i = 0; i < userFloatPropertyCount; ++i) {
				UserFloatProperties[reader.ReadString()] = reader.Read<float>();
			}
		}

		var userStringPropertyCount = reader.ReadSize();
		if (userStringPropertyCount > 0) {
			for (var i = 0; i < userStringPropertyCount; ++i) {
				UserStringProperties[reader.ReadString()] = reader.ReadString();
			}
		}

		if (soundBank.Format.FileVersion < 48) {
			return;
		}

		DopplerScale = reader.Read<float>();

		if (soundBank.Format.FileVersion < 52) {
			return;
		}

		PolyphonyLimitBehavior = reader.Read<int>();

		if (soundBank.Format.FileVersion < 78) {
			return;
		}

		TriggerCooldown = reader.Read<float>();

		if (soundBank.Format.FileVersion < 97) {
			return;
		}

		Flags = reader.Read<uint>();

		if (soundBank.Format.FileVersion < 107) {
			return;
		}

		NonMasterTracks = reader.ReadElementArray<Guid>().ToArray();

		if (soundBank.Format.FileVersion < 118) {
			return;
		}

		ParameterIds = reader.ReadElementArray<ulong>().ToArray();

		if (soundBank.Format.FileVersion < 131) {
			return;
		}

		EventTriggeredInstruments = reader.ReadElementArray<GuidRef<InstrumentChunk>>().ToArray();

		if (soundBank.Format.FileVersion < 137) {
			return;
		}

		Distance = reader.Read<Range<float>>();
	}

	public Guid SnapshotId { get; }
	public GuidRef<TimelineChunk> Timeline { get; }
	public Guid InputBus { get; }
	public Guid MasterTrack { get; }
	public int MaximumPolyphony { get; }
	public int Priority { get; }
	public float DopplerScale { get; }
	public int SchedulingMode { get; }
	public int PolyphonyLimitBehavior { get; }
	public float TriggerCooldown { get; }
	public uint Flags { get; }
	public Range<float> Distance { get; }
	public Memory<Guid> ParameterLayouts { get; }
	public Dictionary<string, float> UserFloatProperties { get; } = [];
	public Dictionary<string, string> UserStringProperties { get; } = [];
	public Memory<Guid> NonMasterTracks { get; }
	public Memory<ulong> ParameterIds { get; }
	public Memory<GuidRef<InstrumentChunk>> EventTriggeredInstruments { get; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.EVTS, ChunkId.EVNT, ChunkId.EVTB];
}
