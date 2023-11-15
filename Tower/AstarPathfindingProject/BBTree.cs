using System;
using System.Diagnostics;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006E RID: 110
	public class BBTree : IAstarPooledObject
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x000219C0 File Offset: 0x0001FBC0
		public Rect Size
		{
			get
			{
				if (this.count == 0)
				{
					return new Rect(0f, 0f, 0f, 0f);
				}
				IntRect rect = this.tree[0].rect;
				return Rect.MinMaxRect((float)rect.xmin * 0.001f, (float)rect.ymin * 0.001f, (float)rect.xmax * 0.001f, (float)rect.ymax * 0.001f);
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00021A3C File Offset: 0x0001FC3C
		public void Clear()
		{
			this.count = 0;
			this.leafNodes = 0;
			if (this.tree != null)
			{
				ArrayPool<BBTree.BBTreeBox>.Release(ref this.tree, false);
			}
			if (this.nodeLookup != null)
			{
				for (int i = 0; i < this.nodeLookup.Length; i++)
				{
					this.nodeLookup[i] = null;
				}
				ArrayPool<TriangleMeshNode>.Release(ref this.nodeLookup, false);
			}
			this.tree = ArrayPool<BBTree.BBTreeBox>.Claim(0);
			this.nodeLookup = ArrayPool<TriangleMeshNode>.Claim(0);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00021AB3 File Offset: 0x0001FCB3
		void IAstarPooledObject.OnEnterPool()
		{
			this.Clear();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00021ABC File Offset: 0x0001FCBC
		private void EnsureCapacity(int c)
		{
			if (c > this.tree.Length)
			{
				BBTree.BBTreeBox[] array = ArrayPool<BBTree.BBTreeBox>.Claim(c);
				this.tree.CopyTo(array, 0);
				ArrayPool<BBTree.BBTreeBox>.Release(ref this.tree, false);
				this.tree = array;
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00021AFC File Offset: 0x0001FCFC
		private void EnsureNodeCapacity(int c)
		{
			if (c > this.nodeLookup.Length)
			{
				TriangleMeshNode[] array = ArrayPool<TriangleMeshNode>.Claim(c);
				this.nodeLookup.CopyTo(array, 0);
				ArrayPool<TriangleMeshNode>.Release(ref this.nodeLookup, false);
				this.nodeLookup = array;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00021B3C File Offset: 0x0001FD3C
		private int GetBox(IntRect rect)
		{
			if (this.count >= this.tree.Length)
			{
				this.EnsureCapacity(this.count + 1);
			}
			this.tree[this.count] = new BBTree.BBTreeBox(rect);
			this.count++;
			return this.count - 1;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00021B94 File Offset: 0x0001FD94
		public void RebuildFrom(TriangleMeshNode[] nodes)
		{
			this.Clear();
			if (nodes.Length == 0)
			{
				return;
			}
			this.EnsureCapacity(Mathf.CeilToInt((float)nodes.Length * 2.1f));
			this.EnsureNodeCapacity(Mathf.CeilToInt((float)nodes.Length * 1.1f));
			int[] array = ArrayPool<int>.Claim(nodes.Length);
			for (int i = 0; i < nodes.Length; i++)
			{
				array[i] = i;
			}
			IntRect[] array2 = ArrayPool<IntRect>.Claim(nodes.Length);
			for (int j = 0; j < nodes.Length; j++)
			{
				Int3 @int;
				Int3 int2;
				Int3 int3;
				nodes[j].GetVertices(out @int, out int2, out int3);
				IntRect intRect = new IntRect(@int.x, @int.z, @int.x, @int.z);
				intRect = intRect.ExpandToContain(int2.x, int2.z);
				intRect = intRect.ExpandToContain(int3.x, int3.z);
				array2[j] = intRect;
			}
			this.RebuildFromInternal(nodes, array, array2, 0, nodes.Length, false);
			ArrayPool<int>.Release(ref array, false);
			ArrayPool<IntRect>.Release(ref array2, false);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00021C90 File Offset: 0x0001FE90
		private static int SplitByX(TriangleMeshNode[] nodes, int[] permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[permutation[i]].position.x > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00021CD8 File Offset: 0x0001FED8
		private static int SplitByZ(TriangleMeshNode[] nodes, int[] permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[permutation[i]].position.z > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00021D20 File Offset: 0x0001FF20
		private int RebuildFromInternal(TriangleMeshNode[] nodes, int[] permutation, IntRect[] nodeBounds, int from, int to, bool odd)
		{
			IntRect intRect = BBTree.NodeBounds(permutation, nodeBounds, from, to);
			int box = this.GetBox(intRect);
			if (to - from <= 4)
			{
				int num = this.tree[box].nodeOffset = this.leafNodes * 4;
				this.EnsureNodeCapacity(num + 4);
				this.leafNodes++;
				for (int i = 0; i < 4; i++)
				{
					this.nodeLookup[num + i] = ((i < to - from) ? nodes[permutation[from + i]] : null);
				}
				return box;
			}
			int num2;
			if (odd)
			{
				int divider = (intRect.xmin + intRect.xmax) / 2;
				num2 = BBTree.SplitByX(nodes, permutation, from, to, divider);
			}
			else
			{
				int divider2 = (intRect.ymin + intRect.ymax) / 2;
				num2 = BBTree.SplitByZ(nodes, permutation, from, to, divider2);
			}
			if (num2 == from || num2 == to)
			{
				if (!odd)
				{
					int divider3 = (intRect.xmin + intRect.xmax) / 2;
					num2 = BBTree.SplitByX(nodes, permutation, from, to, divider3);
				}
				else
				{
					int divider4 = (intRect.ymin + intRect.ymax) / 2;
					num2 = BBTree.SplitByZ(nodes, permutation, from, to, divider4);
				}
				if (num2 == from || num2 == to)
				{
					num2 = (from + to) / 2;
				}
			}
			this.tree[box].left = this.RebuildFromInternal(nodes, permutation, nodeBounds, from, num2, !odd);
			this.tree[box].right = this.RebuildFromInternal(nodes, permutation, nodeBounds, num2, to, !odd);
			return box;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00021E9C File Offset: 0x0002009C
		private static IntRect NodeBounds(int[] permutation, IntRect[] nodeBounds, int from, int to)
		{
			IntRect intRect = nodeBounds[permutation[from]];
			for (int i = from + 1; i < to; i++)
			{
				IntRect intRect2 = nodeBounds[permutation[i]];
				intRect.xmin = Math.Min(intRect.xmin, intRect2.xmin);
				intRect.ymin = Math.Min(intRect.ymin, intRect2.ymin);
				intRect.xmax = Math.Max(intRect.xmax, intRect2.xmax);
				intRect.ymax = Math.Max(intRect.ymax, intRect2.ymax);
			}
			return intRect;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00021F2C File Offset: 0x0002012C
		[Conditional("ASTARDEBUG")]
		private static void DrawDebugRect(IntRect rect)
		{
			Debug.DrawLine(new Vector3((float)rect.xmin, 0f, (float)rect.ymin), new Vector3((float)rect.xmax, 0f, (float)rect.ymin), Color.white);
			Debug.DrawLine(new Vector3((float)rect.xmin, 0f, (float)rect.ymax), new Vector3((float)rect.xmax, 0f, (float)rect.ymax), Color.white);
			Debug.DrawLine(new Vector3((float)rect.xmin, 0f, (float)rect.ymin), new Vector3((float)rect.xmin, 0f, (float)rect.ymax), Color.white);
			Debug.DrawLine(new Vector3((float)rect.xmax, 0f, (float)rect.ymin), new Vector3((float)rect.xmax, 0f, (float)rect.ymax), Color.white);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00022024 File Offset: 0x00020224
		[Conditional("ASTARDEBUG")]
		private static void DrawDebugNode(TriangleMeshNode node, float yoffset, Color color)
		{
			Debug.DrawLine((Vector3)node.GetVertex(1) + Vector3.up * yoffset, (Vector3)node.GetVertex(2) + Vector3.up * yoffset, color);
			Debug.DrawLine((Vector3)node.GetVertex(0) + Vector3.up * yoffset, (Vector3)node.GetVertex(1) + Vector3.up * yoffset, color);
			Debug.DrawLine((Vector3)node.GetVertex(2) + Vector3.up * yoffset, (Vector3)node.GetVertex(0) + Vector3.up * yoffset, color);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000220EB File Offset: 0x000202EB
		public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
		{
			distance = float.PositiveInfinity;
			return this.QueryClosest(p, constraint, ref distance, new NNInfoInternal(null));
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00022104 File Offset: 0x00020304
		public NNInfoInternal QueryClosestXZ(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
		{
			float num = distance * distance;
			float num2 = num;
			if (this.count > 0 && BBTree.SquaredRectPointDistance(this.tree[0].rect, p) < num)
			{
				this.SearchBoxClosestXZ(0, p, ref num, constraint, ref previous);
				if (num < num2)
				{
					distance = Mathf.Sqrt(num);
				}
			}
			return previous;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00022158 File Offset: 0x00020358
		private void SearchBoxClosestXZ(int boxi, Vector3 p, ref float closestSqrDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			if (bbtreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = this.nodeLookup;
				for (int i = 0; i < 4; i++)
				{
					if (array[bbtreeBox.nodeOffset + i] == null)
					{
						return;
					}
					TriangleMeshNode triangleMeshNode = array[bbtreeBox.nodeOffset + i];
					if (constraint == null || constraint.Suitable(triangleMeshNode))
					{
						Vector3 vector = triangleMeshNode.ClosestPointOnNodeXZ(p);
						float num = (vector.x - p.x) * (vector.x - p.x) + (vector.z - p.z) * (vector.z - p.z);
						if (nnInfo.constrainedNode == null || num < closestSqrDist - 1E-06f || (num <= closestSqrDist + 1E-06f && Mathf.Abs(vector.y - p.y) < Mathf.Abs(nnInfo.constClampedPosition.y - p.y)))
						{
							nnInfo.constrainedNode = triangleMeshNode;
							nnInfo.constClampedPosition = vector;
							closestSqrDist = num;
						}
					}
				}
			}
			else
			{
				int left = bbtreeBox.left;
				int right = bbtreeBox.right;
				float num2;
				float num3;
				this.GetOrderedChildren(ref left, ref right, out num2, out num3, p);
				if (num2 <= closestSqrDist)
				{
					this.SearchBoxClosestXZ(left, p, ref closestSqrDist, constraint, ref nnInfo);
				}
				if (num3 <= closestSqrDist)
				{
					this.SearchBoxClosestXZ(right, p, ref closestSqrDist, constraint, ref nnInfo);
				}
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000222B0 File Offset: 0x000204B0
		public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
		{
			float num = distance * distance;
			float num2 = num;
			if (this.count > 0 && BBTree.SquaredRectPointDistance(this.tree[0].rect, p) < num)
			{
				this.SearchBoxClosest(0, p, ref num, constraint, ref previous);
				if (num < num2)
				{
					distance = Mathf.Sqrt(num);
				}
			}
			return previous;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00022304 File Offset: 0x00020504
		private void SearchBoxClosest(int boxi, Vector3 p, ref float closestSqrDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			if (bbtreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = this.nodeLookup;
				for (int i = 0; i < 4; i++)
				{
					if (array[bbtreeBox.nodeOffset + i] == null)
					{
						return;
					}
					TriangleMeshNode triangleMeshNode = array[bbtreeBox.nodeOffset + i];
					Vector3 vector = triangleMeshNode.ClosestPointOnNode(p);
					float sqrMagnitude = (vector - p).sqrMagnitude;
					if (sqrMagnitude < closestSqrDist && (constraint == null || constraint.Suitable(triangleMeshNode)))
					{
						nnInfo.constrainedNode = triangleMeshNode;
						nnInfo.constClampedPosition = vector;
						closestSqrDist = sqrMagnitude;
					}
				}
			}
			else
			{
				int left = bbtreeBox.left;
				int right = bbtreeBox.right;
				float num;
				float num2;
				this.GetOrderedChildren(ref left, ref right, out num, out num2, p);
				if (num < closestSqrDist)
				{
					this.SearchBoxClosest(left, p, ref closestSqrDist, constraint, ref nnInfo);
				}
				if (num2 < closestSqrDist)
				{
					this.SearchBoxClosest(right, p, ref closestSqrDist, constraint, ref nnInfo);
				}
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000223E0 File Offset: 0x000205E0
		private void GetOrderedChildren(ref int first, ref int second, out float firstDist, out float secondDist, Vector3 p)
		{
			firstDist = BBTree.SquaredRectPointDistance(this.tree[first].rect, p);
			secondDist = BBTree.SquaredRectPointDistance(this.tree[second].rect, p);
			if (secondDist < firstDist)
			{
				int num = first;
				first = second;
				second = num;
				float num2 = firstDist;
				firstDist = secondDist;
				secondDist = num2;
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00022441 File Offset: 0x00020641
		public TriangleMeshNode QueryInside(Vector3 p, NNConstraint constraint)
		{
			if (this.count == 0 || !this.tree[0].Contains(p))
			{
				return null;
			}
			return this.SearchBoxInside(0, p, constraint);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002246C File Offset: 0x0002066C
		private TriangleMeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			if (bbtreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = this.nodeLookup;
				for (int i = 0; i < 4; i++)
				{
					if (array[bbtreeBox.nodeOffset + i] == null)
					{
						break;
					}
					TriangleMeshNode triangleMeshNode = array[bbtreeBox.nodeOffset + i];
					if (triangleMeshNode.ContainsPoint((Int3)p) && (constraint == null || constraint.Suitable(triangleMeshNode)))
					{
						return triangleMeshNode;
					}
				}
			}
			else
			{
				if (this.tree[bbtreeBox.left].Contains(p))
				{
					TriangleMeshNode triangleMeshNode2 = this.SearchBoxInside(bbtreeBox.left, p, constraint);
					if (triangleMeshNode2 != null)
					{
						return triangleMeshNode2;
					}
				}
				if (this.tree[bbtreeBox.right].Contains(p))
				{
					TriangleMeshNode triangleMeshNode3 = this.SearchBoxInside(bbtreeBox.right, p, constraint);
					if (triangleMeshNode3 != null)
					{
						return triangleMeshNode3;
					}
				}
			}
			return null;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00022538 File Offset: 0x00020738
		public void OnDrawGizmos()
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (this.count == 0)
			{
				return;
			}
			this.OnDrawGizmos(0, 0);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002256C File Offset: 0x0002076C
		private void OnDrawGizmos(int boxi, int depth)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			Vector3 a = (Vector3)new Int3(bbtreeBox.rect.xmin, 0, bbtreeBox.rect.ymin);
			Vector3 vector = (Vector3)new Int3(bbtreeBox.rect.xmax, 0, bbtreeBox.rect.ymax);
			Vector3 vector2 = (a + vector) * 0.5f;
			Vector3 vector3 = (vector - vector2) * 2f;
			vector3 = new Vector3(vector3.x, 1f, vector3.z);
			vector2.y += (float)(depth * 2);
			Gizmos.color = AstarMath.IntToColor(depth, 1f);
			Gizmos.DrawCube(vector2, vector3);
			if (!bbtreeBox.IsLeaf)
			{
				this.OnDrawGizmos(bbtreeBox.left, depth + 1);
				this.OnDrawGizmos(bbtreeBox.right, depth + 1);
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00022654 File Offset: 0x00020854
		private static bool NodeIntersectsCircle(TriangleMeshNode node, Vector3 p, float radius)
		{
			return float.IsPositiveInfinity(radius) || (p - node.ClosestPointOnNode(p)).sqrMagnitude < radius * radius;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00022688 File Offset: 0x00020888
		private static bool RectIntersectsCircle(IntRect r, Vector3 p, float radius)
		{
			if (float.IsPositiveInfinity(radius))
			{
				return true;
			}
			Vector3 vector = p;
			p.x = Math.Max(p.x, (float)r.xmin * 0.001f);
			p.x = Math.Min(p.x, (float)r.xmax * 0.001f);
			p.z = Math.Max(p.z, (float)r.ymin * 0.001f);
			p.z = Math.Min(p.z, (float)r.ymax * 0.001f);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z) < radius * radius;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0002275C File Offset: 0x0002095C
		private static float SquaredRectPointDistance(IntRect r, Vector3 p)
		{
			Vector3 vector = p;
			p.x = Math.Max(p.x, (float)r.xmin * 0.001f);
			p.x = Math.Min(p.x, (float)r.xmax * 0.001f);
			p.z = Math.Max(p.z, (float)r.ymin * 0.001f);
			p.z = Math.Min(p.z, (float)r.ymax * 0.001f);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z);
		}

		// Token: 0x0400034C RID: 844
		private BBTree.BBTreeBox[] tree;

		// Token: 0x0400034D RID: 845
		private TriangleMeshNode[] nodeLookup;

		// Token: 0x0400034E RID: 846
		private int count;

		// Token: 0x0400034F RID: 847
		private int leafNodes;

		// Token: 0x04000350 RID: 848
		private const int MaximumLeafSize = 4;

		// Token: 0x0200013C RID: 316
		private struct BBTreeBox
		{
			// Token: 0x1700018B RID: 395
			// (get) Token: 0x06000B12 RID: 2834 RVA: 0x000463A7 File Offset: 0x000445A7
			public bool IsLeaf
			{
				get
				{
					return this.nodeOffset >= 0;
				}
			}

			// Token: 0x06000B13 RID: 2835 RVA: 0x000463B8 File Offset: 0x000445B8
			public BBTreeBox(IntRect rect)
			{
				this.nodeOffset = -1;
				this.rect = rect;
				this.left = (this.right = -1);
			}

			// Token: 0x06000B14 RID: 2836 RVA: 0x000463E4 File Offset: 0x000445E4
			public BBTreeBox(int nodeOffset, IntRect rect)
			{
				this.nodeOffset = nodeOffset;
				this.rect = rect;
				this.left = (this.right = -1);
			}

			// Token: 0x06000B15 RID: 2837 RVA: 0x00046410 File Offset: 0x00044610
			public bool Contains(Vector3 point)
			{
				Int3 @int = (Int3)point;
				return this.rect.Contains(@int.x, @int.z);
			}

			// Token: 0x04000753 RID: 1875
			public IntRect rect;

			// Token: 0x04000754 RID: 1876
			public int nodeOffset;

			// Token: 0x04000755 RID: 1877
			public int left;

			// Token: 0x04000756 RID: 1878
			public int right;
		}
	}
}
