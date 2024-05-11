using Robin.Models;

namespace Robin.Chunk.Abstract;

public sealed record DummyChunk : BaseChunk {
	public DummyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }

	public override bool IsFunctionallyEmpty => true;
}
