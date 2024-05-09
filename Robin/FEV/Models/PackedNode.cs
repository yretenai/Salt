using System.Runtime.InteropServices;

namespace Robin.FEV.Models;

public enum PackedTableType {
	Table32Bit,
	Table24Bit,
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public record struct PackedNode {
	public uint Key { get; set; }
	public uint Child { get; set; }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PackedKeyValue<TKey, TValue> where TKey : struct where TValue : struct {
	public TKey Key { get; set; }
	public TValue Value { get; set; }

	public void Deconstruct(out TKey key, out TValue value) {
		key = Key;
		value = Value;
	}
}
