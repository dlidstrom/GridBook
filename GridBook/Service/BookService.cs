namespace GridBook.Service
{
	using System;
	using System.Linq;
	using Common.Logging;
	using GridBook.Domain;
	using GridBook.Domain.Importers;
	using NHibernate;
	using NHibernate.Linq;

	public class BookService
	{
		/// <summary>
		/// Class logger.
		/// </summary>
		private static ILog log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Database session.
		/// </summary>
		private ISession session;

		/// <summary>
		/// Initializes a new instance of the BookService class
		/// with an open session.
		/// </summary>
		/// <param name="session">Database session.</param>
		public BookService(ISession session)
		{
			this.session = session;
		}

		/// <summary>
		/// Add a range of positions to the store. This will also add all successors,
		/// if they have not already been added. It will also create parent-child relationships.
		/// </summary>
		/// <param name="importer">Importer with new positions.</param>
		/// <param name="progressBar">Progress bar.</param>
		public void AddRange(IImporter importer, IProgressBar progressBar)
		{
			Transact(() =>
			{
				int currentPosition = 1;
				progressBar.Update(0);
				foreach (var data in importer.Import())
				{
					progressBar.Update(100.0 * currentPosition++ / importer.Positions);
					var parent = data.Key;
					Add(parent);
				}
			});
		}

		/// <summary>
		/// Adds a parent position to the store. This will also add all successors, if
		/// they have not already been added. It will also create parent-child relationship.
		/// </summary>
		/// <param name="parent">Parent position to add to store.</param>
		public void Add(Board parent)
		{
			Transact(() =>
			{
				var minimalParent = parent.MinimalReflection();
				// only add if not already in store
				if (Find(minimalParent) == null)
				{
					foreach (var successor in parent.CalculateMinimalSuccessors())
					{
						var minimalSuccessor = successor.MinimalReflection();
						// add successor from store, if it exists
						minimalSuccessor = Find(minimalSuccessor) ?? minimalSuccessor;
						minimalParent.AddSuccessor(minimalSuccessor);
						minimalSuccessor.AddParent(minimalParent);
					}

					session.Save(minimalParent);
				}
			});
		}

		/// <summary>
		/// Returns a value indicating whether the item exists in store.
		/// </summary>
		/// <param name="item">Item to look for.</param>
		/// <returns>True if item is in store, false othewise.</returns>
		public bool Contains(Board item)
		{
			return Find(item) != null;
		}

		/// <summary>
		/// Finds an existing board or returns null if there was none.
		/// </summary>
		/// <param name="item">Item to find.</param>
		/// <returns>Stored instance or null if there was none.</returns>
		public Board Find(Board item)
		{
			return Transact(() => from b in session.Query<Board>()
								  where b.Empty == item.Empty && b.Mover == item.Mover
								  select b).SingleOrDefault();
		}

		/// <summary>
		/// Applies function within current transaction. Will start new transaction if there is none.
		/// </summary>
		/// <typeparam name="TResult">Result type of function.</typeparam>
		/// <param name="func">Function that will be applied.</param>
		/// <returns>Result of function.</returns>
		private TResult Transact<TResult>(Func<TResult> func)
		{
			if (session.Transaction.IsActive)
			{
				return func.Invoke();
			}

			TResult result = default(TResult);
			using (var tx = session.BeginTransaction())
			{
				result = func.Invoke();
				tx.Commit();
			}

			return result;
		}

		/// <summary>
		/// Applies action within current transaction. Will start new transaction if there is none.
		/// </summary>
		/// <param name="action">Action that will be applied.</param>
		private void Transact(Action action)
		{
			Transact<bool>(() =>
			{
				action.Invoke();
				return false;
			});
		}
	}
}
