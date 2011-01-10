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

		public static ulong FlipVertical(this long l)
		{
			//int i;
			//u1 temp;

			//for (i = 0; i < 4; i++)
			//{
			//    temp = u1s[i];
			//    u1s[i] = u1s[7 - i];
			//    u1s[7 - i] = temp;
			//}

			return l.ToUInt64();
		}

		public static ulong FlipDiagonal(this long l)
		{
			//const u4 dflipmask4L = 0xF0F0F0F0;
			//const u4 dflipmask4R = 0x0F0F0F0F;
			//const u4 dflipmask2L = 0x0000CCCC;
			//const u4 dflipmask2R = 0x33330000;
			//const u4 dflipmask1L = 0x00AA00AA;
			//const u4 dflipmask1R = 0x55005500;

			//void CBitBoardBlock::FlipDiagonal32(u4& rows) {
			//   u4 templ, tempr;

			//   templ=dflipmask2L&rows;
			//   tempr=dflipmask2R&rows;
			//   rows^=(templ|tempr);
			//   rows|=(templ<<14)|(tempr>>14);

			//   templ=dflipmask1L&rows;
			//   tempr=dflipmask1R&rows;
			//   rows^=(templ|tempr);
			//   rows|=(templ<<7)|(tempr>>7);
			//}

			//u4 templ, tempr;

			//templ = dflipmask4L & u4s[0];
			//tempr = dflipmask4R & u4s[1];
			//u4s[0] ^= templ;
			//u4s[1] ^= tempr;
			//u4s[0] |= tempr << 4;
			//u4s[1] |= templ >> 4;
			//FlipDiagonal32(u4s[0]);
			//FlipDiagonal32(u4s[1]);
			return l.ToUInt64();
		}

		public static ulong FlipHorizontal(this long l)
		{
			//const u4 hflipmask4 = 0x0F0F0F0F;
			//const u4 hflipmask2 = 0x33333333;
			//const u4 hflipmask1 = 0x55555555;

			//u4 templ, tempr;

			//templ = rows & hflipmask4;
			//tempr = rows & ~hflipmask4;
			//rows = (templ << 4) | (tempr >> 4);

			//templ = rows & hflipmask2;
			//tempr = rows & ~hflipmask2;
			//rows = (templ << 2) | (tempr >> 2);

			//templ = rows & hflipmask1;
			//tempr = rows & ~hflipmask1;
			//rows = (templ << 1) | (tempr >> 1);

			//FlipHorizontal32(u4s[0]);
			//FlipHorizontal32(u4s[1]);

			return l.ToUInt64();
		}
	}
}
