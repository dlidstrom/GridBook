namespace GridBook.Service
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using GridBook.Domain.Importers;

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

		public void WritePositions(Stream stream, IImporter importer)
		{
			using (var writer = new StreamWriter(stream))
			{
				writer.WriteLine("LOCK TABLES `board` WRITE;");
				writer.WriteLine("/*!40000 ALTER TABLE `board` DISABLE KEYS */;");
				writer.WriteLine("INSERT INTO `board` VALUES ");
				var parents = new HashSet<Guid>();
				var successors = new HashSet<Guid>();
				bool first = true;

				// for each ply
				int totalTicks = Environment.TickCount;
				for (int ply = 0; ply <= 60; ply++)
				{
					var ticks = Environment.TickCount;
					Console.Error.Write("Ply {0,2}", ply);
					// for each position, add its successors to set
					var q = from kvp in importer.Import()
							where kvp.Key.Ply == ply
							select kvp.Key;
					Parallel.ForEach(q, pos =>
					{
						var bytes = new List<Byte>(BitConverter.GetBytes(pos.Empty));
						bytes.AddRange(BitConverter.GetBytes(pos.Mover));
						lock (parents)
							parents.Add(new Guid(bytes.ToArray()));
						foreach (var successor in pos.CalculateMinimalSuccessors())
						{
							bytes = new List<Byte>(BitConverter.GetBytes(successor.Empty));
							bytes.AddRange(BitConverter.GetBytes(successor.Mover));
							lock (successors)
								successors.Add(new Guid(bytes.ToArray()));
						}
					});

					// print sql-create statements
					writeSql(writer, parents, ply, ref first);

					// for next ply
					parents = successors;
					successors = new HashSet<Guid>();

					// done
					var seconds = (double)(Environment.TickCount - totalTicks) / 1000;
					var done = string.Format(CultureInfo.InvariantCulture, " {0,14}", TimeSpan.FromSeconds((int)seconds));
					// left
					var lastSeconds = ((double)(Environment.TickCount - ticks)) / 1000;
					var secondsLeft = lastSeconds * (60 - ply);
					var left = string.Format(CultureInfo.InvariantCulture, " {0,14}", TimeSpan.FromSeconds((int)secondsLeft));
					Console.Error.WriteLine("{0}{1}", done, left);
				}

				writer.WriteLine(";");
				writer.WriteLine("/*!40000 ALTER TABLE `board` ENABLE KEYS */;");
				writer.WriteLine("UNLOCK TABLES;");
			}
		}

		public void WriteFooter(Stream stream)
		{
			using (var writer = new StreamWriter(stream))
			{
				writer.WriteLine("/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;");
				writer.WriteLine("/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;");
				writer.WriteLine("/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;");
				writer.WriteLine("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
				writer.WriteLine("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
				writer.WriteLine("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
				writer.WriteLine("/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;");
			}
		}

		private static void writeSql(StreamWriter writer, HashSet<Guid> set, int ply, ref bool first)
		{
			if (set.Count > 0)
			{
				if (!first)
				{
					writer.WriteLine(",");
				}

				first = false;

				// move set to list so that we can sort it
				var list = new List<Guid>();
				while (set.Count > 0)
				{
					var g = set.First();
					list.Add(g);
					set.Remove(g);
				}

				list.Sort();
				int count = list.Count;
				foreach (var item in list)
				{
					var bytes = item.ToByteArray();
					var empty = BitConverter.ToInt64(bytes, 0);
					var mover = BitConverter.ToInt64(bytes, 8);
					writer.Write("('{0}',{1},{2},{3})", item, empty, mover, ply);
					if (--count > 0)
					{
						writer.WriteLine(",");
					}
				}
			}
		}
	}
}
