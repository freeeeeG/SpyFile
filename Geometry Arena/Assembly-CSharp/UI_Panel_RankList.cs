using System;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B3 RID: 179
public class UI_Panel_RankList : MonoBehaviour
{
	// Token: 0x06000635 RID: 1589 RVA: 0x00023A29 File Offset: 0x00021C29
	private void Awake()
	{
		UI_Panel_RankList.inst = this;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00023A34 File Offset: 0x00021C34
	public string GetUniqueCode()
	{
		return SteamApps.GetAppOwner().m_SteamID.ToString() + this.jobType;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00023A64 File Offset: 0x00021C64
	public void Open()
	{
		if (this.modeType == 3)
		{
			this.daily_Index = this.Daily_IndexMax();
			this.Daily_UpdateIndex();
			this.objDate.SetActive(true);
			this.objJobs.SetActive(false);
		}
		else
		{
			this.objDate.SetActive(false);
			this.objJobs.SetActive(true);
		}
		this.panelInfinity.ClearAll();
		base.gameObject.SetActive(true);
		MainCanvas.ChildPanelOpen();
		this.UpdateLanguage();
		this.UpdateList();
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00023AE6 File Offset: 0x00021CE6
	public void Refresh()
	{
		if (this.deltaTimeFromLastRefresh < 0.6f)
		{
			Debug.LogWarning("刷新间隔少于0.6秒，不受理");
			return;
		}
		this.deltaTimeFromLastRefresh = 0f;
		this.Open();
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00023B14 File Offset: 0x00021D14
	public void ShowTips_Loading()
	{
		LanguageText.PanelRankList rankListPanel = LanguageText.Inst.rankListPanel;
		this.tips.gameObject.SetActive(true);
		this.tips.text = rankListPanel.tips_Loading;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00023B50 File Offset: 0x00021D50
	public void ShowTips_ComingSoon()
	{
		LanguageText.PanelRankList rankListPanel = LanguageText.Inst.rankListPanel;
		this.tips.gameObject.SetActive(true);
		this.tips.text = rankListPanel.tips_ComeSoon;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00023B8A File Offset: 0x00021D8A
	public void CloseTips()
	{
		this.tips.gameObject.SetActive(false);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00023B9D File Offset: 0x00021D9D
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.ChildPanelClose();
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00023BB0 File Offset: 0x00021DB0
	private void Update()
	{
		this.deltaTimeFromLastRefresh += Time.unscaledDeltaTime;
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
		if (this.waitingThread)
		{
			this.UpdateList();
			return;
		}
		switch (this.modeType)
		{
		case 1:
			if (this.getDataSet)
			{
				Debug.Log("成功获取无尽模式排行榜");
				this.getDataSet = false;
				this.panelInfinity.InitWithDataSet(this.dataSetCommon, this.modeType, this.daily_Index);
				this.CloseTips();
			}
			break;
		case 2:
			if (this.getDataSet)
			{
				Debug.Log("成功获取漫游模式排行榜");
				this.getDataSet = false;
				this.panelInfinity.InitWithDataSet(this.dataSetCommon, this.modeType, this.daily_Index);
				this.CloseTips();
			}
			break;
		case 3:
			if (this.getDataSet)
			{
				Debug.Log("成功获取每日挑战排行榜");
				this.getDataSet = false;
				this.panelInfinity.InitWithDataSet(this.dataSetCommon, this.modeType, this.daily_Index);
				this.CloseTips();
				this.daily_DOFlags = new bool[TempData.inst.diffiOptFlag.Length];
				if (this.dataSetCommon.Tables[0].Rows.Count >= 1)
				{
					string @string = this.dataSetCommon.Tables[0].Rows[0].GetString(22);
					this.daily_DOFlags = MyTool.StringDOToBools(@string, TempData.inst.diffiOptFlag.Length);
				}
			}
			break;
		}
		if (this.ifFlagActive)
		{
			this.ifFlagActive = false;
			LanguageText.PanelRankList rankListPanel = LanguageText.Inst.rankListPanel;
			switch (this.flagIndex)
			{
			case 1:
				this.tips.text = rankListPanel.tips_Flag1NetWorkClose;
				break;
			case 2:
				this.tips.text = rankListPanel.tips_Flag2ConnectError;
				break;
			case 3:
				this.tips.text = rankListPanel.tips_Flag3DataError;
				break;
			case 4:
				this.tips.text = rankListPanel.tips_Flag4SteamError;
				break;
			default:
				Debug.LogError("Error_排行榜_ErrorFlagIndex有误");
				break;
			}
			this.flagIndex = 0;
		}
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00023DE0 File Offset: 0x00021FE0
	private void UpdateLanguage()
	{
		LanguageText.PanelRankList rankListPanel = LanguageText.Inst.rankListPanel;
		DataBase dataBase = DataBase.Inst;
		for (int i = 0; i < this.titleTexts.Length; i++)
		{
			this.titleTexts[i].text = rankListPanel.titles[i];
			if (i == this.modeType)
			{
				this.titleTexts[i].color = Color.yellow;
			}
			else
			{
				this.titleTexts[i].color = Color.white;
			}
		}
		for (int j = 0; j < this.jobTexts.Length; j++)
		{
			this.jobTexts[j].text = dataBase.DataPlayerModels[j].Language_JobName;
			if (j == this.jobType)
			{
				this.jobTexts[j].color = Color.yellow;
			}
			else
			{
				this.jobTexts[j].color = Color.white;
			}
			this.jobChoose_HoriLayout.spacing = this.jobChoose_Spacing[(int)Setting.Inst.language];
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.objJobs.GetComponent<RectTransform>());
		}
		for (int k = 0; k < this.titleRow_Infinity.Length; k++)
		{
			this.titleRow_Infinity[k].text = rankListPanel.titleRowTexts_Infinity[k].ReplaceLineBreak();
		}
		this.textRefresh.text = rankListPanel.refresh;
		this.textReturn.text = rankListPanel.close;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00023F38 File Offset: 0x00022138
	private void UpdateList()
	{
		if (this.onThread)
		{
			this.waitingThread = true;
			return;
		}
		this.waitingThread = false;
		switch (this.modeType)
		{
		case 0:
			this.ShowTips_ComingSoon();
			return;
		case 1:
			this.getDataSet = false;
			this.ShowTips_Loading();
			this.thread = new Thread(new ThreadStart(this.GetDataSet_CommonMode));
			Debug.Log("线程开始_更新无尽模式排行榜");
			this.thread.Start();
			return;
		case 2:
			this.getDataSet = false;
			this.ShowTips_Loading();
			this.thread = new Thread(new ThreadStart(this.GetDataSet_CommonMode));
			Debug.Log("线程开始_更新漫游模式排行榜");
			this.thread.Start();
			return;
		case 3:
			this.getDataSet = false;
			this.ShowTips_Loading();
			this.thread = new Thread(new ThreadStart(this.GetDataSet_CommonMode));
			Debug.Log("线程开始_更新每日挑战排行榜");
			this.thread.Start();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0002402E File Offset: 0x0002222E
	public void SelectMode(int i)
	{
		this.modeType = i;
		this.Open();
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0002403D File Offset: 0x0002223D
	public void SelectJob(int i)
	{
		this.jobType = i;
		if (UI_Panel_RankList_Infinity.inst != null)
		{
			UI_Panel_RankList_Infinity.inst.page = 1;
		}
		this.Open();
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00024064 File Offset: 0x00022264
	public void GetDataSet_CommonMode()
	{
		this.onThread = true;
		this.ifFlagActive = false;
		this.flagIndex = 0;
		SqlManager sqlManager = SqlManager.inst;
		try
		{
			sqlManager.OpenSqlNow();
		}
		catch (Exception arg)
		{
			Debug.LogError("数据库连接异常" + arg);
			this.SetFlag(2);
			return;
		}
		MySqlConnection mySqlConnection = sqlManager.mysql.mySqlConnection;
		string selectCommandText = "";
		switch (this.modeType)
		{
		case 1:
			selectCommandText = string.Format("SELECT * FROM RankList_Infinity WHERE JobID = {0} ORDER BY Stars DESC;", this.jobType);
			break;
		case 2:
			selectCommandText = string.Format("SELECT * FROM RankList_Wander WHERE JobID = {0} ORDER BY Stars DESC;", this.jobType);
			break;
		case 3:
			selectCommandText = string.Format("SELECT * FROM RankList_Daily WHERE DayIndex = {0} ORDER BY Stars DESC;", this.daily_Index);
			break;
		default:
			Debug.LogError("Error_排行榜_模式index有误");
			break;
		}
		if (!Setting.Inst.Option_Network)
		{
			Debug.LogError("Error_网络功能已关闭！");
			this.SetFlag(1);
			return;
		}
		if (SqlManager.inst.mysql.mySqlConnection.State != ConnectionState.Open)
		{
			Debug.LogError("Error_无法连接数据库！");
			this.SetFlag(2);
			return;
		}
		DataSet dataSet = new DataSet();
		try
		{
			new MySqlDataAdapter(selectCommandText, mySqlConnection).Fill(dataSet);
			this.dataSetCommon = dataSet;
			this.getDataSet = true;
		}
		catch (Exception ex)
		{
			this.SetFlag(3);
			SqlManager.inst.mysql = null;
			throw new Exception("Error_数据获取异常:/n" + ex.ToString());
		}
		this.onThread = false;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x000241F0 File Offset: 0x000223F0
	public void SetFlag(int index)
	{
		this.flagIndex = index;
		this.ifFlagActive = true;
		if (index <= 3)
		{
			this.onThread = false;
		}
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0002420B File Offset: 0x0002240B
	private int Daily_IndexMax()
	{
		return NetworkTime.Inst.dayIndex;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00024217 File Offset: 0x00022417
	private int Daily_IndexMin()
	{
		return this.daily_IndexMin;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0002421F File Offset: 0x0002241F
	public void GoFirstPage()
	{
		this.daily_Index = this.Daily_IndexMin();
		this.Daily_UpdateIndex();
		this.panelInfinity.ClearAll();
		this.UpdateList();
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00024244 File Offset: 0x00022444
	public void GoPrevPage(int num)
	{
		this.daily_Index -= num;
		this.Daily_UpdateIndex();
		this.panelInfinity.ClearAll();
		this.UpdateList();
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0002426B File Offset: 0x0002246B
	public void GoNextPage(int num)
	{
		this.daily_Index += num;
		this.Daily_UpdateIndex();
		this.panelInfinity.ClearAll();
		this.UpdateList();
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00024292 File Offset: 0x00022492
	public void GoLastPage()
	{
		this.daily_Index = this.Daily_IndexMax();
		this.Daily_UpdateIndex();
		this.panelInfinity.ClearAll();
		this.UpdateList();
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000242B7 File Offset: 0x000224B7
	private void Daily_UpdateIndex()
	{
		this.daily_Index = Mathf.Clamp(this.daily_Index, this.Daily_IndexMin(), this.Daily_IndexMax());
		this.daily_TextIndexDate.text = NetworkTime.GetString_TimeWithDayIndex(this.daily_Index);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.daily_TransButtons);
	}

	// Token: 0x04000527 RID: 1319
	public static UI_Panel_RankList inst;

	// Token: 0x04000528 RID: 1320
	[SerializeField]
	private int modeType = 1;

	// Token: 0x04000529 RID: 1321
	[SerializeField]
	private int jobType;

	// Token: 0x0400052A RID: 1322
	[SerializeField]
	private bool getDataSet;

	// Token: 0x0400052B RID: 1323
	[SerializeField]
	private Text[] jobTexts = new Text[0];

	// Token: 0x0400052C RID: 1324
	[SerializeField]
	private Text[] titleTexts = new Text[0];

	// Token: 0x0400052D RID: 1325
	[SerializeField]
	private Text[] titleRow_Infinity = new Text[0];

	// Token: 0x0400052E RID: 1326
	[SerializeField]
	private Text textRefresh;

	// Token: 0x0400052F RID: 1327
	[SerializeField]
	private Text textReturn;

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private Text tips;

	// Token: 0x04000531 RID: 1329
	public DataSet dataSetCommon;

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private float deltaTimeFromLastRefresh = 1f;

	// Token: 0x04000533 RID: 1331
	[Header("GameObjs")]
	[SerializeField]
	private GameObject objJobs;

	// Token: 0x04000534 RID: 1332
	[SerializeField]
	private GameObject objDate;

	// Token: 0x04000535 RID: 1333
	[Header("Flag")]
	private int flagIndex;

	// Token: 0x04000536 RID: 1334
	private bool ifFlagActive;

	// Token: 0x04000537 RID: 1335
	[Header("ChildPanels")]
	[SerializeField]
	private UI_Panel_RankList_Infinity panelInfinity;

	// Token: 0x04000538 RID: 1336
	[SerializeField]
	private float[] jobChoose_Spacing = new float[3];

	// Token: 0x04000539 RID: 1337
	[SerializeField]
	private HorizontalLayoutGroup jobChoose_HoriLayout;

	// Token: 0x0400053A RID: 1338
	[Header("Daily")]
	[SerializeField]
	private int daily_IndexMin = 6;

	// Token: 0x0400053B RID: 1339
	[SerializeField]
	public int daily_Index = 1;

	// Token: 0x0400053C RID: 1340
	[SerializeField]
	private Text daily_TextIndexDate;

	// Token: 0x0400053D RID: 1341
	[SerializeField]
	private RectTransform daily_TransButtons;

	// Token: 0x0400053E RID: 1342
	[SerializeField]
	private bool[] daily_DOFlags;

	// Token: 0x0400053F RID: 1343
	[Header("Thread")]
	[SerializeField]
	private bool onThread;

	// Token: 0x04000540 RID: 1344
	[SerializeField]
	private bool waitingThread;

	// Token: 0x04000541 RID: 1345
	private Thread thread;

	// Token: 0x02000157 RID: 343
	private enum EnumMode
	{
		// Token: 0x040009E6 RID: 2534
		UNINITED = -1,
		// Token: 0x040009E7 RID: 2535
		ENDLESS,
		// Token: 0x040009E8 RID: 2536
		WANDER
	}
}
