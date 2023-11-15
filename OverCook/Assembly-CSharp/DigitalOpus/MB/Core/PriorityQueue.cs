using System;
using System.Collections;
using System.Collections.Generic;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000041 RID: 65
	public class PriorityQueue<TPriority, TValue> : ICollection<KeyValuePair<TPriority, TValue>>, IEnumerable, IEnumerable<KeyValuePair<TPriority, TValue>>
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00010CB6 File Offset: 0x0000F0B6
		public PriorityQueue() : this(Comparer<TPriority>.Default)
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00010CC3 File Offset: 0x0000F0C3
		public PriorityQueue(int capacity) : this(capacity, Comparer<TPriority>.Default)
		{
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00010CD1 File Offset: 0x0000F0D1
		public PriorityQueue(int capacity, IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(capacity);
			this._comparer = comparer;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00010CF8 File Offset: 0x0000F0F8
		public PriorityQueue(IComparer<TPriority> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>();
			this._comparer = comparer;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00010D1E File Offset: 0x0000F11E
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data) : this(data, Comparer<TPriority>.Default)
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00010D2C File Offset: 0x0000F12C
		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data, IComparer<TPriority> comparer)
		{
			if (data == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			this._comparer = comparer;
			this._baseHeap = new List<KeyValuePair<TPriority, TValue>>(data);
			for (int i = this._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				this.HeapifyFromBeginningToEnd(i);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00010D8B File Offset: 0x0000F18B
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2)
		{
			if (pq1 == null || pq2 == null)
			{
				throw new ArgumentNullException();
			}
			if (pq1._comparer != pq2._comparer)
			{
				throw new InvalidOperationException("Priority queues to be merged must have equal comparers");
			}
			return PriorityQueue<TPriority, TValue>.MergeQueues(pq1, pq2, pq1._comparer);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00010DC8 File Offset: 0x0000F1C8
		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2, IComparer<TPriority> comparer)
		{
			if (pq1 == null || pq2 == null || comparer == null)
			{
				throw new ArgumentNullException();
			}
			PriorityQueue<TPriority, TValue> priorityQueue = new PriorityQueue<TPriority, TValue>(pq1.Count + pq2.Count, pq1._comparer);
			priorityQueue._baseHeap.AddRange(pq1._baseHeap);
			priorityQueue._baseHeap.AddRange(pq2._baseHeap);
			for (int i = priorityQueue._baseHeap.Count / 2 - 1; i >= 0; i--)
			{
				priorityQueue.HeapifyFromBeginningToEnd(i);
			}
			return priorityQueue;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00010E50 File Offset: 0x0000F250
		public void Enqueue(TPriority priority, TValue value)
		{
			this.Insert(priority, value);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00010E5C File Offset: 0x0000F25C
		public KeyValuePair<TPriority, TValue> Dequeue()
		{
			if (!this.IsEmpty)
			{
				KeyValuePair<TPriority, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00010E94 File Offset: 0x0000F294
		public TValue DequeueValue()
		{
			return this.Dequeue().Value;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00010EAF File Offset: 0x0000F2AF
		public KeyValuePair<TPriority, TValue> Peek()
		{
			if (!this.IsEmpty)
			{
				return this._baseHeap[0];
			}
			throw new InvalidOperationException("Priority queue is empty");
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00010ED4 File Offset: 0x0000F2D4
		public TValue PeekValue()
		{
			return this.Peek().Value;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00010EEF File Offset: 0x0000F2EF
		public bool IsEmpty
		{
			get
			{
				return this._baseHeap.Count == 0;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00010F00 File Offset: 0x0000F300
		private void ExchangeElements(int pos1, int pos2)
		{
			KeyValuePair<TPriority, TValue> value = this._baseHeap[pos1];
			this._baseHeap[pos1] = this._baseHeap[pos2];
			this._baseHeap[pos2] = value;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00010F40 File Offset: 0x0000F340
		private void Insert(TPriority priority, TValue value)
		{
			KeyValuePair<TPriority, TValue> item = new KeyValuePair<TPriority, TValue>(priority, value);
			this._baseHeap.Add(item);
			this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00010F78 File Offset: 0x0000F378
		private int HeapifyFromEndToBeginning(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return -1;
			}
			while (pos > 0)
			{
				int num = (pos - 1) / 2;
				if (this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[pos].Key) <= 0)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
			return pos;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00010FFC File Offset: 0x0000F3FC
		private void DeleteRoot()
		{
			if (this._baseHeap.Count <= 1)
			{
				this._baseHeap.Clear();
				return;
			}
			this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			this.HeapifyFromBeginningToEnd(0);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0001106C File Offset: 0x0000F46C
		private void HeapifyFromBeginningToEnd(int pos)
		{
			if (pos >= this._baseHeap.Count)
			{
				return;
			}
			for (;;)
			{
				int num = pos;
				int num2 = 2 * pos + 1;
				int num3 = 2 * pos + 2;
				if (num2 < this._baseHeap.Count && this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num2].Key) > 0)
				{
					num = num2;
				}
				if (num3 < this._baseHeap.Count && this._comparer.Compare(this._baseHeap[num].Key, this._baseHeap[num3].Key) > 0)
				{
					num = num3;
				}
				if (num == pos)
				{
					break;
				}
				this.ExchangeElements(num, pos);
				pos = num;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00011155 File Offset: 0x0000F555
		public void Add(KeyValuePair<TPriority, TValue> item)
		{
			this.Enqueue(item.Key, item.Value);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0001116B File Offset: 0x0000F56B
		public void Clear()
		{
			this._baseHeap.Clear();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00011178 File Offset: 0x0000F578
		public bool Contains(KeyValuePair<TPriority, TValue> item)
		{
			return this._baseHeap.Contains(item);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00011188 File Offset: 0x0000F588
		public bool TryFindValue(TPriority item, out TValue foundVersion)
		{
			for (int i = 0; i < this._baseHeap.Count; i++)
			{
				if (this._comparer.Compare(item, this._baseHeap[i].Key) == 0)
				{
					foundVersion = this._baseHeap[i].Value;
					return true;
				}
			}
			foundVersion = default(TValue);
			return false;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00011202 File Offset: 0x0000F602
		public int Count
		{
			get
			{
				return this._baseHeap.Count;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0001120F File Offset: 0x0000F60F
		public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int arrayIndex)
		{
			this._baseHeap.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0001121E File Offset: 0x0000F61E
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00011224 File Offset: 0x0000F624
		public bool Remove(KeyValuePair<TPriority, TValue> item)
		{
			int num = this._baseHeap.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this._baseHeap[num] = this._baseHeap[this._baseHeap.Count - 1];
			this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
			int num2 = this.HeapifyFromEndToBeginning(num);
			if (num2 == num)
			{
				this.HeapifyFromBeginningToEnd(num);
			}
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0001129A File Offset: 0x0000F69A
		public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
		{
			return this._baseHeap.GetEnumerator();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000112AC File Offset: 0x0000F6AC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000131 RID: 305
		public List<KeyValuePair<TPriority, TValue>> _baseHeap;

		// Token: 0x04000132 RID: 306
		private IComparer<TPriority> _comparer;
	}
}
