namespace GridBook.Test
{
	using NUnit.Framework;

	public abstract class BaseFixture
	{
		protected virtual void OnFixtureSetup()
		{
		}

		protected virtual void OnFixtureTearDown()
		{
		}

		protected virtual void OnSetup()
		{
		}

		protected virtual void OnTearDown()
		{
		}

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			OnFixtureSetup();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			OnFixtureTearDown();
		}

		[SetUp]
		public void SetUp()
		{
			OnSetup();
		}

		[TearDown]
		public void TearDown()
		{
			OnTearDown();
		}
	}
}
