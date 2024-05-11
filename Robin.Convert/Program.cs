using Robin.Chunk;
using Robin.Chunk.Instruments;

namespace Robin.Convert;

internal class Program {
	private static void Main(string[] args) {
		if (args.Length < 2) {
			Console.Error.WriteLine("Usage: Robin.Convert path/to/Master.bank path/to/soundbank.bank [path/to/output]");
			return;
		}

		if (!FEVSoundBank.TryLoadBank(args[0], null, out var masterSoundBank)) {
			return;
		}

		var files = new List<string>();
		if (Directory.Exists(args[1])) {
			files.AddRange(Directory.GetFiles(args[1], "*.bank", SearchOption.AllDirectories));
		} else {
			files.Add(args[1]);
		}

		foreach (var file in files) {
			if (file.EndsWith(".assets.bank") || file.EndsWith(".strings.bank")) {
				continue;
			}

			Console.WriteLine(Path.GetFileNameWithoutExtension(file));

			if (!FEVSoundBank.TryLoadBank(file, masterSoundBank, out var fevSoundBank)) {
				continue;
			}

			if (!fevSoundBank.TryGetChunks<EventChunk>(out var events)) {
				continue;
			}

			EnumerateEvents(events, args.ElementAtOrDefault(2) ?? string.Empty, args.Length < 3);
		}
	}

	private static void EnumerateEvents(List<EventChunk> chunks, string output, bool dry) {
		foreach (var eventChunk in chunks) {
			if (!eventChunk.EventBody.Timeline.TryGetChunk(eventChunk.Bank, out var timelineChunk)) {
				continue;
			}

			Console.WriteLine(eventChunk.Bank.GetGuidString(eventChunk.Id));

			var triggerBoxes = new HashSet<(uint start, uint length, InstrumentChunk instrumentChunk)>();
			foreach (var triggerBox in timelineChunk.TimelineBody.TriggerBoxes.Span) {
				if (!triggerBox.Instrument.TryGetChunk(timelineChunk.Bank, out var instrumentChunk)) {
					continue;
				}

				triggerBoxes.Add((triggerBox.Start, triggerBox.Length, instrumentChunk));
			}

			foreach (var triggerBox in timelineChunk.TimelineBody.LockedTriggerBoxes.Span) {
				if (!triggerBox.Instrument.TryGetChunk(timelineChunk.Bank, out var instrumentChunk)) {
					continue;
				}

				triggerBoxes.Add((triggerBox.Start, triggerBox.Length, instrumentChunk));
			}

			foreach (var namedMarker in timelineChunk.TimelineBody.NamedMarkers) {
				Console.WriteLine($"\tMarker {timelineChunk.Bank.GetGuidString(namedMarker.Id)} {namedMarker.Start}-{namedMarker.Length} {namedMarker.Name}");
			}

			var eventOutput = Path.Combine(output, eventChunk.Bank.GetPathString(eventChunk.Id));
			foreach (var (start, length, triggerInstrument) in triggerBoxes.OrderBy(x => x.start)) {
				Console.WriteLine($"\tTrigger {start}-{length}");
				ProcessInstrument("\t\t", triggerInstrument, eventOutput, dry);
			}
		}
	}

	private static void ProcessInstrument(string indent, InstrumentChunk instrument, string output, bool dry) {
		var body = instrument.Instrument;
		if (body == null) {
			return;
		}

		var instrumentOutput = Path.Combine(output, instrument.Bank.GetPathString(instrument.Id));

		Console.Write(indent + $" Instrument {instrument.Bank.GetGuidString(instrument.Id)} Volume: {body.Volume} TriggerDelay: {body.TriggerDelay} Offset3DDistance: {body.Offset3DDistance} LoopCount: {body.LoopCount}");

		switch (instrument) {
			case CommandInstrumentChunk commandChunk:
				Console.WriteLine($" Command {commandChunk.CommandBody.Command:G} {instrument.Bank.GetGuidString(commandChunk.CommandBody.Target)}");
				break;
			case EffectInstrumentChunk effectChunk:
				Console.WriteLine($" Effect {instrument.Bank.GetGuidString(effectChunk.EffectBody.Target)}");
				break;
			case EventInstrumentChunk eventChunk:
				Console.WriteLine($" Event {instrument.Bank.GetGuidString(eventChunk.EventBody.Event)}");
				break;
			case MultiInstrumentChunk multiChunk: {
				Console.WriteLine(" Multi");
				var playlistOutput = Path.Combine(instrumentOutput, multiChunk.Bank.GetPathString(multiChunk.Id));
				PrintPlaylist(indent + "\t", multiChunk.PlaylistBody, playlistOutput, dry);
				break;
			}
			case SpawningInstrumentChunk spawningChunk: {
				Console.WriteLine(" Spawning");
				var playlistOutput = Path.Combine(instrumentOutput, spawningChunk.Bank.GetPathString(spawningChunk.Id));
				PrintPlaylist(indent + "\t", spawningChunk.PlaylistBody, playlistOutput, dry);
				break;
			}
			case ProgrammerInstrumentChunk programmerChunk:
				Console.WriteLine($" Programmer {programmerChunk.ProgrammerBody.InstrumentName}");
				break;
			case SilenceInstrumentChunk:
				Console.WriteLine(" Silence");
				break;
			case WaveformInstrumentChunk waveformInstrumentChunk:
				if (waveformInstrumentChunk.WaveformBody.Resource.TryGetChunk(waveformInstrumentChunk.Bank, out var waveformResourceChunk)) {
					Console.Write($" Waveform Bank: {waveformResourceChunk.SoundBankIndex} Sample: {waveformResourceChunk.SampleIndex}");
					var bank = waveformResourceChunk.Bank.EmbeddedSoundBanks?.SoundBanks.ElementAtOrDefault(waveformResourceChunk.SoundBankIndex);
					if (bank == null) {
						Console.WriteLine(" Missing Bank");
						break;
					}

					Console.WriteLine(" Embedded Bank");
					if (dry) {
						break;
					}

					var sample = bank.Samples.ElementAtOrDefault(waveformResourceChunk.SampleIndex);
					if (sample == null) {
						break;
					}

					if (sample.RebuildAsStandardFileFormat(out var data, out var ext)) {
						Directory.CreateDirectory(Path.GetDirectoryName(instrumentOutput)!);
						Console.Out.WriteLine(indent + $"\tWrote {instrumentOutput}");
						File.WriteAllBytes(instrumentOutput + $".{ext}", data);
					}
				} else {
					Console.WriteLine($" Waveform Missing Resource: {instrument.Bank.GetGuidString(waveformInstrumentChunk.WaveformBody.Resource.Id)}");
				}
				break;
			default:
				Console.WriteLine();
				break;
		}
	}

	private static void PrintPlaylist(string indent, PlaylistChunk? playlistChunk, string output, bool dry) {
		if (playlistChunk == null) {
			return;
		}

		Console.WriteLine(indent + $" Playlist {playlistChunk.PlayMode} {playlistChunk.SelectionMode}");

		foreach (var entry in playlistChunk.Entries.Span) {
			if (!entry.Instrument.TryGetChunk(playlistChunk.Bank, out var instrumentChunk)) {
				continue;
			}

			Console.WriteLine(indent + $"\tWeight: {entry.Weight}");
			ProcessInstrument(indent + "\t\t", instrumentChunk, output, dry);
		}
	}
}
