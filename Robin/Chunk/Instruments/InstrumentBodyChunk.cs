using System.Runtime.CompilerServices;
using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record InstrumentBodyChunk : BaseChunk {
	public InstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.INST, nameof(Atom));
		Timeline = reader.Read<GuidRef<TimelineChunk>>();
		Volume = reader.Read<float>();
		Pitch = reader.Read<float>();
		LoopCount = reader.Read<int>();
		Cutoff = reader.Read<int>();
		Offset3DDistance = reader.Read<float>();
		TriggerChancePercent = reader.Read<float>();
		TriggerDelay = reader.Read<Range<float>>();
		Quantization = reader.Read<Quantization>();
		ControlParameter = reader.Read<Guid>();
		AutoPitchReference = reader.Read<float>();
		InitialSeekPosition = reader.Read<float>();
		MaximumPolyphony = reader.Read<int>();
		var size = reader.Read<ushort>();
		if (size != Unsafe.SizeOf<Routable>()) {
			return;
		}

		Routable = reader.Read<Routable>();

		if (soundBank.Format.FileVersion < 53) {
			return;
		}

		PolyphonyLimitBehavior = reader.Read<PolyphonyLimitBehavior>();

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

		if (soundBank.Format.FileVersion < 130) {
			return;
		}

		var subChunkSize = reader.Read<int>();
		reader.Position += subChunkSize; // todo: is variadic type! 0x11 = TimelineNamedMarker, the rest = ???
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
	public int Cutoff { get; }
	public PolyphonyLimitBehavior PolyphonyLimitBehavior { get; }
	public int LeftTrimOffset { get; }
}
