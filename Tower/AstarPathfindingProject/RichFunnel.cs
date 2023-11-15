﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200000D RID: 13
	public class RichFunnel : RichPathPart
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00006AAC File Offset: 0x00004CAC
		public RichFunnel()
		{
			this.left = ListPool<Vector3>.Claim();
			this.right = ListPool<Vector3>.Claim();
			this.nodes = new List<TriangleMeshNode>();
			this.graph = null;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006AFC File Offset: 0x00004CFC
		public RichFunnel Initialize(RichPath path, NavmeshBase graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (this.graph != null)
			{
				throw new InvalidOperationException("Trying to initialize an already initialized object. " + ((graph != null) ? graph.ToString() : null));
			}
			this.graph = graph;
			this.path = path;
			return this;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006B4B File Offset: 0x00004D4B
		public override void OnEnterPool()
		{
			this.left.Clear();
			this.right.Clear();
			this.nodes.Clear();
			this.graph = null;
			this.currentNode = 0;
			this.checkForDestroyedNodesCounter = 0;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006B84 File Offset: 0x00004D84
		public TriangleMeshNode CurrentNode
		{
			get
			{
				TriangleMeshNode triangleMeshNode = this.nodes[this.currentNode];
				if (!triangleMeshNode.Destroyed)
				{
					return triangleMeshNode;
				}
				return null;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006BB0 File Offset: 0x00004DB0
		public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
		{
			this.exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(this.exactStart);
			this.exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(this.exactEnd);
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			this.nodes.Clear();
			if (this.funnelSimplification)
			{
				List<GraphNode> list = ListPool<GraphNode>.Claim(end - start);
				this.SimplifyPath(this.graph, nodes, start, end, list, this.exactStart, this.exactEnd);
				if (this.nodes.Capacity < list.Count)
				{
					this.nodes.Capacity = list.Count;
				}
				for (int i = 0; i < list.Count; i++)
				{
					TriangleMeshNode triangleMeshNode = list[i] as TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						this.nodes.Add(triangleMeshNode);
					}
				}
				ListPool<GraphNode>.Release(ref list);
			}
			else
			{
				if (this.nodes.Capacity < end - start)
				{
					this.nodes.Capacity = end - start;
				}
				for (int j = start; j <= end; j++)
				{
					TriangleMeshNode triangleMeshNode2 = nodes[j] as TriangleMeshNode;
					if (triangleMeshNode2 != null)
					{
						this.nodes.Add(triangleMeshNode2);
					}
				}
			}
			for (int k = 0; k < this.nodes.Count - 1; k++)
			{
				this.nodes[k].GetPortal(this.nodes[k + 1], this.left, this.right, false);
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006D78 File Offset: 0x00004F78
		private void SimplifyPath(IRaycastableGraph graph, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (start > end)
			{
				throw new ArgumentException("start >= end");
			}
			GraphHitInfo graphHitInfo;
			if (!graph.Linecast(startPoint, endPoint, out graphHitInfo, null, null) && graphHitInfo.node == nodes[end])
			{
				graph.Linecast(startPoint, endPoint, out graphHitInfo, result, null);
				long num = 0L;
				long num2 = 0L;
				for (int i = start; i <= end; i++)
				{
					num += (long)((ulong)nodes[i].Penalty + (ulong)((long)((this.path.seeker != null) ? this.path.seeker.tagPenalties[(int)nodes[i].Tag] : 0)));
				}
				for (int j = 0; j < result.Count; j++)
				{
					num2 += (long)((ulong)result[j].Penalty + (ulong)((long)((this.path.seeker != null) ? this.path.seeker.tagPenalties[(int)result[j].Tag] : 0)));
				}
				if ((double)num * 1.4 * (double)result.Count >= (double)(num2 * (long)(end - start + 1)))
				{
					return;
				}
				result.Clear();
			}
			int num3 = start;
			int num4 = 0;
			while (num4++ <= 1000)
			{
				if (start == end)
				{
					result.Add(nodes[end]);
					return;
				}
				int count = result.Count;
				int k = end + 1;
				int num5 = start + 1;
				bool flag = false;
				while (k > num5 + 1)
				{
					int num6 = (k + num5) / 2;
					Vector3 start2 = (start == num3) ? startPoint : ((Vector3)nodes[start].position);
					Vector3 end2 = (num6 == end) ? endPoint : ((Vector3)nodes[num6].position);
					GraphHitInfo graphHitInfo2;
					if (graph.Linecast(start2, end2, out graphHitInfo2, null, null) || graphHitInfo2.node != nodes[num6])
					{
						k = num6;
					}
					else
					{
						flag = true;
						num5 = num6;
					}
				}
				if (!flag)
				{
					result.Add(nodes[start]);
					start = num5;
				}
				else
				{
					Vector3 start3 = (start == num3) ? startPoint : ((Vector3)nodes[start].position);
					Vector3 end3 = (num5 == end) ? endPoint : ((Vector3)nodes[num5].position);
					GraphHitInfo graphHitInfo3;
					graph.Linecast(start3, end3, out graphHitInfo3, result, null);
					long num7 = 0L;
					long num8 = 0L;
					for (int l = start; l <= num5; l++)
					{
						num7 += (long)((ulong)nodes[l].Penalty + (ulong)((long)((this.path.seeker != null) ? this.path.seeker.tagPenalties[(int)nodes[l].Tag] : 0)));
					}
					for (int m = count; m < result.Count; m++)
					{
						num8 += (long)((ulong)result[m].Penalty + (ulong)((long)((this.path.seeker != null) ? this.path.seeker.tagPenalties[(int)result[m].Tag] : 0)));
					}
					if ((double)num7 * 1.4 * (double)(result.Count - count) < (double)(num8 * (long)(num5 - start + 1)) || result[result.Count - 1] != nodes[num5])
					{
						result.RemoveRange(count, result.Count - count);
						result.Add(nodes[start]);
						start++;
					}
					else
					{
						result.RemoveAt(result.Count - 1);
						start = num5;
					}
				}
			}
			Debug.LogError("Was the path really long or have we got cought in an infinite loop?");
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007134 File Offset: 0x00005334
		private void UpdateFunnelCorridor(int splitIndex, List<TriangleMeshNode> prefix)
		{
			this.nodes.RemoveRange(0, splitIndex);
			this.nodes.InsertRange(0, prefix);
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			for (int i = 0; i < this.nodes.Count - 1; i++)
			{
				this.nodes[i].GetPortal(this.nodes[i + 1], this.left, this.right, false);
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000071FC File Offset: 0x000053FC
		private bool CheckForDestroyedNodes()
		{
			int i = 0;
			int count = this.nodes.Count;
			while (i < count)
			{
				if (this.nodes[i].Destroyed)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007238 File Offset: 0x00005438
		public float DistanceToEndOfPath
		{
			get
			{
				TriangleMeshNode triangleMeshNode = this.CurrentNode;
				Vector3 b = (triangleMeshNode != null) ? triangleMeshNode.ClosestPointOnNode(this.currentPosition) : this.currentPosition;
				return (this.exactEnd - b).magnitude;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007278 File Offset: 0x00005478
		public Vector3 ClampToNavmesh(Vector3 position)
		{
			if (this.path.transform != null)
			{
				position = this.path.transform.InverseTransform(position);
			}
			this.ClampToNavmeshInternal(ref position);
			if (this.path.transform != null)
			{
				position = this.path.transform.Transform(position);
			}
			return position;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000072D0 File Offset: 0x000054D0
		public Vector3 Update(Vector3 position, List<Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
		{
			if (this.path.transform != null)
			{
				position = this.path.transform.InverseTransform(position);
			}
			lastCorner = false;
			requiresRepath = false;
			if (this.checkForDestroyedNodesCounter >= 10)
			{
				this.checkForDestroyedNodesCounter = 0;
				requiresRepath |= this.CheckForDestroyedNodes();
			}
			else
			{
				this.checkForDestroyedNodesCounter++;
			}
			bool flag = this.ClampToNavmeshInternal(ref position);
			this.currentPosition = position;
			if (flag)
			{
				requiresRepath = true;
				lastCorner = false;
				buffer.Add(position);
			}
			else if (!this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
			{
				Debug.LogError("Failed to find next corners in the path");
				buffer.Add(position);
			}
			if (this.path.transform != null)
			{
				for (int i = 0; i < buffer.Count; i++)
				{
					buffer[i] = this.path.transform.Transform(buffer[i]);
				}
				position = this.path.transform.Transform(position);
			}
			return position;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000073CC File Offset: 0x000055CC
		private bool ClampToNavmeshInternal(ref Vector3 position)
		{
			TriangleMeshNode triangleMeshNode = this.nodes[this.currentNode];
			if (triangleMeshNode.Destroyed)
			{
				return true;
			}
			if (triangleMeshNode.ContainsPoint(position))
			{
				return false;
			}
			Queue<TriangleMeshNode> queue = RichFunnel.navmeshClampQueue;
			List<TriangleMeshNode> list = RichFunnel.navmeshClampList;
			Dictionary<TriangleMeshNode, TriangleMeshNode> dictionary = RichFunnel.navmeshClampDict;
			triangleMeshNode.TemporaryFlag1 = true;
			dictionary[triangleMeshNode] = null;
			queue.Enqueue(triangleMeshNode);
			list.Add(triangleMeshNode);
			float num = float.PositiveInfinity;
			Vector3 vector = position;
			TriangleMeshNode triangleMeshNode2 = null;
			while (queue.Count > 0)
			{
				TriangleMeshNode triangleMeshNode3 = queue.Dequeue();
				Vector3 vector2 = triangleMeshNode3.ClosestPointOnNodeXZ(position);
				float num2 = VectorMath.MagnitudeXZ(vector2 - position);
				if (num2 <= num * 1.05f + 0.001f)
				{
					if (num2 < num)
					{
						num = num2;
						vector = vector2;
						triangleMeshNode2 = triangleMeshNode3;
					}
					for (int i = 0; i < triangleMeshNode3.connections.Length; i++)
					{
						TriangleMeshNode triangleMeshNode4 = triangleMeshNode3.connections[i].node as TriangleMeshNode;
						if (triangleMeshNode4 != null && !triangleMeshNode4.TemporaryFlag1)
						{
							triangleMeshNode4.TemporaryFlag1 = true;
							dictionary[triangleMeshNode4] = triangleMeshNode3;
							queue.Enqueue(triangleMeshNode4);
							list.Add(triangleMeshNode4);
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].TemporaryFlag1 = false;
			}
			list.ClearFast<TriangleMeshNode>();
			int num3 = this.nodes.IndexOf(triangleMeshNode2);
			position.x = vector.x;
			position.z = vector.z;
			if (num3 == -1)
			{
				List<TriangleMeshNode> list2 = RichFunnel.navmeshClampList;
				while (num3 == -1)
				{
					list2.Add(triangleMeshNode2);
					triangleMeshNode2 = dictionary[triangleMeshNode2];
					num3 = this.nodes.IndexOf(triangleMeshNode2);
				}
				this.exactStart = position;
				this.UpdateFunnelCorridor(num3, list2);
				list2.ClearFast<TriangleMeshNode>();
				this.currentNode = 0;
			}
			else
			{
				this.currentNode = num3;
			}
			dictionary.Clear();
			return this.currentNode + 1 < this.nodes.Count && this.nodes[this.currentNode + 1].Destroyed;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000075F1 File Offset: 0x000057F1
		public void FindWalls(List<Vector3> wallBuffer, float range)
		{
			this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007608 File Offset: 0x00005808
		private void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
		{
			if (range <= 0f)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			range *= range;
			position.y = 0f;
			int num = 0;
			while (!flag || !flag2)
			{
				if ((num >= 0 || !flag) && (num <= 0 || !flag2))
				{
					if (num < 0 && nodeIndex + num < 0)
					{
						flag = true;
					}
					else if (num > 0 && nodeIndex + num >= this.nodes.Count)
					{
						flag2 = true;
					}
					else
					{
						TriangleMeshNode triangleMeshNode = (nodeIndex + num - 1 < 0) ? null : this.nodes[nodeIndex + num - 1];
						TriangleMeshNode triangleMeshNode2 = this.nodes[nodeIndex + num];
						TriangleMeshNode triangleMeshNode3 = (nodeIndex + num + 1 >= this.nodes.Count) ? null : this.nodes[nodeIndex + num + 1];
						if (triangleMeshNode2.Destroyed)
						{
							break;
						}
						if ((triangleMeshNode2.ClosestPointOnNodeXZ(position) - position).sqrMagnitude > range)
						{
							if (num < 0)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							for (int i = 0; i < 3; i++)
							{
								this.triBuffer[i] = 0;
							}
							for (int j = 0; j < triangleMeshNode2.connections.Length; j++)
							{
								TriangleMeshNode triangleMeshNode4 = triangleMeshNode2.connections[j].node as TriangleMeshNode;
								if (triangleMeshNode4 != null)
								{
									int num2 = -1;
									for (int k = 0; k < 3; k++)
									{
										for (int l = 0; l < 3; l++)
										{
											if (triangleMeshNode2.GetVertex(k) == triangleMeshNode4.GetVertex((l + 1) % 3) && triangleMeshNode2.GetVertex((k + 1) % 3) == triangleMeshNode4.GetVertex(l))
											{
												num2 = k;
												k = 3;
												break;
											}
										}
									}
									if (num2 != -1)
									{
										this.triBuffer[num2] = ((triangleMeshNode4 == triangleMeshNode || triangleMeshNode4 == triangleMeshNode3) ? 2 : 1);
									}
								}
							}
							for (int m = 0; m < 3; m++)
							{
								if (this.triBuffer[m] == 0)
								{
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex(m));
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex((m + 1) % 3));
								}
							}
						}
					}
				}
				num = ((num < 0) ? (-num) : (-num - 1));
			}
			if (this.path.transform != null)
			{
				for (int n = 0; n < wallBuffer.Count; n++)
				{
					wallBuffer[n] = this.path.transform.Transform(wallBuffer[n]);
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000788C File Offset: 0x00005A8C
		private bool FindNextCorners(Vector3 origin, int startIndex, List<Vector3> funnelPath, int numCorners, out bool lastCorner)
		{
			lastCorner = false;
			if (this.left == null)
			{
				throw new Exception("left list is null");
			}
			if (this.right == null)
			{
				throw new Exception("right list is null");
			}
			if (funnelPath == null)
			{
				throw new ArgumentNullException("funnelPath");
			}
			if (this.left.Count != this.right.Count)
			{
				throw new ArgumentException("left and right lists must have equal length");
			}
			int count = this.left.Count;
			if (count == 0)
			{
				throw new ArgumentException("no diagonals");
			}
			if (count - startIndex < 3)
			{
				funnelPath.Add(this.left[count - 1]);
				lastCorner = true;
				return true;
			}
			while (this.left[startIndex + 1] == this.left[startIndex + 2] && this.right[startIndex + 1] == this.right[startIndex + 2])
			{
				startIndex++;
				if (count - startIndex <= 3)
				{
					return false;
				}
			}
			Vector3 vector = this.left[startIndex + 2];
			if (vector == this.left[startIndex + 1])
			{
				vector = this.right[startIndex + 2];
			}
			while (VectorMath.IsColinearXZ(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], vector) == VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], origin))
			{
				startIndex++;
				if (count - startIndex < 3)
				{
					funnelPath.Add(this.left[count - 1]);
					lastCorner = true;
					return true;
				}
				vector = this.left[startIndex + 2];
				if (vector == this.left[startIndex + 1])
				{
					vector = this.right[startIndex + 2];
				}
			}
			Vector3 vector2 = origin;
			Vector3 vector3 = this.left[startIndex + 1];
			Vector3 vector4 = this.right[startIndex + 1];
			int num = startIndex + 1;
			int num2 = startIndex + 1;
			int i = startIndex + 2;
			while (i < count)
			{
				if (funnelPath.Count >= numCorners)
				{
					return true;
				}
				if (funnelPath.Count > 2000)
				{
					Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				Vector3 vector5 = this.left[i];
				Vector3 vector6 = this.right[i];
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector6) < 0f)
				{
					goto IL_2AE;
				}
				if (vector2 == vector4 || VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector6) <= 0f)
				{
					vector4 = vector6;
					num = i;
					goto IL_2AE;
				}
				funnelPath.Add(vector3);
				vector2 = vector3;
				int num3 = num2;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				IL_2FB:
				i++;
				continue;
				IL_2AE:
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector5) > 0f)
				{
					goto IL_2FB;
				}
				if (vector2 == vector3 || VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector5) >= 0f)
				{
					vector3 = vector5;
					num2 = i;
					goto IL_2FB;
				}
				funnelPath.Add(vector4);
				vector2 = vector4;
				int num4 = num;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num4;
				num = num4;
				i = num4;
				goto IL_2FB;
			}
			lastCorner = true;
			funnelPath.Add(this.left[count - 1]);
			return true;
		}

		// Token: 0x040000AE RID: 174
		private readonly List<Vector3> left;

		// Token: 0x040000AF RID: 175
		private readonly List<Vector3> right;

		// Token: 0x040000B0 RID: 176
		private List<TriangleMeshNode> nodes;

		// Token: 0x040000B1 RID: 177
		public Vector3 exactStart;

		// Token: 0x040000B2 RID: 178
		public Vector3 exactEnd;

		// Token: 0x040000B3 RID: 179
		private NavmeshBase graph;

		// Token: 0x040000B4 RID: 180
		private int currentNode;

		// Token: 0x040000B5 RID: 181
		private Vector3 currentPosition;

		// Token: 0x040000B6 RID: 182
		private int checkForDestroyedNodesCounter;

		// Token: 0x040000B7 RID: 183
		private RichPath path;

		// Token: 0x040000B8 RID: 184
		private int[] triBuffer = new int[3];

		// Token: 0x040000B9 RID: 185
		public bool funnelSimplification = true;

		// Token: 0x040000BA RID: 186
		private static Queue<TriangleMeshNode> navmeshClampQueue = new Queue<TriangleMeshNode>();

		// Token: 0x040000BB RID: 187
		private static List<TriangleMeshNode> navmeshClampList = new List<TriangleMeshNode>();

		// Token: 0x040000BC RID: 188
		private static Dictionary<TriangleMeshNode, TriangleMeshNode> navmeshClampDict = new Dictionary<TriangleMeshNode, TriangleMeshNode>();
	}
}
