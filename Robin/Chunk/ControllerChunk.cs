using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk;

public sealed record ControllerChunk : BaseChunk {
	public ControllerChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }
}
