using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk.Abstract;

public abstract record BaseChunk(RIFFAtom Atom, FEVSoundBank Bank) {
	public virtual ChunkId ChunkId => Atom.Id;
	public virtual bool IsFunctionallyEmpty => Atom.Length == 0;

	protected static BaseChunk ReadChunk(FEVReader reader, FEVSoundBank soundBank) {
		var atom = reader.Read<RIFFAtom>();
		var slice = reader.Slice(atom.Length, 2);
		var chunk = ReadChunk(slice, atom, soundBank);
	#if DEBUG
		if (chunk is not DummyChunk && slice.Length != slice.Position) {
			Debug.WriteLine($"Chunk {atom.Id} is not serialized fully!", "Robin");
		}
	#endif
		return chunk;
	}

	internal static bool TryReadChunk<T>(FEVReader reader, FEVSoundBank soundBank, [MaybeNullWhen(false)] out T chunk) where T : BaseChunk {
		if (ReadChunk(reader, soundBank) is not T value) {
			chunk = null;
			return false;
		}

		chunk = value;
		return true;
	}

	protected static BaseChunk ReadChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) {
		switch (atom.Id) {
			case ChunkId.BNKI: {
				return new BankInfoChunk(reader, atom, soundBank);
			}
			case ChunkId.EVNT: {
				return new EventChunk(reader, atom, soundBank);
			}
			case ChunkId.EVTB: {
				return new EventBodyChunk(reader, atom, soundBank);
			}
			case ChunkId.FMT: {
				return new FormatChunk(reader, atom, soundBank);
			}
			case ChunkId.HASH: {
				return new HashChunk(reader, atom, soundBank);
			}
			case ChunkId.LIST: {
				var list = new ListChunk(reader, atom, soundBank);
				return list.Body ?? list;
			}
			case ChunkId.LCNT: {
				return new ListCountChunk(reader, atom, soundBank);
			}
			case ChunkId.LWAV: {
				return new LooseWaveformResourceChunk(reader, atom, soundBank);
			}
			case ChunkId.PROJ: {
				return new ProjectChunk(reader, atom, soundBank);
			}
			case ChunkId.SNDH: {
				return new SoundHeaderChunk(reader, atom, soundBank);
			}
			case ChunkId.STDT: {
				return new StringDataChunk(reader, atom, soundBank);
			}
			case ChunkId.TMLN: {
				return new TimelineChunk(reader, atom, soundBank);
			}
			case ChunkId.TLNB: {
				return new TimelineBodyChunk(reader, atom, soundBank);
			}
			case ChunkId.WAV: {
				return new WaveformResourceChunk(reader, atom, soundBank);
			}
			default:
				return new DummyChunk(reader, atom, soundBank);
		}
	}
}
