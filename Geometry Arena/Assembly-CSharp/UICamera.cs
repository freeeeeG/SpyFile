using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class UICamera : MonoBehaviour
{
	// Token: 0x06000482 RID: 1154 RVA: 0x0001C31E File Offset: 0x0001A51E
	private void Awake()
	{
		UICamera.inst = this;
		if (this.uiCamera == null)
		{
			this.uiCamera = base.GetComponent<Camera>();
		}
	}

	// Token: 0x040003DD RID: 989
	public static UICamera inst;

	// Token: 0x040003DE RID: 990
	public Camera uiCamera;
}
