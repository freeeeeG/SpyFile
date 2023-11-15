using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
[SelectionBase]
public class Obj_TrainRail : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000139B6 File Offset: 0x00011BB6
	public Vector3 StartPoint
	{
		get
		{
			return base.transform.TransformPoint(this.startPoint);
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000139C9 File Offset: 0x00011BC9
	public Vector3 MidPoint
	{
		get
		{
			return base.transform.TransformPoint(this.midPoint);
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000139DC File Offset: 0x00011BDC
	public Vector3 EndPoint
	{
		get
		{
			return base.transform.TransformPoint(this.endPoint);
		}
	}

	// Token: 0x040004B3 RID: 1203
	[SerializeField]
	private Vector3 startPoint;

	// Token: 0x040004B4 RID: 1204
	[SerializeField]
	private Vector3 midPoint;

	// Token: 0x040004B5 RID: 1205
	[SerializeField]
	private Vector3 endPoint;
}
