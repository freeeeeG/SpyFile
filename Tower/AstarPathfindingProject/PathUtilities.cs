﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009E RID: 158
	public static class PathUtilities
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x0002C828 File Offset: 0x0002AA28
		public static bool IsPathPossible(GraphNode node1, GraphNode node2)
		{
			return node1.Walkable && node2.Walkable && node1.Area == node2.Area;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002C84C File Offset: 0x0002AA4C
		public static bool IsPathPossible(List<GraphNode> nodes)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			uint area = nodes[0].Area;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!nodes[i].Walkable || nodes[i].Area != area)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002C8A4 File Offset: 0x0002AAA4
		public static bool IsPathPossible(List<GraphNode> nodes, int tagMask)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			if ((tagMask >> (int)nodes[0].Tag & 1) == 0)
			{
				return false;
			}
			if (!PathUtilities.IsPathPossible(nodes))
			{
				return false;
			}
			List<GraphNode> reachableNodes = PathUtilities.GetReachableNodes(nodes[0], tagMask, null);
			bool result = true;
			for (int i = 1; i < nodes.Count; i++)
			{
				if (!reachableNodes.Contains(nodes[i]))
				{
					result = false;
					break;
				}
			}
			ListPool<GraphNode>.Release(ref reachableNodes);
			return result;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002C91C File Offset: 0x0002AB1C
		public static List<GraphNode> GetReachableNodes(GraphNode seed, int tagMask = -1, Func<GraphNode, bool> filter = null)
		{
			Stack<GraphNode> dfsStack = StackPool<GraphNode>.Claim();
			List<GraphNode> reachable = ListPool<GraphNode>.Claim();
			HashSet<GraphNode> map = new HashSet<GraphNode>();
			Action<GraphNode> action;
			if (tagMask == -1 && filter == null)
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && map.Add(node))
					{
						reachable.Add(node);
						dfsStack.Push(node);
					}
				};
			}
			else
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && map.Add(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						reachable.Add(node);
						dfsStack.Push(node);
					}
				};
			}
			action(seed);
			while (dfsStack.Count > 0)
			{
				dfsStack.Pop().GetConnections(action);
			}
			StackPool<GraphNode>.Release(dfsStack);
			return reachable;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002C9C4 File Offset: 0x0002ABC4
		public static List<GraphNode> BFS(GraphNode seed, int depth, int tagMask = -1, Func<GraphNode, bool> filter = null)
		{
			PathUtilities.BFSQueue = (PathUtilities.BFSQueue ?? new Queue<GraphNode>());
			Queue<GraphNode> que = PathUtilities.BFSQueue;
			PathUtilities.BFSMap = (PathUtilities.BFSMap ?? new Dictionary<GraphNode, int>());
			Dictionary<GraphNode, int> map = PathUtilities.BFSMap;
			que.Clear();
			map.Clear();
			List<GraphNode> result = ListPool<GraphNode>.Claim();
			int currentDist = -1;
			Action<GraphNode> action;
			if (tagMask == -1)
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && !map.ContainsKey(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			else
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && !map.ContainsKey(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			action(seed);
			while (que.Count > 0)
			{
				GraphNode graphNode = que.Dequeue();
				currentDist = map[graphNode];
				if (currentDist >= depth)
				{
					break;
				}
				graphNode.GetConnections(action);
			}
			que.Clear();
			map.Clear();
			return result;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		public static List<Vector3> GetSpiralPoints(int count, float clearance)
		{
			List<Vector3> list = ListPool<Vector3>.Claim(count);
			float num = clearance / 6.2831855f;
			float num2 = 0f;
			list.Add(PathUtilities.InvoluteOfCircle(num, num2));
			for (int i = 0; i < count; i++)
			{
				Vector3 b = list[list.Count - 1];
				float num3 = -num2 / 2f + Mathf.Sqrt(num2 * num2 / 4f + 2f * clearance / num);
				float num4 = num2 + num3;
				float num5 = num2 + 2f * num3;
				while (num5 - num4 > 0.01f)
				{
					float num6 = (num4 + num5) / 2f;
					if ((PathUtilities.InvoluteOfCircle(num, num6) - b).sqrMagnitude < clearance * clearance)
					{
						num4 = num6;
					}
					else
					{
						num5 = num6;
					}
				}
				list.Add(PathUtilities.InvoluteOfCircle(num, num5));
				num2 = num5;
			}
			return list;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002CBAA File Offset: 0x0002ADAA
		private static Vector3 InvoluteOfCircle(float a, float t)
		{
			return new Vector3(a * (Mathf.Cos(t) + t * Mathf.Sin(t)), 0f, a * (Mathf.Sin(t) - t * Mathf.Cos(t)));
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002CBD8 File Offset: 0x0002ADD8
		public static void GetPointsAroundPointWorld(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (previousPoints.Count == 0)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < previousPoints.Count; i++)
			{
				vector += previousPoints[i];
			}
			vector /= (float)previousPoints.Count;
			for (int j = 0; j < previousPoints.Count; j++)
			{
				int index = j;
				previousPoints[index] -= vector;
			}
			PathUtilities.GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002CC58 File Offset: 0x0002AE58
		public static void GetPointsAroundPoint(Vector3 center, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			NavGraph navGraph = g as NavGraph;
			if (navGraph == null)
			{
				throw new ArgumentException("g is not a NavGraph");
			}
			NNInfoInternal nearestForce = navGraph.GetNearestForce(center, NNConstraint.Default);
			center = nearestForce.clampedPosition;
			if (nearestForce.node == null)
			{
				return;
			}
			radius = Mathf.Max(radius, 1.4142f * clearanceRadius * Mathf.Sqrt((float)previousPoints.Count));
			clearanceRadius *= clearanceRadius;
			int i = 0;
			while (i < previousPoints.Count)
			{
				Vector3 vector = previousPoints[i];
				float magnitude = vector.magnitude;
				if (magnitude > 0f)
				{
					vector /= magnitude;
				}
				float num = radius;
				vector *= num;
				int num2 = 0;
				Vector3 vector2;
				for (;;)
				{
					vector2 = center + vector;
					GraphHitInfo graphHitInfo;
					if (g.Linecast(center, vector2, out graphHitInfo, null, null))
					{
						if (graphHitInfo.point == Vector3.zero)
						{
							num2++;
							if (num2 > 8)
							{
								goto Block_7;
							}
						}
						else
						{
							vector2 = graphHitInfo.point;
						}
					}
					bool flag = false;
					for (float num3 = 0.1f; num3 <= 1f; num3 += 0.05f)
					{
						Vector3 vector3 = Vector3.Lerp(center, vector2, num3);
						flag = true;
						for (int j = 0; j < i; j++)
						{
							if ((previousPoints[j] - vector3).sqrMagnitude < clearanceRadius)
							{
								flag = false;
								break;
							}
						}
						if (flag || num2 > 8)
						{
							flag = true;
							previousPoints[i] = vector3;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					clearanceRadius *= 0.9f;
					vector = Random.onUnitSphere * Mathf.Lerp(num, radius, (float)(num2 / 5));
					vector.y = 0f;
					num2++;
				}
				IL_194:
				i++;
				continue;
				Block_7:
				previousPoints[i] = vector2;
				goto IL_194;
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002CE0C File Offset: 0x0002B00C
		public static List<Vector3> GetPointsOnNodes(List<GraphNode> nodes, int count, float clearanceRadius = 0f)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			if (nodes.Count == 0)
			{
				throw new ArgumentException("no nodes passed");
			}
			List<Vector3> list = ListPool<Vector3>.Claim(count);
			clearanceRadius *= clearanceRadius;
			if (clearanceRadius > 0f || nodes[0] is TriangleMeshNode || nodes[0] is GridNode)
			{
				List<float> list2 = ListPool<float>.Claim(nodes.Count);
				float num = 0f;
				for (int i = 0; i < nodes.Count; i++)
				{
					float num2 = nodes[i].SurfaceArea();
					num2 += 0.001f;
					num += num2;
					list2.Add(num);
				}
				for (int j = 0; j < count; j++)
				{
					int num3 = 0;
					int num4 = 10;
					bool flag = false;
					while (!flag)
					{
						flag = true;
						if (num3 >= num4)
						{
							clearanceRadius *= 0.80999994f;
							num4 += 10;
							if (num4 > 100)
							{
								clearanceRadius = 0f;
							}
						}
						float item = Random.value * num;
						int num5 = list2.BinarySearch(item);
						if (num5 < 0)
						{
							num5 = ~num5;
						}
						if (num5 >= nodes.Count)
						{
							flag = false;
						}
						else
						{
							Vector3 vector = nodes[num5].RandomPointOnSurface();
							if (clearanceRadius > 0f)
							{
								for (int k = 0; k < list.Count; k++)
								{
									if ((list[k] - vector).sqrMagnitude < clearanceRadius)
									{
										flag = false;
										break;
									}
								}
							}
							if (flag)
							{
								list.Add(vector);
								break;
							}
							num3++;
						}
					}
				}
				ListPool<float>.Release(ref list2);
			}
			else
			{
				for (int l = 0; l < count; l++)
				{
					list.Add(nodes[Random.Range(0, nodes.Count)].RandomPointOnSurface());
				}
			}
			return list;
		}

		// Token: 0x04000429 RID: 1065
		private static Queue<GraphNode> BFSQueue;

		// Token: 0x0400042A RID: 1066
		private static Dictionary<GraphNode, int> BFSMap;
	}
}
