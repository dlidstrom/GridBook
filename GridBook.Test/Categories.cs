namespace GridBook.Test
{
	using System;
	using NUnit.Framework;

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false)]
	public class UnitAttribute : CategoryAttribute
	{
		public UnitAttribute()
			: base("Unit")
		{
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class DatabaseAttribute : CategoryAttribute
	{
		public DatabaseAttribute()
			: base("Database")
		{
		}
	}
}
