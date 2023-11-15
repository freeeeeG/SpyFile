using System;
using UnityEngine;

// Token: 0x02000BF6 RID: 3062
[ExecuteInEditMode]
public class WorldMapTileCollisionOptimiser : MonoBehaviour
{
	// Token: 0x06003E87 RID: 16007 RVA: 0x0012B355 File Offset: 0x00129755
	private void Awake()
	{
		if (Application.isPlaying)
		{
			base.enabled = false;
			return;
		}
		this.RefreshObjects();
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x0012B370 File Offset: 0x00129770
	private void Update()
	{
		if (this.m_gridManager == null)
		{
			this.m_gridManager = GameUtils.GetGridManager(base.transform);
		}
		else
		{
			GridIndex unclampedGridLocationFromPos = this.m_gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
			if (unclampedGridLocationFromPos != this.m_gridIndex)
			{
				this.Optimise(unclampedGridLocationFromPos);
				this.m_gridIndex = unclampedGridLocationFromPos;
			}
		}
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x0012B3DC File Offset: 0x001297DC
	private void RefreshObjects()
	{
		if (this.m_collider == null || !this.m_collider.IsInHierarchyOf(base.gameObject))
		{
			this.m_collider = base.gameObject.RequestChildRecursive("Collision");
		}
		if (this.m_rootMesh == null || !this.m_rootMesh.IsInHierarchyOf(base.gameObject))
		{
			this.m_rootMesh = base.gameObject.RequestChildRecursive("Tile");
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0012B464 File Offset: 0x00129864
	private void Optimise(GridIndex _index)
	{
		int num = Mathf.Abs(_index.Y % 2);
		if (this.m_rootMesh != null)
		{
			float z = this.m_rootMesh.transform.localScale.z;
			if ((z < 0f && num == 0) || (z > 0f && num == 1))
			{
				this.m_rootMesh.transform.localScale = this.m_rootMesh.transform.localScale.WithZ(-z);
			}
		}
		if (this.m_collider != null)
		{
			this.m_collider.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x04003239 RID: 12857
	[SerializeField]
	[AssignChild("Collision", Editorbility.NonEditable)]
	private GameObject m_collider;

	// Token: 0x0400323A RID: 12858
	[SerializeField]
	[AssignChild("Tile", Editorbility.NonEditable)]
	private GameObject m_rootMesh;

	// Token: 0x0400323B RID: 12859
	private GridManager m_gridManager;

	// Token: 0x0400323C RID: 12860
	private GridIndex m_gridIndex;
}
