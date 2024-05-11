namespace Robin.Models;

public enum WaveformLoadingMethod {
	LoadInMemory,
	DecompressInMemory,
	StreamFromDisk,
	Undefined,
}
