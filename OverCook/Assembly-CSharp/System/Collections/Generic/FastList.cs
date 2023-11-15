using System;

namespace System.Collections.Generic
{
	// Token: 0x0200041D RID: 1053
	[Serializable]
	public class FastList<T>
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x0006A067 File Offset: 0x00068467
		public FastList()
		{
			this._items = FastList<T>._emptyArray;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0006A07A File Offset: 0x0006847A
		public FastList(int capacity)
		{
			if (capacity < 0)
			{
				capacity = 0;
			}
			if (capacity == 0)
			{
				this._items = FastList<T>._emptyArray;
			}
			else
			{
				this._items = new T[capacity];
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0006A0B0 File Offset: 0x000684B0
		public FastList(IEnumerable<T> collection)
		{
			ICollection<T> collection2 = null;
			if (collection != null)
			{
				collection2 = (collection as ICollection<T>);
			}
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count == 0)
				{
					this._items = FastList<T>._emptyArray;
				}
				else
				{
					this._items = new T[count];
					collection2.CopyTo(this._items, 0);
					this._size = count;
				}
			}
			else
			{
				this._size = 0;
				this._items = FastList<T>._emptyArray;
				if (collection != null)
				{
					foreach (T item in collection)
					{
						this.Add(item);
					}
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0006A178 File Offset: 0x00068578
		// (set) Token: 0x060012EC RID: 4844 RVA: 0x0006A184 File Offset: 0x00068584
		public int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value < this._size)
				{
					value = this._size;
				}
				if (value != this._items.Length)
				{
					if (value > 0)
					{
						T[] array = new T[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
					}
					else
					{
						this._items = FastList<T>._emptyArray;
					}
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0006A1F8 File Offset: 0x000685F8
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0006A200 File Offset: 0x00068600
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && default(T) == null);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0006A238 File Offset: 0x00068638
		public void Add(T item)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size++] = item;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0006A283 File Offset: 0x00068683
		public void AddRange(IEnumerable<T> collection)
		{
			this.InsertRange(this._size, collection);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0006A292 File Offset: 0x00068692
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0)
			{
				index = 0;
			}
			if (count < 0)
			{
				count = 0;
			}
			if (this._size - index < count)
			{
				return -1;
			}
			return Array.BinarySearch<T>(this._items, index, count, item, comparer);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006A2C8 File Offset: 0x000686C8
		public int BinarySearch(T item)
		{
			return this.BinarySearch(0, this.Count, item, null);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006A2D9 File Offset: 0x000686D9
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return this.BinarySearch(0, this.Count, item, comparer);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006A2EA File Offset: 0x000686EA
		public void Clear()
		{
			if (this._size > 0)
			{
				Array.Clear(this._items, 0, this._size);
				this._size = 0;
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0006A311 File Offset: 0x00068711
		public bool Contains(T item)
		{
			return this._size != 0 && this.IndexOf(item) != -1;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0006A32E File Offset: 0x0006872E
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0006A338 File Offset: 0x00068738
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0006A359 File Offset: 0x00068759
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0006A370 File Offset: 0x00068770
		private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int num = (this._items.Length != 0) ? (this._items.Length * 2) : 4;
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0006A3B9 File Offset: 0x000687B9
		public bool Exists(Predicate<T> match)
		{
			return this.FindIndex(match) != -1;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0006A3C8 File Offset: 0x000687C8
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
			}
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0006A420 File Offset: 0x00068820
		public FastList<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
			}
			FastList<T> fastList = new FastList<T>();
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					fastList.Add(this._items[i]);
				}
			}
			return fastList;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0006A47A File Offset: 0x0006887A
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this._size, match);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0006A48A File Offset: 0x0006888A
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this._size - startIndex, match);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0006A49C File Offset: 0x0006889C
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this._size)
			{
			}
			if (count < 0 || startIndex > this._size - count)
			{
			}
			if (match == null)
			{
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0006A500 File Offset: 0x00068900
		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
			}
			for (int i = this._size - 1; i >= 0; i--)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0006A559 File Offset: 0x00068959
		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(this._size - 1, this._size, match);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0006A570 File Offset: 0x00068970
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0006A580 File Offset: 0x00068980
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
			}
			if (this._size == 0)
			{
				if (startIndex != -1)
				{
				}
			}
			else if (startIndex >= this._size)
			{
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0006A5F8 File Offset: 0x000689F8
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
			}
			for (int i = 0; i < this._size; i++)
			{
				action(this._items[i]);
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0006A634 File Offset: 0x00068A34
		public FastList<T> GetRange(int index, int count)
		{
			if (index < 0)
			{
			}
			if (count < 0)
			{
			}
			if (this._size - index < count)
			{
			}
			FastList<T> fastList = new FastList<T>(count);
			Array.Copy(this._items, index, fastList._items, 0, count);
			fastList._size = count;
			return fastList;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0006A680 File Offset: 0x00068A80
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this._items, item, 0, this._size);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0006A695 File Offset: 0x00068A95
		public int IndexOf(T item, int index)
		{
			if (index > this._size)
			{
			}
			return Array.IndexOf<T>(this._items, item, index, this._size - index);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0006A6B8 File Offset: 0x00068AB8
		public int IndexOf(T item, int index, int count)
		{
			if (index > this._size)
			{
			}
			return Array.IndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006A6D4 File Offset: 0x00068AD4
		public void Insert(int index, T item)
		{
			if (index > this._size)
			{
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = item;
			this._size++;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0006A754 File Offset: 0x00068B54
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
			}
			if (index > this._size)
			{
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > 0)
				{
					this.EnsureCapacity(this._size + count);
					if (index < this._size)
					{
						Array.Copy(this._items, index, this._items, index + count, this._size - index);
					}
					if (this == collection2)
					{
						Array.Copy(this._items, 0, this._items, index, index);
						Array.Copy(this._items, index + count, this._items, index * 2, this._size - index);
					}
					else
					{
						collection2.CopyTo(this._items, index);
					}
					this._size += count;
				}
			}
			else
			{
				foreach (T item in collection)
				{
					this.Insert(index++, item);
				}
			}
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0006A870 File Offset: 0x00068C70
		public int LastIndexOf(T item)
		{
			if (this._size == 0)
			{
				return -1;
			}
			return this.LastIndexOf(item, this._size - 1, this._size);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006A894 File Offset: 0x00068C94
		public int LastIndexOf(T item, int index)
		{
			if (index >= this._size)
			{
			}
			return this.LastIndexOf(item, index, index + 1);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0006A8B0 File Offset: 0x00068CB0
		public int LastIndexOf(T item, int index, int count)
		{
			if (this.Count == 0 || index < 0)
			{
			}
			if (this.Count == 0 || count < 0)
			{
			}
			if (this._size == 0)
			{
				return -1;
			}
			if (index >= this._size)
			{
			}
			if (count > index + 1)
			{
			}
			return Array.LastIndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0006A914 File Offset: 0x00068D14
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006A93C File Offset: 0x00068D3C
		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
			}
			int num = 0;
			while (num < this._size && !match(this._items[num]))
			{
				num++;
			}
			if (num >= this._size)
			{
				return 0;
			}
			int i = num + 1;
			while (i < this._size)
			{
				while (i < this._size && match(this._items[i]))
				{
					i++;
				}
				if (i < this._size)
				{
					this._items[num++] = this._items[i++];
				}
			}
			Array.Clear(this._items, num, this._size - num);
			int result = this._size - num;
			this._size = num;
			return result;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0006AA20 File Offset: 0x00068E20
		public void CyclicRemoveAt(int index)
		{
			this._items[index] = this._items[this._size--];
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0006AA58 File Offset: 0x00068E58
		public void RemoveAt(int index)
		{
			if (index >= this._size)
			{
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = default(T);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0006AAC4 File Offset: 0x00068EC4
		public void RemoveRange(int index, int count)
		{
			if (index < 0)
			{
			}
			if (count < 0)
			{
			}
			if (this._size - index < count)
			{
			}
			if (count > 0)
			{
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				Array.Clear(this._items, this._size, count);
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0006AB3D File Offset: 0x00068F3D
		public void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0006AB4C File Offset: 0x00068F4C
		public void Reverse(int index, int count)
		{
			if (index < 0)
			{
			}
			if (count < 0)
			{
			}
			if (this._size - index < count)
			{
			}
			Array.Reverse(this._items, index, count);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0006AB77 File Offset: 0x00068F77
		public void Sort()
		{
			this.Sort(0, this.Count, null);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0006AB87 File Offset: 0x00068F87
		public void Sort(IComparer<T> comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0006AB97 File Offset: 0x00068F97
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0)
			{
			}
			if (count < 0)
			{
			}
			if (this._size - index < count)
			{
			}
			Array.Sort<T>(this._items, index, count, comparer);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0006ABC4 File Offset: 0x00068FC4
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0006ABF4 File Offset: 0x00068FF4
		public void TrimExcess()
		{
			int num = (int)((double)this._items.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0006AC30 File Offset: 0x00069030
		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
			}
			for (int i = 0; i < this._size; i++)
			{
				if (!match(this._items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000F01 RID: 3841
		private const int _defaultCapacity = 4;

		// Token: 0x04000F02 RID: 3842
		public T[] _items;

		// Token: 0x04000F03 RID: 3843
		private int _size;

		// Token: 0x04000F04 RID: 3844
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04000F05 RID: 3845
		private static readonly T[] _emptyArray = new T[0];
	}
}
