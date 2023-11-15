using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008C RID: 140
public class UI_Icon_GeometryCoin : UI_Text_SimpleTooltip
{
	// Token: 0x060004EF RID: 1263 RVA: 0x0001D3E0 File Offset: 0x0001B5E0
	public void UpdateIcon()
	{
		this.gCoinNum.text = GameData.inst.GeometryCoin.ToString();
		this.gCoinImg.sprite = ResourceLibrary.Inst.sprite_GeometryCoin;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.GetComponent<RectTransform>());
	}

	// Token: 0x0400041B RID: 1051
	public Text gCoinNum;

	// Token: 0x0400041C RID: 1052
	public Image gCoinImg;
}
