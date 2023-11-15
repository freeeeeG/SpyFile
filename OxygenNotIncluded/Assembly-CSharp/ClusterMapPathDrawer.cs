using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
public class ClusterMapPathDrawer : MonoBehaviour
{
	// Token: 0x060053D5 RID: 21461 RVA: 0x001E3368 File Offset: 0x001E1568
	public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x001E3381 File Offset: 0x001E1581
	public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

	// Token: 0x04003807 RID: 14343
	public ClusterMapPath pathPrefab;

	// Token: 0x04003808 RID: 14344
	public Transform pathContainer;
}
