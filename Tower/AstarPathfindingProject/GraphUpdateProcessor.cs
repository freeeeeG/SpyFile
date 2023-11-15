using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

namespace Pathfinding
{
	// Token: 0x0200003B RID: 59
	internal class GraphUpdateProcessor
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002AF RID: 687 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		// (remove) Token: 0x060002B0 RID: 688 RVA: 0x0000ED28 File Offset: 0x0000CF28
		public event Action OnGraphsUpdated;

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000ED5D File Offset: 0x0000CF5D
		public bool IsAnyGraphUpdateQueued
		{
			get
			{
				return this.graphUpdateQueue.Count > 0;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000ED6D File Offset: 0x0000CF6D
		public bool IsAnyGraphUpdateInProgress
		{
			get
			{
				return this.anyGraphUpdateInProgress;
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000ED78 File Offset: 0x0000CF78
		public GraphUpdateProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000EDE2 File Offset: 0x0000CFE2
		public AstarWorkItem GetWorkItem()
		{
			return new AstarWorkItem(new Action(this.QueueGraphUpdatesInternal), new Func<bool, bool>(this.ProcessGraphUpdates));
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000EE04 File Offset: 0x0000D004
		public void EnableMultithreading()
		{
			if (this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
			{
				this.asyncUpdateProfilingSampler = CustomSampler.Create("Graph Update", false);
				this.graphUpdateThread = new Thread(new ThreadStart(this.ProcessGraphUpdatesAsync));
				this.graphUpdateThread.IsBackground = true;
				this.graphUpdateThread.Priority = ThreadPriority.Lowest;
				this.graphUpdateThread.Start();
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000EE74 File Offset: 0x0000D074
		public void DisableMultithreading()
		{
			if (this.graphUpdateThread != null && this.graphUpdateThread.IsAlive)
			{
				this.exitAsyncThread.Set();
				if (!this.graphUpdateThread.Join(5000))
				{
					Debug.LogError("Graph update thread did not exit in 5 seconds");
				}
				this.graphUpdateThread = null;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000EEC5 File Offset: 0x0000D0C5
		public void AddToQueue(GraphUpdateObject ob)
		{
			this.graphUpdateQueue.Enqueue(ob);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000EED4 File Offset: 0x0000D0D4
		private void QueueGraphUpdatesInternal()
		{
			while (this.graphUpdateQueue.Count > 0)
			{
				GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
				if (graphUpdateObject.internalStage != -2)
				{
					Debug.LogError("Expected remaining graph updates to be pending");
				}
				else
				{
					graphUpdateObject.internalStage = 0;
					foreach (object obj in this.astar.data.GetUpdateableGraphs())
					{
						IUpdatableGraph updatableGraph = (IUpdatableGraph)obj;
						NavGraph graph = updatableGraph as NavGraph;
						if (graphUpdateObject.nnConstraint == null || graphUpdateObject.nnConstraint.SuitableGraph(this.astar.data.GetGraphIndex(graph), graph))
						{
							GraphUpdateProcessor.GUOSingle item = default(GraphUpdateProcessor.GUOSingle);
							item.order = GraphUpdateProcessor.GraphUpdateOrder.GraphUpdate;
							item.obj = graphUpdateObject;
							item.graph = updatableGraph;
							graphUpdateObject.internalStage++;
							this.graphUpdateQueueRegular.Enqueue(item);
						}
					}
				}
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			this.anyGraphUpdateInProgress = true;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		private bool ProcessGraphUpdates(bool force)
		{
			if (force)
			{
				this.asyncGraphUpdatesComplete.WaitOne();
			}
			else if (!this.asyncGraphUpdatesComplete.WaitOne(0))
			{
				return false;
			}
			this.ProcessPostUpdates();
			if (!this.ProcessRegularUpdates(force))
			{
				return false;
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
			if (this.OnGraphsUpdated != null)
			{
				this.OnGraphsUpdated();
			}
			this.anyGraphUpdateInProgress = false;
			return true;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000F050 File Offset: 0x0000D250
		private bool ProcessRegularUpdates(bool force)
		{
			while (this.graphUpdateQueueRegular.Count > 0)
			{
				GraphUpdateProcessor.GUOSingle guosingle = this.graphUpdateQueueRegular.Peek();
				GraphUpdateThreading graphUpdateThreading = guosingle.graph.CanUpdateAsync(guosingle.obj);
				if (force || !Application.isPlaying || this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
				{
					graphUpdateThreading &= (GraphUpdateThreading)(-2);
				}
				if ((graphUpdateThreading & GraphUpdateThreading.UnityInit) != GraphUpdateThreading.UnityThread)
				{
					if (this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					guosingle.graph.UpdateAreaInit(guosingle.obj);
				}
				if ((graphUpdateThreading & GraphUpdateThreading.SeparateThread) != GraphUpdateThreading.UnityThread)
				{
					this.graphUpdateQueueRegular.Dequeue();
					this.graphUpdateQueueAsync.Enqueue(guosingle);
					if ((graphUpdateThreading & GraphUpdateThreading.UnityPost) != GraphUpdateThreading.UnityThread && this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
				}
				else
				{
					if (this.StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					this.graphUpdateQueueRegular.Dequeue();
					try
					{
						guosingle.graph.UpdateArea(guosingle.obj);
					}
					catch (Exception ex)
					{
						string str = "Error while updating graphs\n";
						Exception ex2 = ex;
						Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					if ((graphUpdateThreading & GraphUpdateThreading.UnityPost) != GraphUpdateThreading.UnityThread)
					{
						guosingle.graph.UpdateAreaPost(guosingle.obj);
					}
					guosingle.obj.internalStage--;
				}
			}
			return !this.StartAsyncUpdatesIfQueued();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000F190 File Offset: 0x0000D390
		private bool StartAsyncUpdatesIfQueued()
		{
			if (this.graphUpdateQueueAsync.Count > 0)
			{
				this.asyncGraphUpdatesComplete.Reset();
				this.graphUpdateAsyncEvent.Set();
				return true;
			}
			return false;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000F1BC File Offset: 0x0000D3BC
		private void ProcessPostUpdates()
		{
			while (this.graphUpdateQueuePost.Count > 0)
			{
				GraphUpdateProcessor.GUOSingle guosingle = this.graphUpdateQueuePost.Dequeue();
				if ((guosingle.graph.CanUpdateAsync(guosingle.obj) & GraphUpdateThreading.UnityPost) != GraphUpdateThreading.UnityThread)
				{
					try
					{
						guosingle.graph.UpdateAreaPost(guosingle.obj);
					}
					catch (Exception ex)
					{
						string str = "Error while updating graphs (post step)\n";
						Exception ex2 = ex;
						Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
				guosingle.obj.internalStage--;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000F250 File Offset: 0x0000D450
		private void ProcessGraphUpdatesAsync()
		{
			AutoResetEvent[] array = new AutoResetEvent[]
			{
				this.graphUpdateAsyncEvent,
				this.exitAsyncThread
			};
			for (;;)
			{
				WaitHandle[] waitHandles = array;
				if (WaitHandle.WaitAny(waitHandles) == 1)
				{
					break;
				}
				while (this.graphUpdateQueueAsync.Count > 0)
				{
					GraphUpdateProcessor.GUOSingle guosingle = this.graphUpdateQueueAsync.Dequeue();
					try
					{
						if (guosingle.order != GraphUpdateProcessor.GraphUpdateOrder.GraphUpdate)
						{
							throw new NotSupportedException(guosingle.order.ToString() ?? "");
						}
						guosingle.graph.UpdateArea(guosingle.obj);
						this.graphUpdateQueuePost.Enqueue(guosingle);
					}
					catch (Exception ex)
					{
						string str = "Exception while updating graphs:\n";
						Exception ex2 = ex;
						Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
				this.asyncGraphUpdatesComplete.Set();
			}
			while (this.graphUpdateQueueAsync.Count > 0)
			{
				this.graphUpdateQueueAsync.Dequeue().obj.internalStage = -3;
			}
			this.asyncGraphUpdatesComplete.Set();
			Profiler.EndThreadProfiling();
		}

		// Token: 0x040001C1 RID: 449
		private readonly AstarPath astar;

		// Token: 0x040001C2 RID: 450
		private Thread graphUpdateThread;

		// Token: 0x040001C3 RID: 451
		private bool anyGraphUpdateInProgress;

		// Token: 0x040001C4 RID: 452
		private CustomSampler asyncUpdateProfilingSampler;

		// Token: 0x040001C5 RID: 453
		private readonly Queue<GraphUpdateObject> graphUpdateQueue = new Queue<GraphUpdateObject>();

		// Token: 0x040001C6 RID: 454
		private readonly Queue<GraphUpdateProcessor.GUOSingle> graphUpdateQueueAsync = new Queue<GraphUpdateProcessor.GUOSingle>();

		// Token: 0x040001C7 RID: 455
		private readonly Queue<GraphUpdateProcessor.GUOSingle> graphUpdateQueuePost = new Queue<GraphUpdateProcessor.GUOSingle>();

		// Token: 0x040001C8 RID: 456
		private readonly Queue<GraphUpdateProcessor.GUOSingle> graphUpdateQueueRegular = new Queue<GraphUpdateProcessor.GUOSingle>();

		// Token: 0x040001C9 RID: 457
		private readonly ManualResetEvent asyncGraphUpdatesComplete = new ManualResetEvent(true);

		// Token: 0x040001CA RID: 458
		private readonly AutoResetEvent graphUpdateAsyncEvent = new AutoResetEvent(false);

		// Token: 0x040001CB RID: 459
		private readonly AutoResetEvent exitAsyncThread = new AutoResetEvent(false);

		// Token: 0x02000115 RID: 277
		private enum GraphUpdateOrder
		{
			// Token: 0x040006C6 RID: 1734
			GraphUpdate
		}

		// Token: 0x02000116 RID: 278
		private struct GUOSingle
		{
			// Token: 0x040006C7 RID: 1735
			public GraphUpdateProcessor.GraphUpdateOrder order;

			// Token: 0x040006C8 RID: 1736
			public IUpdatableGraph graph;

			// Token: 0x040006C9 RID: 1737
			public GraphUpdateObject obj;
		}
	}
}
