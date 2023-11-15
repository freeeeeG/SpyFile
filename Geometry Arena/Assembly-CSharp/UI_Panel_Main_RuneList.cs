using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CE RID: 206
public class UI_Panel_Main_RuneList : UI_Panel_Main_IconList
{
	// Token: 0x06000703 RID: 1795 RVA: 0x000051D0 File Offset: 0x000033D0
	private void OnEnable()
	{
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x000271D5 File Offset: 0x000253D5
	protected override int IconNum()
	{
		return GameData.inst.runes.Count;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x000271E6 File Offset: 0x000253E6
	protected override bool IfAvailable(int ID)
	{
		return ID >= this.page * this.maxRuneInPage && ID < (this.page + 1) * this.maxRuneInPage;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00027210 File Offset: 0x00025410
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		int index = UI_Panel_Main_RunePanel.inst.listRuneSort[ID];
		obj.GetComponent<UI_Icon_Rune>().UpdateIcon(index, UI_Icon_Rune.EnumIconRuneType.LIST);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002723B File Offset: 0x0002543B
	public void NextPage()
	{
		this.page++;
		this.PageFix();
		this.InitIcons(null);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00027258 File Offset: 0x00025458
	public void PreviousPage()
	{
		this.page--;
		this.PageFix();
		this.InitIcons(null);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00027278 File Offset: 0x00025478
	public void PageFix()
	{
		int b = (GameData.inst.runes.Count - 1) / this.maxRuneInPage;
		int a = 0;
		this.page = math.clamp(this.page, a, b);
		this.textPage.text = (this.page + 1).ToString();
	}

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private int page;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	private Text textPage;

	// Token: 0x040005D6 RID: 1494
	[SerializeField]
	private int maxRuneInPage = 50;
}
