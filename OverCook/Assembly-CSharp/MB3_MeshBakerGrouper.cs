using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class MB3_MeshBakerGrouper : MonoBehaviour
{
	// Token: 0x06000274 RID: 628 RVA: 0x0001AB68 File Offset: 0x00018F68
	private void OnDrawGizmosSelected()
	{
		if (this.grouper == null)
		{
			this.grouper = this.CreateGrouper(this.clusterType, this.data);
		}
		if (this.grouper.d == null)
		{
			this.grouper.d = this.data;
		}
		this.grouper.DrawGizmos(this.sourceObjectBounds);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0001ABCC File Offset: 0x00018FCC
	public MB3_MeshBakerGrouperCore CreateGrouper(MB3_MeshBakerGrouper.ClusterType t, GrouperData data)
	{
		if (t == MB3_MeshBakerGrouper.ClusterType.grid)
		{
			this.grouper = new MB3_MeshBakerGrouperGrid(data);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.pie)
		{
			this.grouper = new MB3_MeshBakerGrouperPie(data);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.agglomerative)
		{
			MB3_TextureBaker component = base.GetComponent<MB3_TextureBaker>();
			List<GameObject> gos;
			if (component != null)
			{
				gos = component.GetObjectsToCombine();
			}
			else
			{
				gos = new List<GameObject>();
			}
			this.grouper = new MB3_MeshBakerGrouperCluster(data, gos);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.none)
		{
			this.grouper = new MB3_MeshBakerGrouperNone(data);
		}
		return this.grouper;
	}

	// Token: 0x0400018D RID: 397
	public MB3_MeshBakerGrouperCore grouper;

	// Token: 0x0400018E RID: 398
	public MB3_MeshBakerGrouper.ClusterType clusterType;

	// Token: 0x0400018F RID: 399
	public GrouperData data = new GrouperData();

	// Token: 0x04000190 RID: 400
	[HideInInspector]
	public Bounds sourceObjectBounds = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x0200005B RID: 91
	public enum ClusterType
	{
		// Token: 0x04000192 RID: 402
		none,
		// Token: 0x04000193 RID: 403
		grid,
		// Token: 0x04000194 RID: 404
		pie,
		// Token: 0x04000195 RID: 405
		agglomerative
	}
}
