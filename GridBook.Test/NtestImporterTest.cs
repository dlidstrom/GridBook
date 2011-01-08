namespace GridBook.Test
{
	using System.Collections.Generic;
	using System.Linq;
	using GridBook.Domain;
	using GridBook.Domain.Importers;
	using NUnit.Framework;
	using System;

	[TestFixture, Unit]
	public class NtestImporterTest
	{
		private IImporter importer;
		private List<KeyValuePair<Board, BookData>> entries;

		[SetUp]
		public void SetUp()
		{
			importer = new NtestImporter("Data/JA_s12.book");
			entries = new List<KeyValuePair<Board, BookData>>(importer.Import());
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
			Assert.AreEqual(91, entries.Count);
		}

		[Test]
		public void VerifyOpeningPositionValues()
		{
			var entry = (from bd in entries
						 where bd.Key.Empty == 18446743970226896895.ToInt64() && bd.Key.Mover==34628698112
						 select bd.Value).Single();
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
