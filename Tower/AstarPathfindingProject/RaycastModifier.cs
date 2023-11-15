using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200007E RID: 126
	[AddComponentMenu("Pathfinding/Modifiers/Raycast Modifier")]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_raycast_modifier.php")]
	[Serializable]
	public class RaycastModifier : MonoModifier
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002556D File Offset: 0x0002376D
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00025574 File Offset: 0x00023774
		public override void Apply(Path p)
		{
			if (!this.useRaycasting && !this.useGraphRaycasting)
			{
				return;
			}
			List<Vector3> list = p.vectorPath;
			this.cachedFilter.path = p;
			this.cachedNNConstraint.graphMask = p.nnConstraint.graphMask;
			if (this.ValidateLine(null, null, p.vectorPath[0], p.vectorPath[p.vectorPath.Count - 1], this.cachedFilter.cachedDelegate, this.cachedNNConstraint))
			{
				Vector3 item = p.vectorPath[0];
				Vector3 item2 = p.vectorPath[p.vectorPath.Count - 1];
				list.ClearFast<Vector3>();
				list.Add(item);
				list.Add(item2);
			}
			else
			{
				int num = RaycastModifier.iterationsByQuality[(int)this.quality];
				for (int i = 0; i < num; i++)
				{
					if (i != 0)
					{
						Polygon.Subdivide(list, RaycastModifier.buffer, 3);
						Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref list);
						RaycastModifier.buffer.ClearFast<Vector3>();
						list.Reverse();
					}
					list = ((this.quality >= RaycastModifier.Quality.High) ? this.ApplyDP(p, list, this.cachedFilter.cachedDelegate, this.cachedNNConstraint) : this.ApplyGreedy(p, list, this.cachedFilter.cachedDelegate, this.cachedNNConstraint));
				}
				if (num % 2 == 0)
				{
					list.Reverse();
				}
			}
			p.vectorPath = list;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000256D4 File Offset: 0x000238D4
		private List<Vector3> ApplyGreedy(Path p, List<Vector3> points, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			bool flag = points.Count == p.path.Count;
			int i = 0;
			while (i < points.Count)
			{
				Vector3 vector = points[i];
				GraphNode n = (flag && points[i] == (Vector3)p.path[i].position) ? p.path[i] : null;
				RaycastModifier.buffer.Add(vector);
				int num = 1;
				int num2 = 2;
				for (;;)
				{
					int num3 = i + num2;
					if (num3 >= points.Count)
					{
						goto Block_2;
					}
					Vector3 vector2 = points[num3];
					GraphNode n2 = (flag && vector2 == (Vector3)p.path[num3].position) ? p.path[num3] : null;
					if (!this.ValidateLine(n, n2, vector, vector2, filter, nnConstraint))
					{
						break;
					}
					num = num2;
					num2 *= 2;
				}
				IL_14F:
				while (num + 1 < num2)
				{
					int num4 = (num + num2) / 2;
					int index = i + num4;
					Vector3 vector3 = points[index];
					GraphNode n3 = (flag && vector3 == (Vector3)p.path[index].position) ? p.path[index] : null;
					if (this.ValidateLine(n, n3, vector, vector3, filter, nnConstraint))
					{
						num = num4;
					}
					else
					{
						num2 = num4;
					}
				}
				i += num;
				continue;
				Block_2:
				num2 = points.Count - i;
				goto IL_14F;
			}
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00025860 File Offset: 0x00023A60
		private List<Vector3> ApplyDP(Path p, List<Vector3> points, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			if (RaycastModifier.DPCosts.Length < points.Count)
			{
				RaycastModifier.DPCosts = new float[points.Count];
				RaycastModifier.DPParents = new int[points.Count];
			}
			for (int i = 0; i < RaycastModifier.DPParents.Length; i++)
			{
				RaycastModifier.DPCosts[i] = (float)(RaycastModifier.DPParents[i] = -1);
			}
			bool flag = points.Count == p.path.Count;
			for (int j = 0; j < points.Count; j++)
			{
				float num = RaycastModifier.DPCosts[j];
				Vector3 vector = points[j];
				bool flag2 = flag && vector == (Vector3)p.path[j].position;
				for (int k = j + 1; k < points.Count; k++)
				{
					float num2 = num + (points[k] - vector).magnitude + 0.0001f;
					if (RaycastModifier.DPParents[k] == -1 || num2 < RaycastModifier.DPCosts[k])
					{
						bool flag3 = flag && points[k] == (Vector3)p.path[k].position;
						if (k != j + 1 && !this.ValidateLine(flag2 ? p.path[j] : null, flag3 ? p.path[k] : null, vector, points[k], filter, nnConstraint))
						{
							break;
						}
						RaycastModifier.DPCosts[k] = num2;
						RaycastModifier.DPParents[k] = j;
					}
				}
			}
			for (int num3 = points.Count - 1; num3 != -1; num3 = RaycastModifier.DPParents[num3])
			{
				RaycastModifier.buffer.Add(points[num3]);
			}
			RaycastModifier.buffer.Reverse();
			Memory.Swap<List<Vector3>>(ref RaycastModifier.buffer, ref points);
			RaycastModifier.buffer.ClearFast<Vector3>();
			return points;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00025A54 File Offset: 0x00023C54
		protected bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2, Func<GraphNode, bool> filter, NNConstraint nnConstraint)
		{
			if (this.useRaycasting)
			{
				if (this.use2DPhysics)
				{
					if (this.thickRaycast && this.thickRaycastRadius > 0f && Physics2D.CircleCast(v1 + this.raycastOffset, this.thickRaycastRadius, v2 - v1, (v2 - v1).magnitude, this.mask))
					{
						return false;
					}
					if (Physics2D.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
				}
				else
				{
					if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
					{
						return false;
					}
					if (this.thickRaycast && this.thickRaycastRadius > 0f)
					{
						if (Physics.CheckSphere(v1 + this.raycastOffset + (v2 - v1).normalized * this.thickRaycastRadius, this.thickRaycastRadius, this.mask))
						{
							return false;
						}
						if (Physics.SphereCast(new Ray(v1 + this.raycastOffset, v2 - v1), this.thickRaycastRadius, (v2 - v1).magnitude, this.mask))
						{
							return false;
						}
					}
				}
			}
			if (this.useGraphRaycasting)
			{
				bool flag = n1 != null && n2 != null;
				if (n1 == null)
				{
					n1 = AstarPath.active.GetNearest(v1, nnConstraint).node;
				}
				if (n2 == null)
				{
					n2 = AstarPath.active.GetNearest(v2, nnConstraint).node;
				}
				if (n1 != null && n2 != null)
				{
					NavGraph graph = n1.Graph;
					NavGraph graph2 = n2.Graph;
					if (graph != graph2)
					{
						return false;
					}
					IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
					GridGraph gridGraph = graph as GridGraph;
					if (flag && gridGraph != null)
					{
						return !gridGraph.Linecast(n1 as GridNodeBase, n2 as GridNodeBase, filter);
					}
					if (raycastableGraph != null)
					{
						GraphHitInfo graphHitInfo;
						return !raycastableGraph.Linecast(v1, v2, out graphHitInfo, null, filter);
					}
				}
			}
			return true;
		}

		// Token: 0x04000394 RID: 916
		public bool useRaycasting = true;

		// Token: 0x04000395 RID: 917
		public LayerMask mask = -1;

		// Token: 0x04000396 RID: 918
		[Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
		public bool thickRaycast;

		// Token: 0x04000397 RID: 919
		[Tooltip("Distance from the ray which will be checked for colliders")]
		public float thickRaycastRadius;

		// Token: 0x04000398 RID: 920
		[Tooltip("Check for intersections with 2D colliders instead of 3D colliders.")]
		public bool use2DPhysics;

		// Token: 0x04000399 RID: 921
		[Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
		public Vector3 raycastOffset = Vector3.zero;

		// Token: 0x0400039A RID: 922
		[Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
		public bool useGraphRaycasting;

		// Token: 0x0400039B RID: 923
		[Tooltip("When using the high quality mode the script will try harder to find a shorter path. This is significantly slower than the greedy low quality approach.")]
		public RaycastModifier.Quality quality = RaycastModifier.Quality.Medium;

		// Token: 0x0400039C RID: 924
		private static readonly int[] iterationsByQuality = new int[]
		{
			1,
			2,
			1,
			3
		};

		// Token: 0x0400039D RID: 925
		private static List<Vector3> buffer = new List<Vector3>();

		// Token: 0x0400039E RID: 926
		private static float[] DPCosts = new float[16];

		// Token: 0x0400039F RID: 927
		private static int[] DPParents = new int[16];

		// Token: 0x040003A0 RID: 928
		private RaycastModifier.Filter cachedFilter = new RaycastModifier.Filter();

		// Token: 0x040003A1 RID: 929
		private NNConstraint cachedNNConstraint = NNConstraint.None;

		// Token: 0x0200014C RID: 332
		public enum Quality
		{
			// Token: 0x040007A2 RID: 1954
			Low,
			// Token: 0x040007A3 RID: 1955
			Medium,
			// Token: 0x040007A4 RID: 1956
			High,
			// Token: 0x040007A5 RID: 1957
			Highest
		}

		// Token: 0x0200014D RID: 333
		private class Filter
		{
			// Token: 0x06000B4C RID: 2892 RVA: 0x00047AED File Offset: 0x00045CED
			public Filter()
			{
				this.cachedDelegate = new Func<GraphNode, bool>(this.CanTraverse);
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x00047B07 File Offset: 0x00045D07
			private bool CanTraverse(GraphNode node)
			{
				return this.path.CanTraverse(node);
			}

			// Token: 0x040007A6 RID: 1958
			public Path path;

			// Token: 0x040007A7 RID: 1959
			public readonly Func<GraphNode, bool> cachedDelegate;
		}
	}
}
