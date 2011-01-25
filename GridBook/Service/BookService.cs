namespace GridBook.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Common.Logging;
	using GridBook.Domain;
	using NHibernate;
	using NHibernate.Linq;
	using GridBook.Domain.Importers;

	public class BookService
	{
		private static ILog log = LogManager.GetCurrentClassLogger();
		private ISession session;

		public BookService(ISession session)
		{
			this.session = session;
		}

		/// <summary>
		/// Adds a collection of positions to the book.
		/// </summary>
		/// <param name="positions">Collection of positions.</param>
		public void AddRangeStateless(IImporter importer)
		{
			AddRangeStateless(importer, new NullProgressBar());
		}

		public void AddRangeStateless(IImporter importer, IProgressBar progressBar)
		{
			var stateless = session.SessionFactory.OpenStatelessSession();
			Transact(stateless, () =>
			{
				int currentPosition = 1;
				progressBar.Update(0);
				foreach (var data in importer.Import())
				{
					progressBar.Update(100.0 * currentPosition / importer.Positions);
					var parent = data.Key;
					var minimalParent = parent.MinimalReflection();
					if (Find(stateless, minimalParent) == null)
					{
						log.DebugFormat("Adding {0}: {1}", currentPosition, parent);
						//foreach (var successor in parent.CalculateMinimalSuccessors())
						//{
						//    var minimalSuccessor = successor.MinimalReflection();
						//    minimalSuccessor = Find(minimalSuccessor) ?? minimalSuccessor;
						//    minimalSuccessor.AddParent(minimalParent);
						//    minimalParent.AddSuccessor(minimalSuccessor);
						//}

						stateless.Insert(parent);
					}
					else
					{
						log.InfoFormat("Existing {0}: {1}", currentPosition, parent);
					}

					currentPosition++;
				}
			});
		}

		public void AddRangeStatefull(IImporter importer, IProgressBar progressBar)
		{
			Transact(() =>
			{
				var start = Board.Start;

				int currentPosition = 1;
				progressBar.Update(0);
				foreach (var data in importer.Import())
				{
					progressBar.Update(100.0 * currentPosition / importer.Positions);
					var parent = data.Key;
					var minimalParent = parent.MinimalReflection();
					//if (Find(minimalParent) == null)
					{
						log.DebugFormat("Adding {0}: {1}", currentPosition, parent);
						minimalParent.AddParent(start);
						start.AddSuccessor(minimalParent);

						//session.Save(minimalParent);
						//foreach (var successor in parent.CalculateMinimalSuccessors())
						//{
						//    var minimalSuccessor = successor.MinimalReflection();
						//    minimalSuccessor = Find(minimalSuccessor) ?? minimalSuccessor;
						//    minimalSuccessor.AddParent(minimalParent);
						//    minimalParent.AddSuccessor(minimalSuccessor);
						//    session.SaveOrUpdate(minimalSuccessor);
						//}

						//session.Save(parent);
					}
					//else
					//{
					//    log.InfoFormat("Existing {0}: {1}", currentPosition, parent);
					//}

					currentPosition++;
					if (currentPosition > 10)
					{
						break;
					}

					session.Save(start);
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
				// get minimal reflection from store, if it exists
				var minimalParent = parent.MinimalReflection();
				minimalParent = Find(minimalParent) ?? minimalParent;

				foreach (var successor in parent.CalculateMinimalSuccessors())
				{
					var minimalSuccessor = successor.MinimalReflection();
					minimalSuccessor = Find(minimalSuccessor) ?? minimalSuccessor;
					minimalSuccessor.AddParent(minimalParent);
					minimalParent.AddSuccessor(minimalSuccessor);
				}

				session.Save(minimalParent);
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

		public Board Find(IStatelessSession session, Board item)
		{
			return Transact(() => session.CreateQuery("select b from Board b where b.Empty = :empty and b.Mover = :mover")
				.SetInt64("empty", item.Empty)
				.SetInt64("mover", item.Mover)
				.UniqueResult<Board>());
		}

		/// <summary>
		/// Applies function within current transaction. Will start new transaction if there is none.
		/// </summary>
		/// <typeparam name="TResult">Result type of function.</typeparam>
		/// <param name="session">Stateless session.</param>
		/// <param name="func">Function that will be applied.</param>
		/// <returns>Result of function.</returns>
		private TResult Transact<TResult>(IStatelessSession session, Func<TResult> func)
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

		private void Transact(IStatelessSession session, Action action)
		{
			Transact<bool>(session, () =>
			{
				action.Invoke();
				return false;
			});
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
