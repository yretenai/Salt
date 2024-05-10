using Robin.FEV.Models;

namespace Robin.FEV.Chunk.Abstract;

public abstract record ModelChunk : BaseChunk, IHasId {
	protected ModelChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		if (reader.Length < 16) {
			return;
		}

		Id = reader.Read<Guid>();
	}

	public Guid Id { get; }
}
