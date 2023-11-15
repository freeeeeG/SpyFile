using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000093 RID: 147
	public class RandomPath : ABPath
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002A76F File Offset: 0x0002896F
		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0002A772 File Offset: 0x00028972
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002A778 File Offset: 0x00028978
		protected override void Reset()
		{
			base.Reset();
			this.searchLength = 5000;
			this.spread = 5000;
			this.aimStrength = 0f;
			this.chosenNodeR = null;
			this.maxGScoreNodeR = null;
			this.maxGScore = 0;
			this.aim = Vector3.zero;
			this.nodesEvaluatedRep = 0;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002A7F1 File Offset: 0x000289F1
		public static RandomPath Construct(Vector3 start, int length, OnPathDelegate callback = null)
		{
			RandomPath path = PathPool.GetPath<RandomPath>();
			path.Setup(start, length, callback);
			return path;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002A804 File Offset: 0x00028A04
		protected RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
		{
			this.callback = callback;
			this.searchLength = length;
			this.originalStartPoint = start;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = start;
			this.endPoint = Vector3.zero;
			this.startIntPoint = (Int3)start;
			return this;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002A850 File Offset: 0x00028A50
		protected override void ReturnPath()
		{
			if (this.path != null && this.path.Count > 0)
			{
				this.endNode = this.path[this.path.Count - 1];
				this.endPoint = (Vector3)this.endNode.position;
				this.originalEndPoint = this.endPoint;
				this.hTarget = this.endNode.position;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002A8D8 File Offset: 0x00028AD8
		protected override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint);
			this.startPoint = nearest.position;
			this.endPoint = this.startPoint;
			this.startIntPoint = (Int3)this.startPoint;
			this.hTarget = (Int3)this.aim;
			this.startNode = nearest.node;
			this.endNode = this.startNode;
			if (this.startNode == null || this.endNode == null)
			{
				base.FailWithError("Couldn't find close nodes to the start point");
				return;
			}
			if (!base.CanTraverse(this.startNode))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			this.heuristicScale = this.aimStrength;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002A9A4 File Offset: 0x00028BA4
		protected override void Initialize()
		{
			PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			if (this.searchLength + this.spread <= 0)
			{
				base.CompleteState = PathCompleteState.Complete;
				this.Trace(pathNode);
				return;
			}
			pathNode.pathID = base.pathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.startNode.Open(this, pathNode, this.pathHandler);
			int searchedNodes = base.searchedNodes;
			base.searchedNodes = searchedNodes + 1;
			if (this.pathHandler.heap.isEmpty)
			{
				base.FailWithError("No open points, the start node didn't open any nodes");
				return;
			}
			this.currentR = this.pathHandler.heap.Remove();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002AA84 File Offset: 0x00028C84
		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = base.searchedNodes;
				base.searchedNodes = searchedNodes + 1;
				if ((ulong)this.currentR.G >= (ulong)((long)this.searchLength))
				{
					if ((ulong)this.currentR.G > (ulong)((long)(this.searchLength + this.spread)))
					{
						if (this.chosenNodeR == null)
						{
							this.chosenNodeR = this.currentR;
						}
						base.CompleteState = PathCompleteState.Complete;
						break;
					}
					this.nodesEvaluatedRep++;
					if (this.rnd.NextDouble() <= (double)(1f / (float)this.nodesEvaluatedRep))
					{
						this.chosenNodeR = this.currentR;
					}
				}
				else if ((ulong)this.currentR.G > (ulong)((long)this.maxGScore))
				{
					this.maxGScore = (int)this.currentR.G;
					this.maxGScoreNodeR = this.currentR;
				}
				this.currentR.node.Open(this, this.currentR, this.pathHandler);
				if (this.pathHandler.heap.isEmpty)
				{
					if (this.chosenNodeR != null)
					{
						base.CompleteState = PathCompleteState.Complete;
						break;
					}
					if (this.maxGScoreNodeR != null)
					{
						this.chosenNodeR = this.maxGScoreNodeR;
						base.CompleteState = PathCompleteState.Complete;
						break;
					}
					base.FailWithError("Not a single node found to search");
					break;
				}
				else
				{
					this.currentR = this.pathHandler.heap.Remove();
					if (num > 500)
					{
						if (DateTime.UtcNow.Ticks >= targetTick)
						{
							return;
						}
						num = 0;
						if (base.searchedNodes > 1000000)
						{
							throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
						}
					}
					num++;
				}
			}
			if (base.CompleteState == PathCompleteState.Complete)
			{
				this.Trace(this.chosenNodeR);
			}
		}

		// Token: 0x0400040B RID: 1035
		public int searchLength;

		// Token: 0x0400040C RID: 1036
		public int spread = 5000;

		// Token: 0x0400040D RID: 1037
		public float aimStrength;

		// Token: 0x0400040E RID: 1038
		private PathNode chosenNodeR;

		// Token: 0x0400040F RID: 1039
		private PathNode maxGScoreNodeR;

		// Token: 0x04000410 RID: 1040
		private int maxGScore;

		// Token: 0x04000411 RID: 1041
		public Vector3 aim;

		// Token: 0x04000412 RID: 1042
		private int nodesEvaluatedRep;

		// Token: 0x04000413 RID: 1043
		private readonly Random rnd = new Random();
	}
}
