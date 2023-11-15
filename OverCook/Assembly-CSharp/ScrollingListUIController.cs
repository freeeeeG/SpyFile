using System;
using UnityEngine;

// Token: 0x02000B4C RID: 2892
public abstract class ScrollingListUIController : ScrollingListUIContainer, IUIController
{
	// Token: 0x06003AC9 RID: 15049 RVA: 0x001142B4 File Offset: 0x001126B4
	public Vector2 GetOffset()
	{
		return this.m_offset;
	}

	// Token: 0x04002FAF RID: 12207
	[SerializeField]
	private Vector2 m_offset;
}
