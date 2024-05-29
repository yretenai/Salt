using Salt.Models;

namespace Salt.Chunk.Abstract;

public sealed record DummyChunk : BaseChunk {
	public DummyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) { }

	public override bool IsFunctionallyEmpty => true;
}
