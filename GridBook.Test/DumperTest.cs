namespace GridBook.Test
{
	using System;
	using System.IO;
	using GridBook.Service;
	using NUnit.Framework;
	using System.Text;

	[TestFixture]
	public class DumperTest
	{
		private Dumper dumper;

		[SetUp]
		public void Setup()
		{
			// Arrange
			dumper = new Dumper("Data/JA_s12.book");
		}

		[Test]
		public void DumperCanGenerateHeader()
		{
			// Act
			using (var stream = new MemoryStream())
			{
				dumper.WriteHeading(stream);
				// Assert
				var result = Encoding.Default.GetString(stream.GetBuffer());
				var heading = "/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;\r\n"
					+ "/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;\r\n"
					+ "/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;\r\n"
					+ "/*!40101 SET NAMES utf8 */;\r\n"
					+ "/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;\r\n"
					+ "/*!40103 SET TIME_ZONE='+00:00' */;\r\n"
					+ "/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;\r\n"
					+ "/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;\r\n"
					+ "/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;\r\n"
					+ "/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;\r\n";
				Assert.AreEqual(heading, result);
			}
		}

		[Test]
		public void DumperCanGenerateBoardTable()
		{
			// Act
			using (var stream = new MemoryStream())
			{
				dumper.WriteBoardTable(stream);
				// Assert
				var result = Encoding.Default.GetString(stream.GetBuffer());
				var table = "DROP TABLE IF EXISTS `board`;\r\n"
					+ "/*!40101 SET @saved_cs_client     = @@character_set_client */;\r\n"
					+ "/*!40101 SET character_set_client = utf8 */;\r\n"
					+ "CREATE TABLE `board` (\r\n"
					+ "  `Id` varchar(40) NOT NULL,\r\n"
					+ "  `Empty` bigint(20) DEFAULT NULL,\r\n"
					+ "  `Mover` bigint(20) DEFAULT NULL,\r\n"
					+ "  `Ply` int(11) NOT NULL,\r\n"
					+ "  PRIMARY KEY (`Id`),\r\n"
					+ "  UNIQUE KEY `Empty` (`Empty`,`Mover`)\r\n"
					+ ") ENGINE=InnoDB DEFAULT CHARSET=latin1;\r\n"
					+ "/*!40101 SET character_set_client = @saved_cs_client */;\r\n";
				Assert.AreEqual(table, result);
			}
		}
	}
}
