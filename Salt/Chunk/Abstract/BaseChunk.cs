using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Salt.Chunk.Instruments;
using Salt.Models;

namespace Salt.Chunk.Abstract;

public abstract record BaseChunk(RIFFAtom Atom, FEVSoundBank Bank) {
	public virtual ChunkId ChunkId => Atom.Id;
	public virtual bool IsFunctionallyEmpty => Atom.Length == 0;

	protected static BaseChunk ReadChunk(FEVReader reader, FEVSoundBank soundBank) {
		var atom = reader.Read<RIFFAtom>();
		var slice = reader.Slice(atom.Length, 2);
		var chunk = ReadChunk(slice, atom, soundBank);
	#if DEBUG
		if (chunk is not DummyChunk && slice.Length != slice.Position) {
			Debug.WriteLine($"Chunk {atom.Id} is not serialized fully!", "Salt");
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
			case ChunkId.LIST: {
				var list = new ListChunk(reader, atom, soundBank);
				return list.Body ?? list;
			}
			case ChunkId.BEFB: break;
			case ChunkId.BEFF: break;
			case ChunkId.BEFX: throw new UnreachableException();
			case ChunkId.BNKI: return new BankInfoChunk(reader, atom, soundBank);
			case ChunkId.BUS: break;
			case ChunkId.BUSS: throw new UnreachableException();
			case ChunkId.CMDI: return new CommandInstrumentChunk(reader, atom, soundBank);
			case ChunkId.CMDB: return new CommandInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.CMDS: throw new UnreachableException();
			case ChunkId.CRVS: throw new UnreachableException();
			case ChunkId.CTRL: break;
			case ChunkId.CTRO: break;
			case ChunkId.CTRS: throw new UnreachableException();
			case ChunkId.CURV: break;
			case ChunkId.DEL: break;
			case ChunkId.EFIB: return new EffectInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.EFIS: throw new UnreachableException();
			case ChunkId.EFIT: return new EffectInstrumentChunk(reader, atom, soundBank);
			case ChunkId.EFX: throw new UnreachableException();
			case ChunkId.EVIT: return new EventInstrumentChunk(reader, atom, soundBank);
			case ChunkId.EVIS: throw new UnreachableException();
			case ChunkId.EVIB: return new EventInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.EVNT: return new EventChunk(reader, atom, soundBank);
			case ChunkId.EVTB: return new EventBodyChunk(reader, atom, soundBank);
			case ChunkId.EVTS: throw new UnreachableException();
			case ChunkId.FMT: return new FormatChunk(reader, atom, soundBank);
			case ChunkId.GBSB: break;
			case ChunkId.GBSS: throw new UnreachableException();
			case ChunkId.GBUS: break;
			case ChunkId.HASH: return new HashChunk(reader, atom, soundBank);
			case ChunkId.IBSB: break;
			case ChunkId.IBSS: throw new UnreachableException();
			case ChunkId.IBUS: break;
			case ChunkId.INST: return new InstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.INTS: throw new UnreachableException();
			case ChunkId.LANG: break;
			case ChunkId.LCNT: return new ListCountChunk(reader, atom, soundBank);
			case ChunkId.LWAV: return new LooseWaveformResourceChunk(reader, atom, soundBank);
			case ChunkId.LWVS: throw new UnreachableException();
			case ChunkId.MAP: break;
			case ChunkId.MBSB: break;
			case ChunkId.MBSS: throw new UnreachableException();
			case ChunkId.MBUS: break;
			case ChunkId.MODB: break;
			case ChunkId.MODS: throw new UnreachableException();
			case ChunkId.MODU: break;
			case ChunkId.MPGS: throw new UnreachableException();
			case ChunkId.MUIT: return new MultiInstrumentChunk(reader, atom, soundBank);
			case ChunkId.MUIS: throw new UnreachableException();
			case ChunkId.MUIB: return new MultiInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.MUTE: break;
			case ChunkId.MXST: break;
			case ChunkId.PARM: break;
			case ChunkId.PEFB: break;
			case ChunkId.PEFF: break;
			case ChunkId.PEFX: throw new UnreachableException();
			case ChunkId.PLAT: return new PlatformChunk(reader, atom, soundBank);
			case ChunkId.PLIT: throw new UnreachableException();
			case ChunkId.PLST: return new PlaylistChunk(reader, atom, soundBank);
			case ChunkId.PMEF: break;
			case ChunkId.PMLB: break;
			case ChunkId.PMLO: break;
			case ChunkId.PMLS: throw new UnreachableException();
			case ChunkId.PRIT: return new ProgrammerInstrumentChunk(reader, atom, soundBank);
			case ChunkId.PRIS: throw new UnreachableException();
			case ChunkId.PRIB: return new ProgrammerInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.PRMB: break;
			case ChunkId.PRMS: throw new UnreachableException();
			case ChunkId.PROJ: return new ProjectChunk(reader, atom, soundBank);
			case ChunkId.PROP: return new PropertyChunk(reader, atom, soundBank);
			case ChunkId.PRPS: throw new UnreachableException();
			case ChunkId.RBSB: break;
			case ChunkId.RBSS: throw new UnreachableException();
			case ChunkId.RBUS: break;
			case ChunkId.PRTB: break;
			case ChunkId.RPTS: throw new UnreachableException();
			case ChunkId.PORT: break;
			case ChunkId.REFI: break;
			case ChunkId.ROUT: break;
			case ChunkId.SCEF: break;
			case ChunkId.SCFB: break;
			case ChunkId.SCFF: break;
			case ChunkId.SCFX: throw new UnreachableException();
			case ChunkId.SEFB: break;
			case ChunkId.SEFF: break;
			case ChunkId.SEFX: throw new UnreachableException();
			case ChunkId.SNAB: break;
			case ChunkId.SNAP: break;
			case ChunkId.SNAS: throw new UnreachableException();
			case ChunkId.SND: break;
			case ChunkId.SNDH: return new SoundHeaderChunk(reader, atom, soundBank);
			case ChunkId.SNLS: throw new UnreachableException();
			case ChunkId.SNLI: return new SilenceInstrumentChunk(reader, atom, soundBank);
			case ChunkId.SNLB: return new SilenceInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.SPIB: return new SpawningInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.SPIS: throw new UnreachableException();
			case ChunkId.SPIT: return new SpawningInstrumentChunk(reader, atom, soundBank);
			case ChunkId.STBL: break;
			case ChunkId.STDT: return new StringDataChunk(reader, atom, soundBank);
			case ChunkId.SUBE: break;
			case ChunkId.SUBS: throw new UnreachableException();
			case ChunkId.TLNB: return new TimelineBodyChunk(reader, atom, soundBank);
			case ChunkId.TLNS: throw new UnreachableException();
			case ChunkId.TMLN: return new TimelineChunk(reader, atom, soundBank);
			case ChunkId.TRAN: break;
			case ChunkId.TRNB: break;
			case ChunkId.TRNS: throw new UnreachableException();
			case ChunkId.TRTL: break;
			case ChunkId.VCA: break;
			case ChunkId.VCAB: break;
			case ChunkId.VCAS: throw new UnreachableException();
			case ChunkId.WAIB: return new WaveformInstrumentBodyChunk(reader, atom, soundBank);
			case ChunkId.WAIS: throw new UnreachableException();
			case ChunkId.WAIT: return new WaveformInstrumentChunk(reader, atom, soundBank);
			case ChunkId.WAV: return new WaveformResourceChunk(reader, atom, soundBank);
			case ChunkId.WAVS: throw new UnreachableException();
		}

		return new DummyChunk(reader, atom, soundBank);
	}
}
