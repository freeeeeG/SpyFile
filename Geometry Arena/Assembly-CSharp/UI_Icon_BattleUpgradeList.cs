using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000087 RID: 135
public class UI_Icon_BattleUpgradeList : UI_Icon
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x0001CDAA File Offset: 0x0001AFAA
	public void Init(int i)
	{
		this.upID = i;
		this.UpdateText();
		base.TextSetNormal();
		this.icon_Upgrade.Init(i);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001CDCC File Offset: 0x0001AFCC
	public void UpdateText()
	{
		Upgrade upgrade = DataBase.Inst.Data_Upgrades[this.upID];
		int num = Battle.inst.ForShow_UpgradeNum[this.upID];
		LanguageText inst = LanguageText.Inst;
		string text = upgrade.Language_Name.Colored(UI_Setting.Inst.rankColors[(int)upgrade.rank]);
		if (num > 1)
		{
			text = text + " x " + num;
		}
		this.textName.text = text;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001CE48 File Offset: 0x0001B048
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (MyInput.KeyShiftHold())
		{
			Battle.inst.RemoveUpgrade(this.upID);
			UI_ToolTip.inst.Close();
			BattleMapCanvas.inst.panel_UpgradeShow.InitIcons(null);
			base.gameObject.SetActive(false);
			return;
		}
		BattleMapCanvas.inst.panel_ConfirmDeleteUpgrade.InitConfirm(this.upID);
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithUpgrade(this.upID);
		base.TextSetHighlight();
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000409 RID: 1033
	public int upID;

	// Token: 0x0400040A RID: 1034
	public Text textName;

	// Token: 0x0400040B RID: 1035
	public UI_Icon_Upgrade icon_Upgrade;
}
