using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008B RID: 139
public class UI_Icon_FactorBattle : UI_Icon
{
	// Token: 0x060004E8 RID: 1256 RVA: 0x0001D367 File Offset: 0x0001B567
	public void Init(int i)
	{
		this.fbID = i;
		this.UpdateText();
		base.TextSetNormal();
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001D37C File Offset: 0x0001B57C
	public void UpdateText()
	{
		FactorBattle factorBattleTotal = Battle.inst.factorBattleTotal;
		LanguageText inst = LanguageText.Inst;
		this.textName.text = inst.battleFactor[this.fbID];
		this.textNum.text = FactorBattle.GetString_NumberFormated_Total(this.fbID);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithFactorBattle(this.fbID);
		base.TextSetHighlight();
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000418 RID: 1048
	public int fbID;

	// Token: 0x04000419 RID: 1049
	public Text textName;

	// Token: 0x0400041A RID: 1050
	public Text textNum;
}
