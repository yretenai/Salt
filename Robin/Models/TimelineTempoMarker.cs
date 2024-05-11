using System.Runtime.InteropServices;

namespace Robin.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct TimelineTempoMarker {
	public Guid Id { get; set; }
	public ulong TimeSignature { get; set; }
	public uint Position { get; set; }
	public float Tempo { get; set; }
}
