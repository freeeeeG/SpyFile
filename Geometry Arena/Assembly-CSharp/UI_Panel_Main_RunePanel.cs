using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CF RID: 207
public class UI_Panel_Main_RunePanel : MonoBehaviour
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x000272DE File Offset: 0x000254DE
	[SerializeField]
	public int[] synthesizeIndexs
	{
		get
		{
			return GameData.inst.runeFusion_MaterialIndexs;
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x000272EA File Offset: 0x000254EA
	private void Awake()
	{
		UI_Panel_Main_RunePanel.inst = this;
		this.panelDailyAward.gameObject.SetActive(false);
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00027303 File Offset: 0x00025503
	private void Update()
	{
		this.Update_HotKey();
		this.DailyAward_UpdateError();
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00027314 File Offset: 0x00025514
	public void Open()
	{
		base.gameObject.SetActive(true);
		if (UI_Panel_Main_NewGame.inst != null)
		{
			UI_Panel_Main_NewGame.inst.gameObject.SetActive(false);
		}
		this.panelSort.gameObject.SetActive(false);
		if (GameData.inst.ifOnFusion)
		{
			this.panelResult.OpenAndInitWithRunes(GameData.inst.runeFusion_MaterialIndexs, GameData.inst.runeFusion_Result, true, false);
		}
		else
		{
			this.panelResult.gameObject.SetActive(false);
		}
		this.panelRuneDetail.UpdatePreview();
		this.UpdateLanguage();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x000273AC File Offset: 0x000255AC
	private void OnEnable()
	{
		this.Thread_DailyAward_ReadSql();
		TutorialMaster.inst.Activate();
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x000273BE File Offset: 0x000255BE
	private void OnDisable()
	{
		TutorialMaster.inst.Activate();
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x000273CC File Offset: 0x000255CC
	private void UpdateLanguage()
	{
		this.Sort();
		this.icon_GeometryCoin.UpdateIcon();
		this.icon_Star.UpdateIcon();
		this.runeList.InitIcons(null);
		LanguageText languageText = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = languageText.runeInfo;
		this.textBuyNewRune.text = runeInfo.button_BuyNewRune.AppendKeycode("F");
		this.textBuyNewBigRune.text = runeInfo.button_BuyNewBigRune.AppendKeycode("G");
		this.textRuneList.text = runeInfo.runeList;
		this.textSortButton.text = runeInfo.sort_OpenButton;
		this.textDailyAward.text = languageText.dailyChallenge.dailyAward;
		this.modeTexts[0].text = runeInfo.mode_Manage;
		this.modeTexts[1].text = runeInfo.mode_Store;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.modeButton_Parent);
		if (this.panelMode == UI_Panel_Main_RunePanel.EnumRunePanelMode.MANAGE)
		{
			this.rightPanel_Manage.SetActive(true);
			this.rightPanel_Store.SetActive(false);
			this.modeInfo.text = runeInfo.mode_ManageInfo.ReplaceLineBreak();
			this.title_Right_Manage_Equip.text = runeInfo.runeEquip;
			this.title_Right_Manage_Fuse.text = runeInfo.runeSynthesis;
			this.textTakeoff.text = runeInfo.button_TakeOff.AppendKeycode("C");
			this.textClear.text = runeInfo.button_Clear.AppendKeycode("Q");
			this.textSynthesize.text = runeInfo.button_Synthesize.AppendKeycode("E");
		}
		else if (this.panelMode == UI_Panel_Main_RunePanel.EnumRunePanelMode.STORE)
		{
			this.rightPanel_Manage.SetActive(false);
			this.rightPanel_Store.SetActive(true);
			this.modeInfo.text = runeInfo.mode_StoreInfo.ReplaceLineBreak();
			this.panelRuneStore.InitIcons(null);
		}
		for (int i = 0; i < this.modeTexts.Length; i++)
		{
			if (i == (int)this.panelMode)
			{
				this.modeTexts[i].color = Color.yellow;
			}
			else
			{
				this.modeTexts[i].color = Color.white;
			}
		}
		this.iconRuneEquip.UpdateIcon(GameData.inst.currentRunesIndex[0], UI_Icon_Rune.EnumIconRuneType.EQUIP);
		this.iconRuneSynthesizes[0].UpdateIcon(this.synthesizeIndexs[0], UI_Icon_Rune.EnumIconRuneType.SYNTHESIZE);
		this.iconRuneSynthesizes[1].UpdateIcon(this.synthesizeIndexs[1], UI_Icon_Rune.EnumIconRuneType.SYNTHESIZE);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00027624 File Offset: 0x00025824
	public void RuneEquip_Equip(int index)
	{
		for (int i = 0; i <= 1; i++)
		{
			if (index == GameData.inst.runeFusion_MaterialIndexs[i])
			{
				GameData.inst.runeFusion_MaterialIndexs[i] = -1;
			}
		}
		GameData.inst.currentRunesIndex[0] = index;
		this.UpdateLanguage();
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002766C File Offset: 0x0002586C
	public void RuneEquip_TakeOff()
	{
		GameData.inst.currentRunesIndex[0] = -1;
		this.UpdateLanguage();
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00027684 File Offset: 0x00025884
	public void RuneSyn_Add(int index)
	{
		if (index == GameData.inst.currentRunesIndex[0])
		{
			GameData.inst.currentRunesIndex[0] = -1;
		}
		for (int i = 0; i < 2; i++)
		{
			if (this.synthesizeIndexs[i] == index)
			{
				this.synthesizeIndexs[i] = -1;
				this.UpdateLanguage();
				return;
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (this.synthesizeIndexs[j] == -1)
			{
				this.synthesizeIndexs[j] = index;
				this.synthesizeCurOrder = 1 - j;
				this.UpdateLanguage();
				return;
			}
		}
		this.synthesizeIndexs[this.synthesizeCurOrder] = index;
		this.synthesizeCurOrder = 1 - this.synthesizeCurOrder;
		this.UpdateLanguage();
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00027726 File Offset: 0x00025926
	public void RuneSyn_Clear()
	{
		this.synthesizeIndexs[0] = -1;
		this.synthesizeIndexs[1] = -1;
		this.UpdateLanguage();
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00027740 File Offset: 0x00025940
	public void RuneSyn_Remove(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.synthesizeIndexs[i] == index)
			{
				this.synthesizeIndexs[i] = -1;
			}
		}
		this.UpdateLanguage();
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00027774 File Offset: 0x00025974
	public void Button_Fuse()
	{
		int num = 1;
		if (GameData.inst.GeometryCoin < (long)num)
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.lackOfGeometryCoin);
			return;
		}
		if (this.synthesizeIndexs[0] < 0 || this.synthesizeIndexs[1] < 0)
		{
			return;
		}
		GameData.inst.GeometryCoin_Use((long)num);
		this.RuneSyn_Syn(false, true);
		this.Open();
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x000277DC File Offset: 0x000259DC
	public void RuneSyn_Syn(bool ifsave, bool firstOpen)
	{
		if (this.synthesizeIndexs[0] < 0 || this.synthesizeIndexs[1] < 0)
		{
			return;
		}
		Rune father = GameData.inst.runes[this.synthesizeIndexs[0]];
		Rune mother = GameData.inst.runes[this.synthesizeIndexs[1]];
		Rune result = Rune.Synthesize(father, mother);
		this.panelResult.OpenAndInitWithRunes(GameData.inst.runeFusion_MaterialIndexs, result, ifsave, firstOpen);
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00027850 File Offset: 0x00025A50
	public void SelectMode(int i)
	{
		this.panelMode = (UI_Panel_Main_RunePanel.EnumRunePanelMode)i;
		this.panelRuneDetail.ClearRuneGood();
		this.panelRuneDetail.UpdatePreview();
		this.UpdateLanguage();
		this.modeTitle.headID = i + 12;
		this.modeTitle.indexID = i + 12;
		TutorialMaster.inst.Activate();
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000278A8 File Offset: 0x00025AA8
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.inst.Panel_NewGame_Open();
		SaveFile.SaveByJson(false);
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000278C8 File Offset: 0x00025AC8
	public bool Syn_IfContain(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.synthesizeIndexs[i] == index)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x000278F0 File Offset: 0x00025AF0
	public void BuyNewRune()
	{
		if (GameData.inst.GeometryCoin < 20L)
		{
			return;
		}
		GameData.inst.GeometryCoin_Use(20L);
		Rune rune = new Rune();
		rune.InitRandom(EnumRank.UNINTED, -1, false, -1, EnumRunePropertyType.UNINITED);
		Rune.AddNewRune(rune);
		this.Open();
		UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_BuyRune.Replace("sPrice", "20").Replace("sRune", rune.Get_Lang_RuneName()));
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00027970 File Offset: 0x00025B70
	public void BuyNewBigRune()
	{
		if (GameData.inst.GeometryCoin < 200L)
		{
			return;
		}
		GameData.inst.GeometryCoin_Use(200L);
		Rune rune = Rune.NewBigRune(this.set_BigRuneNum);
		Rune.AddNewRune(rune);
		this.Open();
		UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_BuyRune.Replace("sPrice", "200").Replace("sRune", rune.Get_Lang_RuneName()));
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x000279F0 File Offset: 0x00025BF0
	private void Update_HotKey()
	{
		if (this.panelResult.gameObject.activeSelf)
		{
			return;
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.Escape) || MyInput.GetKeyDownWithSound(Input.GetMouseButtonDown(1)))
		{
			UI_ToolTip.inst.Close();
			this.Close();
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.Tab))
		{
			this.SelectMode((int)(UI_Panel_Main_RunePanel.EnumRunePanelMode.STORE - this.panelMode));
			return;
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.A))
		{
			this.runeList.PreviousPage();
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.D))
		{
			this.runeList.NextPage();
		}
		UI_Panel_Main_RunePanel.EnumRunePanelMode enumRunePanelMode = this.panelMode;
		if (enumRunePanelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.MANAGE)
		{
			if (enumRunePanelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.STORE)
			{
				return;
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.F))
			{
				this.BuyNewRune();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.G))
			{
				this.BuyNewBigRune();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.R))
			{
				this.panelRuneStore.Icon_TryRefresh();
			}
		}
		else
		{
			if (MyInput.GetKeyDownWithSound(KeyCode.C))
			{
				this.RuneEquip_TakeOff();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.Q))
			{
				this.RuneSyn_Clear();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.E))
			{
				this.Button_Fuse();
				return;
			}
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00027AE5 File Offset: 0x00025CE5
	public void ViewDetail(Rune rune)
	{
		this.panelRuneDetail.OpenWithRune(rune);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00027AF4 File Offset: 0x00025CF4
	public void DailyAward_UpdateError()
	{
		if (!this.dailyAward_IfError)
		{
			if (NetworkTime.Inst.ifError)
			{
				this.dailyAward_IfError = true;
				this.dailyAward_ErrorFlag = 2;
			}
			return;
		}
		if (this.dailyAward_UpdateLeft > 0f)
		{
			this.dailyAward_UpdateLeft -= Time.unscaledDeltaTime;
			return;
		}
		this.dailyAward_UpdateLeft = 0.5f;
		this.Thread_DailyAward_ReadSql();
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00027B55 File Offset: 0x00025D55
	public void DailyAward_SetFlag(int index)
	{
		this.dailyAward_ErrorFlag = index;
		this.dailyAward_IfError = true;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00027B65 File Offset: 0x00025D65
	public void Thread_DailyAward_ReadSql()
	{
		new Thread(new ThreadStart(this.DailyAward_ReadSql)).Start();
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00027B80 File Offset: 0x00025D80
	private void DailyAward_ReadSql()
	{
		this.dailyAward_IfError = false;
		this.dailyAward_ErrorFlag = 0;
		if (NetworkTime.Inst.ifError)
		{
			this.DailyAward_SetFlag(2);
			Debug.LogError("Error_DailyAward_无法连接到时间服务器");
			return;
		}
		SqlManager sqlManager = SqlManager.inst;
		try
		{
			sqlManager.OpenSqlNow();
		}
		catch (Exception arg)
		{
			Debug.LogError("数据库连接异常" + arg);
			this.DailyAward_SetFlag(2);
			return;
		}
		if (!Setting.Inst.Option_Network)
		{
			Debug.LogError("Error_网络功能已关闭！");
			this.DailyAward_SetFlag(1);
			return;
		}
		if (SqlManager.inst.mysql.mySqlConnection.State != ConnectionState.Open)
		{
			Debug.LogError("Error_无法连接数据库！");
			this.DailyAward_SetFlag(2);
			return;
		}
		string string_SteamID;
		try
		{
			string_SteamID = SqlManager.GetString_SteamID();
		}
		catch
		{
			this.DailyAward_SetFlag(4);
			throw new Exception("Error_未登陆Steam");
		}
		MySqlConnection mySqlConnection = sqlManager.mysql.mySqlConnection;
		string selectCommandText = "SELECT * FROM RankList_Daily WHERE SteamID = " + string_SteamID + " ORDER BY DayIndex ASC;";
		DataSet dataSet = new DataSet();
		try
		{
			new MySqlDataAdapter(selectCommandText, mySqlConnection).Fill(dataSet);
		}
		catch (Exception ex)
		{
			this.DailyAward_SetFlag(3);
			SqlManager.inst.mysql = null;
			throw new Exception("Error_数据获取异常:   " + ex.Message.ToString());
		}
		DataTable dataTable = dataSet.Tables[0];
		List<int> list = new List<int>();
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			DataRow d = dataTable.Rows[i];
			int @int = d.GetInt(23);
			int int2 = d.GetInt(21);
			if (int2 < NetworkTime.Inst.dayIndex && @int != 1)
			{
				list.Add(int2);
			}
		}
		this.dailyAward_ListAwardSets = new List<UI_Panel_Main_RunePanel.DailyAwardSet>();
		for (int j = 0; j < list.Count; j++)
		{
			int num = list[j];
			string selectCommandText2 = string.Format("SELECT * FROM RankList_Daily WHERE DayIndex = {0} ORDER BY Stars DESC;", num);
			DataSet dataSet2 = new DataSet();
			try
			{
				new MySqlDataAdapter(selectCommandText2, mySqlConnection).Fill(dataSet2);
			}
			catch (Exception ex2)
			{
				this.DailyAward_SetFlag(3);
				SqlManager.inst.mysql = null;
				throw new Exception("Error_数据获取异常:   " + ex2.Message.ToString());
			}
			bool flag = false;
			DataTable dataTable2 = dataSet2.Tables[0];
			for (int k = 0; k < dataTable2.Rows.Count; k++)
			{
				if (dataTable2.Rows[k].GetString(3) == SqlManager.GetString_SteamID())
				{
					int num2 = k + 1;
					if (num2 > 99999)
					{
						Debug.Log(string.Format("{0}的成绩{1}，没有奖励，已标记为领取过", num, num2));
						this.DailyAward_SetGetAwarded(string_SteamID, num);
						flag = true;
						break;
					}
					UI_Panel_Main_RunePanel.DailyAwardSet dailyAwardSet = new UI_Panel_Main_RunePanel.DailyAwardSet();
					dailyAwardSet.dailyIndex = num;
					dailyAwardSet.ranking = num2;
					this.dailyAward_ListAwardSets.Add(dailyAwardSet);
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogError(string.Format("Error_没在{0}这天找到自己的成绩", num));
				return;
			}
		}
		Debug.Log("成功读取自己的每日成绩");
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00027EBC File Offset: 0x000260BC
	private void DailyAward_SetGetAwarded(string steamID, int dayIndex)
	{
		string sqlStr = " UPDATE RankList_Daily SET IfAwarded = REPLACE(IfAwarded, 0, 1) WHERE Unikey = '" + steamID + dayIndex.ToString() + "';";
		SqlManager.inst.mysql.QuerySet(sqlStr);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00027EF4 File Offset: 0x000260F4
	public void DailyAward_GetAward()
	{
		LanguageText languageText = LanguageText.Inst;
		LanguageText.DailyChallenge dailyChallenge = languageText.dailyChallenge;
		LanguageText.PanelRankList rankListPanel = languageText.rankListPanel;
		if (this.dailyAward_IfError)
		{
			switch (this.dailyAward_ErrorFlag)
			{
			case 0:
			case 2:
				UI_FloatTextControl.inst.NewFloatText(rankListPanel.tips_Flag2ConnectError);
				return;
			case 1:
				UI_FloatTextControl.inst.NewFloatText(rankListPanel.tips_Flag1NetWorkClose);
				return;
			case 3:
				UI_FloatTextControl.inst.NewFloatText(rankListPanel.tips_Flag3DataError);
				return;
			case 4:
				UI_FloatTextControl.inst.NewFloatText(rankListPanel.tips_Flag4SteamError);
				return;
			default:
				return;
			}
		}
		else
		{
			if (this.dailyAward_ListAwardSets == null)
			{
				Debug.LogError("Error " + 2);
				return;
			}
			if (this.dailyAward_ListAwardSets.Count == 0)
			{
				UI_FloatTextControl.inst.NewFloatText(dailyChallenge.noAwards);
				return;
			}
			UI_Panel_Main_RunePanel.DailyAwardSet dailyAwardSet = this.dailyAward_ListAwardSets[0];
			if (dailyAwardSet.ranking <= 0)
			{
				Debug.LogError(string.Format("Error_Ranking{0},index{1}", dailyAwardSet.ranking, dailyAwardSet.dailyIndex));
				return;
			}
			string string_SteamID = SqlManager.GetString_SteamID();
			this.dailyAward_ListAwardSets.RemoveAt(0);
			try
			{
				this.DailyAward_SetGetAwarded(string_SteamID, dailyAwardSet.dailyIndex);
			}
			catch (Exception arg)
			{
				UI_FloatTextControl.inst.NewFloatText(rankListPanel.tips_Flag2ConnectError);
				Debug.LogError("Error_领取失败： " + arg);
				return;
			}
			Debug.Log(string.Format("已领取{0}这一天奖励", dailyAwardSet.dailyIndex));
			this.panelDailyAward.OpenAndInit(dailyAwardSet.ranking, dailyAwardSet.dailyIndex);
			return;
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000051D0 File Offset: 0x000033D0
	public void DailyAward_Button_GetAward()
	{
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00028084 File Offset: 0x00026284
	public string DailyAward_GetString_ToolTipInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		LanguageText languageText = LanguageText.Inst;
		LanguageText.DailyChallenge dailyChallenge = languageText.dailyChallenge;
		LanguageText.PanelRankList rankListPanel = languageText.rankListPanel;
		UI_Setting ui_Setting = UI_Setting.Inst;
		stringBuilder.Append(dailyChallenge.dailyAwardTotal.TextSet(ui_Setting.commonSets.blueSmallTile)).AppendLine();
		stringBuilder.Append(dailyChallenge.awardInfo).AppendLine().AppendLine();
		if (this.dailyAward_IfError)
		{
			switch (this.dailyAward_ErrorFlag)
			{
			case 1:
				stringBuilder.Append(rankListPanel.tips_Flag1NetWorkClose);
				break;
			case 2:
				stringBuilder.Append(rankListPanel.tips_Flag2ConnectError);
				break;
			case 3:
				stringBuilder.Append(rankListPanel.tips_Flag3DataError);
				break;
			case 4:
				stringBuilder.Append(rankListPanel.tips_Flag4SteamError);
				break;
			}
			return stringBuilder.ToString();
		}
		if (this.dailyAward_ListAwardSets == null)
		{
			Debug.LogError("Error_奖励数组null??");
			stringBuilder.Append(rankListPanel.tips_Flag2ConnectError);
			return stringBuilder.ToString();
		}
		if (this.dailyAward_ListAwardSets.Count == 0)
		{
			stringBuilder.Append(dailyChallenge.noAwards);
			return stringBuilder.ToString();
		}
		stringBuilder.Append(dailyChallenge.unawardedList).AppendLine();
		for (int i = 0; i < this.dailyAward_ListAwardSets.Count; i++)
		{
			UI_Panel_Main_RunePanel.DailyAwardSet dailyAwardSet = this.dailyAward_ListAwardSets[i];
			stringBuilder.Append(NetworkTime.GetString_TimeWithDayIndex(dailyAwardSet.dailyIndex)).Append(" ").Append(dailyChallenge.ranking).Append(" ").Append(dailyAwardSet.ranking);
			if (i != this.dailyAward_ListAwardSets.Count - 1)
			{
				stringBuilder.AppendLine();
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00028234 File Offset: 0x00026434
	private void RuneSort_InitSort()
	{
		this.listRuneSort = new List<int>();
		List<Rune> runes = GameData.inst.runes;
		for (int i = 0; i < runes.Count; i++)
		{
			this.listRuneSort.Add(i);
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00028274 File Offset: 0x00026474
	private void RuneSort_SortAsTime_OldToNew()
	{
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (this.listRuneSort[j] > this.listRuneSort[j + 1])
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00028308 File Offset: 0x00026508
	private void RuneSort_SortAsTime_NewToOld()
	{
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (this.listRuneSort[j] < this.listRuneSort[j + 1])
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002839C File Offset: 0x0002659C
	private void RuneSort_SortAsRank_HighToLow()
	{
		List<Rune> runes = GameData.inst.runes;
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (runes[this.listRuneSort[j]].rank < runes[this.listRuneSort[j + 1]].rank)
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00028454 File Offset: 0x00026654
	private void RuneSort_SortAsRank_LowToHigh()
	{
		List<Rune> runes = GameData.inst.runes;
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (runes[this.listRuneSort[j]].rank > runes[this.listRuneSort[j + 1]].rank)
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002850C File Offset: 0x0002670C
	private void RuneSort_SortAsType()
	{
		List<Rune> runes = GameData.inst.runes;
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (runes[this.listRuneSort[j]].typeID > runes[this.listRuneSort[j + 1]].typeID)
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x000285C4 File Offset: 0x000267C4
	private void RuneSort_FavoriteFirst()
	{
		List<Rune> runes = GameData.inst.runes;
		for (int i = 0; i < this.listRuneSort.Count - 1; i++)
		{
			for (int j = 0; j < this.listRuneSort.Count - 1 - i; j++)
			{
				if (!runes[this.listRuneSort[j]].ifFavorite && runes[this.listRuneSort[j + 1]].ifFavorite)
				{
					int value = this.listRuneSort[j];
					this.listRuneSort[j] = this.listRuneSort[j + 1];
					this.listRuneSort[j + 1] = value;
				}
			}
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002867E File Offset: 0x0002687E
	public void SetSortType(int i)
	{
		this.panelSort.Close();
		if (i < 0 || i > 4)
		{
			Debug.LogError("Error_SortTypeOut!");
			return;
		}
		this.sortType = (UI_Panel_Main_RunePanel.EnumSortType)i;
		UI_Panel_Main_RunePanel.inst.UpdateLanguage();
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x000286B0 File Offset: 0x000268B0
	private void Sort()
	{
		this.RuneSort_InitSort();
		switch (this.sortType)
		{
		case UI_Panel_Main_RunePanel.EnumSortType.DEFAULT_TIME_OLDTONEW:
			this.RuneSort_SortAsTime_OldToNew();
			this.RuneSort_FavoriteFirst();
			return;
		case UI_Panel_Main_RunePanel.EnumSortType.TIME_NEWTOOLD:
			this.RuneSort_SortAsTime_NewToOld();
			this.RuneSort_FavoriteFirst();
			return;
		case UI_Panel_Main_RunePanel.EnumSortType.RUNETYPE:
			this.RuneSort_SortAsType();
			this.RuneSort_FavoriteFirst();
			return;
		case UI_Panel_Main_RunePanel.EnumSortType.RUNERANK_HIGHTOLOW:
			this.RuneSort_SortAsRank_HighToLow();
			this.RuneSort_FavoriteFirst();
			return;
		case UI_Panel_Main_RunePanel.EnumSortType.RUNERANK_LOWTOHIGH:
			this.RuneSort_SortAsRank_LowToHigh();
			this.RuneSort_FavoriteFirst();
			return;
		default:
			return;
		}
	}

	// Token: 0x040005D7 RID: 1495
	public static UI_Panel_Main_RunePanel inst;

	// Token: 0x040005D8 RID: 1496
	public UI_Panel_Main_RuneList runeList;

	// Token: 0x040005D9 RID: 1497
	public UI_Panel_Main_RunePanel.EnumRunePanelMode panelMode;

	// Token: 0x040005DA RID: 1498
	[SerializeField]
	private int set_BigRuneNum = 15;

	// Token: 0x040005DB RID: 1499
	[SerializeField]
	private UI_Text_SimpleTooltip modeTitle;

	// Token: 0x040005DC RID: 1500
	[SerializeField]
	public UI_Panel_Rune_RuneDetail panelRuneDetail;

	// Token: 0x040005DD RID: 1501
	[SerializeField]
	private UI_Panel_Daily_Award panelDailyAward;

	// Token: 0x040005DE RID: 1502
	[SerializeField]
	private Text textBuyNewRune;

	// Token: 0x040005DF RID: 1503
	[SerializeField]
	private Text textBuyNewBigRune;

	// Token: 0x040005E0 RID: 1504
	[SerializeField]
	private Text textSortButton;

	// Token: 0x040005E1 RID: 1505
	[SerializeField]
	private Text textDailyAward;

	// Token: 0x040005E2 RID: 1506
	[SerializeField]
	private Text textRuneList;

	// Token: 0x040005E3 RID: 1507
	[Header("ModeChoose")]
	[SerializeField]
	private Text[] modeTexts = new Text[0];

	// Token: 0x040005E4 RID: 1508
	[SerializeField]
	private Text modeInfo;

	// Token: 0x040005E5 RID: 1509
	[SerializeField]
	private RectTransform modeButton_Parent;

	// Token: 0x040005E6 RID: 1510
	[Header("Right_ManageOrStore")]
	[SerializeField]
	private Text title_Right_Manage_Equip;

	// Token: 0x040005E7 RID: 1511
	[SerializeField]
	private Text title_Right_Manage_Fuse;

	// Token: 0x040005E8 RID: 1512
	[SerializeField]
	private Text textTakeoff;

	// Token: 0x040005E9 RID: 1513
	[SerializeField]
	private Text textClear;

	// Token: 0x040005EA RID: 1514
	[SerializeField]
	private Text textSynthesize;

	// Token: 0x040005EB RID: 1515
	[SerializeField]
	private GameObject rightPanel_Manage;

	// Token: 0x040005EC RID: 1516
	[SerializeField]
	private GameObject rightPanel_Store;

	// Token: 0x040005ED RID: 1517
	[Header("资源显示")]
	[SerializeField]
	private UI_Icon_GeometryCoin icon_GeometryCoin;

	// Token: 0x040005EE RID: 1518
	[SerializeField]
	private UI_Icon_Star icon_Star;

	// Token: 0x040005EF RID: 1519
	[Header("装备与合成")]
	[SerializeField]
	private int synthesizeCurOrder;

	// Token: 0x040005F0 RID: 1520
	[SerializeField]
	private UI_Icon_Rune iconRuneEquip;

	// Token: 0x040005F1 RID: 1521
	[SerializeField]
	private UI_Icon_Rune[] iconRuneSynthesizes;

	// Token: 0x040005F2 RID: 1522
	[SerializeField]
	private UI_Panel_Main_RuneSynResult panelResult;

	// Token: 0x040005F3 RID: 1523
	[Header("符文商店")]
	public UI_Panel_Rune_RuneStore panelRuneStore;

	// Token: 0x040005F4 RID: 1524
	[Header("RuneSort")]
	[SerializeField]
	public List<int> listRuneSort = new List<int>();

	// Token: 0x040005F5 RID: 1525
	[SerializeField]
	private UI_Panel_Main_RunePanel.EnumSortType sortType;

	// Token: 0x040005F6 RID: 1526
	[SerializeField]
	public UI_Panel_Rune_RuneSort panelSort;

	// Token: 0x040005F7 RID: 1527
	[Header("Daily")]
	[SerializeField]
	private List<UI_Panel_Main_RunePanel.DailyAwardSet> dailyAward_ListAwardSets;

	// Token: 0x040005F8 RID: 1528
	[SerializeField]
	public bool dailyAward_IfError;

	// Token: 0x040005F9 RID: 1529
	[SerializeField]
	public int dailyAward_ErrorFlag;

	// Token: 0x040005FA RID: 1530
	[SerializeField]
	private float dailyAward_UpdateLeft = 0.5f;

	// Token: 0x0200015B RID: 347
	public enum EnumRunePanelMode
	{
		// Token: 0x040009F5 RID: 2549
		MANAGE,
		// Token: 0x040009F6 RID: 2550
		STORE
	}

	// Token: 0x0200015C RID: 348
	[Serializable]
	private class DailyAwardSet
	{
		// Token: 0x040009F7 RID: 2551
		public int dailyIndex = -1;

		// Token: 0x040009F8 RID: 2552
		public int ranking = -1;
	}

	// Token: 0x0200015D RID: 349
	private enum EnumSortType
	{
		// Token: 0x040009FA RID: 2554
		DEFAULT_TIME_OLDTONEW,
		// Token: 0x040009FB RID: 2555
		TIME_NEWTOOLD,
		// Token: 0x040009FC RID: 2556
		RUNETYPE,
		// Token: 0x040009FD RID: 2557
		RUNERANK_HIGHTOLOW,
		// Token: 0x040009FE RID: 2558
		RUNERANK_LOWTOHIGH
	}
}
