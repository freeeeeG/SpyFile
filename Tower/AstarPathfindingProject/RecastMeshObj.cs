using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000076 RID: 118
	[AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_recast_mesh_obj.php")]
	public class RecastMeshObj : VersionedMonoBehaviour
	{
		// Token: 0x0600062E RID: 1582 RVA: 0x00024620 File Offset: 0x00022820
		public static void GetAllInBounds(List<RecastMeshObj> buffer, Bounds bounds)
		{
			if (!Application.isPlaying)
			{
				RecastMeshObj[] array = Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RecalculateBounds();
					if (array[i].GetBounds().Intersects(bounds))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (Time.timeSinceLevelLoad == 0f)
			{
				RecastMeshObj[] array2 = Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Register();
				}
			}
			for (int k = 0; k < RecastMeshObj.dynamicMeshObjs.Count; k++)
			{
				if (RecastMeshObj.dynamicMeshObjs[k].GetBounds().Intersects(bounds))
				{
					buffer.Add(RecastMeshObj.dynamicMeshObjs[k]);
				}
			}
			Rect rect = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			RecastMeshObj.tree.QueryInBounds(rect, buffer);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00024744 File Offset: 0x00022944
		private void OnEnable()
		{
			this.Register();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002474C File Offset: 0x0002294C
		private void Register()
		{
			if (this.registered)
			{
				return;
			}
			this.registered = true;
			this.area = Mathf.Clamp(this.area, -1, 33554432);
			Renderer component = base.GetComponent<Renderer>();
			Collider component2 = base.GetComponent<Collider>();
			if (component == null && component2 == null)
			{
				throw new Exception("A renderer or a collider should be attached to the GameObject");
			}
			MeshFilter component3 = base.GetComponent<MeshFilter>();
			if (component != null && component3 == null)
			{
				throw new Exception("A renderer was attached but no mesh filter");
			}
			this.bounds = ((component != null) ? component.bounds : component2.bounds);
			this._dynamic = this.dynamic;
			if (this._dynamic)
			{
				RecastMeshObj.dynamicMeshObjs.Add(this);
				return;
			}
			RecastMeshObj.tree.Insert(this);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00024818 File Offset: 0x00022A18
		private void RecalculateBounds()
		{
			Renderer component = base.GetComponent<Renderer>();
			Collider collider = this.GetCollider();
			if (component == null && collider == null)
			{
				throw new Exception("A renderer or a collider should be attached to the GameObject");
			}
			MeshFilter component2 = base.GetComponent<MeshFilter>();
			if (component != null && component2 == null)
			{
				throw new Exception("A renderer was attached but no mesh filter");
			}
			this.bounds = ((component != null) ? component.bounds : collider.bounds);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00024891 File Offset: 0x00022A91
		public Bounds GetBounds()
		{
			if (this._dynamic)
			{
				this.RecalculateBounds();
			}
			return this.bounds;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000248A7 File Offset: 0x00022AA7
		public MeshFilter GetMeshFilter()
		{
			return base.GetComponent<MeshFilter>();
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000248AF File Offset: 0x00022AAF
		public Collider GetCollider()
		{
			return base.GetComponent<Collider>();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000248B8 File Offset: 0x00022AB8
		private void OnDisable()
		{
			this.registered = false;
			if (this._dynamic)
			{
				RecastMeshObj.dynamicMeshObjs.Remove(this);
			}
			else if (!RecastMeshObj.tree.Remove(this))
			{
				throw new Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
			}
			this._dynamic = this.dynamic;
		}

		// Token: 0x0400037A RID: 890
		protected static RecastBBTree tree = new RecastBBTree();

		// Token: 0x0400037B RID: 891
		protected static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();

		// Token: 0x0400037C RID: 892
		[HideInInspector]
		public Bounds bounds;

		// Token: 0x0400037D RID: 893
		public bool dynamic = true;

		// Token: 0x0400037E RID: 894
		public int area;

		// Token: 0x0400037F RID: 895
		private bool _dynamic;

		// Token: 0x04000380 RID: 896
		private bool registered;
	}
}
