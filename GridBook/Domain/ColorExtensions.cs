namespace GridBook.Domain
{
	public static class ColorExtensions
	{
		public static Color Opponent(this Color color)
		{
			if (color == Color.White)
			{
				return Color.Black;
			}

			return Color.White;
		}

		public static string Char(this Color color)
		{
			if (color == Color.White)
			{
				return "O";
			}

			return "*";
		}
	}
}
