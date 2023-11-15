using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000061 RID: 97
	[Serializable]
	public class MB3_MeshBakerGrouperKMeans : MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0001BBF5 File Offset: 0x00019FF5
		public MB3_MeshBakerGrouperKMeans(GrouperData data)
		{
			this.d = data;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0001BC24 File Offset: 0x0001A024
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			List<GameObject> list = new List<GameObject>();
			int num = 20;
			foreach (GameObject gameObject in selection)
			{
				if (!(gameObject == null))
				{
					GameObject gameObject2 = gameObject;
					Renderer component = gameObject2.GetComponent<Renderer>();
					if (component is MeshRenderer || component is SkinnedMeshRenderer)
					{
						list.Add(gameObject2);
					}
				}
			}
			if (list.Count > 0 && num > 0 && num < list.Count)
			{
				MB3_KMeansClustering mb3_KMeansClustering = new MB3_KMeansClustering(list, num);
				mb3_KMeansClustering.Cluster();
				this.clusterCenters = new Vector3[num];
				this.clusterSizes = new float[num];
				for (int i = 0; i < num; i++)
				{
					List<Renderer> cluster = mb3_KMeansClustering.GetCluster(i, out this.clusterCenters[i], out this.clusterSizes[i]);
					if (cluster.Count > 0)
					{
						dictionary.Add("Cluster_" + i, cluster);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0001BD70 File Offset: 0x0001A170
		public override void DrawGizmos(Bounds sceneObjectBounds)
		{
			if (this.clusterCenters != null && this.clusterSizes != null && this.clusterCenters.Length == this.clusterSizes.Length)
			{
				for (int i = 0; i < this.clusterSizes.Length; i++)
				{
					Gizmos.DrawWireSphere(this.clusterCenters[i], this.clusterSizes[i]);
				}
			}
		}

		// Token: 0x040001A0 RID: 416
		public int numClusters = 4;

		// Token: 0x040001A1 RID: 417
		public Vector3[] clusterCenters = new Vector3[0];

		// Token: 0x040001A2 RID: 418
		public float[] clusterSizes = new float[0];
	}
}
