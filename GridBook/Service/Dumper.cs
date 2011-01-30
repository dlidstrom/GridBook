namespace GridBook.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;

	public class Dumper
	{
		private string p;

		public Dumper(string p)
		{
			// TODO: Complete member initialization
			this.p = p;
		}

		public void WriteHeading(Stream stream)
		{
			using (var writer = new StreamWriter(stream))
			{
				writer.WriteLine("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
				writer.WriteLine("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
				writer.WriteLine("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
				writer.WriteLine("/*!40101 SET NAMES utf8 */;");
				writer.WriteLine("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
				writer.WriteLine("/*!40103 SET TIME_ZONE='+00:00' */;");
				writer.WriteLine("/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;");
				writer.WriteLine("/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;");
				writer.WriteLine("/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;");
				writer.WriteLine("/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;");
			}
		}

		public void WriteBoardTable(Stream stream)
		{
			using (var writer = new StreamWriter(stream))
			{
				writer.WriteLine("DROP TABLE IF EXISTS `board`;");
				writer.WriteLine("/*!40101 SET @saved_cs_client     = @@character_set_client */;");
				writer.WriteLine("/*!40101 SET character_set_client = utf8 */;");
				writer.WriteLine("CREATE TABLE `board` (");
				writer.WriteLine("  `Id` varchar(40) NOT NULL,");
				writer.WriteLine("  `Empty` bigint(20) DEFAULT NULL,");
				writer.WriteLine("  `Mover` bigint(20) DEFAULT NULL,");
				writer.WriteLine("  `Ply` int(11) NOT NULL,");
				writer.WriteLine("  PRIMARY KEY (`Id`),");
				writer.WriteLine("  UNIQUE KEY `Empty` (`Empty`,`Mover`)");
				writer.WriteLine(") ENGINE=InnoDB DEFAULT CHARSET=latin1;");
				writer.WriteLine("/*!40101 SET character_set_client = @saved_cs_client */;");
			}
		}
	}
}
