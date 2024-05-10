using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct TriggerBox {
	public Guid Instrument { get; set; }
	public uint Start { get; set; }
	public uint Length { get; set; }
}

public record TimelineNamedMarker {
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public uint Start { get; set; }
	public uint Length { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct TimelineTempoMarker {
	public Guid Id { get; set; }
	public ulong TimeSignature { get; set; }
	public uint Position { get; set; }
	public float Tempo { get; set; }
}
