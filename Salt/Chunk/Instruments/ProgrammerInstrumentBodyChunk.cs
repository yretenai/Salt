using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk.Instruments;

public sealed record ProgrammerInstrumentBodyChunk : ModelChunk, IAddressable {
	public ProgrammerInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.PRIB, nameof(Atom));

		InstrumentName = reader.ReadString();
	}

	public string InstrumentName { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.PRIT, ChunkId.PRIS, ChunkId.PRIB,
	];
}
