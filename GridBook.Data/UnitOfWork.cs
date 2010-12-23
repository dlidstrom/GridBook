namespace GridBook.Data
{
	using System;

	public static class UnitOfWork
	{
		private static IUnitOfWorkFactory factory;

		public static void Initialize(IUnitOfWorkFactory factory)
		{
			UnitOfWork.factory = factory;
		}

		public static IUnitOfWork Start()
		{
			return factory.Create();
		}

		public static IUnitOfWork Current
		{
			get;
			private set;
		}
	}
}
