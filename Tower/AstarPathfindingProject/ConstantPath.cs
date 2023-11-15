using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200008C RID: 140
	public class ConstantPath : Path
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00028FA9 File Offset: 0x000271A9
		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00028FAC File Offset: 0x000271AC
		public static ConstantPath Construct(Vector3 start, int maxGScore, OnPathDelegate callback = null)
		{
			ConstantPath path = PathPool.GetPath<ConstantPath>();
			path.Setup(start, maxGScore, callback);
			return path;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00028FBC File Offset: 0x000271BC
		protected void Setup(Vector3 start, int maxGScore, OnPathDelegate callback)
		{
			this.callback = callback;
			this.startPoint = start;
			this.originalStartPoint = this.startPoint;
			this.endingCondition = new EndingConditionDistance(this, maxGScore);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00028FE5 File Offset: 0x000271E5
		protected override void OnEnterPool()
		{
			base.OnEnterPool();
			if (this.allNodes != null)
			{
				ListPool<GraphNode>.Release(ref this.allNodes);
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00029000 File Offset: 0x00027200
		protected override void Reset()
		{
			base.Reset();
			this.allNodes = ListPool<GraphNode>.Claim();
			this.endingCondition = null;
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00029040 File Offset: 0x00027240
		protected override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint);
			this.startNode = nearest.node;
			if (this.startNode == null)
			{
				base.FailWithError("Could not find close node to the start point");
				return;
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00029098 File Offset: 0x00027298
		protected override void Initialize()
		{
			PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = this.pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.startNode.Open(this, pathNode, this.pathHandler);
			int searchedNodes = base.searchedNodes;
			base.searchedNodes = searchedNodes + 1;
			pathNode.flag1 = true;
			this.allNodes.Add(this.startNode);
			if (this.pathHandler.heap.isEmpty)
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			this.currentR = this.pathHandler.heap.Remove();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00029174 File Offset: 0x00027374
		protected override void Cleanup()
		{
			int count = this.allNodes.Count;
			for (int i = 0; i < count; i++)
			{
				this.pathHandler.GetPathNode(this.allNodes[i]).flag1 = false;
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000291B8 File Offset: 0x000273B8
		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = base.searchedNodes;
				base.searchedNodes = searchedNodes + 1;
				if (this.endingCondition.TargetFound(this.currentR))
				{
					base.CompleteState = PathCompleteState.Complete;
					return;
				}
				if (!this.currentR.flag1)
				{
					this.allNodes.Add(this.currentR.node);
					this.currentR.flag1 = true;
				}
				this.currentR.node.Open(this, this.currentR, this.pathHandler);
				if (this.pathHandler.heap.isEmpty)
				{
					base.CompleteState = PathCompleteState.Complete;
					return;
				}
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

		// Token: 0x040003F1 RID: 1009
		public GraphNode startNode;

		// Token: 0x040003F2 RID: 1010
		public Vector3 startPoint;

		// Token: 0x040003F3 RID: 1011
		public Vector3 originalStartPoint;

		// Token: 0x040003F4 RID: 1012
		public List<GraphNode> allNodes;

		// Token: 0x040003F5 RID: 1013
		public PathEndingCondition endingCondition;
	}
}
