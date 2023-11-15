using System;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class GuideIndicator : MonoBehaviour
{
	// Token: 0x06000F3D RID: 3901 RVA: 0x0002863E File Offset: 0x0002683E
	public void Show(bool value, GameObject FollowObj = null)
	{
		this.isShow = value;
		if (FollowObj != null)
		{
			this.followTr = FollowObj.transform;
		}
		base.gameObject.SetActive(this.isShow);
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0002866D File Offset: 0x0002686D
	private void FixedUpdate()
	{
		if (this.isShow)
		{
			base.transform.position = this.followTr.position;
		}
	}

	// Token: 0x0400079D RID: 1949
	private Transform followTr;

	// Token: 0x0400079E RID: 1950
	private bool isShow;
}
