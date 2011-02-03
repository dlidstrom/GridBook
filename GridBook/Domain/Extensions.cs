namespace GridBook.Domain
{
	using System;

	public static class Extensions
	{
		/// <summary>
		/// Convert 64-bit unsigned long to 64-bit long.
		/// </summary>
		/// <param name="l">64-bit unsigned long.</param>
		/// <returns>64-bit long.</returns>
		public static long ToInt64(this ulong l)
		{
			return BitConverter.ToInt64(BitConverter.GetBytes(l), 0);
		}

		/// <summary>
		/// Convert 64-bit long to 64-bit unsigned long.
		/// </summary>
		/// <param name="l">64-bit long.</param>
		/// <returns>64-bit unsigned long.</returns>
		public static ulong ToUInt64(this long l)
		{
			return BitConverter.ToUInt64(BitConverter.GetBytes(l), 0);
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
