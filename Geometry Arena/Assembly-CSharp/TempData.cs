using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000039 RID: 57
[Serializable]
public class TempData : MonoBehaviour
{
	// Token: 0x06000261 RID: 609 RVA: 0x0000E05D File Offset: 0x0000C25D
	private void Awake()
	{
		TempData.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
		if (SaveFile.IsNewSaveSlot())
		{
			this.NewSaveSlot();
			return;
		}
		this.LoadSaveSlot();
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000E084 File Offset: 0x0000C284
	private void Update()
	{
		if (!this.ifThreadFinish)
		{
			return;
		}
		if (this.timeDateUpdateTimeLeft <= 0f)
		{
			this.ifThreadFinish = false;
			this.Update_NetworkTime();
			this.Update_DetectDayIndex();
			this.timeDateUpdateTimeLeft = this.settings_CountDownTimeMax;
			return;
		}
		this.timeDateUpdateTimeLeft -= Time.unscaledDeltaTime;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000E0D9 File Offset: 0x0000C2D9
	public void NTP_ThreadFinish_Error()
	{
		this.ifThreadFinish = true;
		this.timeDateUpdateTimeLeft = 0.3f;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000E0ED File Offset: 0x0000C2ED
	public void NTP_ThreadFinish_Success()
	{
		this.ifThreadFinish = true;
		this.timeDateUpdateTimeLeft = this.settings_CountDownTimeMax;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000E102 File Offset: 0x0000C302
	private void Update_NetworkTime()
	{
		new Thread(new ThreadStart(this.networkTime.UpdateTimeData)).Start();
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000E120 File Offset: 0x0000C320
	private void Update_DetectDayIndex()
	{
		if (this.currentSceneType != EnumSceneType.MAINMENU)
		{
			return;
		}
		if (!MainCanvas.inst.panelNewGame.gameObject.activeSelf)
		{
			return;
		}
		if (this.daily_Open && this.networkTime.dayIndex >= 0)
		{
			if (this.daily_Index < this.networkTime.dayIndex)
			{
				UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_NewDay);
				this.DailyChallenge_UpdateWithTodayIndex(false);
			}
			if (this.daily_Index > this.networkTime.dayIndex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error_时间倒流？",
					this.daily_Index,
					">",
					this.networkTime.dayIndex
				}));
				UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_NewDay);
				this.DailyChallenge_UpdateWithTodayIndex(false);
			}
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000E210 File Offset: 0x0000C410
	public static bool GetBool_Stridebreaker()
	{
		return TempData.inst.diffiOptFlag[30];
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000E21F File Offset: 0x0000C41F
	public void NewSaveSlot()
	{
		Debug.Log("TempData_New");
		this.modeType = EnumModeType.NORMAL;
		this.InitDifficultyOpitonFlags();
		this.InitSkillModuleFlags();
		this.NewGame();
		this.battle.UpdateBattleFacs();
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000E250 File Offset: 0x0000C450
	public void LoadSaveSlot()
	{
		Debug.Log("TempData_Load");
		this.InitDifficultyOpitonFlags();
		SaveFile saveFile = GameData.SaveFile;
		this.jobId = saveFile.jobID;
		this.InitSkillModuleFlags();
		this.varColorId = saveFile.colorID;
		this.daily_Open = saveFile.daily_IfOpen;
		this.daily_Index = saveFile.daily_DayIndex;
		EnumModeType enumModeType = saveFile.modeType;
		if (enumModeType == EnumModeType.UNINITED)
		{
			if (saveFile.ifInfinity)
			{
				this.modeType = EnumModeType.ENDLESS;
			}
		}
		else
		{
			this.modeType = saveFile.modeType;
		}
		int num = Mathf.Min(this.diffiOptFlag.Length, saveFile.difficultyOptionFlag.Length);
		for (int i = 0; i < num; i++)
		{
			this.diffiOptFlag[i] = saveFile.difficultyOptionFlag[i];
		}
		int num2 = Mathf.Min(this.diffiOptFlagBackUp.Length, saveFile.difficultyOptionFlagBackUp.Length);
		for (int j = 0; j < num2; j++)
		{
			this.diffiOptFlagBackUp[j] = saveFile.difficultyOptionFlagBackUp[j];
		}
		if (saveFile.skillModuleFlags != null && this.skillModuleFlags != null)
		{
			if (saveFile.version <= 705)
			{
				int num3 = Mathf.Min(this.skillModuleFlags[this.jobId].Length, saveFile.skillModuleFlags.Length);
				for (int k = 0; k < num3; k++)
				{
					this.skillModuleFlags[this.jobId][k] = saveFile.skillModuleFlags[k];
				}
			}
			else
			{
				int num4 = Mathf.Min(this.skillModuleFlags.Length, saveFile.skillModuleFlagsNew.Length);
				for (int l = 0; l < num4; l++)
				{
					if (saveFile.skillModuleFlagsNew != null)
					{
						int num5 = Mathf.Min(this.skillModuleFlags[l].Length, saveFile.skillModuleFlagsNew[l].Length);
						for (int m = 0; m < num5; m++)
						{
							this.skillModuleFlags[l][m] = saveFile.skillModuleFlagsNew[l][m];
						}
					}
				}
			}
		}
		if (saveFile.ifOnBattle)
		{
			this.NewGame();
			if (saveFile.battle_EnterVersion != 0)
			{
				this.battle.battle_EnterVersion = saveFile.battle_EnterVersion;
			}
			else
			{
				this.battle.battle_EnterVersion = 0;
			}
			this.battle.myRandom = new MyRandom();
			if (saveFile.battle_MyRandom != null)
			{
				this.battle.myRandom.ReadFromSaveFile(saveFile.battle_MyRandom);
			}
			if (saveFile.version <= 807)
			{
				this.battle.diffiLevel = GameData.Convert_OldDLtoNewDL(saveFile.battle_DiffiLevel);
			}
			else
			{
				this.battle.diffiLevel = saveFile.battle_DiffiLevel;
			}
			this.battle.Fragment = saveFile.battle_Fragment;
			this.battle.fragmentTotal = saveFile.battle_FragTotal;
			this.battle.FragmentUsed = saveFile.battle_FragUsed;
			this.battle.Score = saveFile.battle_Score;
			this.battle.level = saveFile.battle_Level;
			this.battle.wave = saveFile.battle_Wave;
			this.battle.ListLevelTypes = saveFile.battle_ListLevelTypes;
			this.battle.upgradeShop = new UpgradeShop();
			int num6 = Mathf.Min(this.battle.upgradeShop.upgradeGoods.Length, saveFile.battle_UpgradeShop.upgradeGoods.Length);
			for (int n = 0; n < num6; n++)
			{
				this.battle.upgradeShop.upgradeGoods[n] = saveFile.battle_UpgradeShop.upgradeGoods[n];
			}
			this.battle.upgradeShop_IfLocked = saveFile.upgradeShop_Locked;
			this.battle.upgradeShop_RefreshPrice = saveFile.upgradeShop_RefreshCost;
			this.battle.upgradeShop_RefreshFreeChance = saveFile.upgradeShop_RefreshFreeChance;
			this.battle.listUpgradeInt = saveFile.battle_ListUpgradeInt;
			Battle.inst.GetFactorMultis_Upgrates_CurBattle();
			this.battle.UpdateBattleFacs();
			if (saveFile.DIY_EnemyColorBool)
			{
				this.battle.DIY_EnemyColor = true;
				this.battle.levelColor = Color.HSVToRGB((float)saveFile.DIY_EnemyColorFloat, 0.8f, 0.9f);
			}
			if (saveFile.DIY_BulletAlphaBool)
			{
				this.battle.DIY_BulletAlpha_BoolFlag = true;
				this.battle.DIY_BulletAlpha_Float = (float)saveFile.DIY_BulletAlphaFloat;
			}
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000E65C File Offset: 0x0000C85C
	private void InitDifficultyOpitonFlags()
	{
		int num = DataBase.Inst.Data_DifficultyOptions.Length;
		this.diffiOptFlag = new bool[num];
		this.diffiOptFlagBackUp = new bool[num];
		for (int i = 0; i < num; i++)
		{
			this.diffiOptFlag[i] = false;
		}
		this.diffiOptFlag[0] = true;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000E6AC File Offset: 0x0000C8AC
	public void NewGame()
	{
		this.battle = new Battle();
		this.battle.InitNewGame();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000E6C4 File Offset: 0x0000C8C4
	public int GetDiffiOptionNum()
	{
		int num = 0;
		bool[] array = this.diffiOptFlag;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				num++;
			}
		}
		if (this.diffiOptFlag[0])
		{
			num -= 2;
		}
		return num;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000E700 File Offset: 0x0000C900
	public void InitSkillModuleFlags()
	{
		if (this.jobId < 0)
		{
			return;
		}
		PlayerModel[] dataPlayerModels = DataBase.Inst.DataPlayerModels;
		this.skillModuleFlags = new bool[dataPlayerModels.Length][];
		for (int i = 0; i < this.skillModuleFlags.Length; i++)
		{
			int skillModule_MaxEffectID = DataBase.Inst.DataPlayerModels[i].skillModule_MaxEffectID;
			this.skillModuleFlags[i] = new bool[skillModule_MaxEffectID + 1];
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000E765 File Offset: 0x0000C965
	public bool GetBool_SkillModuleOpenFlag(int effectID)
	{
		return effectID < this.skillModuleFlags.Length && this.skillModuleFlags[this.jobId][effectID];
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000E784 File Offset: 0x0000C984
	public void DailyChallenge_UpdateWithTodayIndex(bool ifOpenTip = true)
	{
		if (this.networkTime.ifError)
		{
			Debug.LogError("Error_无法连接到网络");
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.dailyChallenge.error_CantConnectTimeServer);
			return;
		}
		if (this.networkTime.dayIndex < 0)
		{
			Debug.LogError("Error_每日挑战Index异常");
			return;
		}
		if (ifOpenTip)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_Open);
		}
		if (!this.daily_Open)
		{
			this.diffiOptFlagBackUp = new bool[this.diffiOptFlag.Length];
			for (int i = 0; i < this.diffiOptFlagBackUp.Length; i++)
			{
				this.diffiOptFlagBackUp[i] = this.diffiOptFlag[i];
			}
		}
		this.daily_Open = true;
		this.daily_Index = this.networkTime.dayIndex;
		this.modeType = EnumModeType.ENDLESS;
		this.DailyChallenge_UpdateWithTodayIndex_Version0();
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000E85C File Offset: 0x0000CA5C
	public void DailyChallenge_UpdateWithTodayIndex_Version0()
	{
		int dayIndex = this.networkTime.dayIndex;
		int num = 0;
		Random.InitState(dayIndex);
		int no = Random.Range(0, 12);
		float num2 = Random.Range(0.5f, 1.5f);
		DifficultyOption[] data_DifficultyOptions = DataBase.Inst.Data_DifficultyOptions;
		for (int i = 0; i < this.diffiOptFlag.Length; i++)
		{
			this.diffiOptFlag[i] = false;
		}
		int num3 = 26;
		if (dayIndex > 132)
		{
			num3 = this.diffiOptFlag.Length;
		}
		for (int j = 0; j < num3; j++)
		{
			float num4 = Random.Range(0f, 1f);
			float num5 = data_DifficultyOptions[j].dailyChances[num];
			this.diffiOptFlag[j] = (num4 < num5 * num2);
		}
		MainCanvas.inst.Button_SelectJob(no);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000E928 File Offset: 0x0000CB28
	public void DailyChallenge_Close(bool ifOpenTip = true)
	{
		if (ifOpenTip)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_Close);
		}
		this.daily_Open = false;
		this.daily_Index = -1;
		for (int i = 0; i < Mathf.Min(this.diffiOptFlag.Length, this.diffiOptFlagBackUp.Length); i++)
		{
			this.diffiOptFlag[i] = this.diffiOptFlagBackUp[i];
		}
		MainCanvas.inst.Button_SelectJob(this.jobId);
	}

	// Token: 0x0400021C RID: 540
	public static TempData inst;

	// Token: 0x0400021D RID: 541
	public Battle battle;

	// Token: 0x0400021E RID: 542
	public EnumModeType modeType;

	// Token: 0x0400021F RID: 543
	public bool[] diffiOptFlag;

	// Token: 0x04000220 RID: 544
	public bool[] diffiOptFlagBackUp = new bool[0];

	// Token: 0x04000221 RID: 545
	public bool[][] skillModuleFlags;

	// Token: 0x04000222 RID: 546
	public int jobId;

	// Token: 0x04000223 RID: 547
	public int varColorId;

	// Token: 0x04000224 RID: 548
	public PlayerPreview playerPreview = new PlayerPreview();

	// Token: 0x04000225 RID: 549
	public EnumSceneType currentSceneType = EnumSceneType.UNINITED;

	// Token: 0x04000226 RID: 550
	[Header("DailyChallenge")]
	public bool daily_Open;

	// Token: 0x04000227 RID: 551
	public int daily_Index = -1;

	// Token: 0x04000228 RID: 552
	[Header("NetworkTime")]
	[SerializeField]
	public NetworkTime networkTime = new NetworkTime();

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private float timeDateUpdateTimeLeft = 1f;

	// Token: 0x0400022A RID: 554
	[SerializeField]
	private float settings_CountDownTimeMax = 3f;

	// Token: 0x0400022B RID: 555
	[SerializeField]
	public bool ifThreadFinish = true;
}
