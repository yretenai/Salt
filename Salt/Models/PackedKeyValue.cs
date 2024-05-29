using System.Runtime.InteropServices;

namespace Salt.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PackedKeyValue<TKey, TValue> where TKey : struct where TValue : struct {
	public TKey Key { get; set; }
	public TValue Value { get; set; }

	public void Deconstruct(out TKey key, out TValue value) {
		key = Key;
		value = Value;
	}
}
