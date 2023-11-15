using System;
using System.Collections.Generic;
using System.Threading;

namespace Pathfinding.Util
{
	// Token: 0x020000CF RID: 207
	public class ParallelWorkQueue<T>
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x0003AD13 File Offset: 0x00038F13
		public ParallelWorkQueue(Queue<T> queue)
		{
			this.queue = queue;
			this.initialCount = queue.Count;
			this.threadCount = Math.Min(this.initialCount, Math.Max(1, AstarPath.CalculateThreadCount(ThreadCount.AutomaticHighLoad)));
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0003AD4C File Offset: 0x00038F4C
		public IEnumerable<int> Run(int progressTimeoutMillis)
		{
			if (this.initialCount != this.queue.Count)
			{
				throw new InvalidOperationException("Queue has been modified since the constructor");
			}
			if (this.initialCount == 0)
			{
				yield break;
			}
			this.waitEvents = new ManualResetEvent[this.threadCount];
			for (int i = 0; i < this.waitEvents.Length; i++)
			{
				this.waitEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(delegate(object threadIndex)
				{
					this.RunTask((int)threadIndex);
				}, i);
			}
			for (;;)
			{
				WaitHandle[] waitHandles = this.waitEvents;
				if (WaitHandle.WaitAll(waitHandles, progressTimeoutMillis))
				{
					break;
				}
				Queue<T> obj = this.queue;
				int count;
				lock (obj)
				{
					count = this.queue.Count;
				}
				yield return this.initialCount - count;
			}
			if (this.innerException != null)
			{
				throw this.innerException;
			}
			yield break;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0003AD64 File Offset: 0x00038F64
		private void RunTask(int threadIndex)
		{
			try
			{
				for (;;)
				{
					Queue<T> obj = this.queue;
					T arg;
					lock (obj)
					{
						if (this.queue.Count == 0)
						{
							break;
						}
						arg = this.queue.Dequeue();
					}
					this.action(arg, threadIndex);
				}
			}
			catch (Exception ex)
			{
				this.innerException = ex;
				Queue<T> obj = this.queue;
				lock (obj)
				{
					this.queue.Clear();
				}
			}
			finally
			{
				this.waitEvents[threadIndex].Set();
			}
		}

		// Token: 0x04000505 RID: 1285
		public Action<T, int> action;

		// Token: 0x04000506 RID: 1286
		public readonly int threadCount;

		// Token: 0x04000507 RID: 1287
		private readonly Queue<T> queue;

		// Token: 0x04000508 RID: 1288
		private readonly int initialCount;

		// Token: 0x04000509 RID: 1289
		private ManualResetEvent[] waitEvents;

		// Token: 0x0400050A RID: 1290
		private Exception innerException;
	}
}
