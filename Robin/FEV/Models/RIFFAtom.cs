using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public record struct RIFFAtom {
	public ChunkId Id;
	public int Length;

	public override string ToString() => Id.ToString("G");
}
