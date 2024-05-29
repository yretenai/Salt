using System.Runtime.InteropServices;

namespace Salt.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct Range<T> where T : struct {
	public T Minimum { get; set; }
	public T Maximum { get; set; }

	public void Deconstruct(out T minimum, out T maximum) {
		minimum = Minimum;
		maximum = Maximum;
	}

	public override string ToString() => Minimum + "-" + Maximum;
}
