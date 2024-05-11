using Robin.Chunk;

namespace Robin.Convert;

internal class Program {
	private static void Main(string[] args) {
		if (!FEVSoundBank.TryLoadBank(args[0], null, out var masterSoundBank)) {
			return;
		}

		if (!FEVSoundBank.TryLoadBank(args[1], masterSoundBank, out var fevSoundBank)) {
			return;
		}

		if (!fevSoundBank.TryGetChunks<EventChunk>(out var events)) {
			return;
		}

		var eventChunk = events.FirstOrDefault();
		if (eventChunk == null) {
			return;
		}

		if (!eventChunk.EventBody.Timeline.TryGetChunk(fevSoundBank, out var timelineChunk)) {
			return;
		}

		if (!timelineChunk.TimelineBody.LockedTriggerBoxes.Span[0].Instrument.TryGetChunk(fevSoundBank, out var instrumentChunk)) {
			return;
		}

		if (instrumentChunk is not WaveformInstrumentChunk waveformInstrumentChunk || !waveformInstrumentChunk.WaveformBody.Resource.TryGetChunk(fevSoundBank, out var waveformResourceChunk)) {
			return;
		}

		Console.WriteLine($"{fevSoundBank.GetGuidString(eventChunk.Id)} = {waveformResourceChunk}");
	}
}
