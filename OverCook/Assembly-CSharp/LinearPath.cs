using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public class LinearPath : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06002592 RID: 9618 RVA: 0x000B1BEB File Offset: 0x000AFFEB
	public List<Vector3> Points
	{
		get
		{
			return this.m_points;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06002593 RID: 9619 RVA: 0x000B1BF3 File Offset: 0x000AFFF3
	public float[] Distances
	{
		get
		{
			return this.m_distances;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06002594 RID: 9620 RVA: 0x000B1BFB File Offset: 0x000AFFFB
	public float TotalDistance
	{
		get
		{
			return this.m_totalDistance;
		}
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x000B1C04 File Offset: 0x000B0004
	public Vector3 Evaluate(float t)
	{
		if (this.m_points.Count == 1)
		{
			return this.m_points[0];
		}
		float[] distances = this.m_distances;
		float num = t * this.m_totalDistance;
		float num2 = 0f;
		int num3 = -1;
		for (int i = 0; i < distances.Length; i++)
		{
			float num4 = distances[i];
			if (num2 + num4 > num)
			{
				num3 = i;
				break;
			}
			num2 += num4;
		}
		if (num3 == -1 || num3 + 1 > this.m_points.Count - 1)
		{
			return this.m_points[this.m_points.Count - 1];
		}
		num3 = Mathf.Clamp(num3, 0, this.m_points.Count - 1);
		float num5 = num - num2;
		float t2 = num5 / distances[num3];
		return Vector3.Lerp(this.m_points[num3], this.m_points[num3 + 1], t2);
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x000B1CF4 File Offset: 0x000B00F4
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x000B1CF8 File Offset: 0x000B00F8
	public void OnBeforeSerialize()
	{
		this.m_distances = new float[this.m_points.Count];
		float num = 0f;
		for (int i = 1; i < this.m_points.Count; i++)
		{
			float num2 = Vector3.Distance(this.m_points[i - 1], this.m_points[i]);
			num += num2;
			this.m_distances[i - 1] = num2;
		}
		this.m_totalDistance = num;
	}

	// Token: 0x04001D25 RID: 7461
	[SerializeField]
	private List<Vector3> m_points = new List<Vector3>();

	// Token: 0x04001D26 RID: 7462
	[SerializeField]
	private float m_totalDistance;

	// Token: 0x04001D27 RID: 7463
	[SerializeField]
	private float[] m_distances = new float[0];
}
