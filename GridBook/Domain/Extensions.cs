namespace GridBook.Domain
{
	using System;
	using System.Globalization;

	public static class Extensions
	{
		/// <summary>
		/// Convert 64-bit unsigned long to 64-bit long.
		/// </summary>
		/// <param name="l">64-bit unsigned long.</param>
		/// <returns>64-bit long.</returns>
		public static long ToInt64(this ulong l)
		{
			var s = l.ToString("x");
			return long.Parse(s, NumberStyles.AllowHexSpecifier);
		}

		/// <summary>
		/// Convert 64-bit long to 64-bit unsigned long.
		/// </summary>
		/// <param name="l">64-bit long.</param>
		/// <returns>64-bit unsigned long.</returns>
		public static ulong ToUInt64(this long l)
		{
			var s = l.ToString("x");
			return ulong.Parse(s, NumberStyles.AllowHexSpecifier);
		}

		/// <summary>
		/// Flips horizontally.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipHorizontal(this ulong x)
		{
			return Bits.FlipHorizontal(x);
		}

		/// <summary>
		/// Flips vertically.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipVertical(this ulong x)
		{
			return Bits.FlipVertical(x);
		}

		/// <summary>
		/// Flips diagonally (A8-H1).
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipDiagonal(this ulong x)
		{
			return Bits.FlipDiagonal(x);
		}

		/// <summary>
		/// Calculates the least-significant bit set.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Least-significant bit set.</returns>
		public static int LSB(this ulong x)
		{
			return Bits.LSB(x);
		}
	}
}
