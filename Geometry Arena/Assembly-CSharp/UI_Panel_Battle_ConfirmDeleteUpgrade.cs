using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A9 RID: 169
public class UI_Panel_Battle_ConfirmDeleteUpgrade : MonoBehaviour
{
	// Token: 0x060005DC RID: 1500 RVA: 0x00021A7D File Offset: 0x0001FC7D
	public void InitConfirm(int id)
	{
		base.gameObject.SetActive(true);
		this.upID = id;
		this.UpdateLanguage();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00021A98 File Offset: 0x0001FC98
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		this.lang_Title.text = inst.confirm_Title;
		this.lang_Info.text = inst.pauseMenu.info_DeleteUpgrade + DataBase.Inst.Data_Upgrades[this.upID].Language_Name;
		this.lang_Yes.text = inst.confirm_Yes;
		this.lang_No.text = inst.confirm_No;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00021B0F File Offset: 0x0001FD0F
	public void Button_ConfirmDelete()
	{
		Debug.Log("确认删除");
		Battle.inst.RemoveUpgrade(this.upID);
		BattleMapCanvas.inst.panel_UpgradeShow.InitIcons(null);
		base.gameObject.SetActive(false);
	}

	// Token: 0x040004D5 RID: 1237
	[SerializeField]
	private Text lang_Title;

	// Token: 0x040004D6 RID: 1238
	[SerializeField]
	private Text lang_Info;

	// Token: 0x040004D7 RID: 1239
	[SerializeField]
	private Text lang_Yes;

	// Token: 0x040004D8 RID: 1240
	[SerializeField]
	private Text lang_No;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	private int upID;
}
