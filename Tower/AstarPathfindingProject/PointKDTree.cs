using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000073 RID: 115
	public class PointKDTree
	{
		// Token: 0x06000610 RID: 1552 RVA: 0x000238C8 File Offset: 0x00021AC8
		public PointKDTree()
		{
			this.tree[1] = new PointKDTree.Node
			{
				data = this.GetOrCreateList()
			};
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00023920 File Offset: 0x00021B20
		public void Add(GraphNode node)
		{
			this.numNodes++;
			this.Add(node, 1, 0);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002393C File Offset: 0x00021B3C
		public void Rebuild(GraphNode[] nodes, int start, int end)
		{
			if (start < 0 || end < start || end > nodes.Length)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < this.tree.Length; i++)
			{
				GraphNode[] data = this.tree[i].data;
				if (data != null)
				{
					for (int j = 0; j < 21; j++)
					{
						data[j] = null;
					}
					this.arrayCache.Push(data);
					this.tree[i].data = null;
				}
			}
			this.numNodes = end - start;
			this.Build(1, new List<GraphNode>(nodes), start, end);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000239CC File Offset: 0x00021BCC
		private GraphNode[] GetOrCreateList()
		{
			if (this.arrayCache.Count <= 0)
			{
				return new GraphNode[21];
			}
			return this.arrayCache.Pop();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000239EF File Offset: 0x00021BEF
		private int Size(int index)
		{
			if (this.tree[index].data == null)
			{
				return this.Size(2 * index) + this.Size(2 * index + 1);
			}
			return (int)this.tree[index].count;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00023A2C File Offset: 0x00021C2C
		private void CollectAndClear(int index, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			ushort count = this.tree[index].count;
			if (data != null)
			{
				this.tree[index] = default(PointKDTree.Node);
				for (int i = 0; i < (int)count; i++)
				{
					buffer.Add(data[i]);
					data[i] = null;
				}
				this.arrayCache.Push(data);
				return;
			}
			this.CollectAndClear(index * 2, buffer);
			this.CollectAndClear(index * 2 + 1, buffer);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00023AAE File Offset: 0x00021CAE
		private static int MaxAllowedSize(int numNodes, int depth)
		{
			return Math.Min(5 * numNodes / 2 >> depth, 3 * numNodes / 4);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00023AC4 File Offset: 0x00021CC4
		private void Rebalance(int index)
		{
			this.CollectAndClear(index, this.largeList);
			this.Build(index, this.largeList, 0, this.largeList.Count);
			this.largeList.ClearFast<GraphNode>();
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00023AF8 File Offset: 0x00021CF8
		private void EnsureSize(int index)
		{
			if (index >= this.tree.Length)
			{
				PointKDTree.Node[] array = new PointKDTree.Node[Math.Max(index + 1, this.tree.Length * 2)];
				this.tree.CopyTo(array, 0);
				this.tree = array;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00023B3C File Offset: 0x00021D3C
		private void Build(int index, List<GraphNode> nodes, int start, int end)
		{
			this.EnsureSize(index);
			if (end - start <= 10)
			{
				GraphNode[] array = this.tree[index].data = this.GetOrCreateList();
				this.tree[index].count = (ushort)(end - start);
				for (int i = start; i < end; i++)
				{
					array[i - start] = nodes[i];
				}
				return;
			}
			Int3 position;
			Int3 @int = position = nodes[start].position;
			for (int j = start; j < end; j++)
			{
				Int3 position2 = nodes[j].position;
				position = new Int3(Math.Min(position.x, position2.x), Math.Min(position.y, position2.y), Math.Min(position.z, position2.z));
				@int = new Int3(Math.Max(@int.x, position2.x), Math.Max(@int.y, position2.y), Math.Max(@int.z, position2.z));
			}
			Int3 int2 = @int - position;
			int num = (int2.x > int2.y) ? ((int2.x > int2.z) ? 0 : 2) : ((int2.y > int2.z) ? 1 : 2);
			nodes.Sort(start, end - start, PointKDTree.comparers[num]);
			int num2 = (start + end) / 2;
			this.tree[index].split = (nodes[num2 - 1].position[num] + nodes[num2].position[num] + 1) / 2;
			this.tree[index].splitAxis = (byte)num;
			this.Build(index * 2, nodes, start, num2);
			this.Build(index * 2 + 1, nodes, num2, end);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00023D28 File Offset: 0x00021F28
		private void Add(GraphNode point, int index, int depth = 0)
		{
			while (this.tree[index].data == null)
			{
				index = 2 * index + ((point.position[(int)this.tree[index].splitAxis] < this.tree[index].split) ? 0 : 1);
				depth++;
			}
			GraphNode[] data = this.tree[index].data;
			PointKDTree.Node[] array = this.tree;
			int num = index;
			ushort count = array[num].count;
			array[num].count = count + 1;
			data[(int)count] = point;
			if (this.tree[index].count >= 21)
			{
				int num2 = 0;
				while (depth - num2 > 0 && this.Size(index >> num2) > PointKDTree.MaxAllowedSize(this.numNodes, depth - num2))
				{
					num2++;
				}
				this.Rebalance(index >> num2);
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00023E00 File Offset: 0x00022000
		public GraphNode GetNearest(Int3 point, NNConstraint constraint)
		{
			GraphNode result = null;
			long maxValue = long.MaxValue;
			this.GetNearestInternal(1, point, constraint, ref result, ref maxValue);
			return result;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00023E28 File Offset: 0x00022028
		private void GetNearestInternal(int index, Int3 point, NNConstraint constraint, ref GraphNode best, ref long bestSqrDist)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong < bestSqrDist && (constraint == null || constraint.Suitable(data[i])))
					{
						bestSqrDist = sqrMagnitudeLong;
						best = data[i];
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetNearestInternal(num2, point, constraint, ref best, ref bestSqrDist);
			if (num * num < bestSqrDist)
			{
				this.GetNearestInternal(num2 ^ 1, point, constraint, ref best, ref bestSqrDist);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00023F04 File Offset: 0x00022104
		public GraphNode GetNearestConnection(Int3 point, NNConstraint constraint, long maximumSqrConnectionLength)
		{
			GraphNode result = null;
			long maxValue = long.MaxValue;
			long distanceThresholdOffset = (maximumSqrConnectionLength + 3L) / 4L;
			this.GetNearestConnectionInternal(1, point, constraint, ref result, ref maxValue, distanceThresholdOffset);
			return result;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00023F34 File Offset: 0x00022134
		private void GetNearestConnectionInternal(int index, Int3 point, NNConstraint constraint, ref GraphNode best, ref long bestSqrDist, long distanceThresholdOffset)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				Vector3 p = (Vector3)point;
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong - distanceThresholdOffset < bestSqrDist && (constraint == null || constraint.Suitable(data[i])))
					{
						Connection[] connections = (data[i] as PointNode).connections;
						if (connections != null)
						{
							Vector3 vector = (Vector3)data[i].position;
							for (int j = 0; j < connections.Length; j++)
							{
								Vector3 b = ((Vector3)connections[j].node.position + vector) * 0.5f;
								long num = (long)(VectorMath.SqrDistancePointSegment(vector, b, p) * 1000f * 1000f);
								if (num < bestSqrDist)
								{
									bestSqrDist = num;
									best = data[i];
								}
							}
						}
						if (sqrMagnitudeLong < bestSqrDist)
						{
							bestSqrDist = sqrMagnitudeLong;
							best = data[i];
						}
					}
				}
				return;
			}
			long num2 = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num3 = 2 * index + ((num2 < 0L) ? 0 : 1);
			this.GetNearestConnectionInternal(num3, point, constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			if (num2 * num2 - distanceThresholdOffset < bestSqrDist)
			{
				this.GetNearestConnectionInternal(num3 ^ 1, point, constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000240C1 File Offset: 0x000222C1
		public void GetInRange(Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			this.GetInRangeInternal(1, point, sqrRadius, buffer);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000240D0 File Offset: 0x000222D0
		private void GetInRangeInternal(int index, Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					if ((data[i].position - point).sqrMagnitudeLong < sqrRadius)
					{
						buffer.Add(data[i]);
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetInRangeInternal(num2, point, sqrRadius, buffer);
			if (num * num < sqrRadius)
			{
				this.GetInRangeInternal(num2 ^ 1, point, sqrRadius, buffer);
			}
		}

		// Token: 0x0400036E RID: 878
		public const int LeafSize = 10;

		// Token: 0x0400036F RID: 879
		public const int LeafArraySize = 21;

		// Token: 0x04000370 RID: 880
		private PointKDTree.Node[] tree = new PointKDTree.Node[16];

		// Token: 0x04000371 RID: 881
		private int numNodes;

		// Token: 0x04000372 RID: 882
		private readonly List<GraphNode> largeList = new List<GraphNode>();

		// Token: 0x04000373 RID: 883
		private readonly Stack<GraphNode[]> arrayCache = new Stack<GraphNode[]>();

		// Token: 0x04000374 RID: 884
		private static readonly IComparer<GraphNode>[] comparers = new IComparer<GraphNode>[]
		{
			new PointKDTree.CompareX(),
			new PointKDTree.CompareY(),
			new PointKDTree.CompareZ()
		};

		// Token: 0x02000143 RID: 323
		private struct Node
		{
			// Token: 0x0400076F RID: 1903
			public GraphNode[] data;

			// Token: 0x04000770 RID: 1904
			public int split;

			// Token: 0x04000771 RID: 1905
			public ushort count;

			// Token: 0x04000772 RID: 1906
			public byte splitAxis;
		}

		// Token: 0x02000144 RID: 324
		private class CompareX : IComparer<GraphNode>
		{
			// Token: 0x06000B21 RID: 2849 RVA: 0x00046913 File Offset: 0x00044B13
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.x.CompareTo(rhs.position.x);
			}
		}

		// Token: 0x02000145 RID: 325
		private class CompareY : IComparer<GraphNode>
		{
			// Token: 0x06000B23 RID: 2851 RVA: 0x00046938 File Offset: 0x00044B38
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.y.CompareTo(rhs.position.y);
			}
		}

		// Token: 0x02000146 RID: 326
		private class CompareZ : IComparer<GraphNode>
		{
			// Token: 0x06000B25 RID: 2853 RVA: 0x0004695D File Offset: 0x00044B5D
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.z.CompareTo(rhs.position.z);
			}
		}
	}
}
