using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B39 RID: 2873
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	// Token: 0x060058BD RID: 22717 RVA: 0x00208295 File Offset: 0x00206495
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.sprite.sprite = info.Sprite;
	}

	// Token: 0x04003C10 RID: 15376
	[SerializeField]
	private Image sprite;
}
