using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using MySql.Data.MySqlClient;
using Steamworks;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class SqlManager : MonoBehaviour
{
	// Token: 0x060002C4 RID: 708 RVA: 0x00011086 File Offset: 0x0000F286
	private void Awake()
	{
		SqlManager.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
		this.NewOrLoad();
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0001109F File Offset: 0x0000F29F
	private MySqlAccess GetMySqlAccess()
	{
		return this.mysql;
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x000110A7 File Offset: 0x0000F2A7
	public void OpenSqlNow()
	{
		if (this.mysql == null)
		{
			this.mysql = new MySqlAccess();
		}
		this.mysql.InitAndOpen(this.host, this.port, this.userName, this.password, this.databaseName);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x000110E5 File Offset: 0x0000F2E5
	private void Update()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			this.battle_TimeTotal += (double)Time.unscaledDeltaTime;
		}
		this.deltaTimeFromlastConnection += Time.unscaledDeltaTime;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000051D0 File Offset: 0x000033D0
	public void SqlDataInsert_OnWaveStart()
	{
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00011119 File Offset: 0x0000F319
	public void Thread_WaveEnd()
	{
		this.battle_WaveCount++;
		this.battle_DiffiLevelTotal += Battle.inst.diffiLevel;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00011140 File Offset: 0x0000F340
	public void Thread_BattleEnd()
	{
		Debug.Log("Thread_准备开始BattleEnd数据线程");
		new Thread(new ThreadStart(this.SqlDataInsert_OnBattleEnd)).Start();
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00011164 File Offset: 0x0000F364
	private void SqlDataInsert_OnBattleEnd()
	{
		if (!Setting.Inst.Option_Network)
		{
			Debug.LogError("Error_网络功能已关闭！");
			UI_Panel_Battle_BattleAward.SetFlag_Both(1);
			return;
		}
		try
		{
			this.OpenSqlNow();
		}
		catch (Exception ex)
		{
			Debug.Log("Error_数据库连接失败！" + ex.Message.ToString());
			UI_Panel_Battle_BattleAward.SetFlag_Both(2);
			return;
		}
		if (this.GetMySqlAccess() == null)
		{
			Debug.Log("Error_数据库连接失败！");
			UI_Panel_Battle_BattleAward.SetFlag_Both(2);
			return;
		}
		EnumModeType modeType = TempData.inst.modeType;
		if (modeType - EnumModeType.ENDLESS <= 1)
		{
			new Thread(new ThreadStart(this.RankList_InfinityOrWander)).Start();
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001120C File Offset: 0x0000F40C
	public static string GetString_UniCode_DailyChallenge(int dayIndex)
	{
		if (!TempData.inst.daily_Open)
		{
			Debug.LogError("Error_每日挑战未开启");
		}
		return SteamApps.GetAppOwner().m_SteamID.ToString() + dayIndex.ToString();
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00011250 File Offset: 0x0000F450
	public static string GetString_SteamID()
	{
		return SteamApps.GetAppOwner().m_SteamID.ToString();
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00011270 File Offset: 0x0000F470
	private void RankList_InfinityOrWander()
	{
		if (!Setting.Inst.Option_Network)
		{
			Debug.LogError("Error_网络功能已关闭！");
			UI_Panel_Battle_BattleAward.SetFlag_Both(1);
			return;
		}
		try
		{
			CSteamID appOwner = SteamApps.GetAppOwner();
			appOwner.m_SteamID.ToString();
		}
		catch
		{
			Debug.LogError("Error_未登陆Steam");
			UI_Panel_Battle_BattleAward.SetFlag_Both(4);
			return;
		}
		int jobId = TempData.inst.jobId;
		bool flag = TempData.inst.daily_Open;
		int daily_Index = TempData.inst.daily_Index;
		if (flag & daily_Index != NetworkTime.Inst.dayIndex)
		{
			UI_Panel_Battle_BattleAward.UploadSuccess_Daily(3);
			Debug.LogWarning("Warning_挑战超时，不计入排行榜");
			flag = false;
		}
		this.OpenSqlNow();
		if (this.mysql.mySqlConnection.State == ConnectionState.Open)
		{
			Debug.Log("SQL 1 上传俩排行榜");
			EnumModeType modeType;
			if (Battle.inst.battle_EnterVersion < 10000)
			{
				Debug.LogWarning("Warning_存档版本号过低，不计入排行榜");
				UI_Panel_Battle_BattleAward.UploadSuccess_Mode(3);
			}
			else
			{
				Debug.Log("SQL 1.1 上传_模式的排行榜记录");
				modeType = TempData.inst.modeType;
				string text;
				if (modeType != EnumModeType.ENDLESS)
				{
					if (modeType != EnumModeType.WANDER)
					{
						Debug.LogError("Error_LeaderboardDataUploadingModeError!");
						return;
					}
					string str = "SELECT * FROM RankList_Wander WHERE Unikey = '";
					CSteamID appOwner = SteamApps.GetAppOwner();
					text = str + appOwner.m_SteamID.ToString() + jobId.ToString() + "' ;";
				}
				else
				{
					string str2 = "SELECT * FROM RankList_Infinity WHERE Unikey = '";
					CSteamID appOwner = SteamApps.GetAppOwner();
					text = str2 + appOwner.m_SteamID.ToString() + jobId.ToString() + "' ;";
				}
				DataSet dataSet = new DataSet();
				try
				{
					new MySqlDataAdapter(text, this.mysql.mySqlConnection).Fill(dataSet);
					DataTable dataTable = dataSet.Tables[0];
					if (dataTable.Rows.Count == 0)
					{
						Debug.Log("排行榜_当前模式_无记录_新建记录");
						UI_Panel_Battle_BattleAward.UploadSuccess_Mode(2);
						this.mysql.Replace_InfinityOrWander(this.battle_TimeTotal, TempData.inst.battle.SequalWave - 1, this.battle_DiffiLevelTotal, TempData.inst.modeType);
					}
					else
					{
						long @long = dataTable.Rows[0].GetLong(9);
						long score = Battle.inst.Score;
						if (score >= @long)
						{
							Debug.Log(string.Format("数据库_当前模式_新记录{0}to{1}", @long, score));
							UI_Panel_Battle_BattleAward.UploadSuccess_Mode(1);
							this.mysql.Replace_InfinityOrWander(this.battle_TimeTotal, TempData.inst.battle.SequalWave - 1, this.battle_DiffiLevelTotal, TempData.inst.modeType);
						}
						else
						{
							Debug.Log(string.Format("数据库_当前模式_未破记录{0}>{1}", @long, score));
							UI_Panel_Battle_BattleAward.UploadSuccess_Mode(0);
						}
					}
				}
				catch (Exception ex)
				{
					SqlManager.inst.mysql = null;
					Debug.Log("Error_模式数据库连接失败！跳出 以下是报错");
					UI_Panel_Battle_BattleAward.SetFlag_Both(2);
					throw new Exception("SQL:" + text + "/n" + ex.Message.ToString());
				}
			}
			if (flag)
			{
				Debug.Log("SQL 1.2 上传_模式的排行榜记录");
				string text2 = "SELECT * FROM RankList_Daily WHERE Unikey = '" + SqlManager.GetString_UniCode_DailyChallenge(daily_Index) + "' ;";
				DataSet dataSet2 = new DataSet();
				try
				{
					new MySqlDataAdapter(text2, this.mysql.mySqlConnection).Fill(dataSet2);
					DataTable dataTable2 = dataSet2.Tables[0];
					if (dataTable2.Rows.Count == 0)
					{
						Debug.Log("排行榜_每日挑战_无记录_新建记录");
						UI_Panel_Battle_BattleAward.UploadSuccess_Daily(2);
						this.mysql.Replace_Daily(this.battle_TimeTotal, TempData.inst.battle.SequalWave - 1, this.battle_DiffiLevelTotal, daily_Index);
					}
					else
					{
						long long2 = dataTable2.Rows[0].GetLong(9);
						long score2 = Battle.inst.Score;
						if (score2 >= long2)
						{
							Debug.Log(string.Format("数据库_每日挑战_新记录{0}to{1}", long2, score2));
							UI_Panel_Battle_BattleAward.UploadSuccess_Daily(1);
							this.mysql.Replace_Daily(this.battle_TimeTotal, TempData.inst.battle.SequalWave - 1, this.battle_DiffiLevelTotal, daily_Index);
						}
						else
						{
							Debug.Log(string.Format("数据库_每日挑战_未破记录{0}>{1}", long2, score2));
							UI_Panel_Battle_BattleAward.UploadSuccess_Daily(0);
						}
					}
				}
				catch (Exception ex2)
				{
					SqlManager.inst.mysql = null;
					Debug.Log("Error_每日数据库连接失败！跳出 以下是报错");
					UI_Panel_Battle_BattleAward.SetFlag_Daily(2);
					throw new Exception("SQL:" + text2 + "\n" + ex2.Message.ToString());
				}
			}
			Debug.Log("SQL 2 开始找我的名次和记录");
			Debug.Log("SQL 2.1 找我的模式名次和记录");
			modeType = TempData.inst.modeType;
			string text3;
			if (modeType != EnumModeType.ENDLESS)
			{
				if (modeType != EnumModeType.WANDER)
				{
					Debug.LogError("Error_排行榜_模式index有误");
					return;
				}
				text3 = string.Format("SELECT * FROM RankList_Wander WHERE JobID = {0} ORDER BY Stars DESC;", TempData.inst.jobId);
			}
			else
			{
				text3 = string.Format("SELECT * FROM RankList_Infinity WHERE JobID = {0} ORDER BY Stars DESC;", TempData.inst.jobId);
			}
			try
			{
				DataAdapter dataAdapter = new MySqlDataAdapter(text3, this.mysql.mySqlConnection);
				DataSet dataSet3 = new DataSet();
				dataAdapter.Fill(dataSet3);
				DataTable dataTable3 = dataSet3.Tables[0];
				int count = dataTable3.Rows.Count;
				CSteamID appOwner = SteamApps.GetAppOwner();
				string b = appOwner.m_SteamID.ToString() + TempData.inst.jobId;
				int num = -1;
				long num2 = -1L;
				for (int i = 0; i < count; i++)
				{
					DataRow d = dataTable3.Rows[i];
					if (d.GetString(2) == b)
					{
						num = i + 1;
						num2 = d.GetLong(9);
						break;
					}
				}
				if (num == -1 || num2 == -1L)
				{
					Debug.LogError("Error_模式咋没我成绩呢");
				}
				UI_Panel_Battle_BattleAward.FindMyRecord_Mode(num, num2);
			}
			catch (Exception ex3)
			{
				SqlManager.inst.mysql = null;
				Debug.Log("Error_模式数据库连接失败！（找自己成绩时） 跳出 以下是报错");
				UI_Panel_Battle_BattleAward.SetFlag_Mode(3);
				throw new Exception("SQL:" + text3 + "/n" + ex3.Message.ToString());
			}
			if (flag)
			{
				Debug.Log("SQL 2.2 找我的每日名次和记录");
				string text4 = string.Format("SELECT * FROM RankList_Daily WHERE DayIndex = {0} ORDER BY Stars DESC;", daily_Index);
				try
				{
					DataAdapter dataAdapter2 = new MySqlDataAdapter(text4, this.mysql.mySqlConnection);
					DataSet dataSet4 = new DataSet();
					dataAdapter2.Fill(dataSet4);
					DataTable dataTable4 = dataSet4.Tables[0];
					int count2 = dataTable4.Rows.Count;
					string string_UniCode_DailyChallenge = SqlManager.GetString_UniCode_DailyChallenge(daily_Index);
					int num3 = -1;
					long num4 = -1L;
					for (int j = 0; j < count2; j++)
					{
						DataRow d2 = dataTable4.Rows[j];
						if (d2.GetString(2) == string_UniCode_DailyChallenge)
						{
							num3 = j + 1;
							num4 = d2.GetLong(9);
							break;
						}
					}
					if (num3 == -1 || num4 == -1L)
					{
						Debug.LogError("Error_每日咋没我成绩呢");
					}
					UI_Panel_Battle_BattleAward.FindMyRecord_Daily(num3, num4);
				}
				catch (Exception ex4)
				{
					SqlManager.inst.mysql = null;
					Debug.Log("Error_每日数据库连接失败！（找自己成绩时） 跳出 以下是报错");
					UI_Panel_Battle_BattleAward.SetFlag_Daily(3);
					throw new Exception("SQL:" + text4 + "/n" + ex4.Message.ToString());
				}
			}
			this.InitDataBattle();
			return;
		}
		Debug.Log("Error_数据库连接失败！ 原因是SQL没开，所以啥步骤都没执行");
		UI_Panel_Battle_BattleAward.SetFlag_Both(2);
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000119F0 File Offset: 0x0000FBF0
	private void InitDataBattle()
	{
		this.battle_WaveCount = 0;
		this.battle_TimeTotal = 0.0;
		this.battle_DiffiLevelTotal = 0;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00011A10 File Offset: 0x0000FC10
	public void NewOrLoad()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			return;
		}
		if (!SaveFile.IfExistSaveByJson())
		{
			this.InitDataBattle();
			return;
		}
		SaveFile saveFile = GameData.SaveFile;
		if (!saveFile.ifOnBattle)
		{
			this.InitDataBattle();
			return;
		}
		this.battle_WaveCount = saveFile.Sql_Battle_WaveCount;
		this.battle_TimeTotal = saveFile.Sql_Battle_TimeTotal;
		this.battle_DiffiLevelTotal = saveFile.Sql_Battle_DiffiLevelTotal;
	}

	// Token: 0x04000291 RID: 657
	public static SqlManager inst;

	// Token: 0x04000292 RID: 658
	[Header("数据库信息")]
	public string host;

	// Token: 0x04000293 RID: 659
	public string port;

	// Token: 0x04000294 RID: 660
	public string userName;

	// Token: 0x04000295 RID: 661
	public string password;

	// Token: 0x04000296 RID: 662
	public string databaseName;

	// Token: 0x04000297 RID: 663
	[Header("数据库访问类")]
	public MySqlAccess mysql;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	public float deltaTimeFromlastConnection;

	// Token: 0x04000299 RID: 665
	[Header("Battle")]
	public int battle_WaveCount;

	// Token: 0x0400029A RID: 666
	public double battle_TimeTotal;

	// Token: 0x0400029B RID: 667
	public int battle_DiffiLevelTotal;
}
