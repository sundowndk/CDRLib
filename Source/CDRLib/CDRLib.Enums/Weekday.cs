using System;

namespace CDRLib.Enums
{
	[Flags]
	public enum Weekday
	{
		All = 0,
		Monday = 1,
		Tuesday = 2,
		Wednesday = 4,
		Thursday = 8,
		Fridag = 16,
		Saturday = 32,
		Sunday = 64
	}
}

