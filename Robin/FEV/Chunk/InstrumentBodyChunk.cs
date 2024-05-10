using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record InstrumentBodyChunk : BaseChunk {
	public InstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.INST, nameof(Atom));
		Timeline = reader.Read<GuidRef<TimelineChunk>>();
		Volume = reader.Read<float>();
		Pitch = reader.Read<float>();
		LoopCount = reader.Read<int>();
		Cutoff = reader.Read<bool>();
		Offset3DDistance = reader.Read<float>();
		TriggerChancePercent = reader.Read<float>();
		TriggerDelay = reader.Read<Range<float>>();
		Quantization = reader.Read<Quantization>();
		ControlParameter = reader.Read<Guid>();
		AutoPitchReference = reader.Read<float>();
		InitialSeekPosition = reader.Read<float>();
		MaximumPolyphony = reader.Read<int>();
		Routable = reader.Read<Routable>();

		if (soundBank.Format.FileVersion < 53) {
			return;
		}

		PolyphonyLimitBehavior = reader.Read<int>();

		if (soundBank.Format.FileVersion < 71) {
			return;
		}

		LeftTrimOffset = reader.Read<int>();

		if (soundBank.Format.FileVersion < 72) {
			return;
		}

		InitialSeekPercent = reader.Read<float>();

		if (soundBank.Format.FileVersion < 80) {
			return;
		}

		AutoPitchAtMinimum = reader.Read<float>();
	}

	public GuidRef<TimelineChunk> Timeline { get; }
	public Guid ControlParameter { get; }
	public Range<float> TriggerDelay { get; }
	public Quantization Quantization { get; }
	public Routable Routable { get; }
	public float Volume { get; }
	public float Offset3DDistance { get; }
	public float TriggerChancePercent { get; }
	public float InitialSeekPosition { get; }
	public float InitialSeekPercent { get; }
	public float Pitch { get; }
	public float AutoPitchReference { get; }
	public float AutoPitchAtMinimum { get; }
	public int LoopCount { get; }
	public int MaximumPolyphony { get; }
	public bool Cutoff { get; }
	public int PolyphonyLimitBehavior { get; }
	public int LeftTrimOffset { get; }
}
