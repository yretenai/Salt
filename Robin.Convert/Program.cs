using Robin.FEV;
using Robin.FEV.Chunk;

namespace Robin.Convert;

internal class Program {
	private static void Main(string[] args) {
		if (!FEVSoundBank.TryLoadBank(args[0], null, out var fevSoundBank)) {
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
	}
}
