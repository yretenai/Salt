using System.Runtime.CompilerServices;

namespace Salt.Models;

[InlineArray(3)]
public record struct UInt24 {
	public byte V { get; set; }

	public static implicit operator int(UInt24 value) => (value[2] << 16) | (value[1] << 8) | value[0];
}
