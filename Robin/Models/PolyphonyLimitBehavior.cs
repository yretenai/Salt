namespace Robin.Models;

public enum PolyphonyLimitBehavior : uint {
	Fail = 0,
	Mute = 1,
	StealLowest = 2,
}
