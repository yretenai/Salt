using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using DragonLib;

namespace Robin;

public class FEVReader(ReadOnlyMemory<byte> Buffer) {
	public int Position { get; set; }
	public int Length => Buffer.Length;
	public override string ToString() => $"FEVReader {{ Position = 0x{Position:x}, Length = 0x{Length:x} }}";

	public T Read<T>() where T : struct {
		var size = Unsafe.SizeOf<T>();
		if (Position + size > Length) {
			Position = Length;
			return default;
		}

		var value = Peek<T>();
		Position += size;
		return value;
	}

	public T Peek<T>() where T : struct {
		if (Position + Unsafe.SizeOf<T>() > Length) {
			return default;
		}

		var value = MemoryMarshal.Read<T>(Buffer.Span[Position..]);
		return value;
	}

	public ReadOnlyMemory<byte> Read(int n) {
		if (n == 0) {
			return ReadOnlyMemory<byte>.Empty;
		}

		if (Position + n > Length) {
			n = Length - Position;
		}

		var slice = Buffer.Slice(Position, n);
		Position += n;
		return slice;
	}

	public ReadOnlySpan<T> Read<T>(int n) where T : struct => MemoryMarshal.Cast<byte, T>(Read(Unsafe.SizeOf<T>() * n).Span);

	public FEVReader Slice(int n, int alignment = 1) {
		var slice = Read(n);
		if (alignment > 1) {
			Position += n.Align(alignment) - n;

			if (Position > Length) {
				Position = Length;
			}
		}

		return new FEVReader(slice);
	}

	public FEVReader Slice() => new(Read(Read<ushort>()));

	public int ReadSize() {
		var length = (int) Read<ushort>();
		if ((length & 0x8000) != 0) {
			length = (length & 0x7FFF) | (Read<ushort>() << 15);
		}

		return length;
	}

	public ReadOnlySpan<T> ReadElementArray<T>() where T : struct {
		var elementCount = ReadSize();
		if (elementCount is 0 or 1) {
			return ReadOnlySpan<T>.Empty;
		}

		var isUniform = (elementCount & 1) == 1;
		Debug.Assert(isUniform, "isUniform");

		var elementSize = Read<ushort>();
		Debug.Assert(elementSize == Unsafe.SizeOf<T>(), "elementSize == sizeof(T)");

		var slice = Read(elementSize * (elementCount >> 1));
		return MemoryMarshal.Cast<byte, T>(slice.Span);
	}

	public void SkipElementArray() {
		var count = ReadSize();
		var isUniform = (count & 1) == 1;
		if (isUniform) {
			Position += count * Read<ushort>();
		} else {
			for (var i = 0; i < count; ++i) {
				var size = Read<ushort>();
				Position += size;
			}
		}

		if (Position > Length) {
			Position = Length;
		}
	}

	public string ReadString() {
		var length = ReadSize();
		return length is 0 ? string.Empty : Encoding.UTF8.GetString(Read(length).Span);
	}
}
