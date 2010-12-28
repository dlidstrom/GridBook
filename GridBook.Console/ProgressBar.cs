namespace GridBook.Console
{
	using System;

	/// <summary>
	/// Class to create a console progress bar
	/// </summary>
	public class ProgressBar
	{
		/// <summary>
		/// The length of the last output
		/// </summary>
		private int lastOutputLength;

		/// <summary>
		/// The maximum length of the progress bar
		/// </summary>
		private int maximumWidth;

		/// <summary>
		/// Initializes a new instance of the ProgressBar class.
		/// </summary>
		/// <param name="maximumWidth">The maximum width of the progress bar.</param>
		public ProgressBar(int maximumWidth)
		{
			this.maximumWidth = maximumWidth;
			this.Show(" [ ");
		}

		/// <summary>
		/// Updates the progress bar with the secified percent.
		/// </summary>
		/// <param name="percent">The percent.</param>
		public void Update(double percent)
		{
			// Remove the last state
			string clear = string.Empty.PadRight(
				this.lastOutputLength,
				'\b');
			this.Show(clear);

			// Generate new state
			int width = (int)(percent / 100 * this.maximumWidth);
			int fill = this.maximumWidth - width;
			string output = string.Format(
				"{0}{1} ] {2}%",
				string.Empty.PadLeft(width, '='),
				string.Empty.PadLeft(fill, ' '),
				percent.ToString("0.0"));
			this.Show(output);
			this.lastOutputLength = output.Length;
		}

		/// <summary>
		/// Shows the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		private void Show(string value)
		{
			System.Console.Write(value);
		}
	}
}
