using System;
using System.Collections.Generic;
using System.Text;

namespace PileLimitée
{
	public interface IPileLimitée<T>
	{
		/// <summary>
		/// Return the number of elements present inside the collection
		/// </summary>
		int NbElements { get; } 

		/// <summary>
		/// Return the maximum capacity of the collection
		/// </summary>
		int Capacity { get; } 

		/// <summary>
		/// Indicate that the collection is full
		/// </summary>
		bool Full { get; } 

		/// <summary>
		/// Indicate that the collection is empty
		/// </summary>
		bool Empty { get; } 

		/// <summary>
		/// Add a new element to the collection
		/// </summary>
		/// <param name="element">Element to add inside the collection</param>
		/// <returns>True if the operation worked</returns>
		bool Pile(T element); 

		/// <summary>
		/// Remove last added element in the collection
		/// </summary>
		/// <returns>Last added element</returns>
		T Unpile(); 

		/// <summary>
		/// Clear all the element inside the collection
		/// </summary>
		void Clear();

		/// <summary>
		/// Method that return an itterator for traversing
		/// the collections
		/// </summary>
		/// <returns></returns>
		IIterator<T> GetEnumerator();
	}
}
