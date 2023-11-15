using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x020000CB RID: 203
	public class GridLookup<T> where T : class
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x00038CC0 File Offset: 0x00036EC0
		public GridLookup(Int2 size)
		{
			this.size = size;
			this.cells = new GridLookup<T>.Item[size.x * size.y];
			for (int i = 0; i < this.cells.Length; i++)
			{
				this.cells[i] = new GridLookup<T>.Item();
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00038D33 File Offset: 0x00036F33
		public GridLookup<T>.Root AllItems
		{
			get
			{
				return this.all.next;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00038D40 File Offset: 0x00036F40
		public void Clear()
		{
			this.rootLookup.Clear();
			this.all.next = null;
			GridLookup<T>.Item[] array = this.cells;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].next = null;
			}
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00038D84 File Offset: 0x00036F84
		public GridLookup<T>.Root GetRoot(T item)
		{
			GridLookup<T>.Root result;
			this.rootLookup.TryGetValue(item, out result);
			return result;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00038DA4 File Offset: 0x00036FA4
		public GridLookup<T>.Root Add(T item, IntRect bounds)
		{
			GridLookup<T>.Root root = new GridLookup<T>.Root
			{
				obj = item,
				prev = this.all,
				next = this.all.next
			};
			this.all.next = root;
			if (root.next != null)
			{
				root.next.prev = root;
			}
			this.rootLookup.Add(item, root);
			this.Move(item, bounds);
			return root;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00038E14 File Offset: 0x00037014
		public void Remove(T item)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				return;
			}
			this.Move(item, new IntRect(0, 0, -1, -1));
			this.rootLookup.Remove(item);
			root.prev.next = root.next;
			if (root.next != null)
			{
				root.next.prev = root.prev;
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00038E7C File Offset: 0x0003707C
		public void Move(T item, IntRect bounds)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				throw new ArgumentException("The item has not been added to this object");
			}
			if (root.previousBounds == bounds)
			{
				return;
			}
			for (int i = 0; i < root.items.Count; i++)
			{
				GridLookup<T>.Item item2 = root.items[i];
				item2.prev.next = item2.next;
				if (item2.next != null)
				{
					item2.next.prev = item2.prev;
				}
			}
			root.previousBounds = bounds;
			int num = 0;
			for (int j = bounds.ymin; j <= bounds.ymax; j++)
			{
				for (int k = bounds.xmin; k <= bounds.xmax; k++)
				{
					GridLookup<T>.Item item3;
					if (num < root.items.Count)
					{
						item3 = root.items[num];
					}
					else
					{
						item3 = ((this.itemPool.Count > 0) ? this.itemPool.Pop() : new GridLookup<T>.Item());
						item3.root = root;
						root.items.Add(item3);
					}
					num++;
					item3.prev = this.cells[k + j * this.size.x];
					item3.next = item3.prev.next;
					item3.prev.next = item3;
					if (item3.next != null)
					{
						item3.next.prev = item3;
					}
				}
			}
			for (int l = root.items.Count - 1; l >= num; l--)
			{
				GridLookup<T>.Item item4 = root.items[l];
				item4.root = null;
				item4.next = null;
				item4.prev = null;
				root.items.RemoveAt(l);
				this.itemPool.Push(item4);
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00039058 File Offset: 0x00037258
		public List<U> QueryRect<U>(IntRect r) where U : class, T
		{
			List<U> list = ListPool<U>.Claim();
			for (int i = r.ymin; i <= r.ymax; i++)
			{
				int num = i * this.size.x;
				for (int j = r.xmin; j <= r.xmax; j++)
				{
					GridLookup<T>.Item item = this.cells[j + num];
					while (item.next != null)
					{
						item = item.next;
						U u = item.root.obj as U;
						if (!item.root.flag && u != null)
						{
							item.root.flag = true;
							list.Add(u);
						}
					}
				}
			}
			for (int k = r.ymin; k <= r.ymax; k++)
			{
				int num2 = k * this.size.x;
				for (int l = r.xmin; l <= r.xmax; l++)
				{
					GridLookup<T>.Item item2 = this.cells[l + num2];
					while (item2.next != null)
					{
						item2 = item2.next;
						item2.root.flag = false;
					}
				}
			}
			return list;
		}

		// Token: 0x040004F4 RID: 1268
		private Int2 size;

		// Token: 0x040004F5 RID: 1269
		private GridLookup<T>.Item[] cells;

		// Token: 0x040004F6 RID: 1270
		private GridLookup<T>.Root all = new GridLookup<T>.Root();

		// Token: 0x040004F7 RID: 1271
		private Dictionary<T, GridLookup<T>.Root> rootLookup = new Dictionary<T, GridLookup<T>.Root>();

		// Token: 0x040004F8 RID: 1272
		private Stack<GridLookup<T>.Item> itemPool = new Stack<GridLookup<T>.Item>();

		// Token: 0x02000166 RID: 358
		internal class Item
		{
			// Token: 0x04000806 RID: 2054
			public GridLookup<T>.Root root;

			// Token: 0x04000807 RID: 2055
			public GridLookup<T>.Item prev;

			// Token: 0x04000808 RID: 2056
			public GridLookup<T>.Item next;
		}

		// Token: 0x02000167 RID: 359
		public class Root
		{
			// Token: 0x04000809 RID: 2057
			public T obj;

			// Token: 0x0400080A RID: 2058
			public GridLookup<T>.Root next;

			// Token: 0x0400080B RID: 2059
			internal GridLookup<T>.Root prev;

			// Token: 0x0400080C RID: 2060
			internal IntRect previousBounds = new IntRect(0, 0, -1, -1);

			// Token: 0x0400080D RID: 2061
			internal List<GridLookup<T>.Item> items = new List<GridLookup<T>.Item>();

			// Token: 0x0400080E RID: 2062
			internal bool flag;
		}
	}
}
