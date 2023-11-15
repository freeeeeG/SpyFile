using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B23 RID: 2851
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
	// Token: 0x060057BB RID: 22459 RVA: 0x002013E8 File Offset: 0x001FF5E8
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
		float num = sprite.rect.width / sprite.rect.height;
		this.layout.preferredWidth = this.layout.preferredHeight * num;
	}

	// Token: 0x04003B4F RID: 15183
	[SerializeField]
	private Image image;

	// Token: 0x04003B50 RID: 15184
	[SerializeField]
	private LayoutElement layout;
}
