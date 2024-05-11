using Robin.Chunk.Abstract;
using Robin.Models;

namespace Robin.Chunk.Instruments;

public sealed record CommandInstrumentBodyChunk : ModelChunk, IAddressable {
	public CommandInstrumentBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.CMDB, nameof(Atom));

		Command = reader.Read<CommandType>();
		Target = reader.Read<Guid>();
	}

	public CommandType Command { get; }
	public Guid Target { get; }

	public static ReadOnlySpan<ChunkId> ListTypes => [
		ChunkId.CMDI, ChunkId.CMDS, ChunkId.CMDB,
	];
}
