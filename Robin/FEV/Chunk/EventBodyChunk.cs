using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record EventBodyChunk : ModelChunk {
	public EventBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.EVTB, nameof(Atom));

		if (Atom.Length < 0x40) {
			return;
		}

		SnapshotId = reader.Read<Guid>();
		Timeline = reader.Read<Guid>();
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

		var (userFloatPropertyCount, _) = reader.ReadElementSize();
		if (userFloatPropertyCount > 0) {
			for (var i = 0; i < userFloatPropertyCount; ++i) {
				UserFloatProperties[reader.ReadString()] = reader.Read<float>();
			}
		}

		var (userStringPropertyCount, _) = reader.ReadElementSize();
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

		EventTriggeredInstruments = reader.ReadElementArray<Guid>().ToArray();

		if (soundBank.Format.FileVersion < 137) {
			return;
		}

		MinimumDistance = reader.Read<float>();
		MaximumDistance = reader.Read<float>();
	}

	public Guid SnapshotId { get; }
	public Guid Timeline { get; }
	public Guid InputBus { get; }
	public Guid MasterTrack { get; }
	public int MaximumPolyphony { get; }
	public int Priority { get; }
	public float DopplerScale { get; }
	public int SchedulingMode { get; }
	public int PolyphonyLimitBehavior { get; }
	public float TriggerCooldown { get; }
	public uint Flags { get; }
	public float MinimumDistance { get; }
	public float MaximumDistance { get; }
	public Memory<Guid> ParameterLayouts { get; }
	public Dictionary<string, float> UserFloatProperties { get; } = [];
	public Dictionary<string, string> UserStringProperties { get; } = [];
	public Memory<Guid> NonMasterTracks { get; }
	public Memory<ulong> ParameterIds { get; }
	public Memory<Guid> EventTriggeredInstruments { get; }
}
