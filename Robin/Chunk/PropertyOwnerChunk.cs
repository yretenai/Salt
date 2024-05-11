using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public abstract record PropertyOwnerChunk(RIFFAtom Atom, FEVSoundBank Bank) : BaseChunk(Atom, Bank), IHasId, IAddressable {
	public List<PropertyChunk> Properties { get; } = [];

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.EVNT, ChunkId.EVTB, ChunkId.EVTS,
		ChunkId.BUS, ChunkId.BUSS, // todo: expand
		ChunkId.PARM, ChunkId.PRMS, ChunkId.PRMB,
		ChunkId.PMEF,
		ChunkId.SEFX, ChunkId.SEFF, ChunkId.SEFB,
		ChunkId.SNAP, ChunkId.SNAS, ChunkId.SNAB,
		ChunkId.MODU, ChunkId.MODS, ChunkId.MODB,
		ChunkId.VCA, ChunkId.VCAS, ChunkId.VCAB,
		ChunkId.EVIT, ChunkId.EVIS, ChunkId.EVIB,
		ChunkId.PLIT, ChunkId.PLIT,
		ChunkId.WAIT, ChunkId.WAIS, ChunkId.WAIB,
		ChunkId.MUIT, ChunkId.MUIS, ChunkId.MUIB,
		ChunkId.CMDI, ChunkId.CMDS, ChunkId.CMDB,
		ChunkId.EFIT, ChunkId.EFIS, ChunkId.EFIB,
		ChunkId.PRIT, ChunkId.PRIS, ChunkId.PRIB,
		ChunkId.SNLI, ChunkId.SNLS, ChunkId.SNLB,
		ChunkId.SPIT, ChunkId.SPIS, ChunkId.SPIB,
		ChunkId.INST, ChunkId.INTS,
		ChunkId.PROP, ChunkId.PRPS,
	];

	public abstract Guid Id { get; }

	protected void ProcessPropertyChunk(FEVReader reader) {
		if (!TryReadChunk<ListChunk>(reader, Bank, out var list)) {
			return;
		}

		Properties.AddRange(list.Chunks.OfType<PropertyChunk>());
	}
}
