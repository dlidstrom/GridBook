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
	}
}
