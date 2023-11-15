using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class PathManager : MonoBehaviour
{
	// Token: 0x06000BA5 RID: 2981 RVA: 0x0001E60A File Offset: 0x0001C80A
	private void Start()
	{
		this.GenerateVectorPath();
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0001E614 File Offset: 0x0001C814
	private void GenerateVectorPath()
	{
		this.path = new List<Vector2>();
		for (int i = 0; i < this.wayPoints.Length - 1; i++)
		{
			Vector2 item = this.wayPoints[i].position + 0.5f * (this.wayPoints[i + 1].position - this.wayPoints[i].position);
			this.path.Add(item);
		}
		for (int j = 0; j < this.wayPoints.Length - 1; j++)
		{
			PahtDot pahtDot = Object.Instantiate<PahtDot>(this.pathPoint, this.wayPoints[j].position, Quaternion.identity);
			pahtDot.m_Path = this.path;
			pahtDot.index = j;
		}
	}

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private Transform[] wayPoints;

	// Token: 0x040005D1 RID: 1489
	[SerializeField]
	private PahtDot pathPoint;

	// Token: 0x040005D2 RID: 1490
	private List<Vector2> path;
}
