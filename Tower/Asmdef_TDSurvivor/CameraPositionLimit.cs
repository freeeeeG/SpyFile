using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class CameraPositionLimit : MonoBehaviour
{
	// Token: 0x060001D6 RID: 470 RVA: 0x000083A4 File Offset: 0x000065A4
	private void Start()
	{
		this.startPos = base.transform.position;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x000083B8 File Offset: 0x000065B8
	private void LateUpdate()
	{
		Vector3 vector = base.transform.position - this.startPos;
		if (vector.magnitude > this.rangeLimit)
		{
			base.transform.position = this.startPos + vector.normalized * this.rangeLimit;
		}
	}

	// Token: 0x0400016B RID: 363
	[SerializeField]
	private float rangeLimit;

	// Token: 0x0400016C RID: 364
	private Vector3 startPos;

	// Token: 0x0400016D RID: 365
	private Vector3 lastFramePosition;
}
