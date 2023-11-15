using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036E RID: 878
[RequireComponent(typeof(Text))]
public class EmbeddedTextImageLookup : EmbeddedTextImageLookupBase
{
	// Token: 0x060010C6 RID: 4294 RVA: 0x00060CBA File Offset: 0x0005F0BA
	protected override Sprite GetIcon(int _materialNum)
	{
		return this.m_sprites.TryAtIndex(_materialNum);
	}

	// Token: 0x04000CF0 RID: 3312
	[SerializeField]
	private Sprite[] m_sprites = new Sprite[0];
}
