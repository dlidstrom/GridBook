namespace GridBook.Domain
{
	using System;
	using System.Linq;

	/// <summary>
	/// Class with bit manipulation methods.
	/// </summary>
	public static class Bits
	{
		private static int[] bitcount = Enumerable.Repeat(0, ushort.MaxValue + 1).ToArray();

		/// <summary>
		/// Initializes static members of the Bits class.
		/// </summary>
		static Bits()
		{
			for (int i = 0; i <= ushort.MaxValue; i++)
			{
				int c = 0;
				int j = i;
				while (j != 0)
				{
					j = j & (j - 1);
					c++;
				}

				bitcount[i] = c;
			}
		}

		/// <summary>
		/// Calculates the number of bits set in a 64-bit long.
		/// </summary>
		/// <param name="l">64-bit integer.</param>
		/// <returns>Number of bits set.</returns>
		public static int Count(long l)
		{
			return bitcount[(ushort)l]
				+ bitcount[(ushort)(l >> 16)]
				+ bitcount[(ushort)(l >> 32)]
				+ bitcount[(ushort)(l >> 48)];
		}

		/// <summary>
		/// Flips horizontally.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipHorizontal(this ulong x)
		{
			const ulong k1 = 0x5555555555555555;
			const ulong k2 = 0x3333333333333333;
			const ulong k4 = 0x0f0f0f0f0f0f0f0f;
			x = ((x >> 1) & k1) | ((x & k1) << 1);
			x = ((x >> 2) & k2) | ((x & k2) << 2);
			x = ((x >> 4) & k4) | ((x & k4) << 4);

			return x;
		}

		/// <summary>
		/// Flips vertically.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipVertical(this ulong x)
		{
			return (x << 56)
				| ((x << 40) & (0x00ff000000000000))
				| ((x << 24) & (0x0000ff0000000000))
				| ((x << 8) & (0x000000ff00000000))
				| ((x >> 8) & (0x00000000ff000000))
				| ((x >> 24) & (0x0000000000ff0000))
				| ((x >> 40) & (0x000000000000ff00))
				| ((x >> 56));
		}

		/// <summary>
		/// Flips diagonally (A8-H1).
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Result after flip.</returns>
		public static ulong FlipDiagonal(this ulong x)
		{
			const ulong k1 = 0xaa00aa00aa00aa00;
			const ulong k2 = 0xcccc0000cccc0000;
			const ulong k4 = 0xf0f0f0f00f0f0f0f;
			ulong t = x ^ (x << 36);
			x ^= k4 & (t ^ (x >> 36));
			t = k2 & (x ^ (x << 18));
			x ^= t ^ (t >> 18);
			t = k1 & (x ^ (x << 9));
			x ^= t ^ (t >> 9);

			return x;
		}

		/// <summary>
		/// Calculates the least-significant bit set.
		/// </summary>
		/// <param name="x">64-bit integer.</param>
		/// <returns>Least-significant bit set.</returns>
		public static int LSB(this ulong x)
		{
			if (x == 0)
			{
				throw new InvalidOperationException("Can not compute LSB with 0");
			}

			var lsb_64_table = new int[]
			{
				63, 30,  3, 32, 59, 14, 11, 33,
				60, 24, 50,  9, 55, 19, 21, 34,
				61, 29,  2, 53, 51, 23, 41, 18,
				56, 28,  1, 43, 46, 27,  0, 35,
				62, 31, 58,  4,  5, 49, 54,  6,
				15, 52, 12, 40,  7, 42, 45, 16,
				25, 57, 48, 13, 10, 39,  8, 44,
				20, 47, 38, 22, 17, 37, 36, 26
			};

			x ^= x - 1;
			uint folded = (uint)x ^ (uint)(x >> 32);
			return lsb_64_table[folded * 0x78291ACF >> 26];
		}
	}
}
