using System.Runtime.CompilerServices;
using Salt.Chunk.Abstract;
using Salt.Models;

namespace Salt.Chunk;

public sealed record TimelineBodyChunk : ModelChunk, IAddressable {
	public TimelineBodyChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(reader, atom, soundBank) {
		if (soundBank.Format.FileVersion < 109) {
			reader.Position += Unsafe.SizeOf<Guid>(); // ??
		}

		TriggerBoxes = reader.ReadElementArray<TriggerBox>().ToArray();
		LockedTriggerBoxes = reader.ReadElementArray<TriggerBox>().ToArray();
		SustainPoints = reader.ReadElementArray<uint>().ToArray();

		var size = reader.ReadSize() >> 1;
		for (var i = 0; i < size; ++i) {
			var entrySize = reader.Read<ushort>();
			var expected = reader.Position + entrySize;
			NamedMarkers.Add(new TimelineNamedMarker {
				Id = reader.Read<Guid>(),
				Start = reader.Read<uint>(),
				Name = reader.ReadString(),
				Length = reader.Read<uint>(),
			});
			reader.Position = expected;
		}

		TempoMarkers = reader.ReadElementArray<TimelineTempoMarker>().ToArray();
	}

	public ReadOnlyMemory<TriggerBox> TriggerBoxes { get; set; }
	public ReadOnlyMemory<TriggerBox> LockedTriggerBoxes { get; set; }
	public ReadOnlyMemory<uint> SustainPoints { get; set; }
	public List<TimelineNamedMarker> NamedMarkers { get; set; } = [];
	public ReadOnlyMemory<TimelineTempoMarker> TempoMarkers { get; set; }
	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.TLNS, ChunkId.TMLN, ChunkId.TLNB];
}
