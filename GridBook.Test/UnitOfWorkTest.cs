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
			var work = new Mock<IUnitOfWork>();
			factory.Setup(f => f.Create()).Returns(work.Object);
			UnitOfWork.Initialize(factory.Object);

			// Act
			var w = UnitOfWork.Start();

			// Assert
			Assert.AreSame(work.Object, w);
			factory.VerifyAll();
		}
	}
}
