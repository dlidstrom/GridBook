namespace GridBook.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public static class Bits
	{
		private static int[] bitcount = Enumerable.Repeat(0, ushort.MaxValue + 1).ToArray();

		static Bits()
		{
			for (int i = 0; i <= ushort.MaxValue; i++)
			{
				bitcount[i] = count(i);
			}
		}

		public static int Count(ulong l)
		{
			return bitcount[(ushort)l]
				+ bitcount[(ushort)(l >> 16)]
				+ bitcount[(ushort)(l >> 32)]
				+ bitcount[(ushort)(l >> 48)];
		}

		private static int count(int i)
		{
			int c = 0;
			while (i != 0)
			{
				i = i & (i - 1);
				c++;
			}

			return c;
		}
	}
}
