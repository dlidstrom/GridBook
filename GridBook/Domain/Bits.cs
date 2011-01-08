namespace GridBook.Domain
{
	using System.Linq;

	public static class Bits
	{
		private static int[] bitcount = Enumerable.Repeat(0, ushort.MaxValue + 1).ToArray();

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
		/// Calculates the number of bits in a 64-bit long.
		/// </summary>
		/// <param name="l"></param>
		/// <returns></returns>
		public static int Count(long l)
		{
			return bitcount[(ushort)l]
				+ bitcount[(ushort)(l >> 16)]
				+ bitcount[(ushort)(l >> 32)]
				+ bitcount[(ushort)(l >> 48)];
		}
	}
}
