using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C7 RID: 199
public class UI_Panel_Main_LibraryUpgrade : UI_Panel_Main_IconList
{
	// Token: 0x060006D8 RID: 1752 RVA: 0x00026715 File Offset: 0x00024915
	public override void InitIcons(Transform transformParent = null)
	{
		base.InitIcons(this.transParent);
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00026723 File Offset: 0x00024923
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.InitIcons(null);
		this.UpdateLanguage();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transParent);
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002674C File Offset: 0x0002494C
	private void UpdateLanguage()
	{
		LanguageText.UpgradeLibrary upgradeLibrary = LanguageText.Inst.upgradeLibrary;
		this.textMultiTip.text = upgradeLibrary.multiTip;
		this.textManualTip.text = upgradeLibrary.manualTip.ReplaceLineBreak();
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002678B File Offset: 0x0002498B
	protected override int IconNum()
	{
		return DataBase.Inst.Data_UpgradeTypes.Length;
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00026799 File Offset: 0x00024999
	protected override bool IfAvailable(int ID)
	{
		return DataBase.Inst.Data_UpgradeTypes[ID].ifAvailableInRune;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x000267AC File Offset: 0x000249AC
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Panel_UpgradeLibrary_SingleType_Main>().InitWithType(ID);
		obj.GetComponent<RectTransform>().localScale = new Vector2(this.iconScale, this.iconScale);
	}

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	private Text textMultiTip;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private Text textManualTip;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	private RectTransform transParent;
}
