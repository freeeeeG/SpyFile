using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000085 RID: 133
	[AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_relevant_graph_surface.php")]
	public class RelevantGraphSurface : VersionedMonoBehaviour
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00027E43 File Offset: 0x00026043
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00027E4B File Offset: 0x0002604B
		public RelevantGraphSurface Next
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00027E53 File Offset: 0x00026053
		public RelevantGraphSurface Prev
		{
			get
			{
				return this.prev;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00027E5B File Offset: 0x0002605B
		public static RelevantGraphSurface Root
		{
			get
			{
				return RelevantGraphSurface.root;
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00027E62 File Offset: 0x00026062
		public void UpdatePosition()
		{
			this.position = base.transform.position;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00027E75 File Offset: 0x00026075
		private void OnEnable()
		{
			this.UpdatePosition();
			if (RelevantGraphSurface.root == null)
			{
				RelevantGraphSurface.root = this;
				return;
			}
			this.next = RelevantGraphSurface.root;
			RelevantGraphSurface.root.prev = this;
			RelevantGraphSurface.root = this;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00027EB0 File Offset: 0x000260B0
		private void OnDisable()
		{
			if (RelevantGraphSurface.root == this)
			{
				RelevantGraphSurface.root = this.next;
				if (RelevantGraphSurface.root != null)
				{
					RelevantGraphSurface.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00027F3C File Offset: 0x0002613C
		public static void UpdateAllPositions()
		{
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.root;
			while (relevantGraphSurface != null)
			{
				relevantGraphSurface.UpdatePosition();
				relevantGraphSurface = relevantGraphSurface.Next;
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00027F68 File Offset: 0x00026168
		public static void FindAllGraphSurfaces()
		{
			RelevantGraphSurface[] array = Object.FindObjectsOfType(typeof(RelevantGraphSurface)) as RelevantGraphSurface[];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnDisable();
				array[i].OnEnable();
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00027FA8 File Offset: 0x000261A8
		public void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.22352941f, 0.827451f, 0.18039216f, 0.4f);
			Gizmos.DrawLine(base.transform.position - Vector3.up * this.maxRange, base.transform.position + Vector3.up * this.maxRange);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00028018 File Offset: 0x00026218
		public void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.22352941f, 0.827451f, 0.18039216f);
			Gizmos.DrawLine(base.transform.position - Vector3.up * this.maxRange, base.transform.position + Vector3.up * this.maxRange);
		}

		// Token: 0x040003DE RID: 990
		private static RelevantGraphSurface root;

		// Token: 0x040003DF RID: 991
		public float maxRange = 1f;

		// Token: 0x040003E0 RID: 992
		private RelevantGraphSurface prev;

		// Token: 0x040003E1 RID: 993
		private RelevantGraphSurface next;

		// Token: 0x040003E2 RID: 994
		private Vector3 position;
	}
}
