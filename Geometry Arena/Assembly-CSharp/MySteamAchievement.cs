using System;
using Steamworks;
using UnityEngine;

// Token: 0x0200006B RID: 107
public static class MySteamAchievement
{
	// Token: 0x060003F5 RID: 1013 RVA: 0x0001996F File Offset: 0x00017B6F
	public static void EnterGame()
	{
		MySteamAchievement.AddStatInt("acc_EnterGame", 1);
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001997C File Offset: 0x00017B7C
	public static void AddStatInt(string statName, int add)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		int num;
		SteamUserStats.GetStat(statName, out num);
		SteamUserStats.SetStat(statName, num + add);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x000199AA File Offset: 0x00017BAA
	public static void SetStatInt(string statName, int setNum)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(statName, setNum);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x000199C2 File Offset: 0x00017BC2
	public static void SetStatFloat(string statName, float setNum)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(statName, setNum);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x000199DA File Offset: 0x00017BDA
	public static void Detect_LevelEnd()
	{
		MySteamAchievement.Detect_DifficultyOption();
		MySteamAchievement.Detect_DPS();
		MySteamAchievement.Detect_DifficultyLevel();
		MySteamAchievement.Detect_EndlessLevel();
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x000199F0 File Offset: 0x00017BF0
	private static void Detect_DifficultyOption()
	{
		int setNum = Mathf.FloorToInt(Battle.inst.factorBattle_FromDifficultyOption.StarGain * 100f);
		MySteamAchievement.SetStatInt("DO_Max", setNum);
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00019A24 File Offset: 0x00017C24
	private static void Detect_DPS()
	{
		double double_DPS = UI_Icon_BattleDPS.inst.GetDouble_DPS();
		float setNum = (float)double_DPS;
		if (double_DPS > 1E+20)
		{
			setNum = 1E+20f;
		}
		MySteamAchievement.SetStatFloat("DPS_Max", setNum);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00019A5C File Offset: 0x00017C5C
	private static void Detect_DifficultyLevel()
	{
		int num = Battle.inst.diffiLevel;
		num = GameData.Convert_NewDLtoOldDL(num);
		for (int i = 2; i <= num; i++)
		{
			MySteamAchievement.TryUnlockAchievementWithName(string.Format("DL_{0}", i));
		}
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00019A9C File Offset: 0x00017C9C
	private static void Detect_EndlessLevel()
	{
		if (TempData.inst.modeType != EnumModeType.ENDLESS)
		{
			return;
		}
		int wave = Battle.inst.wave;
		MySteamAchievement.SetStatInt("Endless_Max", wave);
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00019AD0 File Offset: 0x00017CD0
	public static void DetectRoleProficiencyLevel()
	{
		for (int i = 0; i < 12; i++)
		{
			int rank = GameData.inst.jobs[i].mastery.GetRank();
			string str = string.Format("Job{0}_", i);
			if (rank >= 1)
			{
				MySteamAchievement.TryUnlockAchievementWithName(str + "1");
				if (rank >= 3)
				{
					MySteamAchievement.TryUnlockAchievementWithName(str + "2");
					if (rank >= 6)
					{
						MySteamAchievement.TryUnlockAchievementWithName(str + "3");
						if (rank >= 10)
						{
							MySteamAchievement.TryUnlockAchievementWithName(str + "4");
						}
					}
				}
			}
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00019B68 File Offset: 0x00017D68
	public static void Test_RelockAchievement_RoleProficiencyLevel()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		for (int i = 0; i < 12; i++)
		{
			int rank = GameData.inst.jobs[i].mastery.GetRank();
			string str = string.Format("Job{0}_", i);
			if (rank >= 1)
			{
				MySteamAchievement.TryRelockAchievementWithName(str + "1");
				MySteamAchievement.TryRelockAchievementWithName(str + "2");
				MySteamAchievement.TryRelockAchievementWithName(str + "3");
				MySteamAchievement.TryRelockAchievementWithName(str + "4");
			}
		}
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00019BF4 File Offset: 0x00017DF4
	public static void UnlockAchievement(int id)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		MySteamAchievement.TryUnlockAchievementWithName(string.Format("NEW_ACHIEVEMENT_1_{0}", id));
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00019C14 File Offset: 0x00017E14
	public static void TryUnlockAchievementWithName(string achiName)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		bool flag;
		SteamUserStats.GetAchievement(achiName, out flag);
		if (flag)
		{
			return;
		}
		SteamUserStats.SetAchievement(achiName);
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00019C44 File Offset: 0x00017E44
	public static void TryRelockAchievementWithName(string achiName)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		bool flag;
		SteamUserStats.GetAchievement(achiName, out flag);
		if (!flag)
		{
			return;
		}
		SteamUserStats.ClearAchievement(achiName);
		Debug.LogWarning("Success: Achievement Relock! " + achiName);
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00019C83 File Offset: 0x00017E83
	public static void RelockAchievement(int id)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		MySteamAchievement.TryRelockAchievementWithName(string.Format("NEW_ACHIEVEMENT_1_{0}", id));
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00019CA4 File Offset: 0x00017EA4
	public static void Test_LockAndUnlockAll()
	{
		for (int i = 7; i < 35; i++)
		{
			MySteamAchievement.RelockAchievement(i);
		}
		for (int j = 7; j < 35; j++)
		{
			MySteamAchievement.UnlockAchievement(j);
		}
	}
}
