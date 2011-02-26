﻿namespace GridBook.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Entry : Entity
	{
		private readonly Board board;
		private readonly int depth;
		private readonly int percent;

		protected Entry()
		{
		}

		public Entry(Board board, int depth, int percent)
		{
			this.board = board;
			this.depth = depth;
			this.percent = percent;
		}

		public virtual Board Board
		{
			get
			{
				return board;
			}
		}

		public virtual int Depth
		{
			get
			{
				return depth;
			}
		}

		public virtual int Percent
		{
			get
			{
				return percent;
			}
		}
	}
}