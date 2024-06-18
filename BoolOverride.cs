using UnityEngine;

namespace Bipolar
{
	public enum BoolOverride
	{
		[InspectorName("Don't Override")]
		DontOverride = 0,
		True = 1,
		False = -1,
	}

	public static class BoolOverrideExtensions
	{
		public static bool TryOverride(this BoolOverride @override, bool value)
		{
			if (@override == BoolOverride.DontOverride)
				return value;

			return @override == BoolOverride.True;
        }

		public static bool TryOverride(this BoolOverride @override, System.Func<bool> value)
		{
			if (@override == BoolOverride.DontOverride)
				return value();

			return @override == BoolOverride.True;
		}
	}
}
