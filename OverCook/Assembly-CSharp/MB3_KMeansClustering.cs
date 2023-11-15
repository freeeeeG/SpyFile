using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class MB3_KMeansClustering
{
	// Token: 0x060000C5 RID: 197 RVA: 0x00008358 File Offset: 0x00006758
	public MB3_KMeansClustering(List<GameObject> gos, int numClusters)
	{
		for (int i = 0; i < gos.Count; i++)
		{
			if (gos[i] != null)
			{
				MB3_KMeansClustering.DataPoint item = new MB3_KMeansClustering.DataPoint(gos[i]);
				this._normalizedDataToCluster.Add(item);
			}
			else
			{
				Debug.LogWarning(string.Format("Object {0} in list of objects to cluster was null.", i));
			}
		}
		if (numClusters <= 0)
		{
			Debug.LogError("Number of clusters must be posititve.");
			numClusters = 1;
		}
		if (this._normalizedDataToCluster.Count <= numClusters)
		{
			Debug.LogError("There must be fewer clusters than objects to cluster");
			numClusters = this._normalizedDataToCluster.Count - 1;
		}
		this._numberOfClusters = numClusters;
		if (this._numberOfClusters <= 0)
		{
			this._numberOfClusters = 1;
		}
		this._clusters = new Vector3[this._numberOfClusters];
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00008448 File Offset: 0x00006848
	private void InitializeCentroids()
	{
		for (int i = 0; i < this._numberOfClusters; i++)
		{
			this._normalizedDataToCluster[i].Cluster = i;
		}
		for (int j = this._numberOfClusters; j < this._normalizedDataToCluster.Count; j++)
		{
			this._normalizedDataToCluster[j].Cluster = UnityEngine.Random.Range(0, this._numberOfClusters);
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000084BC File Offset: 0x000068BC
	private bool UpdateDataPointMeans(bool force)
	{
		if (this.AnyAreEmpty(this._normalizedDataToCluster) && !force)
		{
			return false;
		}
		Vector3[] array = new Vector3[this._numberOfClusters];
		int[] array2 = new int[this._numberOfClusters];
		for (int i = 0; i < this._normalizedDataToCluster.Count; i++)
		{
			int cluster = this._normalizedDataToCluster[i].Cluster;
			array[cluster] += this._normalizedDataToCluster[i].center;
			array2[cluster]++;
		}
		for (int j = 0; j < this._numberOfClusters; j++)
		{
			this._clusters[j] = array[j] / (float)array2[j];
		}
		return true;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000085A4 File Offset: 0x000069A4
	private bool AnyAreEmpty(List<MB3_KMeansClustering.DataPoint> data)
	{
		int[] array = new int[this._numberOfClusters];
		for (int i = 0; i < this._normalizedDataToCluster.Count; i++)
		{
			array[this._normalizedDataToCluster[i].Cluster]++;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00008614 File Offset: 0x00006A14
	private bool UpdateClusterMembership()
	{
		bool result = false;
		float[] array = new float[this._numberOfClusters];
		for (int i = 0; i < this._normalizedDataToCluster.Count; i++)
		{
			for (int j = 0; j < this._numberOfClusters; j++)
			{
				array[j] = this.ElucidanDistance(this._normalizedDataToCluster[i], this._clusters[j]);
			}
			int num = this.MinIndex(array);
			if (num != this._normalizedDataToCluster[i].Cluster)
			{
				result = true;
				this._normalizedDataToCluster[i].Cluster = num;
			}
		}
		return result;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000086CC File Offset: 0x00006ACC
	private float ElucidanDistance(MB3_KMeansClustering.DataPoint dataPoint, Vector3 mean)
	{
		return Vector3.Distance(dataPoint.center, mean);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000086DC File Offset: 0x00006ADC
	private int MinIndex(float[] distances)
	{
		int result = 0;
		double num = (double)distances[0];
		for (int i = 0; i < distances.Length; i++)
		{
			if ((double)distances[i] < num)
			{
				num = (double)distances[i];
				result = i;
			}
		}
		return result;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00008718 File Offset: 0x00006B18
	public List<Renderer> GetCluster(int idx, out Vector3 mean, out float size)
	{
		if (idx < 0 || idx >= this._numberOfClusters)
		{
			Debug.LogError("idx is out of bounds");
			mean = Vector3.zero;
			size = 1f;
			return new List<Renderer>();
		}
		this.UpdateDataPointMeans(true);
		List<Renderer> list = new List<Renderer>();
		mean = this._clusters[idx];
		float num = 0f;
		for (int i = 0; i < this._normalizedDataToCluster.Count; i++)
		{
			if (this._normalizedDataToCluster[i].Cluster == idx)
			{
				float num2 = Vector3.Distance(mean, this._normalizedDataToCluster[i].center);
				if (num2 > num)
				{
					num = num2;
				}
				list.Add(this._normalizedDataToCluster[i].gameObject.GetComponent<Renderer>());
			}
		}
		mean = this._clusters[idx];
		size = num;
		return list;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008818 File Offset: 0x00006C18
	public void Cluster()
	{
		bool flag = true;
		bool flag2 = true;
		this.InitializeCentroids();
		int num = this._normalizedDataToCluster.Count * 1000;
		int num2 = 0;
		while (flag2 && flag && num2 < num)
		{
			num2++;
			flag2 = this.UpdateDataPointMeans(false);
			flag = this.UpdateClusterMembership();
		}
	}

	// Token: 0x040000BB RID: 187
	private List<MB3_KMeansClustering.DataPoint> _normalizedDataToCluster = new List<MB3_KMeansClustering.DataPoint>();

	// Token: 0x040000BC RID: 188
	private Vector3[] _clusters = new Vector3[0];

	// Token: 0x040000BD RID: 189
	private int _numberOfClusters;

	// Token: 0x02000032 RID: 50
	private class DataPoint
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00008870 File Offset: 0x00006C70
		public DataPoint(GameObject go)
		{
			this.gameObject = go;
			this.center = go.transform.position;
			if (go.GetComponent<Renderer>() == null)
			{
				Debug.LogError("Object does not have a renderer " + go);
			}
		}

		// Token: 0x040000BE RID: 190
		public Vector3 center;

		// Token: 0x040000BF RID: 191
		public GameObject gameObject;

		// Token: 0x040000C0 RID: 192
		public int Cluster;
	}
}
