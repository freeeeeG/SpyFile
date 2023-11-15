using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

namespace Pathfinding
{
	// Token: 0x02000045 RID: 69
	public class PathProcessor
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600033E RID: 830 RVA: 0x00012344 File Offset: 0x00010544
		// (remove) Token: 0x0600033F RID: 831 RVA: 0x0001237C File Offset: 0x0001057C
		public event Action<Path> OnPathPreSearch;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000340 RID: 832 RVA: 0x000123B4 File Offset: 0x000105B4
		// (remove) Token: 0x06000341 RID: 833 RVA: 0x000123EC File Offset: 0x000105EC
		public event Action<Path> OnPathPostSearch;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000342 RID: 834 RVA: 0x00012424 File Offset: 0x00010624
		// (remove) Token: 0x06000343 RID: 835 RVA: 0x0001245C File Offset: 0x0001065C
		public event Action OnQueueUnblocked;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00012491 File Offset: 0x00010691
		public int NumThreads
		{
			get
			{
				return this.pathHandlers.Length;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0001249B File Offset: 0x0001069B
		public bool IsUsingMultithreading
		{
			get
			{
				return this.threads != null;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000124A8 File Offset: 0x000106A8
		internal PathProcessor(AstarPath astar, PathReturnQueue returnQueue, int processors, bool multithreaded)
		{
			this.astar = astar;
			this.returnQueue = returnQueue;
			if (processors < 0)
			{
				throw new ArgumentOutOfRangeException("processors");
			}
			if (!multithreaded && processors != 1)
			{
				throw new Exception("Only a single non-multithreaded processor is allowed");
			}
			this.queue = new ThreadControlQueue(processors);
			this.pathHandlers = new PathHandler[processors];
			for (int i = 0; i < processors; i++)
			{
				this.pathHandlers[i] = new PathHandler(i, processors);
			}
			if (multithreaded)
			{
				this.profilingSampler = CustomSampler.Create("Calculating Path", false);
				this.threads = new Thread[processors];
				for (int j = 0; j < processors; j++)
				{
					PathHandler pathHandler = this.pathHandlers[j];
					this.threads[j] = new Thread(delegate()
					{
						this.CalculatePathsThreaded(pathHandler);
					});
					this.threads[j].Name = "Pathfinding Thread " + j.ToString();
					this.threads[j].IsBackground = true;
					this.threads[j].Start();
				}
				return;
			}
			this.threadCoroutine = this.CalculatePaths(this.pathHandlers[0]);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000125F0 File Offset: 0x000107F0
		private int Lock(bool block)
		{
			this.queue.Block();
			if (block)
			{
				while (!this.queue.AllReceiversBlocked)
				{
					if (this.IsUsingMultithreading)
					{
						Thread.Sleep(1);
					}
					else
					{
						this.TickNonMultithreaded();
					}
				}
			}
			this.nextLockID++;
			this.locks.Add(this.nextLockID);
			return this.nextLockID;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00012658 File Offset: 0x00010858
		private void Unlock(int id)
		{
			if (!this.locks.Remove(id))
			{
				throw new ArgumentException("This lock has already been released");
			}
			if (this.locks.Count == 0)
			{
				if (this.OnQueueUnblocked != null)
				{
					this.OnQueueUnblocked();
				}
				this.queue.Unblock();
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000126A9 File Offset: 0x000108A9
		public PathProcessor.GraphUpdateLock PausePathfinding(bool block)
		{
			return new PathProcessor.GraphUpdateLock(this, block);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000126B4 File Offset: 0x000108B4
		public void TickNonMultithreaded()
		{
			if (this.threadCoroutine != null)
			{
				try
				{
					this.threadCoroutine.MoveNext();
				}
				catch (Exception ex)
				{
					this.threadCoroutine = null;
					if (!(ex is ThreadControlQueue.QueueTerminationException))
					{
						Debug.LogException(ex);
						Debug.LogError("Unhandled exception during pathfinding. Terminating.");
						this.queue.TerminateReceivers();
						try
						{
							this.queue.PopNoBlock(false);
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00012734 File Offset: 0x00010934
		public void JoinThreads()
		{
			if (this.threads != null)
			{
				for (int i = 0; i < this.threads.Length; i++)
				{
					if (!this.threads[i].Join(200))
					{
						Debug.LogError("Could not terminate pathfinding thread[" + i.ToString() + "] in 200ms, trying Thread.Abort");
						this.threads[i].Abort();
					}
				}
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00012798 File Offset: 0x00010998
		public void AbortThreads()
		{
			if (this.threads == null)
			{
				return;
			}
			for (int i = 0; i < this.threads.Length; i++)
			{
				if (this.threads[i] != null && this.threads[i].IsAlive)
				{
					this.threads[i].Abort();
				}
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000127E8 File Offset: 0x000109E8
		public int GetNewNodeIndex()
		{
			if (this.nodeIndexPool.Count <= 0)
			{
				int num = this.nextNodeIndex;
				this.nextNodeIndex = num + 1;
				return num;
			}
			return this.nodeIndexPool.Pop();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00012820 File Offset: 0x00010A20
		public void InitializeNode(GraphNode node)
		{
			if (!this.queue.AllReceiversBlocked)
			{
				throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.php#direct");
			}
			for (int i = 0; i < this.pathHandlers.Length; i++)
			{
				this.pathHandlers[i].InitializeNode(node);
			}
			this.astar.hierarchicalGraph.OnCreatedNode(node);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00012878 File Offset: 0x00010A78
		public void DestroyNode(GraphNode node)
		{
			if (node.NodeIndex == -1)
			{
				return;
			}
			this.nodeIndexPool.Push(node.NodeIndex);
			for (int i = 0; i < this.pathHandlers.Length; i++)
			{
				this.pathHandlers[i].DestroyNode(node);
			}
			this.astar.hierarchicalGraph.AddDirtyNode(node);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x000128D4 File Offset: 0x00010AD4
		private void CalculatePathsThreaded(PathHandler pathHandler)
		{
			try
			{
				long num = 100000L;
				long targetTick = DateTime.UtcNow.Ticks + num;
				for (;;)
				{
					Path path = this.queue.Pop();
					IPathInternals pathInternals = path;
					pathInternals.PrepareBase(pathHandler);
					pathInternals.AdvanceState(PathState.Processing);
					if (this.OnPathPreSearch != null)
					{
						this.OnPathPreSearch(path);
					}
					long ticks = DateTime.UtcNow.Ticks;
					pathInternals.Prepare();
					if (path.CompleteState == PathCompleteState.NotCalculated)
					{
						this.astar.debugPathData = pathInternals.PathHandler;
						this.astar.debugPathID = path.pathID;
						pathInternals.Initialize();
						while (path.CompleteState == PathCompleteState.NotCalculated)
						{
							pathInternals.CalculateStep(targetTick);
							targetTick = DateTime.UtcNow.Ticks + num;
							if (this.queue.IsTerminating)
							{
								path.FailWithError("AstarPath object destroyed");
							}
						}
						path.duration = (float)(DateTime.UtcNow.Ticks - ticks) * 0.0001f;
					}
					pathInternals.Cleanup();
					if (path.immediateCallback != null)
					{
						path.immediateCallback(path);
					}
					if (this.OnPathPostSearch != null)
					{
						this.OnPathPostSearch(path);
					}
					this.returnQueue.Enqueue(path);
					pathInternals.AdvanceState(PathState.ReturnQueue);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is ThreadControlQueue.QueueTerminationException)
				{
					if (this.astar.logPathResults == PathLog.Heavy)
					{
						Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID.ToString());
					}
					return;
				}
				Debug.LogException(ex);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.TerminateReceivers();
			}
			finally
			{
				Profiler.EndThreadProfiling();
			}
			Debug.LogError("Error : This part should never be reached.");
			this.queue.ReceiverTerminated();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00012AC0 File Offset: 0x00010CC0
		private IEnumerator CalculatePaths(PathHandler pathHandler)
		{
			long maxTicks = (long)(this.astar.maxFrameTime * 10000f);
			long targetTick = DateTime.UtcNow.Ticks + maxTicks;
			for (;;)
			{
				Path p = null;
				bool blockedBefore = false;
				while (p == null)
				{
					try
					{
						p = this.queue.PopNoBlock(blockedBefore);
						blockedBefore |= (p == null);
					}
					catch (ThreadControlQueue.QueueTerminationException)
					{
						yield break;
					}
					if (p == null)
					{
						yield return null;
					}
				}
				IPathInternals ip = p;
				maxTicks = (long)(this.astar.maxFrameTime * 10000f);
				ip.PrepareBase(pathHandler);
				ip.AdvanceState(PathState.Processing);
				Action<Path> onPathPreSearch = this.OnPathPreSearch;
				if (onPathPreSearch != null)
				{
					onPathPreSearch(p);
				}
				long ticks = DateTime.UtcNow.Ticks;
				long totalTicks = 0L;
				ip.Prepare();
				if (p.CompleteState == PathCompleteState.NotCalculated)
				{
					this.astar.debugPathData = ip.PathHandler;
					this.astar.debugPathID = p.pathID;
					ip.Initialize();
					while (p.CompleteState == PathCompleteState.NotCalculated)
					{
						ip.CalculateStep(targetTick);
						if (p.CompleteState != PathCompleteState.NotCalculated)
						{
							break;
						}
						totalTicks += DateTime.UtcNow.Ticks - ticks;
						yield return null;
						ticks = DateTime.UtcNow.Ticks;
						if (this.queue.IsTerminating)
						{
							p.FailWithError("AstarPath object destroyed");
						}
						targetTick = DateTime.UtcNow.Ticks + maxTicks;
					}
					totalTicks += DateTime.UtcNow.Ticks - ticks;
					p.duration = (float)totalTicks * 0.0001f;
				}
				ip.Cleanup();
				OnPathDelegate immediateCallback = p.immediateCallback;
				if (immediateCallback != null)
				{
					immediateCallback(p);
				}
				Action<Path> onPathPostSearch = this.OnPathPostSearch;
				if (onPathPostSearch != null)
				{
					onPathPostSearch(p);
				}
				this.returnQueue.Enqueue(p);
				ip.AdvanceState(PathState.ReturnQueue);
				if (DateTime.UtcNow.Ticks > targetTick)
				{
					yield return null;
					targetTick = DateTime.UtcNow.Ticks + maxTicks;
				}
				p = null;
				ip = null;
			}
			yield break;
		}

		// Token: 0x0400020E RID: 526
		internal readonly ThreadControlQueue queue;

		// Token: 0x0400020F RID: 527
		private readonly AstarPath astar;

		// Token: 0x04000210 RID: 528
		private readonly PathReturnQueue returnQueue;

		// Token: 0x04000211 RID: 529
		private readonly PathHandler[] pathHandlers;

		// Token: 0x04000212 RID: 530
		private readonly Thread[] threads;

		// Token: 0x04000213 RID: 531
		private IEnumerator threadCoroutine;

		// Token: 0x04000214 RID: 532
		private int nextNodeIndex = 1;

		// Token: 0x04000215 RID: 533
		private readonly Stack<int> nodeIndexPool = new Stack<int>();

		// Token: 0x04000216 RID: 534
		private readonly List<int> locks = new List<int>();

		// Token: 0x04000217 RID: 535
		private int nextLockID;

		// Token: 0x04000218 RID: 536
		private CustomSampler profilingSampler;

		// Token: 0x02000119 RID: 281
		public struct GraphUpdateLock
		{
			// Token: 0x06000A8F RID: 2703 RVA: 0x00043A25 File Offset: 0x00041C25
			public GraphUpdateLock(PathProcessor pathProcessor, bool block)
			{
				this.pathProcessor = pathProcessor;
				this.id = pathProcessor.Lock(block);
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00043A3B File Offset: 0x00041C3B
			public bool Held
			{
				get
				{
					return this.pathProcessor != null && this.pathProcessor.locks.Contains(this.id);
				}
			}

			// Token: 0x06000A91 RID: 2705 RVA: 0x00043A5D File Offset: 0x00041C5D
			public void Release()
			{
				this.pathProcessor.Unlock(this.id);
			}

			// Token: 0x040006D0 RID: 1744
			private PathProcessor pathProcessor;

			// Token: 0x040006D1 RID: 1745
			private int id;
		}
	}
}
