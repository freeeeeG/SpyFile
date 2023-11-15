using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200004A RID: 74
	internal class WorkItemProcessor : IWorkItemContext
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00013250 File Offset: 0x00011450
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00013258 File Offset: 0x00011458
		public bool workItemsInProgressRightNow { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00013261 File Offset: 0x00011461
		public bool anyQueued
		{
			get
			{
				return this.workItems.Count > 0;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00013271 File Offset: 0x00011471
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00013279 File Offset: 0x00011479
		public bool workItemsInProgress { get; private set; }

		// Token: 0x06000370 RID: 880 RVA: 0x00013282 File Offset: 0x00011482
		void IWorkItemContext.QueueFloodFill()
		{
			this.queuedWorkItemFloodFill = true;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001328B File Offset: 0x0001148B
		void IWorkItemContext.SetGraphDirty(NavGraph graph)
		{
			this.anyGraphsDirty = true;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00013294 File Offset: 0x00011494
		public void EnsureValidFloodFill()
		{
			if (this.queuedWorkItemFloodFill)
			{
				this.astar.hierarchicalGraph.RecalculateAll();
				return;
			}
			this.astar.hierarchicalGraph.RecalculateIfNecessary();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000132BF File Offset: 0x000114BF
		public WorkItemProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000132E0 File Offset: 0x000114E0
		public void OnFloodFill()
		{
			this.queuedWorkItemFloodFill = false;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000132E9 File Offset: 0x000114E9
		public void AddWorkItem(AstarWorkItem item)
		{
			this.workItems.Enqueue(item);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000132F8 File Offset: 0x000114F8
		public bool ProcessWorkItems(bool force)
		{
			if (this.workItemsInProgressRightNow)
			{
				throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
			}
			Physics2D.SyncTransforms();
			this.workItemsInProgressRightNow = true;
			this.astar.data.LockGraphStructure(true);
			while (this.workItems.Count > 0)
			{
				if (!this.workItemsInProgress)
				{
					this.workItemsInProgress = true;
					this.queuedWorkItemFloodFill = false;
				}
				AstarWorkItem astarWorkItem = this.workItems[0];
				bool flag;
				try
				{
					if (astarWorkItem.init != null)
					{
						astarWorkItem.init();
						astarWorkItem.init = null;
					}
					if (astarWorkItem.initWithContext != null)
					{
						astarWorkItem.initWithContext(this);
						astarWorkItem.initWithContext = null;
					}
					this.workItems[0] = astarWorkItem;
					if (astarWorkItem.update != null)
					{
						flag = astarWorkItem.update(force);
					}
					else
					{
						flag = (astarWorkItem.updateWithContext == null || astarWorkItem.updateWithContext(this, force));
					}
				}
				catch
				{
					this.workItems.Dequeue();
					this.workItemsInProgressRightNow = false;
					this.astar.data.UnlockGraphStructure();
					throw;
				}
				if (!flag)
				{
					if (force)
					{
						Debug.LogError("Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
					}
					this.workItemsInProgressRightNow = false;
					this.astar.data.UnlockGraphStructure();
					return false;
				}
				this.workItems.Dequeue();
			}
			this.EnsureValidFloodFill();
			if (this.anyGraphsDirty)
			{
				GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
			}
			this.anyGraphsDirty = false;
			this.workItemsInProgressRightNow = false;
			this.workItemsInProgress = false;
			this.astar.data.UnlockGraphStructure();
			return true;
		}

		// Token: 0x04000229 RID: 553
		private readonly AstarPath astar;

		// Token: 0x0400022A RID: 554
		private readonly WorkItemProcessor.IndexedQueue<AstarWorkItem> workItems = new WorkItemProcessor.IndexedQueue<AstarWorkItem>();

		// Token: 0x0400022B RID: 555
		private bool queuedWorkItemFloodFill;

		// Token: 0x0400022C RID: 556
		private bool anyGraphsDirty = true;

		// Token: 0x0200011D RID: 285
		private class IndexedQueue<T>
		{
			// Token: 0x17000179 RID: 377
			public T this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					return this.buffer[(this.start + index) % this.buffer.Length];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					this.buffer[(this.start + index) % this.buffer.Length] = value;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00043E6A File Offset: 0x0004206A
			// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00043E72 File Offset: 0x00042072
			public int Count { get; private set; }

			// Token: 0x06000A9F RID: 2719 RVA: 0x00043E7C File Offset: 0x0004207C
			public void Enqueue(T item)
			{
				if (this.Count == this.buffer.Length)
				{
					T[] array = new T[this.buffer.Length * 2];
					for (int i = 0; i < this.Count; i++)
					{
						array[i] = this[i];
					}
					this.buffer = array;
					this.start = 0;
				}
				this.buffer[(this.start + this.Count) % this.buffer.Length] = item;
				int count = this.Count;
				this.Count = count + 1;
			}

			// Token: 0x06000AA0 RID: 2720 RVA: 0x00043F08 File Offset: 0x00042108
			public T Dequeue()
			{
				if (this.Count == 0)
				{
					throw new InvalidOperationException();
				}
				T result = this.buffer[this.start];
				this.start = (this.start + 1) % this.buffer.Length;
				int count = this.Count;
				this.Count = count - 1;
				return result;
			}

			// Token: 0x040006DE RID: 1758
			private T[] buffer = new T[4];

			// Token: 0x040006DF RID: 1759
			private int start;
		}
	}
}
