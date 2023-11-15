using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009A RID: 154
public class UI_Icon_Star : UI_Text_SimpleTooltip
{
	// Token: 0x06000561 RID: 1377 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
	public void UpdateIcon()
	{
		this.starNum.text = GameData.inst.Star.ToString();
		this.startImg.sprite = ResourceLibrary.Inst.Sprite_Star;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x04000461 RID: 1121
	public Text starNum;

	// Token: 0x04000462 RID: 1122
	public Image startImg;

	// Token: 0x04000463 RID: 1123
	[SerializeField]
	private RectTransform rect;
}
