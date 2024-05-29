namespace Salt.Models;

public enum WaveformLoadingMethod {
	LoadInMemory = 0,
	DecompressInMemory = 1,
	StreamFromDisk = 2,
	Undefined = 3,
}
