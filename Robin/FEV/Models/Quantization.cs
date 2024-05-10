using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct Quantization {
	public QuantizationUnit Unit { get; set; }
	public int Multiplier { get; set; }
}
