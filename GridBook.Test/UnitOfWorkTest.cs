namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using Moq;
	using GridBook.Data;

	[TestFixture]
	public class UnitOfWorkTest
	{
		[Test]
		public void CanStartUnitOfWork()
		{
			// Arrange
			var work = Mock.Of<IUnitOfWork>();
			var factory = Mock.Of<IUnitOfWorkFactory>(f => f.Create() == work);

			// Act
			UnitOfWork.Initialize(factory);

			// Assert
			Assert.AreSame(work, UnitOfWork.Start());
		}

		[Test]
		public void StartingWhenStartedThrows()
		{
			// Arrange
			var work = Mock.Of<IUnitOfWork>();
			var factory = Mock.Of<IUnitOfWorkFactory>(f => f.Create() == work);
			UnitOfWork.Initialize(factory);
			UnitOfWork.Start();

			try
			{
				// Act
				UnitOfWork.Start();
				// Assert
				Assert.Fail("Should fail when starting a started unit of work.");
			}
			catch (InvalidOperationException)
			{
			}
		}
	}
}
