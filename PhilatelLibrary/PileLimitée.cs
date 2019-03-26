using System;
using System.Collections;
using System.Collections.Generic;

namespace PileLimitée
{
	public class PileLimitée<T> : IPileLimitée<T>, IList<T>
	{
		private T[] Elements;

		public PileLimitée()
		{
			NbElements = 0;
			Capacity = 100;
			Elements = new T[Capacity];
		}

		public PileLimitée(int capacity)
		{
			if (capacity <= 0)
				throw new ArgumentOutOfRangeException();

			NbElements = 0;
			Capacity = capacity;
			Elements = new T[Capacity];
		}

		public int NbElements { get; private set; }

		public int Capacity { get; private set; }

		public bool Full => NbElements == Capacity;

		public bool Empty => NbElements == 0;

		public int Count => throw new NotImplementedException();

		public bool IsReadOnly => throw new NotImplementedException();

		public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Clear()
		{
			NbElements = 0;
			Elements = new T[Capacity];
		}

		public IIterator<T> GetEnumerator() => new PileLimitéeReverseIterator(this);

		public bool Pile(T element)
		{
			if(!Full)
			{
				Elements[NbElements] = element;
				NbElements++;
				return false;
			}
			else
			{
				Array.Copy(Elements, 1, Elements, 0, NbElements -1);
				Elements[NbElements -1] = element;
				return true;
			}
		}

		public T Unpile()
		{
			if (Empty)
				throw new InvalidOperationException();

			T last = Elements[NbElements - 1];
			Elements[NbElements - 1] = default(T);
			NbElements--;
			return last;
		}

		public IEnumerable<T> ToArray()
		{
			T[] temp = new T[NbElements];
			int position = 0;
			foreach(T item in this)
			{
				temp[position] = item;
				position++;
			}
			return temp;
		}

		public int IndexOf(T item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, T item)
		{
			if (index < Elements.Length)
				Elements[index] = item;
			else
				throw new IndexOutOfRangeException();
		}

		public void RemoveAt(int index)
		{
			if (index < Elements.Length)
				Elements[index] = default(T);
			else
				throw new IndexOutOfRangeException();
		}

		public void Add(T item) => Pile(item);

		public bool Contains(T item) => Array.IndexOf(Elements, item) != -1;

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			int index =Array.IndexOf( Elements,item);

			if (index != -1)
			{
				Elements[index] = default(T);
				return true;
			}
			return false;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Elements.GetEnumerator();
		}

		private class PileLimitéeIterator : IIterator<T>
		{
			private PileLimitée<T> PileLimitée;

			public int CursorPosition { get; private set; }

			public PileLimitéeIterator(PileLimitée<T> pileLimitée)
			{
				PileLimitée = pileLimitée;
				CursorPosition = 0;
			}

			public T Current => PileLimitée.Elements[CursorPosition];

			public bool MoveNext()
			{
				if (CursorPosition + 1 > PileLimitée.Capacity)
				{
					Reset();
					return false;
				}
				else
				{
					CursorPosition++;
					return true;
				}
			}

			public void Reset()
			{
				CursorPosition = 0;
			}
		}

		private class PileLimitéeReverseIterator : IIterator<T>
		{
			private PileLimitée<T> PileLimitée;

			public int CursorPosition { get; private set; }

			public PileLimitéeReverseIterator(PileLimitée<T> pileLimitée)
			{
				PileLimitée = pileLimitée;
				CursorPosition = PileLimitée.NbElements;
			}

			public T Current => PileLimitée.Elements[CursorPosition];

			public bool MoveNext()
			{
				if (CursorPosition -1 < 0)
				{
					Reset();
					return false;
				}
				else
				{
					CursorPosition--;
					return true;
				}
			}

			public void Reset()
			{
				CursorPosition = PileLimitée.NbElements;
			}
		}
	}
}
