using System;
using System.Data;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B4 RID: 180
public class UI_Panel_RankList_Infinity : UI_Panel_Main_IconList
{
	// Token: 0x0600064C RID: 1612 RVA: 0x0002435B File Offset: 0x0002255B
	private void Awake()
	{
		UI_Panel_RankList_Infinity.inst = this;
		this.transMyRank.gameObject.SetActive(false);
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00024374 File Offset: 0x00022574
	public void InitWithDataSet(DataSet dataSet, int modeType, int daily_Index)
	{
		this.modeType = modeType;
		this.dataSet = dataSet;
		this.daily_Index = daily_Index;
		this.dataTable = dataSet.Tables[0];
		this.UpdatePage();
		try
		{
			this.UpdateMyRank();
		}
		catch
		{
			UI_Panel_RankList.inst.SetFlag(4);
			throw new Exception("无法获取SteamID");
		}
		this.InitIcons(null);
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x000243E4 File Offset: 0x000225E4
	protected override int IconNum()
	{
		int b = this.dataTable.Rows.Count - (this.page - 1) * 10;
		return Mathf.Min(this.maxNum, b);
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0002441A File Offset: 0x0002261A
	private int PageMax()
	{
		return Mathf.Max(1, Mathf.CeilToInt((float)this.dataTable.Rows.Count / 10f));
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002443E File Offset: 0x0002263E
	public void GoFirstPage()
	{
		this.page = 1;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00024454 File Offset: 0x00022654
	public void GoPrevPage(int num)
	{
		this.page -= num;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00024471 File Offset: 0x00022671
	public void GoNextPage(int num)
	{
		this.page += num;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0002448E File Offset: 0x0002268E
	public void GoLastPage()
	{
		this.page = this.PageMax();
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000244A9 File Offset: 0x000226A9
	private void UpdatePage()
	{
		this.page = Mathf.Clamp(this.page, 1, this.PageMax());
		this.textPageNum.text = this.page.ToString();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transButtons);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool IfAvailable(int ID)
	{
		return true;
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000244E4 File Offset: 0x000226E4
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		ID += (this.page - 1) * 10;
		DataRow dataRow = this.dataTable.Rows[ID];
		obj.GetComponent<UI_SingleRow_RankList_Infinity>().InitWithDatarow(ID, dataRow);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00024520 File Offset: 0x00022720
	private void UpdateMyRank()
	{
		this.myRank = 0;
		LanguageText.PanelRankList rankListPanel = LanguageText.Inst.rankListPanel;
		string b = UI_Panel_RankList.inst.GetUniqueCode();
		if (this.modeType == 3)
		{
			b = SteamApps.GetAppOwner().m_SteamID.ToString() + this.daily_Index.ToString();
		}
		int count = this.dataTable.Rows.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			if (this.dataTable.Rows[i].GetString(2) == b)
			{
				this.myRank = i + 1;
				this.textLangMyRankNum.text = this.myRank.ToString();
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.textLangMyRankNum.text = rankListPanel.myRank_Empty;
		}
		this.textLangMyRankString.text = rankListPanel.myRank_Text;
		this.transMyRank.gameObject.SetActive(true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transMyRank);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002461F File Offset: 0x0002281F
	public void GoFindMyRank()
	{
		if (this.myRank == 0)
		{
			return;
		}
		this.page = (this.myRank - 1) / 10 + 1;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x04000542 RID: 1346
	public static UI_Panel_RankList_Infinity inst;

	// Token: 0x04000543 RID: 1347
	private int modeType = 1;

	// Token: 0x04000544 RID: 1348
	private int daily_Index = 1;

	// Token: 0x04000545 RID: 1349
	private DataSet dataSet;

	// Token: 0x04000546 RID: 1350
	private DataTable dataTable;

	// Token: 0x04000547 RID: 1351
	[SerializeField]
	private int maxNum = 10;

	// Token: 0x04000548 RID: 1352
	[SerializeField]
	public int page = 1;

	// Token: 0x04000549 RID: 1353
	[SerializeField]
	private Text textPageNum;

	// Token: 0x0400054A RID: 1354
	[SerializeField]
	private Text textLangMyRankString;

	// Token: 0x0400054B RID: 1355
	[SerializeField]
	private Text textLangMyRankNum;

	// Token: 0x0400054C RID: 1356
	[SerializeField]
	private int myRank;

	// Token: 0x0400054D RID: 1357
	[SerializeField]
	private RectTransform transMyRank;

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private RectTransform transButtons;
}
