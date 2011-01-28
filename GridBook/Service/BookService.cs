namespace GridBook.Service
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Common.Logging;
	using GridBook.Domain;
	using GridBook.Domain.Importers;
	using NHibernate;
	using NHibernate.Linq;
	using System.Collections.Generic;

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
		public void AddRange(IImporter importer, int take)
		{
			int currentPosition = 0;
			log.Info("Reading positions");
			var list = (from kvp in importer.Import()
						select kvp.Key).ToList();
			log.Info("Adding positions");
			for (int i = 0; i < list.Count; i += take)
			{
					Transact(() =>
					{
						for (int j = 0; j < take && i + j < list.Count; j++)
						{
							// only flush session when we commit, this will improve performance
							// for this to work we need to keep track of entities manually, using a C5.HashSet.
							// this is to avoid trying to insert the same entity twice, resulting in a constraint violation
							session.FlushMode = FlushMode.Commit;
							var set = new C5.HashSet<Board>();

							var parent = list[i + j].MinimalReflection();
							if (Find(parent) == null)
							{
								// could be in session, check cache
								set.FindOrAdd(ref parent);

								// for each successor...
								foreach (var successor in parent.CalculateSuccessors())
								{
									var minimalSuccessor = successor.MinimalReflection();
									if (!set.Find(ref minimalSuccessor))
									{
										// not in cache, check store
										minimalSuccessor = Find(minimalSuccessor) ?? minimalSuccessor;
										set.Add(minimalSuccessor);
									}

									parent.AddSuccessor(minimalSuccessor);
									minimalSuccessor.AddParent(parent);
								}

								session.Save(parent);
							}

							currentPosition++;
						}
					});

					double done = 100.0 * currentPosition / importer.Positions;
					log.InfoFormat(CultureInfo.InvariantCulture.NumberFormat, "{0,5:F1}% done ({1}/{2})", done, currentPosition, importer.Positions);
			}
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
