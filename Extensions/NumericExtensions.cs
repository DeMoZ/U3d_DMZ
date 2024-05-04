using System;

namespace DMZ.Extensions
{
	public static class NumericExtensions
	{
		public static bool IsBetween(this int val, int min, int max)
		{
			return val >= min && val <= max;
		}
		
		public static bool IsBetween(this float val, float min, float max)
		{
			return val >= min && val <= max;
		}
		
		public static bool IsBetween(this byte val, byte min, byte max)
		{
			return val >= min && val <= max;
		}
		
		public static bool IsBetween<T>(this T val, T min, T max) where T : IComparable<T>
		{
			return val.CompareTo(min) >= 0 && val.CompareTo(max) <= 0;
		}
	}
}