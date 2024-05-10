using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct Routable {
	public uint OutputChannelLayout { get; set; }
	public uint ChannelMask { get; set; }
	public Guid Output { get; set; }
}
