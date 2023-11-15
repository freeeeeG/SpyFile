using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class MB3_MeshBakerGrouperCluster : MB3_MeshBakerGrouperCore
	{
		// Token: 0x0600028A RID: 650 RVA: 0x0001BDDF File Offset: 0x0001A1DF
		public MB3_MeshBakerGrouperCluster(GrouperData data, List<GameObject> gos)
		{
			this.d = data;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0001BE10 File Offset: 0x0001A210
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			for (int i = 0; i < this._clustersToDraw.Count; i++)
			{
				MB3_AgglomerativeClustering.ClusterNode clusterNode = this._clustersToDraw[i];
				List<Renderer> list = new List<Renderer>();
				for (int j = 0; j < clusterNode.leafs.Length; j++)
				{
					Renderer component = this.cluster.clusters[clusterNode.leafs[j]].leaf.go.GetComponent<Renderer>();
					if (component is MeshRenderer || component is SkinnedMeshRenderer)
					{
						list.Add(component);
					}
				}
				dictionary.Add("Cluster_" + i, list);
			}
			return dictionary;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0001BED0 File Offset: 0x0001A2D0
		public void BuildClusters(List<GameObject> gos, ProgressUpdateCancelableDelegate progFunc)
		{
			MB3_MeshBakerGrouperCluster.<BuildClusters>c__AnonStorey0 <BuildClusters>c__AnonStorey = new MB3_MeshBakerGrouperCluster.<BuildClusters>c__AnonStorey0();
			<BuildClusters>c__AnonStorey.gos = gos;
			if (<BuildClusters>c__AnonStorey.gos.Count == 0)
			{
				Debug.LogWarning("No objects to cluster. Add some objects to the list of Objects To Combine.");
				return;
			}
			if (this.cluster == null)
			{
				this.cluster = new MB3_AgglomerativeClustering();
			}
			List<MB3_AgglomerativeClustering.item_s> list = new List<MB3_AgglomerativeClustering.item_s>();
			int i;
			for (i = 0; i < <BuildClusters>c__AnonStorey.gos.Count; i++)
			{
				if (<BuildClusters>c__AnonStorey.gos[i] != null && list.Find((MB3_AgglomerativeClustering.item_s x) => x.go == <BuildClusters>c__AnonStorey.gos[i]) == null)
				{
					Renderer component = <BuildClusters>c__AnonStorey.gos[i].GetComponent<Renderer>();
					if (component != null && (component is MeshRenderer || component is SkinnedMeshRenderer))
					{
						list.Add(new MB3_AgglomerativeClustering.item_s
						{
							go = <BuildClusters>c__AnonStorey.gos[i],
							coord = component.bounds.center
						});
					}
				}
			}
			this.cluster.items = list;
			this.cluster.agglomerate(progFunc);
			if (!this.cluster.wasCanceled)
			{
				float a;
				float b;
				this._BuildListOfClustersToDraw(progFunc, out a, out b);
				this.d.maxDistBetweenClusters = Mathf.Lerp(a, b, 0.9f);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001C054 File Offset: 0x0001A454
		private void _BuildListOfClustersToDraw(ProgressUpdateCancelableDelegate progFunc, out float smallest, out float largest)
		{
			this._clustersToDraw.Clear();
			if (this.cluster.clusters == null)
			{
				smallest = 1f;
				largest = 10f;
				return;
			}
			if (progFunc != null)
			{
				progFunc("Building Clusters To Draw A:", 0f);
			}
			List<MB3_AgglomerativeClustering.ClusterNode> list = new List<MB3_AgglomerativeClustering.ClusterNode>();
			largest = 1f;
			smallest = 10000000f;
			for (int i = 0; i < this.cluster.clusters.Length; i++)
			{
				MB3_AgglomerativeClustering.ClusterNode clusterNode = this.cluster.clusters[i];
				if (clusterNode.distToMergedCentroid <= this.d.maxDistBetweenClusters)
				{
					if (this.d.includeCellsWithOnlyOneRenderer)
					{
						this._clustersToDraw.Add(clusterNode);
					}
					else if (clusterNode.leaf == null)
					{
						this._clustersToDraw.Add(clusterNode);
					}
				}
				if (clusterNode.distToMergedCentroid > largest)
				{
					largest = clusterNode.distToMergedCentroid;
				}
				if (clusterNode.height > 0 && clusterNode.distToMergedCentroid < smallest)
				{
					smallest = clusterNode.distToMergedCentroid;
				}
			}
			if (progFunc != null)
			{
				progFunc("Building Clusters To Draw B:", 0f);
			}
			for (int j = 0; j < this._clustersToDraw.Count; j++)
			{
				list.Add(this._clustersToDraw[j].cha);
				list.Add(this._clustersToDraw[j].chb);
			}
			for (int k = 0; k < list.Count; k++)
			{
				this._clustersToDraw.Remove(list[k]);
			}
			this._radii = new float[this._clustersToDraw.Count];
			if (progFunc != null)
			{
				progFunc("Building Clusters To Draw C:", 0f);
			}
			for (int l = 0; l < this._radii.Length; l++)
			{
				MB3_AgglomerativeClustering.ClusterNode clusterNode2 = this._clustersToDraw[l];
				Bounds bounds = new Bounds(clusterNode2.centroid, Vector3.one);
				for (int m = 0; m < clusterNode2.leafs.Length; m++)
				{
					Renderer component = this.cluster.clusters[clusterNode2.leafs[m]].leaf.go.GetComponent<Renderer>();
					if (component != null)
					{
						bounds.Encapsulate(component.bounds);
					}
				}
				this._radii[l] = bounds.extents.magnitude;
			}
			if (progFunc != null)
			{
				progFunc("Building Clusters To Draw D:", 0f);
			}
			this._ObjsExtents = largest + 1f;
			this._minDistBetweenClusters = Mathf.Lerp(smallest, 0f, 0.9f);
			if (this._ObjsExtents < 2f)
			{
				this._ObjsExtents = 2f;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0001C334 File Offset: 0x0001A734
		public override void DrawGizmos(Bounds sceneObjectBounds)
		{
			if (this.cluster == null || this.cluster.clusters == null)
			{
				return;
			}
			if (this._lastMaxDistBetweenClusters != this.d.maxDistBetweenClusters)
			{
				float num;
				float num2;
				this._BuildListOfClustersToDraw(null, out num, out num2);
				this._lastMaxDistBetweenClusters = this.d.maxDistBetweenClusters;
			}
			for (int i = 0; i < this._clustersToDraw.Count; i++)
			{
				Gizmos.color = Color.white;
				MB3_AgglomerativeClustering.ClusterNode clusterNode = this._clustersToDraw[i];
				Gizmos.DrawWireSphere(clusterNode.centroid, this._radii[i]);
			}
		}

		// Token: 0x040001A3 RID: 419
		public MB3_AgglomerativeClustering cluster;

		// Token: 0x040001A4 RID: 420
		private float _lastMaxDistBetweenClusters;

		// Token: 0x040001A5 RID: 421
		public float _ObjsExtents = 10f;

		// Token: 0x040001A6 RID: 422
		public float _minDistBetweenClusters = 0.001f;

		// Token: 0x040001A7 RID: 423
		private List<MB3_AgglomerativeClustering.ClusterNode> _clustersToDraw = new List<MB3_AgglomerativeClustering.ClusterNode>();

		// Token: 0x040001A8 RID: 424
		private float[] _radii;
	}
}
