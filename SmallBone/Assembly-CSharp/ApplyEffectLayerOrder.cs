using System;
using FX;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class ApplyEffectLayerOrder : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002D05 File Offset: 0x00000F05
	private void Start()
	{
		this._spriteRenderer.sortingOrder = (int)Effects.GetSortingOrderAndIncrease();
	}

	// Token: 0x0400001E RID: 30
	[SerializeField]
	private SpriteRenderer _spriteRenderer;
}
