using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public sealed record ControllerChunk : BaseChunk {
	public ControllerChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }
}
