using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class RandomSpriteOnEnable : MonoBehaviour
{
	// Token: 0x0600021D RID: 541 RVA: 0x00009296 File Offset: 0x00007496
	private void OnEnable()
	{
		this._spriteRenderer.sprite = this._sprites.Random<Sprite>();
	}

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private Sprite[] _sprites;
}
