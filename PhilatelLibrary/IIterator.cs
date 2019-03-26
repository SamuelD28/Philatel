using System;
using System.Collections.Generic;
using System.Text;

namespace PileLimitée
{
	public interface IIterator<T>
	{
		/// <summary>
		/// Return the next element in the collection and 
		/// place the cursor at the next position
		/// </summary>
		/// <returns>Next element in the collection</returns>
		bool MoveNext();

		/// <summary>
		/// Reset the cursor position
		/// </summary>
		void Reset();

		int CursorPosition { get; }

		/// <summary>
		/// Return the current cursor item
		/// </summary>
		T Current { get; }
	}
}
