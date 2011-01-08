namespace GridBook.Domain
{
	using System;
	using System.Globalization;

	public static class Extensions
	{
		public static long ToInt64(this ulong l)
		{
			var s = l.ToString("x");
			return long.Parse(s, NumberStyles.AllowHexSpecifier);
		}

		public static ulong ToUInt64(this long l)
		{
			var s = l.ToString("x");
			return ulong.Parse(s, NumberStyles.AllowHexSpecifier);
		}
	}
}
