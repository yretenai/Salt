using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public abstract record InstrumentChunk(RIFFAtom Atom, FEVSoundBank Bank) : PropertyOwnerChunk(Atom, Bank), IAddressable {
	public InstrumentBodyChunk? Instrument { get; private set; }

	public new static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.EVIT, ChunkId.EVIS, ChunkId.EVIB,
		ChunkId.PLIT, ChunkId.PLIT,
		ChunkId.WAIT, ChunkId.WAIS, ChunkId.WAIB,
		ChunkId.GRIT,
		ChunkId.MUIT, ChunkId.MUIS, ChunkId.MUIB,
		ChunkId.CMDI, ChunkId.CMDS, ChunkId.CMDB,
		ChunkId.EFIT, ChunkId.EFIS, ChunkId.EFIB,
		ChunkId.PRIT, ChunkId.PRIS, ChunkId.PRIB,
		ChunkId.SNLI, ChunkId.SNLS, ChunkId.SNLB,
		ChunkId.SPIT, ChunkId.SPIS, ChunkId.SPIB,
		ChunkId.INST,
	];

	protected void ProcessInstrumentChunk(FEVReader reader) {
		if (!TryReadChunk<InstrumentBodyChunk>(reader, Bank, out var instrumentBody)) {
			return;
		}

		Instrument = instrumentBody;

		ProcessPropertyChunk(reader);
	}
}
