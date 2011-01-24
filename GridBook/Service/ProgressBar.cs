namespace GridBook.Service
{
	using System;
	using System.Globalization;

	public interface IProgressBar
	{
		void Update(double percent);
	}

	/// <summary>
	/// Class to create a console progress bar
	/// </summary>
	public class ProgressBar : IProgressBar
	{
		/// <summary>
		/// The last output
		/// </summary>
		private string lastOutput = string.Empty;

		/// <summary>
		/// The maximum length of the progress bar
		/// </summary>
		private int maximumWidth;

		/// <summary>
		/// Initializes a new instance of the ProgressBar class.
		/// </summary>
		public ProgressBar()
			: this(100)
		{
		}

		/// <summary>
		/// Initializes a new instance of the ProgressBar class.
		/// </summary>
		/// <param name="maximumWidth">The maximum width of the progress bar.</param>
		public ProgressBar(int maximumWidth)
		{
			this.maximumWidth = Math.Min(maximumWidth, 8 * Console.WindowWidth / 10);
			this.Show(" [ ");
		}

		/// <summary>
		/// Updates the progress bar with the specified percent.
		/// </summary>
		/// <param name="percent">The percent.</param>
		public void Update(double percent)
		{
			// Generate new state
			int width = (int)(percent / 100 * this.maximumWidth);
			int fill = this.maximumWidth - width;
			string output = string.Format("{0}{1} ] {2}%",
				string.Empty.PadLeft(width, '#'),
				string.Empty.PadLeft(fill, ' '),
				percent.ToString("0.0", CultureInfo.InvariantCulture.NumberFormat));
			if (this.lastOutput != output)
			{
				// Remove the last state
				string clear = string.Empty.PadRight(this.lastOutput.Length, '\b');
				this.Show(clear);
				this.Show(output);
				this.lastOutput = output;
			}
		}

		/// <summary>
		/// Shows the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		private void Show(string value)
		{
			Console.Write(value);
		}
	}

	public class NullProgressBar : IProgressBar
	{
		public void Update(double percent)
		{
		}
	}
}
