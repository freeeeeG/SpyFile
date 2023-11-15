using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Steamworks;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class LevelManager : Singleton<LevelManager>
{
	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06001084 RID: 4228 RVA: 0x0002D713 File Offset: 0x0002B913
	// (set) Token: 0x06001085 RID: 4229 RVA: 0x0002D71B File Offset: 0x0002B91B
	public bool LevelWin { get; set; }

	// Token: 0x06001086 RID: 4230 RVA: 0x0002D724 File Offset: 0x0002B924
	public void ClearAllSteamStats()
	{
		SteamUserStats.ResetAllStats(true);
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0002D730 File Offset: 0x0002B930
	private void SetAchievements()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		foreach (string text in this.achievements)
		{
			bool flag;
			SteamUserStats.GetAchievement(text, out flag);
			if (!flag && PlayerPrefs.GetInt(text, 0) == 1)
			{
				SteamUserStats.SetAchievement(text);
				SteamUserStats.StoreStats();
			}
		}
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0002D784 File Offset: 0x0002B984
	public void SetAchievement(string achievement)
	{
		PlayerPrefs.SetInt(achievement, 1);
		if (SteamManager.Initialized)
		{
			bool flag;
			SteamUserStats.GetAchievement(achievement, out flag);
			if (!flag)
			{
				SteamUserStats.SetAchievement(achievement);
				SteamUserStats.StoreStats();
			}
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06001089 RID: 4233 RVA: 0x0002D7B8 File Offset: 0x0002B9B8
	public int MaxGameLevel
	{
		get
		{
			return Mathf.Min(this.GameLevels.Length - 1, this.permitGameLevel);
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x0600108A RID: 4234 RVA: 0x0002D7CF File Offset: 0x0002B9CF
	// (set) Token: 0x0600108B RID: 4235 RVA: 0x0002D7DC File Offset: 0x0002B9DC
	public int GameLevel
	{
		get
		{
			return PlayerPrefs.GetInt("GameLevel", 0);
		}
		set
		{
			PlayerPrefs.SetInt("GameLevel", Mathf.Min(value, this.MaxGameLevel));
			if (SteamManager.Initialized)
			{
				SteamUserStats.RequestCurrentStats();
				int num;
				if (SteamUserStats.GetStat("Player_Level", out num) && value > num)
				{
					SteamUserStats.SetStat("Player_Level", value);
				}
			}
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x0600108C RID: 4236 RVA: 0x0002D82A File Offset: 0x0002BA2A
	// (set) Token: 0x0600108D RID: 4237 RVA: 0x0002D838 File Offset: 0x0002BA38
	public int GameExp
	{
		get
		{
			return PlayerPrefs.GetInt("GameExp", 0);
		}
		set
		{
			PlayerPrefs.SetInt("GameExp", value);
			if (SteamManager.Initialized)
			{
				SteamUserStats.RequestCurrentStats();
				int num;
				if (SteamUserStats.GetStat("Player_EXP", out num) && value > num)
				{
					SteamUserStats.SetStat("Player_EXP", value);
				}
			}
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x0600108E RID: 4238 RVA: 0x0002D87B File Offset: 0x0002BA7B
	// (set) Token: 0x0600108F RID: 4239 RVA: 0x0002D894 File Offset: 0x0002BA94
	public int PassDiifcutly
	{
		get
		{
			return Mathf.Min(this.PermitDifficulty, PlayerPrefs.GetInt("MaxDiff", 0));
		}
		set
		{
			PlayerPrefs.SetInt("MaxDiff", Mathf.Min(6, value));
			if (SteamManager.Initialized)
			{
				SteamUserStats.RequestCurrentStats();
				int num;
				if (SteamUserStats.GetStat("Player_Diff", out num) && value > num)
				{
					SteamUserStats.SetStat("Player_Diff", Mathf.Min(6, value));
				}
			}
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06001090 RID: 4240 RVA: 0x0002D8E3 File Offset: 0x0002BAE3
	// (set) Token: 0x06001091 RID: 4241 RVA: 0x0002D8F0 File Offset: 0x0002BAF0
	public int PassChallengeLevel
	{
		get
		{
			return PlayerPrefs.GetInt("PassChallengeLevel", 0);
		}
		set
		{
			PlayerPrefs.SetInt("PassChallengeLevel", Mathf.Min(this.ChallengeLevels.Length, value));
			if (SteamManager.Initialized)
			{
				SteamUserStats.RequestCurrentStats();
				int num;
				if (SteamUserStats.GetStat("PassChallengeLevel", out num) && value > num)
				{
					SteamUserStats.SetStat("PassChallengeLevel", Mathf.Min(this.ChallengeLevels.Length, value));
				}
			}
		}
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0002D94D File Offset: 0x0002BB4D
	public int GetChallengeScore(int level)
	{
		return PlayerPrefs.GetInt("ChallengeScore" + level.ToString(), 0);
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0002D968 File Offset: 0x0002BB68
	public void SetChallengeScore(int level, int value)
	{
		PlayerPrefs.SetInt("ChallengeScore" + level.ToString(), value);
		if (SteamManager.Initialized)
		{
			SteamUserStats.RequestCurrentStats();
			int num;
			if (SteamUserStats.GetStat("ChallengeScore" + level.ToString(), out num) && value > num)
			{
				SteamUserStats.SetStat("ChallengeScore" + level.ToString(), value);
			}
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06001094 RID: 4244 RVA: 0x0002D9CF File Offset: 0x0002BBCF
	// (set) Token: 0x06001095 RID: 4245 RVA: 0x0002D9DC File Offset: 0x0002BBDC
	public int LifeTotalRefactor
	{
		get
		{
			return PlayerPrefs.GetInt("LifeTotalRefactor", 0);
		}
		set
		{
			if (value > PlayerPrefs.GetInt("LifeTotalRefactor", 0))
			{
				PlayerPrefs.SetInt("LifeTotalRefactor", value);
				if (SteamManager.Initialized)
				{
					SteamUserStats.SetStat("ACH_TotalRefactor", value);
				}
			}
		}
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0002DA0C File Offset: 0x0002BC0C
	public void Initialize()
	{
		LitJsonRegister.Register();
		this.SaveGameFilePath = Application.persistentDataPath + "/GameSave.json";
		this.GetSteamStat();
		this.SetAchievements();
		this.LevelDIC = new Dictionary<int, LevelAttribute>();
		foreach (LevelAttribute levelAttribute in this.StandardLevels)
		{
			this.LevelDIC.Add(levelAttribute.ModeID, levelAttribute);
		}
		foreach (LevelAttribute levelAttribute2 in this.ChallengeLevels)
		{
			this.LevelDIC.Add(levelAttribute2.ModeID, levelAttribute2);
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
	private void GetSteamStat()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.RequestCurrentStats();
			int gameLevel;
			if (SteamUserStats.GetStat("Player_Level", out gameLevel))
			{
				this.GameLevel = gameLevel;
			}
			int gameExp;
			if (SteamUserStats.GetStat("Player_EXP", out gameExp))
			{
				this.GameExp = gameExp;
			}
			int passDiifcutly;
			if (SteamUserStats.GetStat("Player_Diff", out passDiifcutly))
			{
				this.PassDiifcutly = passDiifcutly;
			}
			int passChallengeLevel;
			if (SteamUserStats.GetStat("PassChallengeLevel", out passChallengeLevel))
			{
				this.PassChallengeLevel = passChallengeLevel;
			}
		}
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0002DB0E File Offset: 0x0002BD0E
	private void DeleteGameSave()
	{
		if (File.Exists(this.SaveGameFilePath))
		{
			File.Delete(this.SaveGameFilePath);
		}
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0002DB28 File Offset: 0x0002BD28
	public LevelAttribute GetLevelAtt(int mode)
	{
		if (this.LevelDIC.ContainsKey(mode))
		{
			return this.LevelDIC[mode];
		}
		Debug.LogWarning("错误的模式代码");
		return null;
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0002DB50 File Offset: 0x0002BD50
	public void SetGameLevel(int level)
	{
		this.GameLevel = level;
		foreach (ContentAttribute contentAttribute in this.AllContent)
		{
			contentAttribute.isLock = contentAttribute.initialLock;
		}
		for (int i = 0; i < this.GameLevel + 1; i++)
		{
			ContentAttribute[] unlockItems = this.GameLevels[i].UnlockItems;
			for (int j = 0; j < unlockItems.Length; j++)
			{
				unlockItems[j].isLock = false;
			}
		}
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0002DBEC File Offset: 0x0002BDEC
	public int GainExp(int wave)
	{
		int num = Mathf.RoundToInt(this.CurrentLevel.ExpIntensify * 5f * (float)wave * (1f + (float)(wave / 10) * 0.25f));
		this.AddExp(num);
		return num;
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0002DC30 File Offset: 0x0002BE30
	private void UnlockBonus(string bo)
	{
		foreach (ContentAttribute contentAttribute in this.AllContent)
		{
			if (contentAttribute.Name == bo)
			{
				contentAttribute.isLock = false;
				break;
			}
		}
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0002DC94 File Offset: 0x0002BE94
	private void AddExp(int exp)
	{
		if (this.GameLevel >= this.MaxGameLevel)
		{
			this.GameExp += exp;
			return;
		}
		int num = this.GameLevels[this.GameLevel].ExpRequire - this.GameExp;
		if (exp >= num)
		{
			int i = this.GameLevel;
			this.GameLevel = i + 1;
			foreach (ContentAttribute contentAttribute in this.GameLevels[this.GameLevel].UnlockItems)
			{
				this.UnlockBonus(contentAttribute.Name);
			}
			this.GameExp = 0;
			this.AddExp(exp - num);
			return;
		}
		this.GameExp += exp;
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0002DD44 File Offset: 0x0002BF44
	public GameLevelInfo GetLevelInfo(int level)
	{
		return this.GameLevels[level];
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0002DD54 File Offset: 0x0002BF54
	public void StartNewGame(int modeID)
	{
		if (this.StartingGame)
		{
			return;
		}
		this.StartingGame = true;
		this.CurrentLevel = this.GetLevelAtt(modeID);
		if (this.CurrentLevel == null)
		{
			Debug.LogWarning("没有找到对应的关卡文件");
			return;
		}
		this.LastGameSave.ClearGame();
		this.GameSaveContents.Clear();
		this.DeleteGameSave();
		Singleton<Game>.Instance.LoadScene(1);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0002DDBE File Offset: 0x0002BFBE
	public void ContinueLastGame()
	{
		if (this.StartingGame)
		{
			return;
		}
		this.StartingGame = true;
		this.DeleteGameSave();
		Singleton<Game>.Instance.LoadScene(1);
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0002DDE4 File Offset: 0x0002BFE4
	private List<ContentStruct> SaveContens()
	{
		List<ContentStruct> list = new List<ContentStruct>();
		foreach (GameTileContent gameTileContent in this.GameSaveContents)
		{
			ContentStruct item;
			gameTileContent.SaveContent(out item);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0002DE44 File Offset: 0x0002C044
	private List<EnemySequenceStruct> SaveSequences()
	{
		List<EnemySequenceStruct> list = new List<EnemySequenceStruct>();
		foreach (List<EnemySequence> sequencesList in WaveSystem.LevelSequence)
		{
			list.Add(new EnemySequenceStruct
			{
				SequencesList = sequencesList
			});
		}
		return list;
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0002DEAC File Offset: 0x0002C0AC
	private List<BlueprintStruct> SaveAllBlueprints()
	{
		List<BlueprintStruct> list = new List<BlueprintStruct>();
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints)
		{
			BlueprintStruct blueprintStruct = new BlueprintStruct();
			blueprintStruct.Name = bluePrintGrid.Strategy.Attribute.Name;
			blueprintStruct.ElementRequirements = new List<int>();
			blueprintStruct.QualityRequirements = new List<int>();
			for (int i = 0; i < bluePrintGrid.Strategy.Compositions.Count; i++)
			{
				blueprintStruct.ElementRequirements.Add(bluePrintGrid.Strategy.Compositions[i].elementRequirement);
				blueprintStruct.QualityRequirements.Add(bluePrintGrid.Strategy.Compositions[i].qualityRequeirement);
			}
			list.Add(blueprintStruct);
		}
		return list;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0002DFA0 File Offset: 0x0002C1A0
	private List<TechnologyStruct> SaveAllTechs()
	{
		List<TechnologyStruct> list = new List<TechnologyStruct>();
		foreach (Technology technology in TechnologySystem.GetTechnologies)
		{
			list.Add(new TechnologyStruct
			{
				TechName = (int)technology.TechnologyName,
				IsAbnormal = technology.IsAbnormal,
				TechSaveValue = technology.SaveValue,
				CanAbnormal = technology.CanAbnormal
			});
		}
		return list;
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x0002E030 File Offset: 0x0002C230
	private List<int> SaveRules()
	{
		List<int> list = new List<int>();
		foreach (Rule rule in RuleFactory.BattleRules)
		{
			list.Add((int)rule.RuleName);
		}
		return list;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x0002E090 File Offset: 0x0002C290
	private List<TechnologyStruct> SavePickingTechs()
	{
		List<TechnologyStruct> list = new List<TechnologyStruct>();
		foreach (Technology technology in TechnologySystem.PickingTechs)
		{
			list.Add(new TechnologyStruct
			{
				TechName = (int)technology.TechnologyName,
				IsAbnormal = technology.IsAbnormal,
				TechSaveValue = technology.SaveValue,
				CanAbnormal = technology.CanAbnormal
			});
		}
		return list;
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0002E120 File Offset: 0x0002C320
	private List<ShapeInfo> SaveCurrentShapes()
	{
		return Singleton<GameManager>.Instance.GetCurrentPickingShapes();
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0002E12C File Offset: 0x0002C32C
	private List<string> SaveBattleRecipes()
	{
		List<string> list = new List<string>();
		foreach (TurretAttribute turretAttribute in Singleton<StaticData>.Instance.ContentFactory.BattleRecipes)
		{
			list.Add(turretAttribute.Name);
		}
		return list;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0002E194 File Offset: 0x0002C394
	private void LoadGameSave()
	{
		if (this.LastGameSave.HasLastGame)
		{
			this.CurrentLevel = this.GetLevelAtt(this.LastGameSave.SaveRes.Mode);
		}
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0002E1BF File Offset: 0x0002C3BF
	public void LoadGame()
	{
		this.LoadByJson();
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0002E1C8 File Offset: 0x0002C3C8
	private void SaveByJson()
	{
		if (this.NeedLoadGame)
		{
			if (!this.LevelEnd && this.CurrentLevel.CanSaveGame)
			{
				if (DraggingShape.PickingShape != null)
				{
					DraggingShape.PickingShape.UndoShape();
				}
				this.LastGameSave.SaveGame(this.SaveAllTechs(), this.SavePickingTechs(), this.SaveAllBlueprints(), GameRes.SaveRes, this.SaveContens(), this.SaveSequences(), this.SaveCurrentShapes(), this.SaveRules(), this.SaveBattleRecipes());
				this.LevelEnd = true;
			}
			else if (!this.LevelEnd && !this.CurrentLevel.CanSaveGame)
			{
				this.LastGameSave.ClearGame();
			}
			this.GameSaveContents.Clear();
			this.DeleteGameSave();
			string text = Application.persistentDataPath + "/GameSave.json";
			Debug.Log(text);
			string str = JsonMapper.ToJson(this.LastGameSave);
			StreamWriter streamWriter = new StreamWriter(text);
			streamWriter.Write(EncryptionTool.EncryptString(str, this.key));
			streamWriter.Close();
			Debug.Log("战斗成功存档");
		}
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0002E2D0 File Offset: 0x0002C4D0
	private void LoadByJson()
	{
		this.SetGameLevel(this.GameLevel);
		if (this.NeedLoadGame && File.Exists(this.SaveGameFilePath))
		{
			try
			{
				StreamReader streamReader = new StreamReader(this.SaveGameFilePath);
				string json = EncryptionTool.DecryptString(streamReader.ReadToEnd(), this.key);
				streamReader.Close();
				GameSave lastGameSave = JsonMapper.ToObject<GameSave>(json);
				this.LastGameSave = lastGameSave;
				this.LoadGameSave();
				Debug.Log("成功读取战斗");
				return;
			}
			catch
			{
				this.LastGameSave = new GameSave();
				Debug.LogWarning("读取战斗数据有错误");
				return;
			}
		}
		this.LastGameSave = new GameSave();
		Debug.Log("没有可读取战斗");
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0002E380 File Offset: 0x0002C580
	public void SaveAll()
	{
		this.SaveByJson();
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0002E388 File Offset: 0x0002C588
	private void OnApplicationQuit()
	{
		this.SaveAll();
	}

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private string[] achievements;

	// Token: 0x040008D9 RID: 2265
	private string key = "123456789";

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private GameLevelInfo[] GameLevels;

	// Token: 0x040008DB RID: 2267
	[Header("允许最大等级")]
	[SerializeField]
	private int permitGameLevel;

	// Token: 0x040008DC RID: 2268
	public int PermitDifficulty;

	// Token: 0x040008DD RID: 2269
	public List<ContentAttribute> AllContent;

	// Token: 0x040008DE RID: 2270
	public LevelAttribute[] StandardLevels;

	// Token: 0x040008DF RID: 2271
	public LevelAttribute[] ChallengeLevels;

	// Token: 0x040008E0 RID: 2272
	private Dictionary<int, LevelAttribute> LevelDIC;

	// Token: 0x040008E1 RID: 2273
	public LevelAttribute CurrentLevel;

	// Token: 0x040008E2 RID: 2274
	private string SaveGameFilePath;

	// Token: 0x040008E3 RID: 2275
	[HideInInspector]
	public bool StartingGame;

	// Token: 0x040008E4 RID: 2276
	[HideInInspector]
	public List<GameTileContent> GameSaveContents;

	// Token: 0x040008E5 RID: 2277
	[Header("是否读取存档")]
	[SerializeField]
	private bool NeedLoadGame;

	// Token: 0x040008E6 RID: 2278
	[Header("是否有未完成游戏")]
	public GameSave LastGameSave;

	// Token: 0x040008E7 RID: 2279
	public bool LevelEnd = true;
}
