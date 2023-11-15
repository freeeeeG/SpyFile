using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200008F RID: 143
	public class FloodPath : Path
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0002932D File Offset: 0x0002752D
		public override bool FloodingPath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00029330 File Offset: 0x00027530
		public bool HasPathTo(GraphNode node)
		{
			return this.parents != null && this.parents.ContainsKey(node);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00029348 File Offset: 0x00027548
		public GraphNode GetParent(GraphNode node)
		{
			return this.parents[node];
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00029365 File Offset: 0x00027565
		public static FloodPath Construct(Vector3 start, OnPathDelegate callback = null)
		{
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00029374 File Offset: 0x00027574
		public static FloodPath Construct(GraphNode start, OnPathDelegate callback = null)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00029391 File Offset: 0x00027591
		protected void Setup(Vector3 start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = start;
			this.startPoint = start;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000293AF File Offset: 0x000275AF
		protected void Setup(GraphNode start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = (Vector3)start.position;
			this.startNode = start;
			this.startPoint = (Vector3)start.position;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000293E8 File Offset: 0x000275E8
		protected override void Reset()
		{
			base.Reset();
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.parents = new Dictionary<GraphNode, GraphNode>();
			this.saveParents = true;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00029420 File Offset: 0x00027620
		protected override void Prepare()
		{
			if (this.startNode == null)
			{
				this.nnConstraint.tags = this.enabledTags;
				NNInfo nearest = AstarPath.active.GetNearest(this.originalStartPoint, this.nnConstraint);
				this.startPoint = nearest.position;
				this.startNode = nearest.node;
			}
			else
			{
				if (this.startNode.Destroyed)
				{
					base.FailWithError("Start node has been destroyed");
					return;
				}
				this.startPoint = (Vector3)this.startNode.position;
			}
			if (this.startNode == null)
			{
				base.FailWithError("Couldn't find a close node to the start point");
				return;
			}
			if (!base.CanTraverse(this.startNode))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000294D4 File Offset: 0x000276D4
		protected override void Initialize()
		{
			PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = this.pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.parents[this.startNode] = null;
			this.startNode.Open(this, pathNode, this.pathHandler);
			int searchedNodes = base.searchedNodes;
			base.searchedNodes = searchedNodes + 1;
			if (this.pathHandler.heap.isEmpty)
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			this.currentR = this.pathHandler.heap.Remove();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000295A8 File Offset: 0x000277A8
		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = base.searchedNodes;
				base.searchedNodes = searchedNodes + 1;
				this.currentR.node.Open(this, this.currentR, this.pathHandler);
				if (this.saveParents)
				{
					this.parents[this.currentR.node] = this.currentR.parent.node;
				}
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

		// Token: 0x040003F7 RID: 1015
		public Vector3 originalStartPoint;

		// Token: 0x040003F8 RID: 1016
		public Vector3 startPoint;

		// Token: 0x040003F9 RID: 1017
		public GraphNode startNode;

		// Token: 0x040003FA RID: 1018
		public bool saveParents = true;

		// Token: 0x040003FB RID: 1019
		protected Dictionary<GraphNode, GraphNode> parents;
	}
}
