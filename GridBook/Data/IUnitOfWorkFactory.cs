namespace GridBook.Data
{
	using System;

	public interface IUnitOfWorkFactory
	{
		IUnitOfWork Create();
	}
}
