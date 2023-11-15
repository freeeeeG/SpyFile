using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C6 RID: 198
public class UI_Panel_Main_LibraryMain : MonoBehaviour
{
	// Token: 0x060006D1 RID: 1745 RVA: 0x000264D8 File Offset: 0x000246D8
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.InitChildPanel();
		MainCanvas.ChildPanelOpen();
		this.UpdateLanguage();
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x000264F7 File Offset: 0x000246F7
	public void Close()
	{
		MainCanvas.ChildPanelClose();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x0002650C File Offset: 0x0002470C
	private void InitChildPanel()
	{
		switch (this.libraryType)
		{
		case UI_Panel_Main_LibraryMain.EnumLibraryType.UPGRADE:
			this.libUpgrade.Open();
			this.libEnemy.Close();
			this.libBattleItem.Close();
			return;
		case UI_Panel_Main_LibraryMain.EnumLibraryType.ENEMY:
			this.libUpgrade.Close();
			this.libEnemy.Open();
			this.libBattleItem.Close();
			return;
		case UI_Panel_Main_LibraryMain.EnumLibraryType.BATTLEITEM:
			this.libUpgrade.Close();
			this.libEnemy.Close();
			this.libBattleItem.Open();
			return;
		default:
			return;
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00026598 File Offset: 0x00024798
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.UpgradeLibrary upgradeLibrary = inst.upgradeLibrary;
		LanguageText.EnemyLibrary enemyLibrary = inst.enemyLibrary;
		LanguageText.BattleItemLibrary battleItemLibrary = inst.battleItemLibrary;
		this.textTitle_LibUpgrade.text = upgradeLibrary.title;
		this.textTitle_LibEnemy.text = enemyLibrary.title;
		this.textTitle_LibBattleitem.text = battleItemLibrary.title;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTitles);
		Color button_Highlight = UI_Setting.Inst.button_Highlight;
		switch (this.libraryType)
		{
		case UI_Panel_Main_LibraryMain.EnumLibraryType.UPGRADE:
			this.textTitle_LibUpgrade.color = button_Highlight;
			this.textTitle_LibEnemy.color = Color.white;
			this.textTitle_LibBattleitem.color = Color.white;
			return;
		case UI_Panel_Main_LibraryMain.EnumLibraryType.ENEMY:
			this.textTitle_LibUpgrade.color = Color.white;
			this.textTitle_LibEnemy.color = button_Highlight;
			this.textTitle_LibBattleitem.color = Color.white;
			return;
		case UI_Panel_Main_LibraryMain.EnumLibraryType.BATTLEITEM:
			this.textTitle_LibUpgrade.color = Color.white;
			this.textTitle_LibEnemy.color = Color.white;
			this.textTitle_LibBattleitem.color = button_Highlight;
			return;
		default:
			return;
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x000266A9 File Offset: 0x000248A9
	public void SwitchLibraryType(int i)
	{
		if (i < 0 || i > 2)
		{
			Debug.LogError("Error_LibraryTypeError!");
			return;
		}
		this.libraryType = (UI_Panel_Main_LibraryMain.EnumLibraryType)i;
		this.InitChildPanel();
		this.UpdateLanguage();
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x000266D4 File Offset: 0x000248D4
	private void Update()
	{
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
			UI_ToolTip.inst.Close();
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.Tab))
		{
			int num = (int)(this.libraryType + 1);
			num %= 3;
			this.SwitchLibraryType(num);
		}
	}

	// Token: 0x0400059D RID: 1437
	[SerializeField]
	private UI_Panel_Main_LibraryUpgrade libUpgrade;

	// Token: 0x0400059E RID: 1438
	[SerializeField]
	private UI_Panel_Main_LibraryEnemy libEnemy;

	// Token: 0x0400059F RID: 1439
	[SerializeField]
	private UI_Panel_Main_LibraryBattleItem libBattleItem;

	// Token: 0x040005A0 RID: 1440
	[SerializeField]
	private UI_Panel_Main_LibraryMain.EnumLibraryType libraryType;

	// Token: 0x040005A1 RID: 1441
	[SerializeField]
	private Text textTitle_LibUpgrade;

	// Token: 0x040005A2 RID: 1442
	[SerializeField]
	private Text textTitle_LibEnemy;

	// Token: 0x040005A3 RID: 1443
	[SerializeField]
	private Text textTitle_LibBattleitem;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private RectTransform rectTitles;

	// Token: 0x0200015A RID: 346
	private enum EnumLibraryType
	{
		// Token: 0x040009F1 RID: 2545
		UPGRADE,
		// Token: 0x040009F2 RID: 2546
		ENEMY,
		// Token: 0x040009F3 RID: 2547
		BATTLEITEM
	}
}
