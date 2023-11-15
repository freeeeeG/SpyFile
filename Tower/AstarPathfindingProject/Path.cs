using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000050 RID: 80
	public abstract class Path : IPathInternals
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00013DA6 File Offset: 0x00011FA6
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00013DAE File Offset: 0x00011FAE
		public PathState PipelineState { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00013DB7 File Offset: 0x00011FB7
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00013DC0 File Offset: 0x00011FC0
		public PathCompleteState CompleteState
		{
			get
			{
				return this.completeState;
			}
			protected set
			{
				object obj = this.stateLock;
				lock (obj)
				{
					if (this.completeState != PathCompleteState.Error)
					{
						this.completeState = value;
					}
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00013E0C File Offset: 0x0001200C
		public bool error
		{
			get
			{
				return this.CompleteState == PathCompleteState.Error;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00013E17 File Offset: 0x00012017
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00013E1F File Offset: 0x0001201F
		public string errorLog { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00013E28 File Offset: 0x00012028
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00013E30 File Offset: 0x00012030
		public int searchedNodes { get; protected set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00013E39 File Offset: 0x00012039
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00013E41 File Offset: 0x00012041
		bool IPathInternals.Pooled { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00013E4A File Offset: 0x0001204A
		[Obsolete("Has been renamed to 'Pooled' to use more widely underestood terminology", true)]
		internal bool recycled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00013E4D File Offset: 0x0001204D
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00013E55 File Offset: 0x00012055
		public ushort pathID { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00013E5E File Offset: 0x0001205E
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x00013E66 File Offset: 0x00012066
		public int[] tagPenalties
		{
			get
			{
				return this.manualTagPenalties;
			}
			set
			{
				if (value == null || value.Length != 32)
				{
					this.manualTagPenalties = null;
					this.internalTagPenalties = Path.ZeroTagPenalties;
					return;
				}
				this.manualTagPenalties = value;
				this.internalTagPenalties = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00013E93 File Offset: 0x00012093
		public virtual bool FloodingPath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00013E98 File Offset: 0x00012098
		public float GetTotalLength()
		{
			if (this.vectorPath == null)
			{
				return float.PositiveInfinity;
			}
			float num = 0f;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				num += Vector3.Distance(this.vectorPath[i], this.vectorPath[i + 1]);
			}
			return num;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00013EF4 File Offset: 0x000120F4
		public IEnumerator WaitForPath()
		{
			if (this.PipelineState == PathState.Created)
			{
				throw new InvalidOperationException("This path has not been started yet");
			}
			while (this.PipelineState != PathState.Returned)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00013F03 File Offset: 0x00012103
		public void BlockUntilCalculated()
		{
			AstarPath.BlockUntilCalculated(this);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00013F0C File Offset: 0x0001210C
		internal uint CalculateHScore(GraphNode node)
		{
			switch (this.heuristic)
			{
			case Heuristic.Manhattan:
			{
				Int3 position = node.position;
				uint num = (uint)((float)(Math.Abs(this.hTarget.x - position.x) + Math.Abs(this.hTarget.y - position.y) + Math.Abs(this.hTarget.z - position.z)) * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			case Heuristic.DiagonalManhattan:
			{
				Int3 @int = this.GetHTarget() - node.position;
				@int.x = Math.Abs(@int.x);
				@int.y = Math.Abs(@int.y);
				@int.z = Math.Abs(@int.z);
				int num2 = Math.Min(@int.x, @int.z);
				int num3 = Math.Max(@int.x, @int.z);
				uint num = (uint)((float)(14 * num2 / 10 + (num3 - num2) + @int.y) * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			case Heuristic.Euclidean:
			{
				uint num = (uint)((float)(this.GetHTarget() - node.position).costMagnitude * this.heuristicScale);
				if (this.hTargetNode != null)
				{
					num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
				}
				return num;
			}
			default:
				return 0U;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000140D3 File Offset: 0x000122D3
		public uint GetTagPenalty(int tag)
		{
			return (uint)this.internalTagPenalties[tag];
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000140DD File Offset: 0x000122DD
		protected Int3 GetHTarget()
		{
			return this.hTarget;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000140E5 File Offset: 0x000122E5
		public bool CanTraverse(GraphNode node)
		{
			if (this.traversalProvider != null)
			{
				return this.traversalProvider.CanTraverse(this, node);
			}
			return node.Walkable && (this.enabledTags >> (int)node.Tag & 1) != 0;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001411C File Offset: 0x0001231C
		public uint GetTraversalCost(GraphNode node)
		{
			if (this.traversalProvider != null)
			{
				return this.traversalProvider.GetTraversalCost(this, node);
			}
			return this.GetTagPenalty((int)node.Tag) + node.Penalty;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014147 File Offset: 0x00012347
		public virtual uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
		{
			return currentCost;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001414A File Offset: 0x0001234A
		public bool IsDone()
		{
			return this.PipelineState > PathState.Processing;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00014158 File Offset: 0x00012358
		void IPathInternals.AdvanceState(PathState s)
		{
			object obj = this.stateLock;
			lock (obj)
			{
				this.PipelineState = (PathState)Math.Max((int)this.PipelineState, (int)s);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000141A4 File Offset: 0x000123A4
		[Obsolete("Use the 'PipelineState' property instead")]
		public PathState GetState()
		{
			return this.PipelineState;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000141AC File Offset: 0x000123AC
		public void FailWithError(string msg)
		{
			this.Error();
			if (this.errorLog != "")
			{
				this.errorLog = this.errorLog + "\n" + msg;
				return;
			}
			this.errorLog = msg;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000141E5 File Offset: 0x000123E5
		[Obsolete("Use FailWithError instead")]
		protected void LogError(string msg)
		{
			this.Log(msg);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000141EE File Offset: 0x000123EE
		[Obsolete("Use FailWithError instead")]
		protected void Log(string msg)
		{
			this.errorLog += msg;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00014202 File Offset: 0x00012402
		public void Error()
		{
			this.CompleteState = PathCompleteState.Error;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001420C File Offset: 0x0001240C
		private void ErrorCheck()
		{
			if (!this.hasBeenReset)
			{
				this.FailWithError("Please use the static Construct function for creating paths, do not use the normal constructors.");
			}
			if (((IPathInternals)this).Pooled)
			{
				this.FailWithError("The path is currently in a path pool. Are you sending the path for calculation twice?");
			}
			if (this.pathHandler == null)
			{
				this.FailWithError("Field pathHandler is not set. Please report this bug.");
			}
			if (this.PipelineState > PathState.Processing)
			{
				this.FailWithError("This path has already been processed. Do not request a path with the same path object twice.");
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00014268 File Offset: 0x00012468
		protected virtual void OnEnterPool()
		{
			if (this.vectorPath != null)
			{
				ListPool<Vector3>.Release(ref this.vectorPath);
			}
			if (this.path != null)
			{
				ListPool<GraphNode>.Release(ref this.path);
			}
			this.callback = null;
			this.immediateCallback = null;
			this.traversalProvider = null;
			this.pathHandler = null;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000142B8 File Offset: 0x000124B8
		protected virtual void Reset()
		{
			if (AstarPath.active == null)
			{
				throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
			}
			this.hasBeenReset = true;
			this.PipelineState = PathState.Created;
			this.releasedNotSilent = false;
			this.pathHandler = null;
			this.callback = null;
			this.immediateCallback = null;
			this.errorLog = "";
			this.completeState = PathCompleteState.NotCalculated;
			this.path = ListPool<GraphNode>.Claim();
			this.vectorPath = ListPool<Vector3>.Claim();
			this.currentR = null;
			this.duration = 0f;
			this.searchedNodes = 0;
			this.nnConstraint = PathNNConstraint.Default;
			this.next = null;
			this.heuristic = AstarPath.active.heuristic;
			this.heuristicScale = AstarPath.active.heuristicScale;
			this.enabledTags = -1;
			this.tagPenalties = null;
			this.pathID = AstarPath.active.GetNextPathID();
			this.hTarget = Int3.zero;
			this.hTargetNode = null;
			this.traversalProvider = null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000143AC File Offset: 0x000125AC
		public void Claim(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (this.claimed[i] == o)
				{
					throw new ArgumentException("You have already claimed the path with that object (" + ((o != null) ? o.ToString() : null) + "). Are you claiming the path with the same object twice?");
				}
			}
			this.claimed.Add(o);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00014419 File Offset: 0x00012619
		[Obsolete("Use Release(o, true) instead")]
		internal void ReleaseSilent(object o)
		{
			this.Release(o, true);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014424 File Offset: 0x00012624
		public void Release(object o, bool silent = false)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (this.claimed[i] == o)
				{
					this.claimed.RemoveAt(i);
					if (!silent)
					{
						this.releasedNotSilent = true;
					}
					if (this.claimed.Count == 0 && this.releasedNotSilent)
					{
						PathPool.Pool(this);
					}
					return;
				}
			}
			if (this.claimed.Count == 0)
			{
				throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + ((o != null) ? o.ToString() : null) + ") twice?\nCheck out the documentation on path pooling for help.");
			}
			throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + ((o != null) ? o.ToString() : null) + "). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000144E4 File Offset: 0x000126E4
		protected virtual void Trace(PathNode from)
		{
			PathNode pathNode = from;
			int num = 0;
			while (pathNode != null)
			{
				pathNode = pathNode.parent;
				num++;
				if (num > 16384)
				{
					Debug.LogWarning("Infinite loop? >16384 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
					break;
				}
			}
			if (this.path.Capacity < num)
			{
				this.path.Capacity = num;
			}
			if (this.vectorPath.Capacity < num)
			{
				this.vectorPath.Capacity = num;
			}
			pathNode = from;
			for (int i = 0; i < num; i++)
			{
				this.path.Add(pathNode.node);
				pathNode = pathNode.parent;
			}
			int num2 = num / 2;
			for (int j = 0; j < num2; j++)
			{
				GraphNode value = this.path[j];
				this.path[j] = this.path[num - j - 1];
				this.path[num - j - 1] = value;
			}
			for (int k = 0; k < num; k++)
			{
				this.vectorPath.Add((Vector3)this.path[k].position);
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000145FC File Offset: 0x000127FC
		protected void DebugStringPrefix(PathLog logMode, StringBuilder text)
		{
			text.Append(this.error ? "Path Failed : " : "Path Completed : ");
			text.Append("Computation Time ");
			text.Append(this.duration.ToString((logMode == PathLog.Heavy) ? "0.000 ms " : "0.00 ms "));
			text.Append("Searched Nodes ").Append(this.searchedNodes);
			if (!this.error)
			{
				text.Append(" Path Length ");
				text.Append((this.path == null) ? "Null" : this.path.Count.ToString());
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000146A8 File Offset: 0x000128A8
		protected void DebugStringSuffix(PathLog logMode, StringBuilder text)
		{
			if (this.error)
			{
				text.Append("\nError: ").Append(this.errorLog);
			}
			if (logMode == PathLog.Heavy && !AstarPath.active.IsUsingMultithreading)
			{
				text.Append("\nCallback references ");
				if (this.callback != null)
				{
					text.Append(this.callback.Target.GetType().FullName).AppendLine();
				}
				else
				{
					text.AppendLine("NULL");
				}
			}
			text.Append("\nPath Number ").Append(this.pathID).Append(" (unique id)");
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00014748 File Offset: 0x00012948
		protected virtual string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!this.error && logMode == PathLog.OnlyErrors))
			{
				return "";
			}
			StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			this.DebugStringPrefix(logMode, debugStringBuilder);
			this.DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014793 File Offset: 0x00012993
		protected virtual void ReturnPath()
		{
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000147AC File Offset: 0x000129AC
		protected void PrepareBase(PathHandler pathHandler)
		{
			if (pathHandler.PathID > this.pathID)
			{
				pathHandler.ClearPathIDs();
			}
			this.pathHandler = pathHandler;
			pathHandler.InitializeForPath(this);
			if (this.internalTagPenalties == null || this.internalTagPenalties.Length != 32)
			{
				this.internalTagPenalties = Path.ZeroTagPenalties;
			}
			try
			{
				this.ErrorCheck();
			}
			catch (Exception ex)
			{
				this.FailWithError(ex.Message);
			}
		}

		// Token: 0x060003E7 RID: 999
		protected abstract void Prepare();

		// Token: 0x060003E8 RID: 1000 RVA: 0x00014824 File Offset: 0x00012A24
		protected virtual void Cleanup()
		{
		}

		// Token: 0x060003E9 RID: 1001
		protected abstract void Initialize();

		// Token: 0x060003EA RID: 1002
		protected abstract void CalculateStep(long targetTick);

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00014826 File Offset: 0x00012A26
		PathHandler IPathInternals.PathHandler
		{
			get
			{
				return this.pathHandler;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001482E File Offset: 0x00012A2E
		void IPathInternals.OnEnterPool()
		{
			this.OnEnterPool();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00014836 File Offset: 0x00012A36
		void IPathInternals.Reset()
		{
			this.Reset();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001483E File Offset: 0x00012A3E
		void IPathInternals.ReturnPath()
		{
			this.ReturnPath();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00014846 File Offset: 0x00012A46
		void IPathInternals.PrepareBase(PathHandler handler)
		{
			this.PrepareBase(handler);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001484F File Offset: 0x00012A4F
		void IPathInternals.Prepare()
		{
			this.Prepare();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00014857 File Offset: 0x00012A57
		void IPathInternals.Cleanup()
		{
			this.Cleanup();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001485F File Offset: 0x00012A5F
		void IPathInternals.Initialize()
		{
			this.Initialize();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00014867 File Offset: 0x00012A67
		void IPathInternals.CalculateStep(long targetTick)
		{
			this.CalculateStep(targetTick);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00014870 File Offset: 0x00012A70
		string IPathInternals.DebugString(PathLog logMode)
		{
			return this.DebugString(logMode);
		}

		// Token: 0x04000247 RID: 583
		protected PathHandler pathHandler;

		// Token: 0x04000248 RID: 584
		public OnPathDelegate callback;

		// Token: 0x04000249 RID: 585
		public OnPathDelegate immediateCallback;

		// Token: 0x0400024B RID: 587
		private object stateLock = new object();

		// Token: 0x0400024C RID: 588
		public ITraversalProvider traversalProvider;

		// Token: 0x0400024D RID: 589
		protected PathCompleteState completeState;

		// Token: 0x0400024F RID: 591
		public List<GraphNode> path;

		// Token: 0x04000250 RID: 592
		public List<Vector3> vectorPath;

		// Token: 0x04000251 RID: 593
		protected PathNode currentR;

		// Token: 0x04000252 RID: 594
		public float duration;

		// Token: 0x04000255 RID: 597
		protected bool hasBeenReset;

		// Token: 0x04000256 RID: 598
		public NNConstraint nnConstraint = PathNNConstraint.Default;

		// Token: 0x04000257 RID: 599
		internal Path next;

		// Token: 0x04000258 RID: 600
		public Heuristic heuristic;

		// Token: 0x04000259 RID: 601
		public float heuristicScale = 1f;

		// Token: 0x0400025B RID: 603
		protected GraphNode hTargetNode;

		// Token: 0x0400025C RID: 604
		protected Int3 hTarget;

		// Token: 0x0400025D RID: 605
		public int enabledTags = -1;

		// Token: 0x0400025E RID: 606
		private static readonly int[] ZeroTagPenalties = new int[32];

		// Token: 0x0400025F RID: 607
		protected int[] internalTagPenalties;

		// Token: 0x04000260 RID: 608
		protected int[] manualTagPenalties;

		// Token: 0x04000261 RID: 609
		private List<object> claimed = new List<object>();

		// Token: 0x04000262 RID: 610
		private bool releasedNotSilent;
	}
}
