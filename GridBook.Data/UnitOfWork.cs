namespace GridBook.Data
{
	using System;

	public static class UnitOfWork
	{
		private static IUnitOfWorkFactory factory;
		private static IUnitOfWork innerUnitOfWork;

		public static void Initialize(IUnitOfWorkFactory factory)
		{
			UnitOfWork.factory = factory;
		}

		public static IUnitOfWork Start()
		{
			if (innerUnitOfWork != null)
			{
				throw new InvalidOperationException("You can only have one unit of work at the same time.");
			}

			innerUnitOfWork = factory.Create();
			return innerUnitOfWork;
		}

		public static IUnitOfWork Current
		{
			get;
			private set;
		}
	}
}
