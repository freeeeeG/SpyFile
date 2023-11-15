using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class LanguageText : ScriptableObject
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000040 RID: 64 RVA: 0x00003AD8 File Offset: 0x00001CD8
	public static LanguageText Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.languageTexts[(int)Setting.Inst.language];
			}
			EnumLanguage language = Setting.Inst.language;
			string path;
			if (language != EnumLanguage.ENGLISH)
			{
				if (language != EnumLanguage.CHINESE_SIM)
				{
					Debug.LogError("未选择语言！返回Main_Eng!");
					path = "Assets/Language/Main_ENG";
				}
				else
				{
					path = "Assets/Language/Main_CHNS";
				}
			}
			else
			{
				path = "Assets/Language/Main_ENG";
			}
			return Resources.Load<LanguageText>(path);
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003B48 File Offset: 0x00001D48
	public string GetFactorName(int no)
	{
		if (no >= 1 && no <= 15)
		{
			return this.factor[no];
		}
		if (no == 70)
		{
			return this.main_RoleSkill;
		}
		if (no == 80)
		{
			return this.main_Color;
		}
		Debug.LogError("FactorName??");
		return "??";
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003B83 File Offset: 0x00001D83
	public string GetFactorNameAbb(int no)
	{
		if (no >= 1 && no <= 15)
		{
			return this.factorAbb[no];
		}
		if (no == 70)
		{
			return this.factorAbbOthers[0];
		}
		if (no == 80)
		{
			return this.factorAbbOthers[1];
		}
		Debug.LogError("FactorName??");
		return "??";
	}

	// Token: 0x0400002F RID: 47
	public UpdateLog asset_UpdateLog;

	// Token: 0x04000030 RID: 48
	public CommonLog asset_ManualLog;

	// Token: 0x04000031 RID: 49
	public CommonLog asset_Credits;

	// Token: 0x04000032 RID: 50
	public LanguageText.StringReplace stringReplace;

	// Token: 0x04000033 RID: 51
	public string player;

	// Token: 0x04000034 RID: 52
	public string enemy;

	// Token: 0x04000035 RID: 53
	[Header("Factor相关")]
	public string[] factor;

	// Token: 0x04000036 RID: 54
	public string[] factorAbb;

	// Token: 0x04000037 RID: 55
	public string[] factorAbbOthers;

	// Token: 0x04000038 RID: 56
	public string[] factorInfinity;

	// Token: 0x04000039 RID: 57
	public string[] battleFactor;

	// Token: 0x0400003A RID: 58
	public string[] battleFactorInfo;

	// Token: 0x0400003B RID: 59
	public LanguageText.Shape[] shapes;

	// Token: 0x0400003C RID: 60
	[Header("Main")]
	public string main_Job;

	// Token: 0x0400003D RID: 61
	public string main_Color;

	// Token: 0x0400003E RID: 62
	public string main_Ability;

	// Token: 0x0400003F RID: 63
	public string main_Talents;

	// Token: 0x04000040 RID: 64
	public string main_DifficultyOptions;

	// Token: 0x04000041 RID: 65
	public string main_RoleSkill;

	// Token: 0x04000042 RID: 66
	public string main_Job_UnlockInfo;

	// Token: 0x04000043 RID: 67
	public string main_Color_UnlockInfo;

	// Token: 0x04000044 RID: 68
	public string talent_Proficiency;

	// Token: 0x04000045 RID: 69
	public string talent_CurLevel;

	// Token: 0x04000046 RID: 70
	public string talent_MaxLevel;

	// Token: 0x04000047 RID: 71
	public string talent_Cost;

	// Token: 0x04000048 RID: 72
	public string talent_Proficiency_Progress;

	// Token: 0x04000049 RID: 73
	public string talent_NextLevelPreview;

	// Token: 0x0400004A RID: 74
	public string talent_AllRoles;

	// Token: 0x0400004B RID: 75
	public string talent_Level;

	// Token: 0x0400004C RID: 76
	public string talent_CurLevelView;

	// Token: 0x0400004D RID: 77
	public string talent_InfinityPre;

	// Token: 0x0400004E RID: 78
	public string[] ability_Desc;

	// Token: 0x0400004F RID: 79
	public string confirm_Yes;

	// Token: 0x04000050 RID: 80
	public string confirm_No;

	// Token: 0x04000051 RID: 81
	public string confirm_Title;

	// Token: 0x04000052 RID: 82
	public string confirm_Confirm;

	// Token: 0x04000053 RID: 83
	public string confirm_Tips;

	// Token: 0x04000054 RID: 84
	public LanguageText.Tutorial tutorial = new LanguageText.Tutorial();

	// Token: 0x04000055 RID: 85
	public LanguageText.Demo demo = new LanguageText.Demo();

	// Token: 0x04000056 RID: 86
	public LanguageText.MainMenu mainMenu = new LanguageText.MainMenu();

	// Token: 0x04000057 RID: 87
	public LanguageText.Lang_UpdateLog main_UpdateLog = new LanguageText.Lang_UpdateLog();

	// Token: 0x04000058 RID: 88
	public LanguageText.Main_Common main_Common = new LanguageText.Main_Common();

	// Token: 0x04000059 RID: 89
	public LanguageText.Modes main_Modes = new LanguageText.Modes();

	// Token: 0x0400005A RID: 90
	public LanguageText.NewGamePanel newGamePanel = new LanguageText.NewGamePanel();

	// Token: 0x0400005B RID: 91
	public LanguageText.PanelRankList rankListPanel = new LanguageText.PanelRankList();

	// Token: 0x0400005C RID: 92
	public LanguageText.Manual manual = new LanguageText.Manual();

	// Token: 0x0400005D RID: 93
	[Header("图鉴")]
	public LanguageText.UpgradeLibrary upgradeLibrary = new LanguageText.UpgradeLibrary();

	// Token: 0x0400005E RID: 94
	public LanguageText.EnemyLibrary enemyLibrary = new LanguageText.EnemyLibrary();

	// Token: 0x0400005F RID: 95
	public LanguageText.BattleItemLibrary battleItemLibrary = new LanguageText.BattleItemLibrary();

	// Token: 0x04000060 RID: 96
	[SerializeField]
	public LanguageText.BulletEffectInfo bulletEffectInfo = new LanguageText.BulletEffectInfo();

	// Token: 0x04000061 RID: 97
	[Header("DailyChallenge")]
	public LanguageText.DailyChallenge dailyChallenge = new LanguageText.DailyChallenge();

	// Token: 0x04000062 RID: 98
	[Header("BattleUI")]
	public string wave;

	// Token: 0x04000063 RID: 99
	public string level_Infinity;

	// Token: 0x04000064 RID: 100
	public string level_Wander;

	// Token: 0x04000065 RID: 101
	public string[] waveStage = new string[3];

	// Token: 0x04000066 RID: 102
	public string score;

	// Token: 0x04000067 RID: 103
	public string fragment;

	// Token: 0x04000068 RID: 104
	public string shop;

	// Token: 0x04000069 RID: 105
	public string challenge;

	// Token: 0x0400006A RID: 106
	public string fight;

	// Token: 0x0400006B RID: 107
	public string[] shapeThemes;

	// Token: 0x0400006C RID: 108
	[SerializeField]
	public LanguageText.PauseMenu pauseMenu = new LanguageText.PauseMenu();

	// Token: 0x0400006D RID: 109
	[SerializeField]
	public LanguageText.ChallengeMenu challengeMenu = new LanguageText.ChallengeMenu();

	// Token: 0x0400006E RID: 110
	[SerializeField]
	public LanguageText.ShopMenu shopMenu = new LanguageText.ShopMenu();

	// Token: 0x0400006F RID: 111
	[Header("Skill")]
	public LanguageText.Skill skill = new LanguageText.Skill();

	// Token: 0x04000070 RID: 112
	[Header("Common")]
	public string[] ranks = new string[3];

	// Token: 0x04000071 RID: 113
	public LanguageText.BattleAward battleAward = new LanguageText.BattleAward();

	// Token: 0x04000072 RID: 114
	public LanguageText.FloatText floatText = new LanguageText.FloatText();

	// Token: 0x04000073 RID: 115
	public LanguageText.Input input = new LanguageText.Input();

	// Token: 0x04000074 RID: 116
	public LanguageText.PanelSetting panelSetting = new LanguageText.PanelSetting();

	// Token: 0x04000075 RID: 117
	public string newTip;

	// Token: 0x04000076 RID: 118
	public string autoFire;

	// Token: 0x04000077 RID: 119
	[Header("ToolTip")]
	public LanguageText.ToolTip_Ability toolTip_Ability = new LanguageText.ToolTip_Ability();

	// Token: 0x04000078 RID: 120
	public LanguageText.ToolTip_FactorBattle toolTip_FactorBattle = new LanguageText.ToolTip_FactorBattle();

	// Token: 0x04000079 RID: 121
	public LanguageText.Tooltip_GeometryCoin tooltip_GeometryCoin = new LanguageText.Tooltip_GeometryCoin();

	// Token: 0x0400007A RID: 122
	public string[] toolTip_TipStringsHead = new string[11];

	// Token: 0x0400007B RID: 123
	public string[] toolTip_TipStrings = new string[0];

	// Token: 0x0400007C RID: 124
	public LanguageText.Optimization optimization = new LanguageText.Optimization();

	// Token: 0x0400007D RID: 125
	[Header("Rune")]
	public LanguageText.RuneInfo runeInfo = new LanguageText.RuneInfo();

	// Token: 0x0400007E RID: 126
	[Header("技能模块")]
	public LanguageText.SkillModule skillModule = new LanguageText.SkillModule();

	// Token: 0x0400007F RID: 127
	[Header("DIY")]
	public LanguageText.DIY diy = new LanguageText.DIY();

	// Token: 0x02000100 RID: 256
	[Serializable]
	public class BulletEffectInfo
	{
		// Token: 0x040007B8 RID: 1976
		public string growing;

		// Token: 0x040007B9 RID: 1977
		public string sudden;
	}

	// Token: 0x02000101 RID: 257
	[Serializable]
	public class PauseMenu
	{
		// Token: 0x040007BA RID: 1978
		public string title_Pause;

		// Token: 0x040007BB RID: 1979
		public string continueGame;

		// Token: 0x040007BC RID: 1980
		public string giveUp;

		// Token: 0x040007BD RID: 1981
		public string saveAndExit;

		// Token: 0x040007BE RID: 1982
		public string confirm_GiveUp;

		// Token: 0x040007BF RID: 1983
		public string confirm_Save;

		// Token: 0x040007C0 RID: 1984
		public string info_CantSave;

		// Token: 0x040007C1 RID: 1985
		public string info_CantSave_PlayerDie;

		// Token: 0x040007C2 RID: 1986
		public string info_DeleteUpgrade;

		// Token: 0x040007C3 RID: 1987
		public string info_Tooltip_Upgrade_DeleteTip;
	}

	// Token: 0x02000102 RID: 258
	[Serializable]
	public class ChallengeMenu
	{
		// Token: 0x040007C4 RID: 1988
		public string panel_challenge;

		// Token: 0x040007C5 RID: 1989
		public string title_CurrentDiffilevel;

		// Token: 0x040007C6 RID: 1990
		public string title_DiffiLevelUpPreview;

		// Token: 0x040007C7 RID: 1991
		public string button_LevelUp;

		// Token: 0x040007C8 RID: 1992
		public string button_Return;

		// Token: 0x040007C9 RID: 1993
		public string button_LevelMax;

		// Token: 0x040007CA RID: 1994
		public string info_NoEffect;
	}

	// Token: 0x02000103 RID: 259
	[Serializable]
	public class ShopMenu
	{
		// Token: 0x040007CB RID: 1995
		public string panel_Shop;

		// Token: 0x040007CC RID: 1996
		public string upgrade;

		// Token: 0x040007CD RID: 1997
		public string upgrade_tipN;

		// Token: 0x040007CE RID: 1998
		public string info_Price;

		// Token: 0x040007CF RID: 1999
		public string button_Buy;

		// Token: 0x040007D0 RID: 2000
		public string button_Return;

		// Token: 0x040007D1 RID: 2001
		public string soldOut_Title;

		// Token: 0x040007D2 RID: 2002
		public string soldOut_Info;

		// Token: 0x040007D3 RID: 2003
		public string smallTitle_Ability;

		// Token: 0x040007D4 RID: 2004
		public string smallTitle_Special;

		// Token: 0x040007D5 RID: 2005
		public string smallTitle_Bullet;

		// Token: 0x040007D6 RID: 2006
		public string refresh;

		// Token: 0x040007D7 RID: 2007
		public string lock_Locked;

		// Token: 0x040007D8 RID: 2008
		public string lock_Unlocked;

		// Token: 0x040007D9 RID: 2009
		public string refresh_FreeOnce;

		// Token: 0x040007DA RID: 2010
		public string mysteryBag_Get;

		// Token: 0x040007DB RID: 2011
		public string mysteryBag_Refuse;

		// Token: 0x040007DC RID: 2012
		public string mysteryBag_Title;

		// Token: 0x040007DD RID: 2013
		public string mysteryBag_Tip;
	}

	// Token: 0x02000104 RID: 260
	[Serializable]
	public class FloatText
	{
		// Token: 0x040007DE RID: 2014
		public string talentLevelUp;

		// Token: 0x040007DF RID: 2015
		public string talent_LackOfStar;

		// Token: 0x040007E0 RID: 2016
		public string talent_LevelMax;

		// Token: 0x040007E1 RID: 2017
		public string lackOfGeometryCoin;

		// Token: 0x040007E2 RID: 2018
		public string tip_EnterRestStage;

		// Token: 0x040007E3 RID: 2019
		public string tip_EnterWaveStage;

		// Token: 0x040007E4 RID: 2020
		public string diffiOptionOpen;

		// Token: 0x040007E5 RID: 2021
		public string diffiOptionClose;

		// Token: 0x040007E6 RID: 2022
		public string skillModuleOpen;

		// Token: 0x040007E7 RID: 2023
		public string skillModuleClose;

		// Token: 0x040007E8 RID: 2024
		public string upgrade_Gain;

		// Token: 0x040007E9 RID: 2025
		public string upgrade_Delete;

		// Token: 0x040007EA RID: 2026
		public string upgrade_SoldOut;

		// Token: 0x040007EB RID: 2027
		public string lifeRecover;

		// Token: 0x040007EC RID: 2028
		public string upgrade_LackOfFragment;

		// Token: 0x040007ED RID: 2029
		public string stage_Enter;

		// Token: 0x040007EE RID: 2030
		public string shop_Refresh;

		// Token: 0x040007EF RID: 2031
		public string wander_LevelUp;

		// Token: 0x040007F0 RID: 2032
		public string diffiLevelUp;

		// Token: 0x040007F1 RID: 2033
		public string diffiLevelDown;

		// Token: 0x040007F2 RID: 2034
		public string diffiLevelMax;

		// Token: 0x040007F3 RID: 2035
		public string battleItemActive;

		// Token: 0x040007F4 RID: 2036
		public string rune_BuyGeometryCoin;

		// Token: 0x040007F5 RID: 2037
		public string rune_BuyRune;

		// Token: 0x040007F6 RID: 2038
		public string rune_RecycleRune;

		// Token: 0x040007F7 RID: 2039
		public string rune_Refresh;

		// Token: 0x040007F8 RID: 2040
		public string upgradeStore_Locked;

		// Token: 0x040007F9 RID: 2041
		public string upgradeStore_Unlocked;

		// Token: 0x040007FA RID: 2042
		public string dailyChallenge_Open;

		// Token: 0x040007FB RID: 2043
		public string dailyChallenge_Close;

		// Token: 0x040007FC RID: 2044
		public string dailyChallenge_NewDay;

		// Token: 0x040007FD RID: 2045
		public string dailyChallenge_LockRole;

		// Token: 0x040007FE RID: 2046
		public string dailyChallenge_LockDO;

		// Token: 0x040007FF RID: 2047
		public string dailyChallenge_LockMode;
	}

	// Token: 0x02000105 RID: 261
	[Serializable]
	public class DailyChallenge
	{
		// Token: 0x04000800 RID: 2048
		public string daily;

		// Token: 0x04000801 RID: 2049
		public string dailyChallenge;

		// Token: 0x04000802 RID: 2050
		public string dailyAward;

		// Token: 0x04000803 RID: 2051
		public string dailyAwardTotal;

		// Token: 0x04000804 RID: 2052
		public string currentDate;

		// Token: 0x04000805 RID: 2053
		public string nextRefresh;

		// Token: 0x04000806 RID: 2054
		public string timeCountDown;

		// Token: 0x04000807 RID: 2055
		public string error_CantConnectTimeServer;

		// Token: 0x04000808 RID: 2056
		public string ranking;

		// Token: 0x04000809 RID: 2057
		public string awardInfo;

		// Token: 0x0400080A RID: 2058
		public string unawardedList;

		// Token: 0x0400080B RID: 2059
		public string noAwards;

		// Token: 0x0400080C RID: 2060
		public string awardPanelTitle;
	}

	// Token: 0x02000106 RID: 262
	[Serializable]
	public class Input
	{
		// Token: 0x0400080D RID: 2061
		public string key_Move;

		// Token: 0x0400080E RID: 2062
		public string key_Shoot;

		// Token: 0x0400080F RID: 2063
		public string key_Skill;
	}

	// Token: 0x02000107 RID: 263
	[Serializable]
	public class Skill
	{
		// Token: 0x04000810 RID: 2064
		public string[] skillType = new string[3];
	}

	// Token: 0x02000108 RID: 264
	[Serializable]
	public class BattleAward
	{
		// Token: 0x04000811 RID: 2065
		public string title_GameOver;

		// Token: 0x04000812 RID: 2066
		public string title_Finish;

		// Token: 0x04000813 RID: 2067
		public string info_Finish;

		// Token: 0x04000814 RID: 2068
		public string youHaveGot;

		// Token: 0x04000815 RID: 2069
		public string button_Restart;

		// Token: 0x04000816 RID: 2070
		public string button_ReturnToMainMenu;

		// Token: 0x04000817 RID: 2071
		public string aboutLeaderboard_BigTitle;

		// Token: 0x04000818 RID: 2072
		public string aboutLeaderboard_NewRecord;

		// Token: 0x04000819 RID: 2073
		public string aboutLeaderboard_NoBrokenRecords;

		// Token: 0x0400081A RID: 2074
		public string aboutLeaderboard_MyHighestRecord;

		// Token: 0x0400081B RID: 2075
		public string aboutLeaderboard_Ranking;

		// Token: 0x0400081C RID: 2076
		public string aboutLeaderboard_Stars;

		// Token: 0x0400081D RID: 2077
		public string aboutLeaderboard_Uploading;

		// Token: 0x0400081E RID: 2078
		public string aboutLeaderboard_Retry;

		// Token: 0x0400081F RID: 2079
		public string aboutLeaderboard_OpenNetwordAndRetry;

		// Token: 0x04000820 RID: 2080
		public string[] aboutLeaderboard_ErrorTips;

		// Token: 0x04000821 RID: 2081
		public string aboutLeaderboard_Error_Mode_VersionTooOld;

		// Token: 0x04000822 RID: 2082
		public string aboutLeaderboard_Error_Daily_TimeOut;
	}

	// Token: 0x02000109 RID: 265
	[Serializable]
	public class MainMenu
	{
		// Token: 0x04000823 RID: 2083
		public string main_NewGame;

		// Token: 0x04000824 RID: 2084
		public string main_GiveUp;

		// Token: 0x04000825 RID: 2085
		public string main_ContinueGame;

		// Token: 0x04000826 RID: 2086
		public string main_QuitApp;

		// Token: 0x04000827 RID: 2087
		public string main_Setting;

		// Token: 0x04000828 RID: 2088
		public string main_UpdateLog;

		// Token: 0x04000829 RID: 2089
		public string main_Manual;

		// Token: 0x0400082A RID: 2090
		public string main_Credits;

		// Token: 0x0400082B RID: 2091
		public string main_RankLists;

		// Token: 0x0400082C RID: 2092
		public string main_UpgradeList;

		// Token: 0x0400082D RID: 2093
		public string preview_StartGame;

		// Token: 0x0400082E RID: 2094
		public string preview_Return;
	}

	// Token: 0x0200010A RID: 266
	[Serializable]
	public class Lang_UpdateLog
	{
		// Token: 0x0400082F RID: 2095
		public string title;

		// Token: 0x04000830 RID: 2096
		public string previousPage;

		// Token: 0x04000831 RID: 2097
		public string nextPage;

		// Token: 0x04000832 RID: 2098
		public string close;

		// Token: 0x04000833 RID: 2099
		public string firstPage;

		// Token: 0x04000834 RID: 2100
		public string lastPage;
	}

	// Token: 0x0200010B RID: 267
	[Serializable]
	public class Main_Common
	{
		// Token: 0x04000835 RID: 2101
		public string option_On;

		// Token: 0x04000836 RID: 2102
		public string option_Off;

		// Token: 0x04000837 RID: 2103
		public string option_Chosen;

		// Token: 0x04000838 RID: 2104
		public string optino_Unchosen;

		// Token: 0x04000839 RID: 2105
		public string locked;

		// Token: 0x0400083A RID: 2106
		public string tip_ShiftBuy10times;

		// Token: 0x0400083B RID: 2107
		public string tip_CtrlBuy100times;
	}

	// Token: 0x0200010C RID: 268
	[Serializable]
	public class Modes
	{
		// Token: 0x0400083C RID: 2108
		[SerializeField]
		public LanguageText.Mode_Single[] mode_Singles = new LanguageText.Mode_Single[0];
	}

	// Token: 0x0200010D RID: 269
	[Serializable]
	public class Mode_Single
	{
		// Token: 0x0400083D RID: 2109
		public string titleName;

		// Token: 0x0400083E RID: 2110
		public string unlockTip;

		// Token: 0x0400083F RID: 2111
		public string info;
	}

	// Token: 0x0200010E RID: 270
	[Serializable]
	public class ToolTip_Ability
	{
		// Token: 0x04000840 RID: 2112
		public string main_Basic;

		// Token: 0x04000841 RID: 2113
		public string main_Role;

		// Token: 0x04000842 RID: 2114
		public string main_Color;

		// Token: 0x04000843 RID: 2115
		public string main_Talent;

		// Token: 0x04000844 RID: 2116
		public string main_Do;

		// Token: 0x04000845 RID: 2117
		public string main_Rune;

		// Token: 0x04000846 RID: 2118
		public string main_FromSkillModule;

		// Token: 0x04000847 RID: 2119
		public string battle_Init;

		// Token: 0x04000848 RID: 2120
		public string battle_Upgrade;

		// Token: 0x04000849 RID: 2121
		public string battle_Skill;

		// Token: 0x0400084A RID: 2122
		public string battle_Buff;

		// Token: 0x0400084B RID: 2123
		public string battle_Buff_LayerNumber;

		// Token: 0x0400084C RID: 2124
		public string battle_Buff_CurrentView;

		// Token: 0x0400084D RID: 2125
		public string common_From;

		// Token: 0x0400084E RID: 2126
		public string main_InfinityInfo;
	}

	// Token: 0x0200010F RID: 271
	[Serializable]
	public class NewGamePanel
	{
		// Token: 0x0400084F RID: 2127
		public string title_Skill;

		// Token: 0x04000850 RID: 2128
		public string title_Mode;

		// Token: 0x04000851 RID: 2129
		public string selectAll;

		// Token: 0x04000852 RID: 2130
		public string clearAll;
	}

	// Token: 0x02000110 RID: 272
	[Serializable]
	public class ToolTip_FactorBattle
	{
		// Token: 0x04000853 RID: 2131
		public string basic;

		// Token: 0x04000854 RID: 2132
		public string fromWaveLevel;

		// Token: 0x04000855 RID: 2133
		public string fromDiffiOpt;

		// Token: 0x04000856 RID: 2134
		public string fromDiffiLevel;

		// Token: 0x04000857 RID: 2135
		public string upgradeBasicWeight;

		// Token: 0x04000858 RID: 2136
		public string upgradeTotalWeight;

		// Token: 0x04000859 RID: 2137
		public string allUpgradeTotalWeight;
	}

	// Token: 0x02000111 RID: 273
	[Serializable]
	public class Tooltip_GeometryCoin
	{
		// Token: 0x0400085A RID: 2138
		public string geometryCoin;

		// Token: 0x0400085B RID: 2139
		public string gainBonus;

		// Token: 0x0400085C RID: 2140
		public string basic;

		// Token: 0x0400085D RID: 2141
		public string fromMastery;

		// Token: 0x0400085E RID: 2142
		public string fromMode_Endless;

		// Token: 0x0400085F RID: 2143
		public string fromMode_Wander;

		// Token: 0x04000860 RID: 2144
		public string fromDaily;
	}

	// Token: 0x02000112 RID: 274
	[Serializable]
	public class PanelSetting
	{
		// Token: 0x04000861 RID: 2145
		public string[] titles = new string[3];

		// Token: 0x04000862 RID: 2146
		public string confirm;

		// Token: 0x04000863 RID: 2147
		public string resolution;

		// Token: 0x04000864 RID: 2148
		public string[] boolInfos = new string[6];

		// Token: 0x04000865 RID: 2149
		public string[] setToolTipInfos = new string[16];
	}

	// Token: 0x02000113 RID: 275
	[Serializable]
	public class PanelRankList
	{
		// Token: 0x04000866 RID: 2150
		public string[] titles = new string[3];

		// Token: 0x04000867 RID: 2151
		public string close;

		// Token: 0x04000868 RID: 2152
		public string refresh;

		// Token: 0x04000869 RID: 2153
		public string[] titleRowTexts_Infinity = new string[12];

		// Token: 0x0400086A RID: 2154
		public string tips_Loading;

		// Token: 0x0400086B RID: 2155
		public string tips_Flag1NetWorkClose;

		// Token: 0x0400086C RID: 2156
		public string tips_Flag2ConnectError;

		// Token: 0x0400086D RID: 2157
		public string tips_Flag3DataError;

		// Token: 0x0400086E RID: 2158
		public string tips_Flag4SteamError;

		// Token: 0x0400086F RID: 2159
		public string tips_ComeSoon;

		// Token: 0x04000870 RID: 2160
		public string myRank_Text;

		// Token: 0x04000871 RID: 2161
		public string myRank_Empty;
	}

	// Token: 0x02000114 RID: 276
	[Serializable]
	public class Optimization
	{
		// Token: 0x04000872 RID: 2162
		public string bulletOptmization_On;

		// Token: 0x04000873 RID: 2163
		public string bulletOptmization_Off;

		// Token: 0x04000874 RID: 2164
		public string bulletOptmization_OpenTip;

		// Token: 0x04000875 RID: 2165
		public string bulletOptmization_FactorTip;

		// Token: 0x04000876 RID: 2166
		public string bulletOptmization_CurrentActual;
	}

	// Token: 0x02000115 RID: 277
	[Serializable]
	public class UpgradeLibrary
	{
		// Token: 0x04000877 RID: 2167
		public string title;

		// Token: 0x04000878 RID: 2168
		public string multiTip;

		// Token: 0x04000879 RID: 2169
		public string manualTip;
	}

	// Token: 0x02000116 RID: 278
	[Serializable]
	public class EnemyLibrary
	{
		// Token: 0x0400087A RID: 2170
		public string title;

		// Token: 0x0400087B RID: 2171
		public string multiTip;

		// Token: 0x0400087C RID: 2172
		public string manualTip;

		// Token: 0x0400087D RID: 2173
		public string enemyInfo_Shape;

		// Token: 0x0400087E RID: 2174
		public string enemyInfo_Rank;

		// Token: 0x0400087F RID: 2175
		public string enemyInfo_Life;

		// Token: 0x04000880 RID: 2176
		public string enemyInfo_Size;

		// Token: 0x04000881 RID: 2177
		public string enemyInfo_Speed;

		// Token: 0x04000882 RID: 2178
		public string enemyInfo_MoveType;

		// Token: 0x04000883 RID: 2179
		public string[] enemyRanks;

		// Token: 0x04000884 RID: 2180
		public string[] enemyMoveType;

		// Token: 0x04000885 RID: 2181
		public string[] enemySplitType;

		// Token: 0x04000886 RID: 2182
		public string[] enemySummonType;
	}

	// Token: 0x02000117 RID: 279
	[Serializable]
	public class Manual
	{
		// Token: 0x04000887 RID: 2183
		public string title;

		// Token: 0x04000888 RID: 2184
		public string skipTutorial;
	}

	// Token: 0x02000118 RID: 280
	[Serializable]
	public class BattleItemLibrary
	{
		// Token: 0x04000889 RID: 2185
		public string title;

		// Token: 0x0400088A RID: 2186
		public string multiTip;

		// Token: 0x0400088B RID: 2187
		public string manualTip;
	}

	// Token: 0x02000119 RID: 281
	[Serializable]
	public class Shape
	{
		// Token: 0x0400088C RID: 2188
		public string shapeName;

		// Token: 0x0400088D RID: 2189
		public string shapeInfo;
	}

	// Token: 0x0200011A RID: 282
	[Serializable]
	public class RuneInfo
	{
		// Token: 0x0400088E RID: 2190
		public string rune;

		// Token: 0x0400088F RID: 2191
		public string prop_OriginUpgrade;

		// Token: 0x04000890 RID: 2192
		public string prop_UpgradeType;

		// Token: 0x04000891 RID: 2193
		public string prop_BlockUpgradeType;

		// Token: 0x04000892 RID: 2194
		public string runeList;

		// Token: 0x04000893 RID: 2195
		public string title_RuneEquip;

		// Token: 0x04000894 RID: 2196
		public string title_RuneSynthesis;

		// Token: 0x04000895 RID: 2197
		public string title_RuneDetail;

		// Token: 0x04000896 RID: 2198
		public string runeEquip;

		// Token: 0x04000897 RID: 2199
		public string runeSynthesis;

		// Token: 0x04000898 RID: 2200
		public string button_TakeOff;

		// Token: 0x04000899 RID: 2201
		public string button_Clear;

		// Token: 0x0400089A RID: 2202
		public string button_Synthesize;

		// Token: 0x0400089B RID: 2203
		public string button_ReFuse;

		// Token: 0x0400089C RID: 2204
		public string button_DecideLater;

		// Token: 0x0400089D RID: 2205
		public string button_BuyNewRune;

		// Token: 0x0400089E RID: 2206
		public string button_BuyNewBigRune;

		// Token: 0x0400089F RID: 2207
		public string slot_CurrentSlot;

		// Token: 0x040008A0 RID: 2208
		public string slot_CurrentSlotEmpty;

		// Token: 0x040008A1 RID: 2209
		public string outButton_ClickTip;

		// Token: 0x040008A2 RID: 2210
		public string outButton_Locked;

		// Token: 0x040008A3 RID: 2211
		public string outButton_LockTip;

		// Token: 0x040008A4 RID: 2212
		public string outButton_Info;

		// Token: 0x040008A5 RID: 2213
		public string outButton_CurrentRune;

		// Token: 0x040008A6 RID: 2214
		public string outButton_CurrentRuneEmpty;

		// Token: 0x040008A7 RID: 2215
		public string newRune_Title;

		// Token: 0x040008A8 RID: 2216
		public string newRune_Confirm;

		// Token: 0x040008A9 RID: 2217
		public string[] synResult_SmallTitles;

		// Token: 0x040008AA RID: 2218
		public string synResult_Choose;

		// Token: 0x040008AB RID: 2219
		public string synResult_Tip;

		// Token: 0x040008AC RID: 2220
		public string synResult_BigTitle;

		// Token: 0x040008AD RID: 2221
		public string sort_OpenButton;

		// Token: 0x040008AE RID: 2222
		public string[] sort_ChooseButtons;

		// Token: 0x040008AF RID: 2223
		public string sort_PanelTitle;

		// Token: 0x040008B0 RID: 2224
		public string runeStore_BuyGeometryCoin_Title;

		// Token: 0x040008B1 RID: 2225
		public string runeStore_BuyGeometryCoin_Info;

		// Token: 0x040008B2 RID: 2226
		public string runeStore_BuyRuneGood_Title;

		// Token: 0x040008B3 RID: 2227
		public string runeStore_BuyRuneGood_Info;

		// Token: 0x040008B4 RID: 2228
		public string runeStore_PanelTitle;

		// Token: 0x040008B5 RID: 2229
		public string runeStore_FreeRefreshTimeCountDown;

		// Token: 0x040008B6 RID: 2230
		public string mode_Manage;

		// Token: 0x040008B7 RID: 2231
		public string mode_Store;

		// Token: 0x040008B8 RID: 2232
		public string mode_ManageInfo;

		// Token: 0x040008B9 RID: 2233
		public string mode_StoreInfo;

		// Token: 0x040008BA RID: 2234
		public string tip_Favorite;

		// Token: 0x040008BB RID: 2235
		public string tip_Defavoirite;

		// Token: 0x040008BC RID: 2236
		public string tip_ViewDetail;

		// Token: 0x040008BD RID: 2237
		public string runeIcon_InEquip;

		// Token: 0x040008BE RID: 2238
		public string runeIcon_ToFuse;

		// Token: 0x040008BF RID: 2239
		public string runeIcon_New;

		// Token: 0x040008C0 RID: 2240
		public string mid_ButtonEquip;

		// Token: 0x040008C1 RID: 2241
		public string mid_ButtonFuse;

		// Token: 0x040008C2 RID: 2242
		public string mid_ButtonMark;

		// Token: 0x040008C3 RID: 2243
		public string mid_ButtonMark_Cancle;

		// Token: 0x040008C4 RID: 2244
		public string mid_ButtonRecycle;

		// Token: 0x040008C5 RID: 2245
		public string mid_ButtonBuy;

		// Token: 0x040008C6 RID: 2246
		public string mid_BuyPrice;

		// Token: 0x040008C7 RID: 2247
		public string mid_RecyclePrice;

		// Token: 0x040008C8 RID: 2248
		public string floatTip_CantRecycleFavorite;

		// Token: 0x040008C9 RID: 2249
		public string autoFuse_IconName;

		// Token: 0x040008CA RID: 2250
		public string autoFuse_FloatTip_Enable;

		// Token: 0x040008CB RID: 2251
		public string autoFuse_FloatTip_Disable;

		// Token: 0x040008CC RID: 2252
		public string autoFuse_Start;

		// Token: 0x040008CD RID: 2253
		public string autoFuse_Stop;

		// Token: 0x040008CE RID: 2254
		public string autoFuse_TipInfo_NotProcess;

		// Token: 0x040008CF RID: 2255
		public string autoFuse_TipInfo_OnProcess;

		// Token: 0x040008D0 RID: 2256
		public string autoFuse_StopTip_LackOfGeometryCoin;

		// Token: 0x040008D1 RID: 2257
		public string autoFuse_StopTip_Finish;

		// Token: 0x040008D2 RID: 2258
		public string autoFuse_StopTip_CantSelectMore;

		// Token: 0x040008D3 RID: 2259
		public string fuseRule;
	}

	// Token: 0x0200011B RID: 283
	[Serializable]
	public class SkillModule
	{
		// Token: 0x040008D4 RID: 2260
		public string skillModule_Title;

		// Token: 0x040008D5 RID: 2261
		public string skillModule_Total;

		// Token: 0x040008D6 RID: 2262
		public string stringJobSkillModule;

		// Token: 0x040008D7 RID: 2263
		public string unlockTip;

		// Token: 0x040008D8 RID: 2264
		public string specialEffect;

		// Token: 0x040008D9 RID: 2265
		public string abilityChange;
	}

	// Token: 0x0200011C RID: 284
	[Serializable]
	public class Demo
	{
		// Token: 0x040008DA RID: 2266
		public string lockTip;

		// Token: 0x040008DB RID: 2267
		public string shanguo_Success;

		// Token: 0x040008DC RID: 2268
		public string shanguo_Fail;
	}

	// Token: 0x0200011D RID: 285
	[Serializable]
	public class Tutorial
	{
		// Token: 0x040008DD RID: 2269
		public string Init_AskTitle;

		// Token: 0x040008DE RID: 2270
		public string Init_Yes;

		// Token: 0x040008DF RID: 2271
		public string Init_No;

		// Token: 0x040008E0 RID: 2272
		public string InText_Skip;
	}

	// Token: 0x0200011E RID: 286
	[Serializable]
	public class StringReplace
	{
		// Token: 0x040008E1 RID: 2273
		public string shrad;

		// Token: 0x040008E2 RID: 2274
		public string encost;

		// Token: 0x040008E3 RID: 2275
		public string enrec;

		// Token: 0x040008E4 RID: 2276
		public string enmax;

		// Token: 0x040008E5 RID: 2277
		public string sBuffMax;

		// Token: 0x040008E6 RID: 2278
		public string sBuffSpd;

		// Token: 0x040008E7 RID: 2279
		public string sLaserLifetime;

		// Token: 0x040008E8 RID: 2280
		public string sLaserWidth;

		// Token: 0x040008E9 RID: 2281
		public string sLaserDamage;

		// Token: 0x040008EA RID: 2282
		public string sLaserLength;

		// Token: 0x040008EB RID: 2283
		public string sLaserOverload;

		// Token: 0x040008EC RID: 2284
		public string sLaserFireNum;

		// Token: 0x040008ED RID: 2285
		public string sLaserDamageFrequency;

		// Token: 0x040008EE RID: 2286
		public string ssSword;

		// Token: 0x040008EF RID: 2287
		public string ssSwords;

		// Token: 0x040008F0 RID: 2288
		public string ssSwordNum;

		// Token: 0x040008F1 RID: 2289
		public string ssSwordDamage;

		// Token: 0x040008F2 RID: 2290
		public string ssSwordHitBack;

		// Token: 0x040008F3 RID: 2291
		public string ssSwordLength;

		// Token: 0x040008F4 RID: 2292
		public string ssSwordWidth;

		// Token: 0x040008F5 RID: 2293
		public string ssSwordSpeed;

		// Token: 0x040008F6 RID: 2294
		public string ssBulletDrone;

		// Token: 0x040008F7 RID: 2295
		public string ssBulletDrones;

		// Token: 0x040008F8 RID: 2296
		public string ssGrenadeDrone;

		// Token: 0x040008F9 RID: 2297
		public string ssTargetDrone;

		// Token: 0x040008FA RID: 2298
		public string ssLaserDrones;

		// Token: 0x040008FB RID: 2299
		public string ssLaserDrone;

		// Token: 0x040008FC RID: 2300
		public string ssItemDrone;

		// Token: 0x040008FD RID: 2301
		public string ssLightDrone;
	}

	// Token: 0x0200011F RID: 287
	[Serializable]
	public class DIY
	{
		// Token: 0x040008FE RID: 2302
		public string customizeEnemyColor;

		// Token: 0x040008FF RID: 2303
		public string customizeBulletAlpha;

		// Token: 0x04000900 RID: 2304
		public string endlessAutoRun;

		// Token: 0x04000901 RID: 2305
		public string endlessAutoRunTip;
	}
}
