using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public record struct PackedNode {
	public uint Key { get; set; }
	public uint Child { get; set; }
}
