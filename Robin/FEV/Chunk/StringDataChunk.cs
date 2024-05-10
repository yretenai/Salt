using System.Collections.ObjectModel;
using DragonLib;
using Robin.FEV.Chunk.Abstract;
using Robin.FEV.Models;

namespace Robin.FEV.Chunk;

public record StringDataChunk : BaseChunk, IAddressable {
	public StringDataChunk(FEVReader reader, RIFFAtom atom, FEVSoundBank soundBank) : base(atom, soundBank) {
		ArgumentOutOfRangeException.ThrowIfNotEqual((int) Atom.Id, (int) ChunkId.STDT, nameof(Atom));

		//        4           6          8                 0xA        0xC               0xE          0xF        0x12
		// 0x12 = TableSize + NodeSize + NodeElementSize + GuidSize + GuidElementSize + StringSize + LeafSize + IndiceSize
		if (Atom.Length < 0x12) {
			return;
		}

		var tableType = reader.Read<PackedTableType>();
		ArgumentOutOfRangeException.ThrowIfGreaterThan((int) tableType, 1, nameof(tableType));

		Nodes = reader.ReadElementArray<PackedNode>().ToArray();
		Guids = reader.ReadElementArray<Guid>().ToArray();
		StringData = reader.Read(reader.ReadSize());

		switch (tableType) {
			case PackedTableType.Table32Bit: {
				Leafs = reader.Read<int>(reader.ReadSize()).ToArray();
				Indices = reader.Read<int>(reader.ReadSize()).ToArray();
				break;
			}
			case PackedTableType.Table24Bit: {
				var leafs = reader.Read<UInt24>(reader.ReadSize());
				var fullLeafs = new Memory<int>(new int[leafs.Length]);
				for (var i = 0; i < leafs.Length; ++i) {
					fullLeafs.Span[i] = leafs[i];
				}

				var indices = reader.Read<UInt24>(reader.ReadSize());
				var fullIndices = new Memory<int>(new int[indices.Length]);
				for (var i = 0; i < indices.Length; ++i) {
					fullIndices.Span[i] = indices[i];
				}

				Leafs = fullLeafs;
				Indices = fullIndices;
				break;
			}
		}
	}


	public ReadOnlyMemory<PackedNode> Nodes { get; set; }
	public ReadOnlyMemory<Guid> Guids { get; set; }
	public ReadOnlyMemory<byte> StringData { get; set; }
	public ReadOnlyMemory<int> Leafs { get; set; }
	public ReadOnlyMemory<int> Indices { get; set; }

	private Dictionary<Guid, string>? Dictionary { get; set; }
	private Dictionary<string, Guid>? ReverseDictionary { get; set; }

	public override bool IsFunctionallyEmpty => base.IsFunctionallyEmpty || Guids.Length == 0;

	public static ReadOnlySpan<ChunkId> ListTypes => [ChunkId.STDT];


	public ReadOnlyDictionary<Guid, string> ToDictionary() {
		if (Guids.Length == 0) {
			return ReadOnlyDictionary<Guid, string>.Empty;
		}

		if (Dictionary == null) {
			var dict = new Dictionary<Guid, string>();
			for (var i = 0; i < Guids.Length; ++i) {
				var guid = Guids.Span[i];
				var name = string.Empty;
				var currentNode = Leafs.Span[i] & 0xFFFFFF;
				while (currentNode != 0xFFFFFF) {
					var node = Nodes.Span[currentNode];
					var stringOffset = (int) node.Key & 0xFFFFFF;
					if (stringOffset != 0xFFFFFF) {
						name = StringData[stringOffset..].Span.ReadUTF8StringNonNull() + name;
					}

					currentNode = Indices.Span[currentNode] & 0xFFFFFF;
				}

				dict[guid] = name;
			}

			Dictionary = dict;
		}

		return Dictionary.AsReadOnly();
	}

	public ReadOnlyDictionary<string, Guid> ToReverseDictionary() {
		if (Guids.Length == 0) {
			return ReadOnlyDictionary<string, Guid>.Empty;
		}

		if (ReverseDictionary == null) {
			var dict = ToDictionary();
			var reverse = new Dictionary<string, Guid>();
			foreach (var (key, value) in dict) {
				reverse[value] = key;
			}

			ReverseDictionary = reverse;
		}

		return ReverseDictionary.AsReadOnly();
	}

	public override string ToString() => $"{nameof(StringDataChunk)} {{ Count = {Guids.Length} }}";
}
