using System.Runtime.InteropServices;

namespace Salt.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct EventParameterStub {
	public Guid Parameter { get; set; }
	public float InitialValue { get; set; }
	public uint StubIndex { get; set; }
}
