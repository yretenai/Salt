namespace Robin.Models;

public enum ChunkId : uint {
	/// <summary> RIFF </summary>
	RIFF = 0x46464952,

	/// <summary> FEV </summary>
	FEV = 0x20564546,

	/// <summary> LIST </summary>
	LIST = 0x5453494c,

	/// <summary> BuiltinEffectBody </summary>
	BEFB = 0x42464542,

	/// <summary> BuiltinEffect </summary>
	BEFF = 0x46464542,

	/// <summary> BuiltinEffects </summary>
	BEFX = 0x58464542,

	/// <summary> BankInfo </summary>
	BNKI = 0x494b4e42,

	/// <summary> Bus </summary>
	BUS = 0x20535542,

	/// <summary> Buses </summary>
	BUSS = 0x53535542,

	/// <summary> CommandInstrumentBody </summary>
	CMDB = 0x42444d43,

	/// <summary> CommandInstrument </summary>
	CMDI = 0x49444d43,

	/// <summary> CommandInstruments </summary>
	CMDS = 0x53444d43,

	/// <summary> Curves </summary>
	CRVS = 0x53565243,

	/// <summary> Controller </summary>
	CTRL = 0x4c525443,

	/// <summary> ControllerOwner </summary>
	CTRO = 0x4f525443,

	/// <summary> Controllers </summary>
	CTRS = 0x53525443,

	/// <summary> Curve </summary>
	CURV = 0x56525543,

	/// <summary> DeleteInfo </summary>
	DEL = 0x204c4544,

	/// <summary> EffectInstrumentBody </summary>
	EFIB = 0x42494645,

	/// <summary> EffectInstruments </summary>
	EFIS = 0x53494645,

	/// <summary> EffectInstrument </summary>
	EFIT = 0x54494645,

	/// <summary> Effects </summary>
	EFX = 0x20584645,

	/// <summary> EventInstrumentBody </summary>
	EVIB = 0x42495645,

	/// <summary> EventInstruments </summary>
	EVIS = 0x53495645,

	/// <summary> EventInstrument </summary>
	EVIT = 0x54495645,

	/// <summary> Event </summary>
	EVNT = 0x544e5645,

	/// <summary> EventBody </summary>
	EVTB = 0x42545645,

	/// <summary> Events </summary>
	EVTS = 0x53545645,

	/// <summary> FormatInfo </summary>
	FMT = 0x20544d46,

	/// <summary> GroupBusBody </summary>
	GBSB = 0x42534247,

	/// <summary> GroupBuses </summary>
	GBSS = 0x53534247,

	/// <summary> GroupBus </summary>
	GBUS = 0x53554247,

	/// <summary> HashData </summary>
	HASH = 0x48534148,

	/// <summary> InputBusBody </summary>
	IBSB = 0x42534249,

	/// <summary> InputBuses </summary>
	IBSS = 0x53534249,

	/// <summary> InputBus </summary>
	IBUS = 0x53554249,

	/// <summary> Instrument </summary>
	INST = 0x54534e49,

	/// <summary> Instruments </summary>
	INTS = 0x53544e49,

	/// <summary> Languages </summary>
	LANG = 0x474e414c,

	/// <summary> ListCount </summary>
	LCNT = 0x544e434c,

	/// <summary> LooseWaveformResource </summary>
	LWAV = 0x5641574c,

	/// <summary> LooseWaveformResources </summary>
	LWVS = 0x5356574c,

	/// <summary> Mapping </summary>
	MAP = 0x2050414d,

	/// <summary> MasterBusBody </summary>
	MBSB = 0x4253424d,

	/// <summary> MasterBuses </summary>
	MBSS = 0x5353424d,

	/// <summary> MasterBus </summary>
	MBUS = 0x5355424d,

	/// <summary> ModulatorBody </summary>
	MODB = 0x42444f4d,

	/// <summary> Modulators </summary>
	MODS = 0x53444f4d,

	/// <summary> Modulator </summary>
	MODU = 0x55444f4d,

	/// <summary> Mappings </summary>
	MPGS = 0x5347504d,

	/// <summary> MultiInstrumentBody </summary>
	MUIB = 0x4249554d,

	/// <summary> MultiInstruments </summary>
	MUIS = 0x5349554d,

	/// <summary> MultiInstrument </summary>
	MUIT = 0x5449554d,

	/// <summary> MuteInfo </summary>
	MUTE = 0x4554554d,

	/// <summary> MixerStrip </summary>
	MXST = 0x5453584d,

	/// <summary> Parameter </summary>
	PARM = 0x4d524150,

	/// <summary> PluginEffectBody </summary>
	PEFB = 0x42464550,

	/// <summary> PluginEffect </summary>
	PEFF = 0x46464550,

	/// <summary> PluginEffects </summary>
	PEFX = 0x58464550,

	/// <summary> PlatformInfo </summary>
	PLAT = 0x54414c50,

	/// <summary> PlaylistInstrument </summary>
	PLIT = 0x54494c50,

	/// <summary> Playlist </summary>
	PLST = 0x54534c50,

	/// <summary> ParameterizedEffect </summary>
	PMEF = 0x46454d50,

	/// <summary> ParameterLayoutBody </summary>
	PMLB = 0x424c4d50,

	/// <summary> ParameterLayout </summary>
	PMLO = 0x4f4c4d50,

	/// <summary> ParameterLayouts </summary>
	PMLS = 0x534c4d50,

	/// <summary> ProgrammerInstrumentBody </summary>
	PRIB = 0x42495250,

	/// <summary> ProgrammerInstruments </summary>
	PRIS = 0x53495250,

	/// <summary> ProgrammerInstrument </summary>
	PRIT = 0x54495250,

	/// <summary> ParameterBody </summary>
	PRMB = 0x424d5250,

	/// <summary> Parameters </summary>
	PRMS = 0x534d5250,

	/// <summary> Project </summary>
	PROJ = 0x4a4f5250,

	/// <summary> Property </summary>
	PROP = 0x504f5250,

	/// <summary> PropertyOwner </summary>
	PRPS = 0x53505250,

	/// <summary> ReturnBusBody </summary>
	RBSB = 0x42534252,

	/// <summary> ReturnBuses </summary>
	RBSS = 0x53534252,

	/// <summary> ReturnBus </summary>
	RBUS = 0x53554252,

	/// <summary> PortBody </summary>
	PRTB = 0x42545250,

	/// <summary> Ports </summary>
	RPTS = 0x53545250,

	/// <summary> Port </summary>
	PORT = 0x54524f50,

	/// <summary> ModelRefreshInfo </summary>
	REFI = 0x49464552,

	/// <summary> Routable </summary>
	ROUT = 0x54554f52,

	/// <summary> SideChainEffect </summary>
	SCEF = 0x46454353,

	/// <summary> SideChainEffectBody </summary>
	SCFB = 0x42464353,

	/// <summary> SideChainEffect </summary>
	SCFF = 0x46464353,

	/// <summary> SideChainEffects </summary>
	SCFX = 0x58464353,

	/// <summary> SendEffectBody </summary>
	SEFB = 0x42464553,

	/// <summary> SendEffect </summary>
	SEFF = 0x46464553,

	/// <summary> SendEffects </summary>
	SEFX = 0x58464553,

	/// <summary> SnapshotBody </summary>
	SNAB = 0x42414e53,

	/// <summary> Snapshot </summary>
	SNAP = 0x50414e53,

	/// <summary> Snapshots </summary>
	SNAS = 0x53414e53,

	/// <summary> SoundData </summary>
	SND = 0x20444e53,

	/// <summary> SoundDataHeader </summary>
	SNDH = 0x48444e53,

	/// <summary> SilenceInstruments </summary>
	SNLS = 0x534E4C53,

	/// <summary> SilenceInstrument </summary>
	SNLI = 0x494E4C53,

	/// <summary> SilenceInstrumentBody </summary>
	SNLB = 0x424E4C53,

	/// <summary> SpawnInstrumentBody </summary>
	SPIB = 0x42495053,

	/// <summary> SpawnInstruments </summary>
	SPIS = 0x53495053,

	/// <summary> SpawnInstrument </summary>
	SPIT = 0x54495053,

	/// <summary> SoundTable </summary>
	STBL = 0x4c425453,

	/// <summary> StringData </summary>
	STDT = 0x54445453,

	/// <summary> SubEvent </summary>
	SUBE = 0x45425553,

	/// <summary> SubEvents </summary>
	SUBS = 0x53425553,

	/// <summary> TimelineBody </summary>
	TLNB = 0x424e4c54,

	/// <summary> Timelines </summary>
	TLNS = 0x534e4c54,

	/// <summary> Timeline </summary>
	TMLN = 0x4e4c4d54,

	/// <summary> Transition </summary>
	TRAN = 0x4e415254,

	/// <summary> TransitionRegionBody </summary>
	TRNB = 0x424e5254,

	/// <summary> TransitionRegions </summary>
	TRNS = 0x534e5254,

	/// <summary> TransitionTimeline </summary>
	TRTL = 0x4c545254,

	/// <summary> VCA </summary>
	VCA = 0x20414356,

	/// <summary> VCABody </summary>
	VCAB = 0x42414356,

	/// <summary> VCAs </summary>
	VCAS = 0x53414356,

	/// <summary> WaveformInstrumentBody </summary>
	WAIB = 0x42494157,

	/// <summary> WaveformInstruments </summary>
	WAIS = 0x53494157,

	/// <summary> WaveformInstrument </summary>
	WAIT = 0x54494157,

	/// <summary> WaveformResource </summary>
	WAV = 0x20564157,

	/// <summary> WaveformResources </summary>
	WAVS = 0x53564157,
}
