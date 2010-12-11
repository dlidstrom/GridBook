namespace GridBook.Test
{
	using NUnit.Framework;

	[TestFixture]
	public class NtestImporterTest
	{
		private NtestImporter importer;

		[SetUp]
		public void SetUp()
		{
			importer = new NtestImporter("Data/JA_s12.book");
		}

		[Test]
		public void VersionIsOne()
		{
			Assert.AreEqual(1, importer.Version);
		}

		[Test]
		public void CanReadNumberOfPositions()
		{
			Assert.AreEqual(91, importer.Positions);
		}

		[Test]
		public void ReadsAllEntries()
		{
			Assert.AreEqual(91, importer.Entries.Count);
		}

		[Test]
		public void VerifyOpeningPositionValues()
		{
			var entry = importer.Entries[new Board(18446743970226896895, 34628698112)];
			Assert.AreEqual(12, entry.Height);
			Assert.AreEqual(4, entry.Prune);
			Assert.IsFalse(entry.WLD);
			Assert.IsFalse(entry.KnownSolve);
			Assert.AreEqual(-354, entry.Cutoff);
			Assert.AreEqual(-39, entry.HeuristicValue);
			Assert.AreEqual(-39, entry.BlackValue);
			Assert.AreEqual(-39, entry.WhiteValue);
			Assert.AreEqual(1, entry.Games[0]);
			Assert.AreEqual(0, entry.Games[1]);
		}
	}
}
