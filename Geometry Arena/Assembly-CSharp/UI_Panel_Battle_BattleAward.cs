using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A6 RID: 166
public class UI_Panel_Battle_BattleAward : MonoBehaviour
{
	// Token: 0x060005BE RID: 1470 RVA: 0x00020B68 File Offset: 0x0001ED68
	private void Awake()
	{
		UI_Panel_Battle_BattleAward.inst = this;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00020B70 File Offset: 0x0001ED70
	public void InitAndSave(bool ifFinish)
	{
		if (this.awarded)
		{
			return;
		}
		this.awarded = true;
		Debug.Log("结算");
		BattleMapCanvas.inst.CloseAllWindows();
		BattleMapCanvas.inst.SetPauseMenu(false);
		if (Player.inst != null)
		{
			Player.inst.unit.Die(true);
		}
		if (ifFinish)
		{
			this.AfterFinish();
		}
		base.gameObject.SetActive(true);
		this.UpdateLanguage(ifFinish);
		EnumModeType modeType = TempData.inst.modeType;
		if (modeType == EnumModeType.ENDLESS)
		{
			int num = Battle.inst.SequalWave - 1;
			if (num > GameData.inst.maxEndless)
			{
				GameData.inst.maxEndless = num;
			}
		}
		long score = Battle.inst.Score;
		this.text_StarNum.text = score.ToString();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectStar);
		GameData.inst.GetStar(score);
		int num2 = ifFinish ? 1 : 0;
		int num3 = Battle.inst.SequalWave - 1 + num2;
		num3 = Mathf.Max(0, num3);
		Mastery.CurrentJobGainExps(num3);
		MySteamAchievement.DetectRoleProficiencyLevel();
		int num4 = 0;
		this.geometryCoin_InAward.UpdateIcon(num3, ref num4);
		GameData.inst.GeometryCoin_Get((long)num4);
		SaveFile.SaveByJson(false);
		this.Thread_UpdateChildPanel_AboutLeaderboard_TryUpload();
		MySteamAchievement.SetStatInt("max_Battle_Star", (int)Mathf.Min(2.1474836E+09f, (float)score));
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			MySteamAchievement.TryUnlockAchievementWithName("WanderMode");
		}
		if (TempData.inst.daily_Open)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_Once");
		}
		try
		{
			SaveFileBackUp.BackUp();
		}
		catch (Exception ex)
		{
			Debug.LogError("存档大备份保存失败：" + ex.ToString());
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00020D1C File Offset: 0x0001EF1C
	private void AfterFinish()
	{
		if (!GameData.inst.ifFinished)
		{
			Debug.Log("第一次通关");
			GameData.inst.ifFinished = true;
		}
		MySteamAchievement.TryUnlockAchievementWithName("NormalMode");
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00020D4C File Offset: 0x0001EF4C
	private void UpdateLanguage(bool ifFinish)
	{
		LanguageText languageText = LanguageText.Inst;
		this.lang_FinishInfo.gameObject.SetActive(ifFinish);
		if (ifFinish)
		{
			this.lang_Title.text = languageText.battleAward.title_Finish;
			if (GameParameters.Inst.ifDemo)
			{
				this.lang_FinishInfo.fontSize = 25;
				this.lang_FinishInfo.lineSpacing = 1f;
				if (TempData.inst.battle.factorBattle_FromDifficultyOption.StarGain >= 1.5f)
				{
					this.lang_FinishInfo.text = languageText.demo.shanguo_Success.ReplaceLineBreak().Replace("sStar", MyTool.DecimalToMultiPercentString((double)Battle.inst.factorBattle_FromDifficultyOption.StarGain)).Colored(Color.yellow);
				}
				else
				{
					this.lang_FinishInfo.text = languageText.demo.shanguo_Fail.ReplaceLineBreak().Replace("sStar", MyTool.DecimalToMultiPercentString((double)Battle.inst.factorBattle_FromDifficultyOption.StarGain)).Colored(Color.yellow);
				}
			}
			else
			{
				this.lang_FinishInfo.text = languageText.battleAward.info_Finish;
			}
		}
		else
		{
			this.lang_Title.text = languageText.battleAward.title_GameOver;
		}
		this.lang_YouHaveGot.text = languageText.battleAward.youHaveGot;
		this.lang_Confirm.text = languageText.battleAward.button_ReturnToMainMenu;
		this.lang_Restart.text = languageText.battleAward.button_Restart;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00020ED4 File Offset: 0x0001F0D4
	private void Update()
	{
		LanguageText.BattleAward battleAward = LanguageText.Inst.battleAward;
		if (this.int_Leaderboard_UploadSuccessState_Mode != -1)
		{
			switch (this.int_Leaderboard_UploadSuccessState_Mode)
			{
			case 0:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Mode.text = battleAward.aboutLeaderboard_NoBrokenRecords;
				break;
			case 1:
			case 2:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Mode.text = battleAward.aboutLeaderboard_NewRecord;
				break;
			case 3:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Mode.text = battleAward.aboutLeaderboard_Error_Mode_VersionTooOld.ReplaceLineBreak();
				UI_Panel_Battle_BattleAward.inst.button_Retry_Mode.enabled = false;
				break;
			}
			this.int_Leaderboard_UploadSuccessState_Mode = -1;
		}
		if (this.int_Leaderboard_UploadSuccessState_Daily != -1)
		{
			switch (this.int_Leaderboard_UploadSuccessState_Daily)
			{
			case 0:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Daily.text = battleAward.aboutLeaderboard_NoBrokenRecords;
				break;
			case 1:
			case 2:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Daily.text = battleAward.aboutLeaderboard_NewRecord;
				break;
			case 3:
				UI_Panel_Battle_BattleAward.inst.text_Leaderboard_New_OrNot_Daily.text = battleAward.aboutLeaderboard_Error_Daily_TimeOut.ReplaceLineBreak();
				this.text_Leaderboard_Best_Info_Daily.text = "-";
				UI_Panel_Battle_BattleAward.inst.button_Retry_Daily.enabled = false;
				break;
			}
			this.int_Leaderboard_UploadSuccessState_Daily = -1;
		}
		if (UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Mode)
		{
			this.text_Leaderboard_Best_Info_Mode.text = string.Concat(new object[]
			{
				battleAward.aboutLeaderboard_Ranking,
				" ",
				this.int_Leaderboard_FindMe_Ranking_Mode,
				"\n",
				battleAward.aboutLeaderboard_Stars,
				" ",
				this.long_Leaderboard_FindMe_Star_Mode
			});
			this.text_Leaderboard_Best_Title_Mode.text = battleAward.aboutLeaderboard_MyHighestRecord;
			UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Mode = false;
			this.button_Retry_Mode.enabled = false;
		}
		if (UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Daily)
		{
			this.text_Leaderboard_Best_Info_Daily.text = string.Concat(new object[]
			{
				battleAward.aboutLeaderboard_Ranking,
				" ",
				this.int_Leaderboard_FindMe_Ranking_Daily,
				"\n",
				battleAward.aboutLeaderboard_Stars,
				" ",
				this.long_Leaderboard_FindMe_Star_Daily
			});
			this.text_Leaderboard_Best_Title_Daily.text = battleAward.aboutLeaderboard_MyHighestRecord;
			UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Daily = false;
			this.button_Retry_Daily.enabled = false;
		}
		if (this.bool_Leaderboard_FlagToUpdate_Mode)
		{
			int num = this.int_Leaderboard_ErrorState_Mode;
			string str;
			if (num - 1 <= 3)
			{
				str = battleAward.aboutLeaderboard_ErrorTips[this.int_Leaderboard_ErrorState_Mode];
			}
			else
			{
				str = "Error";
				Debug.LogError("Error_WrongErrorState");
			}
			if (this.int_Leaderboard_ErrorState_Mode == 1)
			{
				this.text_Leaderboard_Best_Info_Mode.text = str + "\n" + battleAward.aboutLeaderboard_OpenNetwordAndRetry;
			}
			else
			{
				this.text_Leaderboard_Best_Info_Mode.text = str + "\n" + battleAward.aboutLeaderboard_Retry;
			}
			this.bool_Leaderboard_FlagToUpdate_Mode = false;
			this.int_Leaderboard_ErrorState_Mode = 0;
		}
		if (this.bool_Leaderboard_FlagToUpdate_Daily)
		{
			int num = this.int_Leaderboard_ErrorState_Daily;
			string str2;
			if (num - 1 <= 3)
			{
				str2 = battleAward.aboutLeaderboard_ErrorTips[this.int_Leaderboard_ErrorState_Daily];
			}
			else
			{
				str2 = "Error";
				Debug.LogError("Error_WrongErrorState");
			}
			if (this.int_Leaderboard_ErrorState_Daily == 1)
			{
				this.text_Leaderboard_Best_Info_Daily.text = str2 + "\n" + battleAward.aboutLeaderboard_OpenNetwordAndRetry;
			}
			else
			{
				this.text_Leaderboard_Best_Info_Daily.text = str2 + "\n" + battleAward.aboutLeaderboard_Retry;
			}
			this.bool_Leaderboard_FlagToUpdate_Daily = false;
			this.int_Leaderboard_ErrorState_Daily = 0;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00021254 File Offset: 0x0001F454
	public static void SetFlag_Mode(int index)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			Debug.LogError("Error_PanelAwardInst==null");
			return;
		}
		if (index <= 0 || index >= 5)
		{
			Debug.LogError("Error_异常Flag有问题");
			return;
		}
		Debug.LogError("Error_异常Flag_Mode_" + index);
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_ErrorState_Mode = index;
		UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FlagToUpdate_Mode = true;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x000212B8 File Offset: 0x0001F4B8
	public static void SetFlag_Daily(int index)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			Debug.LogError("Error_PanelAwardInst==null");
			return;
		}
		if (index <= 0 || index >= 5)
		{
			Debug.LogError("Error_异常Flag有问题");
			return;
		}
		Debug.LogError("Error_异常Flag_Daily_" + index);
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_ErrorState_Daily = index;
		UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FlagToUpdate_Daily = true;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0002131B File Offset: 0x0001F51B
	public static void SetFlag_Both(int index)
	{
		if (index <= 0 || index >= 5)
		{
			Debug.LogError("Error_异常Flag有问题");
			return;
		}
		Debug.LogError("Error_异常Flag_Both_" + index);
		UI_Panel_Battle_BattleAward.SetFlag_Mode(index);
		UI_Panel_Battle_BattleAward.SetFlag_Daily(index);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00021351 File Offset: 0x0001F551
	public static void UploadSuccess_Mode(int state)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			Debug.LogError("Error_BattleAwardInstNull");
			return;
		}
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_UploadSuccessState_Mode = state;
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00021376 File Offset: 0x0001F576
	public static void UploadSuccess_Daily(int state)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			Debug.LogError("Error_BattleAwardInstNull");
			return;
		}
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_UploadSuccessState_Daily = state;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002139B File Offset: 0x0001F59B
	public static void FindMyRecord_Mode(int ranking, long star)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			return;
		}
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_FindMe_Ranking_Mode = ranking;
		UI_Panel_Battle_BattleAward.inst.long_Leaderboard_FindMe_Star_Mode = star;
		UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Mode = true;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000213CC File Offset: 0x0001F5CC
	public static void FindMyRecord_Daily(int ranking, long star)
	{
		if (UI_Panel_Battle_BattleAward.inst == null)
		{
			Debug.LogError("Error_BattleAwardInstNull");
			return;
		}
		UI_Panel_Battle_BattleAward.inst.int_Leaderboard_FindMe_Ranking_Daily = ranking;
		UI_Panel_Battle_BattleAward.inst.long_Leaderboard_FindMe_Star_Daily = star;
		UI_Panel_Battle_BattleAward.inst.bool_Leaderboard_FindMe_Flag_Daily = true;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00021408 File Offset: 0x0001F608
	public void Thread_UpdateChildPanel_AboutLeaderboard_TryUpload()
	{
		LanguageText languageText = LanguageText.Inst;
		LanguageText.BattleAward battleAward = languageText.battleAward;
		this.text_Leaderboard_BigTitle_Mode.text = battleAward.aboutLeaderboard_BigTitle;
		this.text_Leaderboard_SmallTitle_Mode.text = languageText.main_Modes.mode_Singles[(int)TempData.inst.modeType].titleName + " - " + DataBase.Inst.DataPlayerModels[TempData.inst.jobId].Language_JobName;
		this.text_Leaderboard_New_OrNot_Mode.text = "-";
		this.text_Leaderboard_Best_Title_Mode.text = "-";
		this.text_Leaderboard_Best_Info_Mode.text = battleAward.aboutLeaderboard_Uploading.ReplaceLineBreak();
		this.button_Retry_Mode.enabled = true;
		if (TempData.inst.modeType == EnumModeType.NORMAL)
		{
			this.objMode.SetActive(false);
		}
		if (!TempData.inst.daily_Open)
		{
			this.objDaily.SetActive(false);
		}
		else
		{
			this.objDaily.SetActive(true);
			this.text_Leaderboard_BigTitle_Daily.text = languageText.dailyChallenge.dailyChallenge;
			this.text_Leaderboard_SmallTitle_Daily.text = NetworkTime.GetString_TimeWithDayIndex(TempData.inst.daily_Index);
			this.text_Leaderboard_New_OrNot_Daily.text = "-";
			this.text_Leaderboard_Best_Title_Daily.text = "-";
			this.text_Leaderboard_Best_Info_Daily.text = battleAward.aboutLeaderboard_Uploading.ReplaceLineBreak();
			this.button_Retry_Daily.enabled = true;
		}
		SqlManager.inst.Thread_BattleEnd();
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00021579 File Offset: 0x0001F779
	public void Button_Retry()
	{
		if (this.int_Leaderboard_FindMe_Ranking_Mode > 0 && this.int_Leaderboard_FindMe_Ranking_Daily > 0)
		{
			Debug.LogError("Error_不该重新尝试");
			return;
		}
		if (!Setting.Inst.Option_Network)
		{
			Setting.Inst.setBools[15] = true;
		}
		this.Thread_UpdateChildPanel_AboutLeaderboard_TryUpload();
	}

	// Token: 0x040004A4 RID: 1188
	public static UI_Panel_Battle_BattleAward inst;

	// Token: 0x040004A5 RID: 1189
	[SerializeField]
	private bool awarded;

	// Token: 0x040004A6 RID: 1190
	[SerializeField]
	private Text lang_Title;

	// Token: 0x040004A7 RID: 1191
	[SerializeField]
	private Text lang_YouHaveGot;

	// Token: 0x040004A8 RID: 1192
	[SerializeField]
	private Text lang_FinishInfo;

	// Token: 0x040004A9 RID: 1193
	[SerializeField]
	private Text text_StarNum;

	// Token: 0x040004AA RID: 1194
	[SerializeField]
	private Text lang_Confirm;

	// Token: 0x040004AB RID: 1195
	[SerializeField]
	private Text lang_Restart;

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private RectTransform rectStar;

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private UI_Icon_GeometryCoin_InAward geometryCoin_InAward;

	// Token: 0x040004AE RID: 1198
	[Header("AboutLeaderboard Mode")]
	[SerializeField]
	private GameObject objMode;

	// Token: 0x040004AF RID: 1199
	[SerializeField]
	private Text text_Leaderboard_BigTitle_Mode;

	// Token: 0x040004B0 RID: 1200
	[SerializeField]
	private Text text_Leaderboard_SmallTitle_Mode;

	// Token: 0x040004B1 RID: 1201
	[SerializeField]
	private Text text_Leaderboard_New_OrNot_Mode;

	// Token: 0x040004B2 RID: 1202
	[SerializeField]
	private Text text_Leaderboard_Best_Title_Mode;

	// Token: 0x040004B3 RID: 1203
	[SerializeField]
	private Text text_Leaderboard_Best_Info_Mode;

	// Token: 0x040004B4 RID: 1204
	[SerializeField]
	private Button button_Retry_Mode;

	// Token: 0x040004B5 RID: 1205
	[Header("AboutLeaderboard Daily")]
	[SerializeField]
	private GameObject objDaily;

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private Text text_Leaderboard_BigTitle_Daily;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	private Text text_Leaderboard_SmallTitle_Daily;

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	private Text text_Leaderboard_New_OrNot_Daily;

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	private Text text_Leaderboard_Best_Title_Daily;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	private Text text_Leaderboard_Best_Info_Daily;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private Button button_Retry_Daily;

	// Token: 0x040004BC RID: 1212
	[Header("Find Me Mode")]
	[SerializeField]
	private bool bool_Leaderboard_FindMe_Flag_Mode;

	// Token: 0x040004BD RID: 1213
	[SerializeField]
	private int int_Leaderboard_FindMe_Ranking_Mode;

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private long long_Leaderboard_FindMe_Star_Mode;

	// Token: 0x040004BF RID: 1215
	[Header("Find Me Daily")]
	[SerializeField]
	private bool bool_Leaderboard_FindMe_Flag_Daily;

	// Token: 0x040004C0 RID: 1216
	[SerializeField]
	private int int_Leaderboard_FindMe_Ranking_Daily;

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	private long long_Leaderboard_FindMe_Star_Daily;

	// Token: 0x040004C2 RID: 1218
	[Header("Error_Mode")]
	[SerializeField]
	private int int_Leaderboard_ErrorState_Mode;

	// Token: 0x040004C3 RID: 1219
	[SerializeField]
	private bool bool_Leaderboard_FlagToUpdate_Mode;

	// Token: 0x040004C4 RID: 1220
	[Header("Error_Daily")]
	[SerializeField]
	private int int_Leaderboard_ErrorState_Daily;

	// Token: 0x040004C5 RID: 1221
	[SerializeField]
	private bool bool_Leaderboard_FlagToUpdate_Daily;

	// Token: 0x040004C6 RID: 1222
	[SerializeField]
	private int int_Leaderboard_UploadSuccessState_Mode = -1;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	private int int_Leaderboard_UploadSuccessState_Daily = -1;
}
