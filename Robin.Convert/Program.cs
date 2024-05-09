using Robin.FEV;

namespace Robin.Convert;

internal class Program {
	private static void Main(string[] args) {
		FEVSoundBank.TryLoadBank(args[0], null, out var fevSoundBank);
	}
}
