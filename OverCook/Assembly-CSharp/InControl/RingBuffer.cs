using System;

namespace InControl
{
	// Token: 0x02000345 RID: 837
	internal class RingBuffer<T>
	{
		// Token: 0x06000FE9 RID: 4073 RVA: 0x0005C5A8 File Offset: 0x0005A9A8
		public RingBuffer(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentException("RingBuffer size must be 1 or greater.");
			}
			this.size = size + 1;
			this.data = new T[this.size];
			this.head = 0;
			this.tail = 0;
			this.sync = new object();
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0005C600 File Offset: 0x0005AA00
		public void Enqueue(T value)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.size > 1)
				{
					this.head = (this.head + 1) % this.size;
					if (this.head == this.tail)
					{
						this.tail = (this.tail + 1) % this.size;
					}
				}
				this.data[this.head] = value;
			}
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0005C690 File Offset: 0x0005AA90
		public T Dequeue()
		{
			object obj = this.sync;
			T result;
			lock (obj)
			{
				if (this.size > 1 && this.tail != this.head)
				{
					this.tail = (this.tail + 1) % this.size;
				}
				result = this.data[this.tail];
			}
			return result;
		}

		// Token: 0x04000C13 RID: 3091
		private int size;

		// Token: 0x04000C14 RID: 3092
		private T[] data;

		// Token: 0x04000C15 RID: 3093
		private int head;

		// Token: 0x04000C16 RID: 3094
		private int tail;

		// Token: 0x04000C17 RID: 3095
		private object sync;
	}
}
