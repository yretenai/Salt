using System.Runtime.InteropServices;
using Robin.FEV.Chunk;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct TriggerBox {
	public GuidRef<InstrumentChunk> Instrument { get; set; }
	public uint Start { get; set; }
	public uint Length { get; set; }
}
