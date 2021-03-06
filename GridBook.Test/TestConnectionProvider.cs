﻿namespace GridBook.Test
{
	using System.Data;
	using NHibernate.Connection;
	using System;

	public class TestConnectionProvider : DriverConnectionProvider
	{
		[ThreadStatic]
		private static IDbConnection connection;

		public override IDbConnection GetConnection()
		{
			if (connection == null)
			{
				connection = Driver.CreateConnection();
				connection.ConnectionString = ConnectionString;
				connection.Open();
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
