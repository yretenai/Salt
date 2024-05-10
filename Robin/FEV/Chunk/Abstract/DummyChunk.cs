using Robin.FEV.Models;

namespace Robin.FEV.Chunk.Abstract;

public sealed record DummyChunk : BaseChunk {
	public DummyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }

	public override bool IsFunctionallyEmpty => true;
}
