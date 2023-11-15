using System;
using TMPro;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class BuyGroundTips : TileTips
{
	// Token: 0x06000EAD RID: 3757 RVA: 0x00025EED File Offset: 0x000240ED
	public void ReadInfo(int cost)
	{
		this.isShowing = true;
		this.costTxt.text = GameMultiLang.GetTraduction("BUY") + "<sprite=7>" + cost.ToString();
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00025F1C File Offset: 0x0002411C
	public override void Update()
	{
		if (this.isShowing && Singleton<InputManager>.Instance.GetKeyDown(KeyBindingActions.BuyGround))
		{
			this.BuyOneGroundTile();
		}
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00025F39 File Offset: 0x00024139
	public void BuyOneGroundTile()
	{
		Singleton<GameManager>.Instance.BuyOneGround();
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00025F45 File Offset: 0x00024145
	public override void ClosePanel()
	{
		base.ClosePanel();
		this.isShowing = false;
	}

	// Token: 0x04000711 RID: 1809
	[SerializeField]
	private TextMeshProUGUI costTxt;

	// Token: 0x04000712 RID: 1810
	private bool isShowing;
}
