using System.Runtime.InteropServices;
using Salt.Chunk.Instruments;

namespace Salt.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PlaylistEntry {
	public GuidRef<InstrumentChunk> Instrument { get; set; }
	public float Weight { get; set; }
}
