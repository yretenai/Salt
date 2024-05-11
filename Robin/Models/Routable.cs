using System.Runtime.InteropServices;

namespace Robin.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct Routable {
	public Guid Output { get; set; }
	public uint OutputChannelLayout { get; set; }
	public uint ChannelMask { get; set; }
}
