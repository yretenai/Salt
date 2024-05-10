using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public abstract record ControllerOwnerChunk(RIFFAtom Atom, FEVSoundBank Bank) : BaseChunk(Atom, Bank), IHasId, IAddressable {
	public ControllerChunk? Controller { get; private set; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.TMLN, ChunkId.TLNS, ChunkId.TLNB,
		ChunkId.PMLO, ChunkId.PMLS, ChunkId.PMLB,
		ChunkId.CTRL, ChunkId.CTRO, ChunkId.CTRS,
	];

	public abstract Guid Id { get; }

	protected void ProcessControllerChunk(FEVReader reader) {
		if (!TryReadChunk<ControllerChunk>(reader, Bank, out var controllerBody)) {
			return;
		}

		Controller = controllerBody;
	}
}
