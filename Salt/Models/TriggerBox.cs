using System.Runtime.InteropServices;
using Salt.Chunk.Instruments;

namespace Salt.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct TriggerBox {
	public GuidRef<InstrumentChunk> Instrument { get; set; }
	public uint Start { get; set; }
	public uint Length { get; set; }
}
