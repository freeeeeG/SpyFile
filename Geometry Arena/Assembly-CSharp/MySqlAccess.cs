using System;
using System.Data;
using MySql.Data.MySqlClient;
using Steamworks;
using UnityEngine;

// Token: 0x02000045 RID: 69
[Serializable]
public class MySqlAccess
{
	// Token: 0x060002B6 RID: 694 RVA: 0x00010364 File Offset: 0x0000E564
	public void InitAndOpen(string _host, string _port, string _userName, string _password, string _databaseName)
	{
		this.host = _host;
		this.port = _port;
		this.userName = _userName;
		this.password = _password;
		this.databaseName = _databaseName;
		this.OpenSql();
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00010394 File Offset: 0x0000E594
	private void OpenSql()
	{
		if (SqlManager.inst.deltaTimeFromlastConnection < 3f)
		{
			Debug.LogWarning("距离上次尝试连接间隔少于3秒，退出");
			return;
		}
		if (SqlManager.inst.deltaTimeFromlastConnection > 18f)
		{
			this.canOperateFlag = true;
		}
		SqlManager.inst.deltaTimeFromlastConnection = 0f;
		if (!this.canOperateFlag)
		{
			return;
		}
		this.canOperateFlag = false;
		try
		{
			this.CloseSql();
		}
		catch (Exception arg)
		{
			Debug.LogError("啥情况咋关不掉呢" + arg);
			this.canOperateFlag = true;
		}
		try
		{
			Debug.Log("尝试连接数据库");
			string connectionString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4};charset=utf8", new object[]
			{
				this.databaseName,
				this.host,
				this.userName,
				this.password,
				this.port
			});
			this.mySqlConnection = new MySqlConnection(connectionString);
			this.mySqlConnection.Open();
			this.canOperateFlag = true;
		}
		catch (Exception ex)
		{
			this.canOperateFlag = true;
			throw new Exception("mySQL连接失败，" + ex.Message.ToString());
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000104C0 File Offset: 0x0000E6C0
	public void CloseSql()
	{
		if (this.mySqlConnection != null)
		{
			this.mySqlConnection.Dispose();
			this.mySqlConnection = null;
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x000104DC File Offset: 0x0000E6DC
	public void Insert_WaveEnd(float playerDamage, float time, float hurt)
	{
		playerDamage = 0f;
		hurt = 0f;
		TempData inst = TempData.inst;
		Battle battle = inst.battle;
		Factor factor = Player.inst.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle();
		FactorBattle factorBattleTotal = battle.factorBattleTotal;
		string[] array = new string[7];
		array[0] = " INSERT INTO Data_WaveEnd(SteamID, InsertTime, JobID, VarColorID, Infinity, ActualWave, PlayerDamage, SpendTime, PlayerHurt,Upgrade_Total,Upgrade_Normal,Upgrade_Rare,Upgrade_Epic,DiffiLevel,DiffiOptionNum,Att_Life, Att_MoveSpd, Att_FireSpd, Att_BulletDmg, Att_BulletSpd, Att_BulletRange,Att_BodySize, Att_BulletSize, Att_CritChance, Att_CritEffect, Att_Accuracy, Att_Recoil,FB_Star, FB_EnemyAmount, FB_EnemyGene, FB_Frag, FB_NormalEnemy, FB_EliteEnemy,FB_EnemySpeed, FB_EnemyLife, FB_BasicUpgrade, FB_RareUpgrade, FB_EpicUpgrade)VALUES";
		int num = 1;
		string format = "({0}, {1},{2} , {3}, {4}, {5}, {6}, {7}, {8},";
		object[] array2 = new object[9];
		array2[0] = SteamApps.GetAppOwner().m_SteamID;
		array2[1] = this.String_DateForSql();
		array2[2] = inst.jobId;
		array2[3] = inst.varColorId;
		int num2 = 4;
		int modeType = (int)inst.modeType;
		array2[num2] = modeType.ToString();
		array2[5] = battle.SequalWave;
		array2[6] = playerDamage;
		array2[7] = time;
		array2[8] = hurt;
		array[num] = string.Format(format, array2);
		array[2] = string.Format("{0},{1},{2},{3},{4},{5},", new object[]
		{
			battle.listUpgradeInt.Count,
			battle.upgrade_NormalNum,
			battle.upgrade_RareNum,
			battle.upgrade_EpicNum,
			battle.diffiLevel,
			inst.GetDiffiOptionNum()
		});
		array[3] = string.Format("{0}, {1}, {2}, {3}, {4}, {5},", new object[]
		{
			factor.lifeMaxPlayer,
			factor.moveSpd,
			factor.fireSpd,
			factor.bulletDmg,
			factor.bulletSpd,
			factor.bulletRng
		});
		array[4] = string.Format("{0}, {1}, {2}, {3}, {4}, {5},", new object[]
		{
			factor.bodySize,
			factor.bulletSize,
			factor.critChc,
			factor.critDmg,
			factor.accuracy,
			factor.recoil
		});
		array[5] = string.Format("{0}, {1}, {2}, {3}, {4}, {5},", new object[]
		{
			factorBattleTotal.StarGain,
			factorBattleTotal.EnemyAmount,
			factorBattleTotal.EnemyGeneSpd,
			factorBattleTotal.FragDrop,
			factorBattleTotal.Enemy_NormalRate,
			factorBattleTotal.Enemy_EliteRate
		});
		array[6] = string.Format("{0}, {1}, {2}, {3}, {4}); ", new object[]
		{
			factorBattleTotal.Enemy_ModSpeed,
			factorBattleTotal.Enemy_ModLife,
			factorBattleTotal.Shop_NormalRate,
			factorBattleTotal.Shop_RareRate,
			factorBattleTotal.Shop_EpicRate
		});
		string text = string.Concat(array);
		Debug.Log(text);
		this.QuerySet(text);
	}

	// Token: 0x060002BA RID: 698 RVA: 0x000107D0 File Offset: 0x0000E9D0
	public void Insert_BattleEnd(double playerDamage, double time, double hurt, int waveCount, int diffiLevelTotal)
	{
		TempData inst = TempData.inst;
		GameData inst2 = GameData.inst;
		Battle battle = inst.battle;
		Player.inst.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle();
		FactorBattle factorBattleTotal = battle.factorBattleTotal;
		Debug.Log("INSERT INTO Data_BattleEnd(SteamID, InsertTime, JobID, VarColorID, Infinity, TalentAll, TalentThis, SkillLevel, ColorLevel,DiffiOptionNum, EndWave, WaveCount, TotalPlayerDamage, TotalWaveTime, TotalPlayerHurt, TotalDiffiLevel)VALUES" + string.Format("({0}, {1},{2} , {3}, {4},", new object[]
		{
			SteamApps.GetAppOwner().m_SteamID,
			this.String_DateForSql(),
			inst.jobId,
			inst.varColorId,
			(int)inst.modeType
		}) + string.Format(" {0}, {1}, {2}, {3},", new object[]
		{
			inst2.TalentTotalLevel_AllRole(),
			inst2.TalentTotalLevel_ThisRole(),
			inst2.jobs[inst.jobId].skillLevel,
			inst2.jobs[inst.jobId].colorLevel
		}) + string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}); ", new object[]
		{
			inst.GetDiffiOptionNum(),
			battle.SequalWave,
			waveCount,
			playerDamage,
			time,
			hurt,
			diffiLevelTotal
		}));
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00010938 File Offset: 0x0000EB38
	public void Replace_InfinityOrWander(double time, int waveCount, int diffiLevelTotal, EnumModeType modeType)
	{
		int num = 0;
		if (waveCount > 0)
		{
			num = ((float)diffiLevelTotal / (float)waveCount).RoundToInt();
		}
		TempData inst = TempData.inst;
		GameData inst2 = GameData.inst;
		Battle battle = inst.battle;
		GameParameters inst3 = GameParameters.Inst;
		string text;
		if (modeType != EnumModeType.ENDLESS)
		{
			if (modeType != EnumModeType.WANDER)
			{
				Debug.LogError("Error_ModeTypeError!");
				return;
			}
			string str = "Replace INTO RankList_Wander(GameVersion,Unikey, SteamID,SteamName,InsertTime,JobID,VarColorID ,WaveCount,Stars,DOmulti,DLaverage,UpgradeAll,UpgradeRank0,UpgradeRank1,UpgradeRank2,UpgradeRank3,TalentThis,TalentAll,FragmentCount,TotalWaveTime,FragmentLeft,FragmentUsed)";
			string format = "VALUES({0}, {1}, {2}, '{3}',";
			object[] array = new object[4];
			array[0] = battle.battle_EnterVersion;
			int num2 = 1;
			CSteamID appOwner = SteamApps.GetAppOwner();
			array[num2] = appOwner.m_SteamID.ToString() + inst.jobId.ToString();
			array[2] = SteamApps.GetAppOwner().m_SteamID;
			array[3] = MySqlAccess.GetString_SteamName();
			text = str + string.Format(format, array) + string.Format(" {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, ", new object[]
			{
				this.String_DateForSql(),
				inst.jobId,
				inst.varColorId,
				waveCount,
				battle.Score,
				this.GetString_DifficultyOption(battle),
				num,
				battle.listUpgradeInt.Count,
				battle.upgrade_NormalNum,
				battle.upgrade_RareNum,
				battle.upgrade_EpicNum,
				battle.upgrade_LegendNum
			}) + string.Format("{0}, {1}, {2}, {3},{4},{5}); ", new object[]
			{
				inst2.TalentTotalLevel_ThisRole(),
				inst2.TalentTotalLevel_AllRole(),
				battle.fragmentTotal,
				((float)time / 60f).RoundToInt(),
				battle.Fragment,
				battle.FragmentUsed
			});
		}
		else
		{
			string str2 = "Replace INTO RankList_Infinity(GameVersion,Unikey, SteamID,SteamName,InsertTime,JobID,VarColorID ,WaveCount,Stars,DOmulti,DLaverage,UpgradeAll,UpgradeRank0,UpgradeRank1,UpgradeRank2,UpgradeRank3,TalentThis,TalentAll,FragmentCount,TotalWaveTime,FragmentLeft,FragmentUsed)";
			string format2 = "VALUES({0}, {1}, {2}, '{3}',";
			object[] array2 = new object[4];
			array2[0] = battle.battle_EnterVersion;
			int num3 = 1;
			CSteamID appOwner = SteamApps.GetAppOwner();
			array2[num3] = appOwner.m_SteamID.ToString() + inst.jobId.ToString();
			array2[2] = SteamApps.GetAppOwner().m_SteamID;
			array2[3] = MySqlAccess.GetString_SteamName();
			text = str2 + string.Format(format2, array2) + string.Format(" {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, ", new object[]
			{
				this.String_DateForSql(),
				inst.jobId,
				inst.varColorId,
				waveCount,
				battle.Score,
				this.GetString_DifficultyOption(battle),
				num,
				battle.listUpgradeInt.Count,
				battle.upgrade_NormalNum,
				battle.upgrade_RareNum,
				battle.upgrade_EpicNum,
				battle.upgrade_LegendNum
			}) + string.Format("{0}, {1}, {2}, {3},{4},{5}); ", new object[]
			{
				inst2.TalentTotalLevel_ThisRole(),
				inst2.TalentTotalLevel_AllRole(),
				battle.fragmentTotal,
				((float)time / 60f).RoundToInt(),
				battle.Fragment,
				battle.FragmentUsed
			});
		}
		Debug.Log(text);
		this.QuerySet(text);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00010CB0 File Offset: 0x0000EEB0
	public void Replace_Daily(double time, int waveCount, int diffiLevelTotal, int dayIndex)
	{
		if (waveCount < 1)
		{
			waveCount = 1;
		}
		int num = ((float)diffiLevelTotal / (float)waveCount).RoundToInt();
		TempData inst = TempData.inst;
		GameData inst2 = GameData.inst;
		Battle battle = inst.battle;
		GameParameters inst3 = GameParameters.Inst;
		string text = "Replace INTO RankList_Daily(GameVersion,Unikey, SteamID,SteamName,InsertTime,JobID,VarColorID ,WaveCount,Stars,DOmulti,DLaverage,UpgradeAll,TalentThis,TalentAll,FragmentCount,TotalWaveTime,DayIndex,StringDO)" + string.Format("VALUES({0}, {1}, {2}, '{3}',", new object[]
		{
			battle.battle_EnterVersion,
			SqlManager.GetString_UniCode_DailyChallenge(TempData.inst.daily_Index),
			SteamApps.GetAppOwner().m_SteamID,
			MySqlAccess.GetString_SteamName()
		}) + string.Format(" {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, ", new object[]
		{
			this.String_DateForSql(),
			inst.jobId,
			inst.varColorId,
			waveCount,
			battle.Score,
			this.GetString_DifficultyOption(battle),
			num,
			battle.listUpgradeInt.Count
		}) + string.Format("{0}, {1}, {2}, {3},{4},'{5}'); ", new object[]
		{
			inst2.TalentTotalLevel_ThisRole(),
			inst2.TalentTotalLevel_AllRole(),
			battle.fragmentTotal,
			((float)time / 60f).RoundToInt(),
			dayIndex,
			MyTool.BoolsToStringDO(TempData.inst.diffiOptFlag)
		});
		Debug.Log(text);
		this.QuerySet(text);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00010E2C File Offset: 0x0000F02C
	private string GetString_DifficultyOption(Battle battle)
	{
		return battle.factorBattle_FromDifficultyOption.StarGain.ToString().Replace(',', '.');
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00010E58 File Offset: 0x0000F058
	public void Insert_AppQuit(double dpsTotal, double time, double hurt, int waveCount, int diffiLevelTotal, int diffiOptinoTotal, int waveTotal)
	{
		GameData inst = GameData.inst;
		string text = "INSERT INTO Data_Game(SteamID, InsertTime, BattleCount, TotalDps, TotalWaveTime, TotalPlayerHurt, TotalDiffiLevel,TotalDiffiOption,TalentAll,WaveCount)VALUES" + string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6},{7},{8},{9}); ", new object[]
		{
			SteamApps.GetAppOwner().m_SteamID,
			this.String_DateForSql(),
			waveCount,
			dpsTotal,
			time,
			hurt,
			diffiLevelTotal,
			diffiOptinoTotal,
			inst.TalentTotalLevel_AllRole(),
			waveTotal
		});
		Debug.Log(text);
		this.QuerySet(text);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00010F04 File Offset: 0x0000F104
	public string String_DateForSql()
	{
		DateTime now = DateTime.Now;
		return string.Concat(new object[]
		{
			"'",
			now.Year,
			"/",
			now.Month,
			"/",
			now.Day,
			" ",
			now.Hour,
			":",
			now.Minute,
			":",
			now.Second,
			"'"
		});
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00010FB9 File Offset: 0x0000F1B9
	public void QuerySet(string sqlStr)
	{
		MySqlAccess.sqlString = sqlStr;
		this.SubThread_QuerySet();
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00010FC8 File Offset: 0x0000F1C8
	public void SubThread_QuerySet()
	{
		if (!Setting.Inst.Option_Network)
		{
			Debug.LogError("Error_网络功能已关闭！");
			return;
		}
		if (this.mySqlConnection.State == ConnectionState.Open)
		{
			DataSet dataSet = new DataSet();
			try
			{
				new MySqlDataAdapter(MySqlAccess.sqlString, this.mySqlConnection)
				{
					SelectCommand = 
					{
						CommandTimeout = 9
					}
				}.Fill(dataSet);
			}
			catch (Exception ex)
			{
				throw new Exception("SQL:" + MySqlAccess.sqlString + "/n" + ex.Message.ToString());
			}
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001105C File Offset: 0x0000F25C
	public static string GetString_SteamName()
	{
		return SteamFriends.GetFriendPersonaName(SteamApps.GetAppOwner()).Replace("'", "\\'");
	}

	// Token: 0x04000288 RID: 648
	public MySqlConnection mySqlConnection;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	private string host;

	// Token: 0x0400028A RID: 650
	[SerializeField]
	private string port;

	// Token: 0x0400028B RID: 651
	[SerializeField]
	private string userName;

	// Token: 0x0400028C RID: 652
	[SerializeField]
	private string password;

	// Token: 0x0400028D RID: 653
	[SerializeField]
	private string databaseName;

	// Token: 0x0400028E RID: 654
	public static string sqlString;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	private bool canOperateFlag = true;

	// Token: 0x04000290 RID: 656
	public string savefileID;
}
