using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

// Token: 0x02000065 RID: 101
[Serializable]
public class SaveFile
{
	// Token: 0x060003DD RID: 989 RVA: 0x00018B94 File Offset: 0x00016D94
	public static void SaveByJson(bool ifBattle)
	{
		SaveFile saveFile = new SaveFile();
		GameData inst = GameData.inst;
		TempData inst2 = TempData.inst;
		saveFile.ifOnBattle = false;
		saveFile.version = GameParameters.Inst.version;
		saveFile.star = inst.Star;
		saveFile.starUsed = inst.starUsed;
		saveFile.starTotal = inst.starTotal;
		saveFile.geometryCoin = inst.GeometryCoin;
		saveFile.geometryCoinUsed = inst.geometryCoinUsed;
		saveFile.geometryCoinTotal = inst.geometryCoinTotal;
		saveFile.maxEndless = inst.maxEndless;
		saveFile.runeStore = inst.runeStore;
		saveFile.file_Jobs = new File_Job[inst.jobs.Length];
		saveFile.runes = inst.runes;
		saveFile.currentRunesIndex = inst.currentRunesIndex;
		saveFile.record = inst.record;
		for (int i = 0; i < saveFile.file_Jobs.Length; i++)
		{
			saveFile.file_Jobs[i] = new File_Job();
			saveFile.file_Jobs[i].Clone(inst.jobs[i]);
			saveFile.file_Jobs[i].talentLevels = inst.jobs[i].TalentLevels;
		}
		saveFile.ifFinished = inst.ifFinished;
		saveFile.jobID = inst2.jobId;
		saveFile.colorID = inst2.varColorId;
		saveFile.daily_IfOpen = inst2.daily_Open;
		saveFile.daily_DayIndex = inst2.daily_Index;
		saveFile.difficultyOptionFlag = inst2.diffiOptFlag;
		saveFile.difficultyOptionFlagBackUp = inst2.diffiOptFlagBackUp;
		saveFile.skillModuleFlagsNew = inst2.skillModuleFlags;
		saveFile.modeType = inst2.modeType;
		saveFile.Sql_Battle_WaveCount = SqlManager.inst.battle_WaveCount;
		saveFile.Sql_Battle_TimeTotal = SqlManager.inst.battle_TimeTotal;
		saveFile.Sql_Battle_DiffiLevelTotal = SqlManager.inst.battle_DiffiLevelTotal;
		if (ifBattle)
		{
			if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
			{
				saveFile.ifOnBattle = true;
				Battle inst3 = Battle.inst;
				saveFile.battle_EnterVersion = inst3.battle_EnterVersion;
				saveFile.battle_DiffiLevel = inst3.diffiLevel;
				saveFile.battle_Fragment = inst3.Fragment;
				saveFile.battle_FragTotal = inst3.fragmentTotal;
				saveFile.battle_FragUsed = inst3.FragmentUsed;
				saveFile.battle_Score = inst3.Score;
				saveFile.battle_Level = inst3.level;
				saveFile.battle_Wave = inst3.wave;
				saveFile.battle_ListLevelTypes = inst3.ListLevelTypes;
				saveFile.battle_ListUpgradeInt = inst3.listUpgradeInt;
				saveFile.battle_UpgradeShop = inst3.upgradeShop;
				saveFile.upgradeShop_Locked = inst3.upgradeShop_IfLocked;
				saveFile.upgradeShop_RefreshCost = inst3.upgradeShop_RefreshPrice;
				saveFile.upgradeShop_RefreshFreeChance = inst3.upgradeShop_RefreshFreeChance;
				saveFile.battle_MyRandom = new MyRandom_ForSaveFile();
				saveFile.battle_MyRandom.InitFromMyRandom(inst3.myRandom);
				saveFile.battle_BattleItems = new SaveFile_BattleItem[BattleManager.inst.listBattleItems.Count];
				if (Battle.inst.GetBool_JusticeBlade())
				{
					saveFile.battle_JusticeBladeLayer = 0;
					foreach (BattleBuff battleBuff in BattleManager.inst.listBattleBuffs)
					{
						if (battleBuff.typeID == 228)
						{
							saveFile.battle_JusticeBladeLayer = battleBuff.layerThis;
							break;
						}
					}
				}
				saveFile.DIY_EnemyColorBool = inst3.DIY_EnemyColor;
				if (saveFile.DIY_EnemyColorBool)
				{
					saveFile.DIY_EnemyColorFloat = (double)inst3.levelColor.Hue();
				}
				saveFile.DIY_BulletAlphaBool = inst3.DIY_BulletAlpha_BoolFlag;
				if (saveFile.DIY_BulletAlphaBool)
				{
					saveFile.DIY_BulletAlphaFloat = (double)inst3.DIY_BulletAlpha_Float;
				}
				for (int j = 0; j < saveFile.battle_BattleItems.Length; j++)
				{
					saveFile.battle_BattleItems[j] = new SaveFile_BattleItem();
					SaveFile_BattleItem saveFile_BattleItem = saveFile.battle_BattleItems[j];
					BattleItem battleItem = BattleManager.inst.listBattleItems[j];
					saveFile_BattleItem.typeID = battleItem.typeID;
					Vector2 vector = battleItem.transform.position;
					saveFile_BattleItem.x = (double)vector.x;
					saveFile_BattleItem.y = (double)vector.y;
				}
				int num = 0;
				foreach (Fragment fragment in BattleManager.inst.listFragments)
				{
					num += fragment.goldValue;
				}
				int num2 = Mathf.Sqrt((float)num).RoundToInt();
				if (num > 0)
				{
					saveFile.battle_Fragments = new SaveFile_Fragment[num2 + 1];
					for (int k = 0; k < num2 + 1; k++)
					{
						saveFile.battle_Fragments[k] = new SaveFile_Fragment();
						SaveFile_Fragment saveFile_Fragment = saveFile.battle_Fragments[k];
						Vector2 vector2 = BattleManager.ChooseBattleItemGenePosInScene();
						saveFile_Fragment.value = num / num2;
						saveFile_Fragment.x = (double)vector2.x;
						saveFile_Fragment.y = (double)vector2.y;
						saveFile_Fragment.scale = 0.6000000238418579;
					}
					saveFile.battle_Fragments[num2].value = num % num2;
				}
				else
				{
					saveFile.battle_Fragments = new SaveFile_Fragment[0];
				}
				if (TempData.inst.modeType == EnumModeType.WANDER || TempData.inst.diffiOptFlag[28])
				{
					saveFile.battle_Wander_PlayerLife = (int)MyTool.DoubleToLong(Player.inst.unit.life);
					saveFile.battle_Wander_PlayerShield = Player.inst.shield;
					saveFile.battle_Wander_BattleTiemLeft = (double)BattleManager.inst.waveTimeLeft;
				}
			}
			else
			{
				Debug.LogError("存档错误 根本不在战斗场景啊！！");
			}
		}
		string text = JsonMapper.ToJson(saveFile);
		StreamWriter streamWriter = new StreamWriter(SaveFile_Path.GetPathCurrentOS[0]);
		text = MyEncrypt.Encrypt(text);
		streamWriter.Write(text);
		streamWriter.Close();
		if (SaveFile.ifReadSuccess)
		{
			SaveFile.DeleteOldSave();
		}
		Debug.Log("存档成功");
		SaveFile.SaveFileBackup();
		Debug.Log("存档备份成功");
		GameData.saveFile = null;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00019174 File Offset: 0x00017374
	public static SaveFile ReadByJson()
	{
		int num = 0;
		if (!SaveFile.IfExistSaveByJson(ref num))
		{
			Debug.LogWarning("Warning_无存档，新建存档！");
			return new SaveFile();
		}
		string savePath = SaveFile.GetSavePath();
		if (num != 0)
		{
			Debug.Log("读取旧版本存档 ：" + num);
		}
		SaveFile result;
		try
		{
			StreamReader streamReader = new StreamReader(savePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			if (num == 0)
			{
				text = MyEncrypt.Decipher(text);
			}
			Debug.Log("Decipher Success");
			SaveFile saveFile = JsonMapper.ToObject<SaveFile>(text);
			SaveFile.ifReadSuccess = true;
			Debug.Log("读档结果：成功读取最新存档");
			result = saveFile;
		}
		catch
		{
			result = SaveFile.ReadByJason_ReadBackUp(1);
		}
		return result;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00019218 File Offset: 0x00017418
	public static SaveFile ReadByJason_ReadBackUp(int backID)
	{
		if (backID > 5)
		{
			Debug.Log("读档结果：所有存档备份都已损坏！");
			return null;
		}
		int num = 0;
		if (!SaveFile.IfExistSaveByJson(ref num))
		{
			Debug.LogError("暂时没有存档！");
			return new SaveFile();
		}
		string pathCurrenOS_Backup = SaveFile_Path.GetPathCurrenOS_Backup(backID);
		SaveFile result;
		try
		{
			StreamReader streamReader = new StreamReader(pathCurrenOS_Backup);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			if (num == 0)
			{
				text = MyEncrypt.Decipher(text);
			}
			Debug.Log("Decipher Success");
			Debug.Log("读取存档解密\n" + text);
			SaveFile saveFile = JsonMapper.ToObject<SaveFile>(text);
			SaveFile.ifReadSuccess = true;
			Debug.Log("读档结果：当前存档损坏，但成功读取存档备份：" + backID);
			result = saveFile;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			result = SaveFile.ReadByJason_ReadBackUp(backID + 1);
		}
		return result;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x000192D4 File Offset: 0x000174D4
	private static string GetSavePath()
	{
		for (int i = 0; i < SaveFile_Path.GetPathCurrentOS.Length; i++)
		{
			string text = SaveFile_Path.GetPathCurrentOS[i];
			if (File.Exists(text))
			{
				return text;
			}
		}
		Debug.LogError("错误：无存档路径却调用读取路径");
		return null;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00019310 File Offset: 0x00017510
	public static bool IfExistSaveByJson(ref int index)
	{
		for (int i = 0; i < SaveFile_Path.GetPathCurrentOS.Length; i++)
		{
			if (File.Exists(SaveFile_Path.GetPathCurrentOS[i]))
			{
				index = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00019344 File Offset: 0x00017544
	private static void DeleteOldSave()
	{
		for (int i = 1; i < SaveFile_Path.GetPathCurrentOS.Length; i++)
		{
			string path = SaveFile_Path.GetPathCurrentOS[i];
			if (File.Exists(path))
			{
				Debug.Log("清除旧路径存档" + i);
				File.Delete(path);
			}
		}
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00019390 File Offset: 0x00017590
	public static bool IfExistSaveByJson()
	{
		for (int i = 0; i < SaveFile_Path.GetPathCurrentOS.Length; i++)
		{
			if (File.Exists(SaveFile_Path.GetPathCurrentOS[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x000193C0 File Offset: 0x000175C0
	public static bool IsNewSaveSlot()
	{
		return !SaveFile.IfExistSaveByJson();
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x000193CC File Offset: 0x000175CC
	public static void SaveFileBackup()
	{
		string pathCurrenOS_Backup = SaveFile_Path.GetPathCurrenOS_Backup(5);
		if (File.Exists(pathCurrenOS_Backup))
		{
			File.Delete(pathCurrenOS_Backup);
		}
		for (int i = 4; i >= 1; i--)
		{
			string pathCurrenOS_Backup2 = SaveFile_Path.GetPathCurrenOS_Backup(i);
			string pathCurrenOS_Backup3 = SaveFile_Path.GetPathCurrenOS_Backup(i + 1);
			if (File.Exists(pathCurrenOS_Backup2))
			{
				File.Move(pathCurrenOS_Backup2, pathCurrenOS_Backup3);
			}
		}
		File.Copy(SaveFile_Path.GetPathCurrentOS[0], SaveFile_Path.GetPathCurrenOS_Backup(1));
	}

	// Token: 0x04000340 RID: 832
	public static bool ifReadSuccess;

	// Token: 0x04000341 RID: 833
	[Header("宏观")]
	public bool ifOnBattle;

	// Token: 0x04000342 RID: 834
	public int version;

	// Token: 0x04000343 RID: 835
	public string steamID;

	// Token: 0x04000344 RID: 836
	[Header("玩家档案数据")]
	public long star;

	// Token: 0x04000345 RID: 837
	public long starUsed;

	// Token: 0x04000346 RID: 838
	public long starTotal;

	// Token: 0x04000347 RID: 839
	public long geometryCoin;

	// Token: 0x04000348 RID: 840
	public long geometryCoinUsed;

	// Token: 0x04000349 RID: 841
	public long geometryCoinTotal;

	// Token: 0x0400034A RID: 842
	public bool ifFinished;

	// Token: 0x0400034B RID: 843
	public int maxEndless;

	// Token: 0x0400034C RID: 844
	public File_Job[] file_Jobs;

	// Token: 0x0400034D RID: 845
	public Record record;

	// Token: 0x0400034E RID: 846
	[Header("符文")]
	public List<Rune> runes;

	// Token: 0x0400034F RID: 847
	public int[] currentRunesIndex = new int[]
	{
		-1
	};

	// Token: 0x04000350 RID: 848
	public RuneStore runeStore;

	// Token: 0x04000351 RID: 849
	[Header("主界面选择数据")]
	public int jobID;

	// Token: 0x04000352 RID: 850
	public int colorID;

	// Token: 0x04000353 RID: 851
	public bool daily_IfOpen;

	// Token: 0x04000354 RID: 852
	public int daily_DayIndex = -1;

	// Token: 0x04000355 RID: 853
	public bool ifInfinity;

	// Token: 0x04000356 RID: 854
	public EnumModeType modeType = EnumModeType.UNINITED;

	// Token: 0x04000357 RID: 855
	public bool[] difficultyOptionFlag = new bool[0];

	// Token: 0x04000358 RID: 856
	public bool[] difficultyOptionFlagBackUp = new bool[0];

	// Token: 0x04000359 RID: 857
	public bool[] skillModuleFlags = new bool[0];

	// Token: 0x0400035A RID: 858
	public bool[][] skillModuleFlagsNew = new bool[0][];

	// Token: 0x0400035B RID: 859
	[Header("战斗内数据")]
	public int battle_EnterVersion;

	// Token: 0x0400035C RID: 860
	public long battle_Fragment;

	// Token: 0x0400035D RID: 861
	public long battle_Score;

	// Token: 0x0400035E RID: 862
	public long battle_FragTotal;

	// Token: 0x0400035F RID: 863
	public long battle_FragUsed;

	// Token: 0x04000360 RID: 864
	public int battle_LevelColorID;

	// Token: 0x04000361 RID: 865
	public int battle_Level;

	// Token: 0x04000362 RID: 866
	public int battle_Wave;

	// Token: 0x04000363 RID: 867
	public int battle_DiffiLevel;

	// Token: 0x04000364 RID: 868
	public int battle_Wander_PlayerLife;

	// Token: 0x04000365 RID: 869
	public int battle_Wander_PlayerShield;

	// Token: 0x04000366 RID: 870
	public double battle_Wander_BattleTiemLeft;

	// Token: 0x04000367 RID: 871
	public int battle_JusticeBladeLayer;

	// Token: 0x04000368 RID: 872
	public bool DIY_EnemyColorBool;

	// Token: 0x04000369 RID: 873
	public double DIY_EnemyColorFloat;

	// Token: 0x0400036A RID: 874
	public bool DIY_BulletAlphaBool;

	// Token: 0x0400036B RID: 875
	public double DIY_BulletAlphaFloat = 1.0;

	// Token: 0x0400036C RID: 876
	public MyRandom_ForSaveFile battle_MyRandom = new MyRandom_ForSaveFile();

	// Token: 0x0400036D RID: 877
	public bool upgradeShop_Locked;

	// Token: 0x0400036E RID: 878
	public int upgradeShop_RefreshCost;

	// Token: 0x0400036F RID: 879
	public int upgradeShop_RefreshFreeChance;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	public SaveFile_BattleItem[] battle_BattleItems = new SaveFile_BattleItem[0];

	// Token: 0x04000371 RID: 881
	[SerializeField]
	public SaveFile_Fragment[] battle_Fragments = new SaveFile_Fragment[0];

	// Token: 0x04000372 RID: 882
	[Header("Sql")]
	public int Sql_Battle_WaveCount;

	// Token: 0x04000373 RID: 883
	public double Sql_Battle_PlayerDamageTotal;

	// Token: 0x04000374 RID: 884
	public double Sql_Battle_TimeTotal;

	// Token: 0x04000375 RID: 885
	public double Sql_Battle_PlayerHurtTotal;

	// Token: 0x04000376 RID: 886
	public int Sql_Battle_DiffiLevelTotal;

	// Token: 0x04000377 RID: 887
	[Header("设置")]
	public int resolutionIndex;

	// Token: 0x04000378 RID: 888
	public EnumLanguage set_Language = EnumLanguage.CHINESE_SIM;

	// Token: 0x04000379 RID: 889
	public bool[] setBools = new bool[0];

	// Token: 0x0400037A RID: 890
	public double[] setFloats = new double[0];

	// Token: 0x0400037B RID: 891
	[SerializeField]
	public List<EnumLevelType> battle_ListLevelTypes;

	// Token: 0x0400037C RID: 892
	[SerializeField]
	public List<int> battle_ListUpgradeInt = new List<int>();

	// Token: 0x0400037D RID: 893
	[SerializeField]
	public UpgradeShop battle_UpgradeShop;
}
