﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class EuclideanEmbedding
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00022826 File Offset: 0x00020A26
		private uint GetRandom()
		{
			this.rval = 12820163U * this.rval + 1140671485U;
			return this.rval;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00022848 File Offset: 0x00020A48
		private void EnsureCapacity(int index)
		{
			if (index > this.maxNodeIndex)
			{
				object obj = this.lockObj;
				lock (obj)
				{
					if (index > this.maxNodeIndex)
					{
						if (index >= this.costs.Length)
						{
							uint[] array = new uint[Math.Max(index * 2, this.pivots.Length * 2)];
							for (int i = 0; i < this.costs.Length; i++)
							{
								array[i] = this.costs[i];
							}
							this.costs = array;
						}
						this.maxNodeIndex = index;
					}
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000228E4 File Offset: 0x00020AE4
		public uint GetHeuristic(int nodeIndex1, int nodeIndex2)
		{
			nodeIndex1 *= this.pivotCount;
			nodeIndex2 *= this.pivotCount;
			if (nodeIndex1 >= this.costs.Length || nodeIndex2 >= this.costs.Length)
			{
				this.EnsureCapacity((nodeIndex1 > nodeIndex2) ? nodeIndex1 : nodeIndex2);
			}
			uint num = 0U;
			for (int i = 0; i < this.pivotCount; i++)
			{
				uint num2 = (uint)Math.Abs((int)(this.costs[nodeIndex1 + i] - this.costs[nodeIndex2 + i]));
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00022960 File Offset: 0x00020B60
		private void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
		{
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				NNInfo nearest = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
				if (nearest.node != null && nearest.node.Walkable)
				{
					nodes.Add(nearest.node);
				}
				this.GetClosestWalkableNodesToChildrenRecursively(transform, nodes);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000229EC File Offset: 0x00020BEC
		private void PickNRandomNodes(int count, List<GraphNode> buffer)
		{
			int n = 0;
			Action<GraphNode> <>9__0;
			foreach (NavGraph navGraph in AstarPath.active.graphs)
			{
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (!node.Destroyed && node.Walkable)
						{
							int n = n;
							n++;
							if ((ulong)this.GetRandom() % (ulong)((long)n) < (ulong)((long)count))
							{
								if (buffer.Count < count)
								{
									buffer.Add(node);
									return;
								}
								buffer[(int)((ulong)this.GetRandom() % (ulong)((long)buffer.Count))] = node;
							}
						}
					});
				}
				navGraph.GetNodes(action);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00022A5C File Offset: 0x00020C5C
		private GraphNode PickAnyWalkableNode()
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			GraphNode first = null;
			Action<GraphNode> <>9__0;
			foreach (NavGraph navGraph in graphs)
			{
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (node != null && node.Walkable && first == null)
						{
							first = node;
						}
					});
				}
				navGraph.GetNodes(action);
			}
			return first;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00022ABC File Offset: 0x00020CBC
		public void RecalculatePivots()
		{
			if (this.mode == HeuristicOptimizationMode.None)
			{
				this.pivotCount = 0;
				this.pivots = null;
				return;
			}
			this.rval = (uint)this.seed;
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			switch (this.mode)
			{
			case HeuristicOptimizationMode.Random:
				this.PickNRandomNodes(this.spreadOutCount, list);
				break;
			case HeuristicOptimizationMode.RandomSpreadOut:
			{
				if (this.pivotPointRoot != null)
				{
					this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				}
				if (list.Count == 0)
				{
					GraphNode graphNode = this.PickAnyWalkableNode();
					if (graphNode == null)
					{
						Debug.LogError("Could not find any walkable node in any of the graphs.");
						ListPool<GraphNode>.Release(ref list);
						return;
					}
					list.Add(graphNode);
				}
				int num = this.spreadOutCount - list.Count;
				for (int i = 0; i < num; i++)
				{
					list.Add(null);
				}
				break;
			}
			case HeuristicOptimizationMode.Custom:
				if (this.pivotPointRoot == null)
				{
					throw new Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
				}
				this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				break;
			default:
				throw new Exception("Invalid HeuristicOptimizationMode: " + this.mode.ToString());
			}
			this.pivots = list.ToArray();
			ListPool<GraphNode>.Release(ref list);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00022BF4 File Offset: 0x00020DF4
		public void RecalculateCosts()
		{
			if (this.pivots == null)
			{
				this.RecalculatePivots();
			}
			if (this.mode == HeuristicOptimizationMode.None)
			{
				return;
			}
			this.pivotCount = 0;
			for (int i = 0; i < this.pivots.Length; i++)
			{
				if (this.pivots[i] != null && (this.pivots[i].Destroyed || !this.pivots[i].Walkable))
				{
					throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
				}
			}
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int j = 0; j < this.pivots.Length; j++)
				{
					if (this.pivots[j] == null)
					{
						throw new Exception("Invalid pivot nodes (null)");
					}
				}
			}
			Debug.Log("Recalculating costs...");
			this.pivotCount = this.pivots.Length;
			Action<int> startCostCalculation = null;
			int numComplete = 0;
			OnPathDelegate onComplete = delegate(Path path)
			{
				int numComplete = numComplete;
				numComplete++;
				if (numComplete == this.pivotCount)
				{
					this.ApplyGridGraphEndpointSpecialCase();
				}
			};
			startCostCalculation = delegate(int pivotIndex)
			{
				GraphNode pivot = this.pivots[pivotIndex];
				FloodPath floodPath = null;
				floodPath = FloodPath.Construct(pivot, onComplete);
				floodPath.immediateCallback = delegate(Path _p)
				{
					_p.Claim(this);
					MeshNode meshNode = pivot as MeshNode;
					uint costOffset = 0U;
					if (meshNode != null && meshNode.connections != null)
					{
						for (int l = 0; l < meshNode.connections.Length; l++)
						{
							costOffset = Math.Max(costOffset, meshNode.connections[l].cost);
						}
					}
					NavGraph[] graphs = AstarPath.active.graphs;
					Action<GraphNode> <>9__3;
					for (int m = graphs.Length - 1; m >= 0; m--)
					{
						NavGraph navGraph = graphs[m];
						Action<GraphNode> action;
						if ((action = <>9__3) == null)
						{
							action = (<>9__3 = delegate(GraphNode node)
							{
								int num6 = node.NodeIndex * this.pivotCount + pivotIndex;
								this.EnsureCapacity(num6);
								PathNode pathNode = ((IPathInternals)floodPath).PathHandler.GetPathNode(node);
								if (costOffset > 0U)
								{
									this.costs[num6] = ((pathNode.pathID == floodPath.pathID && pathNode.parent != null) ? Math.Max(pathNode.parent.G - costOffset, 0U) : 0U);
									return;
								}
								this.costs[num6] = ((pathNode.pathID == floodPath.pathID) ? pathNode.G : 0U);
							});
						}
						navGraph.GetNodes(action);
					}
					if (this.mode == HeuristicOptimizationMode.RandomSpreadOut && pivotIndex < this.pivots.Length - 1)
					{
						if (this.pivots[pivotIndex + 1] == null)
						{
							int num = -1;
							uint num2 = 0U;
							int num3 = this.maxNodeIndex / this.pivotCount;
							for (int n = 1; n < num3; n++)
							{
								uint num4 = 1073741824U;
								for (int num5 = 0; num5 <= pivotIndex; num5++)
								{
									num4 = Math.Min(num4, this.costs[n * this.pivotCount + num5]);
								}
								GraphNode node2 = ((IPathInternals)floodPath).PathHandler.GetPathNode(n).node;
								if ((num4 > num2 || num == -1) && node2 != null && !node2.Destroyed && node2.Walkable)
								{
									num = n;
									num2 = num4;
								}
							}
							if (num == -1)
							{
								Debug.LogError("Failed generating random pivot points for heuristic optimizations");
								return;
							}
							this.pivots[pivotIndex + 1] = ((IPathInternals)floodPath).PathHandler.GetPathNode(num).node;
						}
						startCostCalculation(pivotIndex + 1);
					}
					_p.Release(this, false);
				};
				AstarPath.StartPath(floodPath, true);
			};
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int k = 0; k < this.pivots.Length; k++)
				{
					startCostCalculation(k);
				}
			}
			else
			{
				startCostCalculation(0);
			}
			this.dirty = false;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00022D2C File Offset: 0x00020F2C
		private void ApplyGridGraphEndpointSpecialCase()
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				GridGraph gridGraph = graphs[i] as GridGraph;
				if (gridGraph != null)
				{
					GridNodeBase[] nodes = gridGraph.nodes;
					int num = (gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6);
					for (int j = 0; j < gridGraph.depth; j++)
					{
						for (int k = 0; k < gridGraph.width; k++)
						{
							GridNodeBase gridNodeBase = nodes[j * gridGraph.width + k];
							if (!gridNodeBase.Walkable)
							{
								int num2 = gridNodeBase.NodeIndex * this.pivotCount;
								for (int l = 0; l < this.pivotCount; l++)
								{
									this.costs[num2 + l] = uint.MaxValue;
								}
								for (int m = 0; m < num; m++)
								{
									int num3;
									int num4;
									if (gridGraph.neighbours == NumNeighbours.Six)
									{
										num3 = k + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[m]];
										num4 = j + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[m]];
									}
									else
									{
										num3 = k + gridGraph.neighbourXOffsets[m];
										num4 = j + gridGraph.neighbourZOffsets[m];
									}
									if (num3 >= 0 && num4 >= 0 && num3 < gridGraph.width && num4 < gridGraph.depth)
									{
										GridNodeBase gridNodeBase2 = gridGraph.nodes[num4 * gridGraph.width + num3];
										if (gridNodeBase2.Walkable)
										{
											for (int n = 0; n < this.pivotCount; n++)
											{
												uint val = this.costs[gridNodeBase2.NodeIndex * this.pivotCount + n] + gridGraph.neighbourCosts[m];
												this.costs[num2 + n] = Math.Min(this.costs[num2 + n], val);
											}
										}
									}
								}
								for (int num5 = 0; num5 < this.pivotCount; num5++)
								{
									if (this.costs[num2 + num5] == 4294967295U)
									{
										this.costs[num2 + num5] = 0U;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00022F40 File Offset: 0x00021140
		public void OnDrawGizmos()
		{
			if (this.pivots != null)
			{
				for (int i = 0; i < this.pivots.Length; i++)
				{
					Gizmos.color = new Color(0.62352943f, 0.36862746f, 0.7607843f, 0.8f);
					if (this.pivots[i] != null && !this.pivots[i].Destroyed)
					{
						Gizmos.DrawCube((Vector3)this.pivots[i].position, Vector3.one);
					}
				}
			}
		}

		// Token: 0x04000356 RID: 854
		public HeuristicOptimizationMode mode;

		// Token: 0x04000357 RID: 855
		public int seed;

		// Token: 0x04000358 RID: 856
		public Transform pivotPointRoot;

		// Token: 0x04000359 RID: 857
		public int spreadOutCount = 1;

		// Token: 0x0400035A RID: 858
		[NonSerialized]
		public bool dirty;

		// Token: 0x0400035B RID: 859
		private uint[] costs = new uint[8];

		// Token: 0x0400035C RID: 860
		private int maxNodeIndex;

		// Token: 0x0400035D RID: 861
		private int pivotCount;

		// Token: 0x0400035E RID: 862
		private GraphNode[] pivots;

		// Token: 0x0400035F RID: 863
		private const uint ra = 12820163U;

		// Token: 0x04000360 RID: 864
		private const uint rc = 1140671485U;

		// Token: 0x04000361 RID: 865
		private uint rval;

		// Token: 0x04000362 RID: 866
		private object lockObj = new object();
	}
}
