using System.Runtime.InteropServices;
using Robin.Chunk.Instruments;

namespace Robin.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PlaylistEntry {
	public GuidRef<InstrumentChunk> Instrument { get; set; }
	public float Weight { get; set; }
}
