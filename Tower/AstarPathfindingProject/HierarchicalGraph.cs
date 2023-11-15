using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200003D RID: 61
	public class HierarchicalGraph
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000F905 File Offset: 0x0000DB05
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000F90D File Offset: 0x0000DB0D
		public int version { get; private set; }

		// Token: 0x060002C3 RID: 707 RVA: 0x0000F918 File Offset: 0x0000DB18
		public HierarchicalGraph()
		{
			this.connectionCallback = delegate(GraphNode neighbour)
			{
				int hierarchicalNodeIndex = neighbour.HierarchicalNodeIndex;
				if (hierarchicalNodeIndex == 0)
				{
					if (this.currentChildren.Count < 256 && neighbour.Walkable)
					{
						neighbour.HierarchicalNodeIndex = this.currentHierarchicalNodeIndex;
						this.temporaryQueue.Enqueue(neighbour);
						this.currentChildren.Add(neighbour);
						return;
					}
				}
				else if (hierarchicalNodeIndex != this.currentHierarchicalNodeIndex && !this.currentConnections.Contains(hierarchicalNodeIndex))
				{
					this.currentConnections.Add(hierarchicalNodeIndex);
				}
			};
			this.Grow();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		private void Grow()
		{
			List<GraphNode>[] array = new List<GraphNode>[Math.Max(64, this.children.Length * 2)];
			List<int>[] array2 = new List<int>[array.Length];
			int[] array3 = new int[array.Length];
			byte[] array4 = new byte[array.Length];
			this.children.CopyTo(array, 0);
			this.connections.CopyTo(array2, 0);
			this.areas.CopyTo(array3, 0);
			this.dirty.CopyTo(array4, 0);
			for (int i = this.children.Length; i < array.Length; i++)
			{
				array[i] = ListPool<GraphNode>.Claim(256);
				array2[i] = new List<int>();
				if (i > 0)
				{
					this.freeNodeIndices.Push(i);
				}
			}
			this.children = array;
			this.connections = array2;
			this.areas = array3;
			this.dirty = array4;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000FA75 File Offset: 0x0000DC75
		private int GetHierarchicalNodeIndex()
		{
			if (this.freeNodeIndices.Count == 0)
			{
				this.Grow();
			}
			return this.freeNodeIndices.Pop();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000FA98 File Offset: 0x0000DC98
		internal void OnCreatedNode(GraphNode node)
		{
			if (node.NodeIndex >= this.dirtyNodes.Length)
			{
				GraphNode[] array = new GraphNode[Math.Max(node.NodeIndex + 1, this.dirtyNodes.Length * 2)];
				this.dirtyNodes.CopyTo(array, 0);
				this.dirtyNodes = array;
			}
			this.AddDirtyNode(node);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		public void AddDirtyNode(GraphNode node)
		{
			if (!node.IsHierarchicalNodeDirty)
			{
				node.IsHierarchicalNodeDirty = true;
				if (this.numDirtyNodes < this.dirtyNodes.Length)
				{
					this.dirtyNodes[this.numDirtyNodes] = node;
					this.numDirtyNodes++;
					return;
				}
				int val = 0;
				for (int i = this.numDirtyNodes - 1; i >= 0; i--)
				{
					if (this.dirtyNodes[i].Destroyed)
					{
						this.numDirtyNodes--;
						this.dirty[this.dirtyNodes[i].HierarchicalNodeIndex] = 1;
						this.dirtyNodes[i] = this.dirtyNodes[this.numDirtyNodes];
						this.dirtyNodes[this.numDirtyNodes] = null;
					}
					else
					{
						val = Math.Max(val, this.dirtyNodes[i].NodeIndex);
					}
				}
				if (this.numDirtyNodes >= this.dirtyNodes.Length)
				{
					throw new Exception(string.Concat(new string[]
					{
						"Failed to compactify dirty nodes array. This should not happen. ",
						val.ToString(),
						" ",
						this.numDirtyNodes.ToString(),
						" ",
						this.dirtyNodes.Length.ToString()
					}));
				}
				this.AddDirtyNode(node);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000FC27 File Offset: 0x0000DE27
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000FC2F File Offset: 0x0000DE2F
		public int NumConnectedComponents { get; private set; }

		// Token: 0x060002CA RID: 714 RVA: 0x0000FC38 File Offset: 0x0000DE38
		public uint GetConnectedComponent(int hierarchicalNodeIndex)
		{
			return (uint)this.areas[hierarchicalNodeIndex];
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000FC44 File Offset: 0x0000DE44
		private void RemoveHierarchicalNode(int hierarchicalNode, bool removeAdjacentSmallNodes)
		{
			this.freeNodeIndices.Push(hierarchicalNode);
			List<int> list = this.connections[hierarchicalNode];
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				if (this.dirty[num] == 0)
				{
					if (removeAdjacentSmallNodes && this.children[num].Count < 128)
					{
						this.dirty[num] = 2;
						this.RemoveHierarchicalNode(num, false);
					}
					else
					{
						this.connections[num].Remove(hierarchicalNode);
					}
				}
			}
			list.Clear();
			List<GraphNode> list2 = this.children[hierarchicalNode];
			for (int j = 0; j < list2.Count; j++)
			{
				this.AddDirtyNode(list2[j]);
			}
			list2.ClearFast<GraphNode>();
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000FCFC File Offset: 0x0000DEFC
		public void RecalculateIfNecessary()
		{
			if (this.numDirtyNodes > 0)
			{
				for (int i = 0; i < this.numDirtyNodes; i++)
				{
					this.dirty[this.dirtyNodes[i].HierarchicalNodeIndex] = 1;
				}
				for (int j = 1; j < this.dirty.Length; j++)
				{
					if (this.dirty[j] == 1)
					{
						this.RemoveHierarchicalNode(j, true);
					}
				}
				for (int k = 1; k < this.dirty.Length; k++)
				{
					this.dirty[k] = 0;
				}
				for (int l = 0; l < this.numDirtyNodes; l++)
				{
					this.dirtyNodes[l].HierarchicalNodeIndex = 0;
				}
				for (int m = 0; m < this.numDirtyNodes; m++)
				{
					GraphNode graphNode = this.dirtyNodes[m];
					this.dirtyNodes[m] = null;
					graphNode.IsHierarchicalNodeDirty = false;
					if (graphNode.HierarchicalNodeIndex == 0 && graphNode.Walkable && !graphNode.Destroyed)
					{
						this.FindHierarchicalNodeChildren(this.GetHierarchicalNodeIndex(), graphNode);
					}
				}
				this.numDirtyNodes = 0;
				this.FloodFill();
				this.gizmoVersion++;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000FE12 File Offset: 0x0000E012
		public void RecalculateAll()
		{
			AstarPath.active.data.GetNodes(delegate(GraphNode node)
			{
				this.AddDirtyNode(node);
			});
			this.RecalculateIfNecessary();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000FE38 File Offset: 0x0000E038
		private void FloodFill()
		{
			for (int i = 0; i < this.areas.Length; i++)
			{
				this.areas[i] = 0;
			}
			Stack<int> stack = this.temporaryStack;
			int num = 0;
			for (int j = 1; j < this.areas.Length; j++)
			{
				if (this.areas[j] == 0)
				{
					num++;
					this.areas[j] = num;
					stack.Push(j);
					while (stack.Count > 0)
					{
						int num2 = stack.Pop();
						List<int> list = this.connections[num2];
						for (int k = list.Count - 1; k >= 0; k--)
						{
							int num3 = list[k];
							if (this.areas[num3] != num)
							{
								this.areas[num3] = num;
								stack.Push(num3);
							}
						}
					}
				}
			}
			this.NumConnectedComponents = Math.Max(1, num + 1);
			int version = this.version;
			this.version = version + 1;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000FF24 File Offset: 0x0000E124
		private void FindHierarchicalNodeChildren(int hierarchicalNode, GraphNode startNode)
		{
			this.currentChildren = this.children[hierarchicalNode];
			this.currentConnections = this.connections[hierarchicalNode];
			this.currentHierarchicalNodeIndex = hierarchicalNode;
			Queue<GraphNode> queue = this.temporaryQueue;
			queue.Enqueue(startNode);
			startNode.HierarchicalNodeIndex = hierarchicalNode;
			this.currentChildren.Add(startNode);
			while (queue.Count > 0)
			{
				queue.Dequeue().GetConnections(this.connectionCallback);
			}
			for (int i = 0; i < this.currentConnections.Count; i++)
			{
				this.connections[this.currentConnections[i]].Add(hierarchicalNode);
			}
			queue.Clear();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		public void OnDrawGizmos(RetainedGizmos gizmos)
		{
			RetainedGizmos.Hasher hasher = new RetainedGizmos.Hasher(AstarPath.active);
			hasher.AddHash(this.gizmoVersion);
			if (!gizmos.Draw(hasher))
			{
				RetainedGizmos.Builder builder = ObjectPool<RetainedGizmos.Builder>.Claim();
				Vector3[] array = ArrayPool<Vector3>.Claim(this.areas.Length);
				for (int i = 0; i < this.areas.Length; i++)
				{
					Int3 @int = Int3.zero;
					List<GraphNode> list = this.children[i];
					if (list.Count > 0)
					{
						for (int j = 0; j < list.Count; j++)
						{
							@int += list[j].position;
						}
						@int /= (float)list.Count;
						array[i] = (Vector3)@int;
					}
				}
				for (int k = 0; k < this.areas.Length; k++)
				{
					if (this.children[k].Count > 0)
					{
						for (int l = 0; l < this.connections[k].Count; l++)
						{
							if (this.connections[k][l] > k)
							{
								builder.DrawLine(array[k], array[this.connections[k][l]], Color.black);
							}
						}
					}
				}
				builder.Submit(gizmos, hasher);
			}
		}

		// Token: 0x040001CC RID: 460
		private const int Tiling = 16;

		// Token: 0x040001CD RID: 461
		private const int MaxChildrenPerNode = 256;

		// Token: 0x040001CE RID: 462
		private const int MinChildrenPerNode = 128;

		// Token: 0x040001CF RID: 463
		private List<GraphNode>[] children = new List<GraphNode>[0];

		// Token: 0x040001D0 RID: 464
		private List<int>[] connections = new List<int>[0];

		// Token: 0x040001D1 RID: 465
		private int[] areas = new int[0];

		// Token: 0x040001D2 RID: 466
		private byte[] dirty = new byte[0];

		// Token: 0x040001D4 RID: 468
		public Action onConnectedComponentsChanged;

		// Token: 0x040001D5 RID: 469
		private Action<GraphNode> connectionCallback;

		// Token: 0x040001D6 RID: 470
		private Queue<GraphNode> temporaryQueue = new Queue<GraphNode>();

		// Token: 0x040001D7 RID: 471
		private List<GraphNode> currentChildren;

		// Token: 0x040001D8 RID: 472
		private List<int> currentConnections;

		// Token: 0x040001D9 RID: 473
		private int currentHierarchicalNodeIndex;

		// Token: 0x040001DA RID: 474
		private Stack<int> temporaryStack = new Stack<int>();

		// Token: 0x040001DB RID: 475
		private int numDirtyNodes;

		// Token: 0x040001DC RID: 476
		private GraphNode[] dirtyNodes = new GraphNode[128];

		// Token: 0x040001DD RID: 477
		private Stack<int> freeNodeIndices = new Stack<int>();

		// Token: 0x040001DE RID: 478
		private int gizmoVersion;
	}
}
