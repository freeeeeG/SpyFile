using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class RangeIndicator : ReusableObject
{
	// Token: 0x06000CF3 RID: 3315 RVA: 0x0002173E File Offset: 0x0001F93E
	public void ShowSprite(bool show)
	{
		this.sprite.enabled = show;
	}

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private SpriteRenderer sprite;
}
