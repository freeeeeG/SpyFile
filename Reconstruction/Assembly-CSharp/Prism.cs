using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class Prism : BuildingContent
{
	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00020AC9 File Offset: 0x0001ECC9
	// (set) Token: 0x06000C8C RID: 3212 RVA: 0x00020AD0 File Offset: 0x0001ECD0
	private float RotSpeed
	{
		get
		{
			return 180f;
		}
		set
		{
			this.rotSpeed = value;
		}
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00020AD9 File Offset: 0x0001ECD9
	public override bool GameUpdate()
	{
		base.GameUpdate();
		this.SelfRotateControl();
		return true;
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x00020AE9 File Offset: 0x0001ECE9
	private void SelfRotateControl()
	{
		this.rotTrans.Rotate(Vector3.forward * -this.RotSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x04000633 RID: 1587
	private float rotSpeed;
}
