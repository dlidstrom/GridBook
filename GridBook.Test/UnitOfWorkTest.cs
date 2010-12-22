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
			var factory = new Mock<IUnitOfWorkFactory>();
			UnitOfWork.Initialize(factory.Object);

			// Act
			IUnitOfWork work = UnitOfWork.Start();

			// Assert
			factory.Verify();
		}
	}
}
