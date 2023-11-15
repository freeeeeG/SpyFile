using System;
using UnityEngine;

// Token: 0x02000BC5 RID: 3013
public class PopIn : MonoBehaviour
{
	// Token: 0x06005E8E RID: 24206 RVA: 0x0022B694 File Offset: 0x00229894
	private void OnEnable()
	{
		this.StartPopIn(true);
	}

	// Token: 0x06005E8F RID: 24207 RVA: 0x0022B6A0 File Offset: 0x002298A0
	private void Update()
	{
		float num = Mathf.Lerp(base.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x06005E90 RID: 24208 RVA: 0x0022B6EC File Offset: 0x002298EC
	public void StartPopIn(bool force_reset = false)
	{
		if (force_reset)
		{
			base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		this.targetScale = 1f;
	}

	// Token: 0x06005E91 RID: 24209 RVA: 0x0022B71B File Offset: 0x0022991B
	public void StartPopOut()
	{
		this.targetScale = 0f;
	}

	// Token: 0x04003FD1 RID: 16337
	private float targetScale;

	// Token: 0x04003FD2 RID: 16338
	public float speed;
}
