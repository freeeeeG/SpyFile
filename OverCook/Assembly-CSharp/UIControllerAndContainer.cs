using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class UIControllerAndContainer : UISubElementContainer, IUIController
{
	// Token: 0x060010BB RID: 4283 RVA: 0x00060393 File Offset: 0x0005E793
	public Vector2 GetOffset()
	{
		return this.m_offset;
	}

	// Token: 0x04000CE5 RID: 3301
	[SerializeField]
	private Vector2 m_offset;
}
