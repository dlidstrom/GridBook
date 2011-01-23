namespace GridBook.Test
{
	using System.Data;
	using NHibernate.Connection;

	public class TestConnectionProvider : DriverConnectionProvider
	{
		private static IDbConnection connection;

		public override IDbConnection GetConnection()
		{
			if (connection == null)
			{
				connection = base.GetConnection();
			}

			return connection;
		}

		public override void CloseConnection(IDbConnection conn)
		{
		}

		public static void CloseDatabase()
		{
			if (connection != null)
			{
				connection.Close();
				connection = null;
			}
		}
	}
}
