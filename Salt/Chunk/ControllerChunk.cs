using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record ControllerChunk : BaseChunk {
	public ControllerChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }
}
