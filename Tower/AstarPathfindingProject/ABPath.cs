using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200008B RID: 139
	public class ABPath : Path
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00028190 File Offset: 0x00026390
		protected virtual bool hasEndPoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002819B File Offset: 0x0002639B
		public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			path.Setup(start, end, callback);
			return path;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000281AB File Offset: 0x000263AB
		protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
		{
			this.callback = callbackDelegate;
			this.UpdateStartEnd(start, end);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000281BC File Offset: 0x000263BC
		public static ABPath FakePath(List<Vector3> vectorPath, List<GraphNode> nodePath = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			for (int i = 0; i < vectorPath.Count; i++)
			{
				path.vectorPath.Add(vectorPath[i]);
			}
			path.completeState = PathCompleteState.Complete;
			((IPathInternals)path).AdvanceState(PathState.Returned);
			if (vectorPath.Count > 0)
			{
				path.UpdateStartEnd(vectorPath[0], vectorPath[vectorPath.Count - 1]);
			}
			if (nodePath != null)
			{
				for (int j = 0; j < nodePath.Count; j++)
				{
					path.path.Add(nodePath[j]);
				}
				if (nodePath.Count > 0)
				{
					path.startNode = nodePath[0];
					path.endNode = nodePath[nodePath.Count - 1];
				}
			}
			return path;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00028275 File Offset: 0x00026475
		protected void UpdateStartEnd(Vector3 start, Vector3 end)
		{
			this.originalStartPoint = start;
			this.originalEndPoint = end;
			this.startPoint = start;
			this.endPoint = end;
			this.startIntPoint = (Int3)start;
			this.hTarget = (Int3)end;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000282AC File Offset: 0x000264AC
		public override uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
		{
			if (this.startNode != null && this.endNode != null)
			{
				if (a == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - ((b == this.endNode) ? this.hTarget : b.position)).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - ((a == this.endNode) ? this.hTarget : a.position)).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (a == this.endNode)
				{
					return (uint)((double)(this.hTarget - b.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.endNode)
				{
					return (uint)((double)(this.hTarget - a.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			else
			{
				if (a == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - b.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == this.startNode)
				{
					return (uint)((double)(this.startIntPoint - a.position).costMagnitude * (currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			return currentCost;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000284CC File Offset: 0x000266CC
		protected override void Reset()
		{
			base.Reset();
			this.startNode = null;
			this.endNode = null;
			this.originalStartPoint = Vector3.zero;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.endPoint = Vector3.zero;
			this.calculatePartial = false;
			this.partialBestTarget = null;
			this.startIntPoint = default(Int3);
			this.hTarget = default(Int3);
			this.endNodeCosts = null;
			this.gridSpecialCaseNode = null;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00028550 File Offset: 0x00026750
		protected virtual bool EndPointGridGraphSpecialCase(GraphNode closestWalkableEndNode)
		{
			GridNode gridNode = closestWalkableEndNode as GridNode;
			if (gridNode != null)
			{
				GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
				GridNode gridNode2 = AstarPath.active.GetNearest(this.originalEndPoint, ABPath.NNConstraintNone).node as GridNode;
				if (gridNode != gridNode2 && gridNode2 != null && gridNode.GraphIndex == gridNode2.GraphIndex)
				{
					int num = gridNode.NodeInGridIndex % gridGraph.width;
					int num2 = gridNode.NodeInGridIndex / gridGraph.width;
					int num3 = gridNode2.NodeInGridIndex % gridGraph.width;
					int num4 = gridNode2.NodeInGridIndex / gridGraph.width;
					bool flag = false;
					switch (gridGraph.neighbours)
					{
					case NumNeighbours.Four:
						if ((num == num3 && Math.Abs(num2 - num4) == 1) || (num2 == num4 && Math.Abs(num - num3) == 1))
						{
							flag = true;
						}
						break;
					case NumNeighbours.Eight:
						if (Math.Abs(num - num3) <= 1 && Math.Abs(num2 - num4) <= 1)
						{
							flag = true;
						}
						break;
					case NumNeighbours.Six:
						for (int i = 0; i < 6; i++)
						{
							int num5 = num3 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
							int num6 = num4 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
							if (num == num5 && num2 == num6)
							{
								flag = true;
								break;
							}
						}
						break;
					default:
						throw new Exception("Unhandled NumNeighbours");
					}
					if (flag)
					{
						this.SetFlagOnSurroundingGridNodes(gridNode2, 1, true);
						this.endPoint = (Vector3)gridNode2.position;
						this.hTarget = gridNode2.position;
						this.endNode = gridNode2;
						this.hTargetNode = this.endNode;
						this.gridSpecialCaseNode = gridNode2;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000286F8 File Offset: 0x000268F8
		private void SetFlagOnSurroundingGridNodes(GridNode gridNode, int flag, bool flagState)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
			int num = (gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6);
			int num2 = gridNode.NodeInGridIndex % gridGraph.width;
			int num3 = gridNode.NodeInGridIndex / gridGraph.width;
			if (flag != 1 && flag != 2)
			{
				throw new ArgumentOutOfRangeException("flag");
			}
			for (int i = 0; i < num; i++)
			{
				int num4;
				int num5;
				if (gridGraph.neighbours == NumNeighbours.Six)
				{
					num4 = num2 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
					num5 = num3 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
				}
				else
				{
					num4 = num2 + gridGraph.neighbourXOffsets[i];
					num5 = num3 + gridGraph.neighbourZOffsets[i];
				}
				if (num4 >= 0 && num5 >= 0 && num4 < gridGraph.width && num5 < gridGraph.depth)
				{
					GridNodeBase node = gridGraph.nodes[num5 * gridGraph.width + num4];
					PathNode pathNode = this.pathHandler.GetPathNode(node);
					if (flag == 1)
					{
						pathNode.flag1 = flagState;
					}
					else
					{
						pathNode.flag2 = flagState;
					}
				}
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00028814 File Offset: 0x00026A14
		protected override void Prepare()
		{
			this.nnConstraint.tags = this.enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint);
			PathNNConstraint pathNNConstraint = this.nnConstraint as PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			this.startPoint = nearest.position;
			this.startIntPoint = (Int3)this.startPoint;
			this.startNode = nearest.node;
			if (this.startNode == null)
			{
				base.FailWithError("Couldn't find a node close to the start point");
				return;
			}
			if (!base.CanTraverse(this.startNode))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			if (this.hasEndPoint)
			{
				NNInfo nearest2 = AstarPath.active.GetNearest(this.endPoint, this.nnConstraint);
				this.endPoint = nearest2.position;
				this.endNode = nearest2.node;
				if (this.endNode == null)
				{
					base.FailWithError("Couldn't find a node close to the end point");
					return;
				}
				if (!base.CanTraverse(this.endNode))
				{
					base.FailWithError("The node closest to the end point could not be traversed");
					return;
				}
				if (this.startNode.Area != this.endNode.Area)
				{
					base.FailWithError("There is no valid path to the target");
					return;
				}
				if (!this.EndPointGridGraphSpecialCase(nearest2.node))
				{
					this.hTarget = (Int3)this.endPoint;
					this.hTargetNode = this.endNode;
					this.pathHandler.GetPathNode(this.endNode).flag1 = true;
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002898C File Offset: 0x00026B8C
		protected virtual void CompletePathIfStartIsValidTarget()
		{
			if (this.hasEndPoint && this.pathHandler.GetPathNode(this.startNode).flag1)
			{
				this.CompleteWith(this.startNode);
				this.Trace(this.pathHandler.GetPathNode(this.startNode));
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000289DC File Offset: 0x00026BDC
		protected override void Initialize()
		{
			if (this.startNode != null)
			{
				this.pathHandler.GetPathNode(this.startNode).flag2 = true;
			}
			if (this.endNode != null)
			{
				this.pathHandler.GetPathNode(this.endNode).flag2 = true;
			}
			PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
			pathNode.node = this.startNode;
			pathNode.pathID = this.pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0U;
			pathNode.G = base.GetTraversalCost(this.startNode);
			pathNode.H = base.CalculateHScore(this.startNode);
			this.CompletePathIfStartIsValidTarget();
			if (base.CompleteState == PathCompleteState.Complete)
			{
				return;
			}
			this.startNode.Open(this, pathNode, this.pathHandler);
			int searchedNodes = base.searchedNodes;
			base.searchedNodes = searchedNodes + 1;
			this.partialBestTarget = pathNode;
			if (!this.pathHandler.heap.isEmpty)
			{
				this.currentR = this.pathHandler.heap.Remove();
				return;
			}
			if (this.calculatePartial)
			{
				this.CompletePartial(this.partialBestTarget);
				return;
			}
			base.FailWithError("The start node either had no neighbours, or no neighbours that the path could traverse");
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00028B0C File Offset: 0x00026D0C
		protected override void Cleanup()
		{
			if (this.startNode != null)
			{
				PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
				pathNode.flag1 = false;
				pathNode.flag2 = false;
			}
			if (this.endNode != null)
			{
				PathNode pathNode2 = this.pathHandler.GetPathNode(this.endNode);
				pathNode2.flag1 = false;
				pathNode2.flag2 = false;
			}
			if (this.gridSpecialCaseNode != null)
			{
				PathNode pathNode3 = this.pathHandler.GetPathNode(this.gridSpecialCaseNode);
				pathNode3.flag1 = false;
				pathNode3.flag2 = false;
				this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 1, false);
				this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 2, false);
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00028BA7 File Offset: 0x00026DA7
		private void CompletePartial(PathNode node)
		{
			base.CompleteState = PathCompleteState.Partial;
			this.endNode = node.node;
			this.endPoint = this.endNode.ClosestPointOnNode(this.originalEndPoint);
			this.Trace(node);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00028BDC File Offset: 0x00026DDC
		private void CompleteWith(GraphNode node)
		{
			if (this.endNode != node)
			{
				GridNode gridNode = node as GridNode;
				if (gridNode == null)
				{
					throw new Exception("Some path is not cleaning up the flag1 field. This is a bug.");
				}
				this.endPoint = gridNode.ClosestPointOnNode(this.originalEndPoint);
				this.endNode = node;
			}
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00028C28 File Offset: 0x00026E28
		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = base.searchedNodes;
				base.searchedNodes = searchedNodes + 1;
				if (this.currentR.flag1)
				{
					this.CompleteWith(this.currentR.node);
					break;
				}
				if (this.currentR.H < this.partialBestTarget.H)
				{
					this.partialBestTarget = this.currentR;
				}
				this.currentR.node.Open(this, this.currentR, this.pathHandler);
				if (this.pathHandler.heap.isEmpty)
				{
					if (this.calculatePartial && this.partialBestTarget != null)
					{
						this.CompletePartial(this.partialBestTarget);
						return;
					}
					base.FailWithError("Searched all reachable nodes, but could not find target. This can happen if you have nodes with a different tag blocking the way to the goal. You can enable path.calculatePartial to handle that case workaround (though this comes with a performance cost).");
					return;
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
				this.Trace(this.currentR);
				return;
			}
			if (this.calculatePartial && this.partialBestTarget != null)
			{
				this.CompletePartial(this.partialBestTarget);
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00028D78 File Offset: 0x00026F78
		protected override string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			base.DebugStringPrefix(logMode, stringBuilder);
			if (!base.error && logMode == PathLog.Heavy)
			{
				Vector3 vector;
				if (this.hasEndPoint && this.endNode != null)
				{
					PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
					stringBuilder.Append("\nEnd Node\n\tG: ");
					stringBuilder.Append(pathNode.G);
					stringBuilder.Append("\n\tH: ");
					stringBuilder.Append(pathNode.H);
					stringBuilder.Append("\n\tF: ");
					stringBuilder.Append(pathNode.F);
					stringBuilder.Append("\n\tPoint: ");
					StringBuilder stringBuilder2 = stringBuilder;
					vector = this.endPoint;
					stringBuilder2.Append(vector.ToString());
					stringBuilder.Append("\n\tGraph: ");
					stringBuilder.Append(this.endNode.GraphIndex);
				}
				stringBuilder.Append("\nStart Node");
				stringBuilder.Append("\n\tPoint: ");
				StringBuilder stringBuilder3 = stringBuilder;
				vector = this.startPoint;
				stringBuilder3.Append(vector.ToString());
				stringBuilder.Append("\n\tGraph: ");
				if (this.startNode != null)
				{
					stringBuilder.Append(this.startNode.GraphIndex);
				}
				else
				{
					stringBuilder.Append("< null startNode >");
				}
			}
			base.DebugStringSuffix(logMode, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00028EE8 File Offset: 0x000270E8
		[Obsolete]
		public Vector3 GetMovementVector(Vector3 point)
		{
			if (this.vectorPath == null || this.vectorPath.Count == 0)
			{
				return Vector3.zero;
			}
			if (this.vectorPath.Count == 1)
			{
				return this.vectorPath[0] - point;
			}
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				float sqrMagnitude = (VectorMath.ClosestPointOnSegment(this.vectorPath[i], this.vectorPath[i + 1], point) - point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					num2 = i;
				}
			}
			return this.vectorPath[num2 + 1] - point;
		}

		// Token: 0x040003E5 RID: 997
		public GraphNode startNode;

		// Token: 0x040003E6 RID: 998
		public GraphNode endNode;

		// Token: 0x040003E7 RID: 999
		public Vector3 originalStartPoint;

		// Token: 0x040003E8 RID: 1000
		public Vector3 originalEndPoint;

		// Token: 0x040003E9 RID: 1001
		public Vector3 startPoint;

		// Token: 0x040003EA RID: 1002
		public Vector3 endPoint;

		// Token: 0x040003EB RID: 1003
		public Int3 startIntPoint;

		// Token: 0x040003EC RID: 1004
		public bool calculatePartial;

		// Token: 0x040003ED RID: 1005
		protected PathNode partialBestTarget;

		// Token: 0x040003EE RID: 1006
		protected int[] endNodeCosts;

		// Token: 0x040003EF RID: 1007
		private GridNode gridSpecialCaseNode;

		// Token: 0x040003F0 RID: 1008
		private static readonly NNConstraint NNConstraintNone = NNConstraint.None;
	}
}
