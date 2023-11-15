using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class GameData : MonoBehaviour
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000205 RID: 517 RVA: 0x0000C6DC File Offset: 0x0000A8DC
	public static SaveFile SaveFile
	{
		get
		{
			if (GameData.saveFile == null)
			{
				GameData.saveFile = SaveFile.ReadByJson();
			}
			return GameData.saveFile;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000207 RID: 519 RVA: 0x0000C710 File Offset: 0x0000A910
	// (set) Token: 0x06000206 RID: 518 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
	public long Star
	{
		get
		{
			return (19960614L - this.starEnc) / 140933L;
		}
		set
		{
			this.starEnc = 19960614L - value * 140933L;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000209 RID: 521 RVA: 0x0000C747 File Offset: 0x0000A947
	// (set) Token: 0x06000208 RID: 520 RVA: 0x0000C72B File Offset: 0x0000A92B
	public long GeometryCoin
	{
		get
		{
			return (19960614L - this.geometryCoinEnc) / 140933L;
		}
		set
		{
			this.geometryCoinEnc = 19960614L - value * 140933L;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600020A RID: 522 RVA: 0x0000C764 File Offset: 0x0000A964
	public Rune CurrentRune
	{
		get
		{
			int num = this.currentRunesIndex[0];
			if (num >= 0 && num < this.runes.Count)
			{
				return this.runes[num];
			}
			return null;
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000C79A File Offset: 0x0000A99A
	private void Awake()
	{
		GameData.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
		if (SaveFile.IsNewSaveSlot())
		{
			this.NewSaveSlot();
		}
		else
		{
			this.LoadSaveSlot();
		}
		this.setting = Setting.NewOrLoad();
		MySteamAchievement.EnterGame();
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000C7D2 File Offset: 0x0000A9D2
	private void Update()
	{
		if (this.runeStore != null)
		{
			this.runeStore.RefreshTime_TryCountDownInUpdate();
			return;
		}
		Debug.LogError("Error_RuneStore==null!");
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
	public void UseStar(int num)
	{
		if ((long)num > this.Star)
		{
			Debug.LogError("Star不足");
			return;
		}
		this.Star -= (long)num;
		this.starUsed += (long)num;
		MySteamAchievement.AddStatInt("acc_StarUsed", num);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000C849 File Offset: 0x0000AA49
	public int GetTalentLevel(int jobId, int talentID)
	{
		return this.jobs[jobId].TalentLevels[talentID];
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000C85A File Offset: 0x0000AA5A
	public void GetStar(long num)
	{
		if (num < 0L)
		{
			Debug.LogError("GetStarNum<0!");
		}
		this.Star += num;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000C87C File Offset: 0x0000AA7C
	public void GeometryCoin_Get(long num)
	{
		if (num < 0L)
		{
			Debug.LogError("GetGCoin<0!");
		}
		this.GeometryCoin += num;
		this.geometryCoinTotal += num;
		MySteamAchievement.AddStatInt("acc_GeometryCoins", (int)num);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000C8CA File Offset: 0x0000AACA
	public void GeometryCoin_Use(long num)
	{
		if (num < 0L)
		{
			Debug.LogError("UseGCoin<0!");
		}
		this.GeometryCoin -= num;
		this.geometryCoinUsed += num;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000C904 File Offset: 0x0000AB04
	public void NewSaveSlot()
	{
		this.Star = 0L;
		this.starUsed = 0L;
		this.starTotal = 0L;
		this.GeometryCoin = 0L;
		this.geometryCoinUsed = 0L;
		this.geometryCoinTotal = 0L;
		this.ifFinished = false;
		this.runes = new List<Rune>();
		this.runeStore = new RuneStore();
		this.runeStore.TryInit();
		this.currentRunesIndex = new int[1];
		this.currentRunesIndex[0] = -1;
		this.InitJobsArray();
		this.InitJobsTalentsArray();
		this.record.NewRecord();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000C9AC File Offset: 0x0000ABAC
	public void LoadSaveSlot()
	{
		if (!SaveFile.IfExistSaveByJson())
		{
			Debug.LogError("没有存档！");
			return;
		}
		this.InitJobsArray();
		this.InitJobsTalentsArray();
		SaveFile saveFile = GameData.SaveFile;
		this.Star = saveFile.star;
		this.starUsed = saveFile.starUsed;
		this.maxEndless = saveFile.maxEndless;
		if (saveFile.version <= 215)
		{
			Debug.LogWarning("Warning_更新星星记录");
			this.starTotal = saveFile.star + saveFile.starUsed;
			if (saveFile.ifOnBattle)
			{
				this.starTotal += saveFile.battle_Score;
			}
		}
		else
		{
			this.starTotal = saveFile.starTotal;
		}
		this.GeometryCoin = saveFile.geometryCoin;
		this.geometryCoinTotal = saveFile.geometryCoinTotal;
		this.geometryCoinUsed = saveFile.geometryCoinUsed;
		this.runes = new List<Rune>();
		if (saveFile.runes != null)
		{
			for (int i = 0; i < saveFile.runes.Count; i++)
			{
				Rune.AddRune(saveFile.runes[i]);
			}
		}
		this.currentRunesIndex = new int[1];
		this.currentRunesIndex[0] = -1;
		if (saveFile.currentRunesIndex != null)
		{
			if (saveFile.currentRunesIndex.Length == 0)
			{
				this.currentRunesIndex[0] = -1;
			}
			else if (saveFile.currentRunesIndex[0] != -1)
			{
				this.currentRunesIndex[0] = saveFile.currentRunesIndex[0];
			}
			else
			{
				this.currentRunesIndex[0] = -1;
			}
		}
		this.runeStore = saveFile.runeStore;
		if (this.runeStore == null)
		{
			this.runeStore = new RuneStore();
		}
		this.runeStore.TryInit();
		if (saveFile.version <= 207)
		{
			long num = saveFile.starUsed;
			if (num > 2147483646L)
			{
				num = 2147483646L;
			}
			MySteamAchievement.SetStatInt("acc_StarUsed", (int)num);
		}
		if (saveFile.version <= 404)
		{
			long num2 = saveFile.geometryCoinTotal;
			if (this.geometryCoinTotal > 2147483646L)
			{
				this.geometryCoinTotal = 2147483646L;
			}
			MySteamAchievement.SetStatInt("acc_GeometryCoins", (int)this.geometryCoinTotal);
		}
		this.ifFinished = saveFile.ifFinished;
		int num3 = Mathf.Min(this.jobs.Length, saveFile.file_Jobs.Length);
		for (int j = 0; j < num3; j++)
		{
			int num4 = DataBase.Inst.DataPlayerModels[j].talents.Length;
			int num5 = Mathf.Min(num4, saveFile.file_Jobs[j].talentLevels.Length);
			File_Job file_Job = this.jobs[j];
			file_Job.mastery.exps = saveFile.file_Jobs[j].mastery.exps;
			int[] array = new int[num4];
			for (int k = 0; k < num5; k++)
			{
				array[k] = saveFile.file_Jobs[j].talentLevels[k];
			}
			file_Job.SetTalentLevels(array);
		}
		if (saveFile.version <= 215)
		{
			this.GeometryCoin_Get((long)((double)Mastery.GetExpTotal() * 0.18));
		}
		this.record.Clone(saveFile.record);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000CCD0 File Offset: 0x0000AED0
	private void InitJobsArray()
	{
		this.jobs = new File_Job[DataBase.Inst.DataPlayerModels.Length];
		for (int i = 0; i < this.jobs.Length; i++)
		{
			this.jobs[i] = new File_Job(i);
			this.jobs[i].mastery.exps = 0L;
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000CD2C File Offset: 0x0000AF2C
	private void InitJobsTalentsArray()
	{
		for (int i = 0; i < this.jobs.Length; i++)
		{
			File_Job file_Job = this.jobs[i];
			int[] array = new int[file_Job.TalentLevels.Length];
			for (int j = 0; j < file_Job.TalentLevels.Length; j++)
			{
				Talent talent = DataBase.GetTalent(i, j);
				array[j] = talent.originLevel;
			}
			file_Job.SetTalentLevels(array);
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000CD90 File Offset: 0x0000AF90
	public bool IfJobUnlocked(int jobId)
	{
		return (!GameParameters.Inst.ifDemo || (jobId != 2 && jobId != 5 && jobId != 8)) && DataBase.Inst.DataPlayerModels[jobId].ifUnlocked();
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
	public bool IfColorUnlockedToCurJob(int colorId)
	{
		int rank = (int)DataBase.Inst.Data_VarColors[colorId].rank;
		return this.jobs[TempData.inst.jobId].colorLevel >= rank;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000CE00 File Offset: 0x0000B000
	public int TalentTotalLevel_ThisRole()
	{
		int jobId = TempData.inst.jobId;
		int num = 0;
		for (int i = 0; i < this.jobs[jobId].TalentLevels.Length; i++)
		{
			if (i == this.jobs[jobId].TalentLevels.Length - 1)
			{
				num += this.jobs[jobId].TalentLevels[i] / 4;
			}
			else if (i >= 2)
			{
				num += this.jobs[jobId].TalentLevels[i];
			}
		}
		return num;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000CE78 File Offset: 0x0000B078
	public int TalentTotalLevel_AllRole()
	{
		int num = 0;
		for (int i = 0; i < DataBase.Inst.DataPlayerModels.Length; i++)
		{
			for (int j = 0; j < this.jobs[i].TalentLevels.Length; j++)
			{
				if (j == this.jobs[i].TalentLevels.Length - 1)
				{
					num += this.jobs[i].TalentLevels[j] / 4;
				}
				else if (j >= 2)
				{
					num += this.jobs[i].TalentLevels[j];
				}
			}
		}
		return num;
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000CEF9 File Offset: 0x0000B0F9
	public static int Convert_OldDLtoNewDL(int oldDL)
	{
		if (oldDL <= 0)
		{
			return 0;
		}
		if (oldDL > 23)
		{
			Debug.LogError("Error_OldDL>23??");
			return 17;
		}
		if (oldDL < 12)
		{
			return oldDL / 2;
		}
		return oldDL - 6;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000CF1F File Offset: 0x0000B11F
	public static int Convert_NewDLtoOldDL(int newDL)
	{
		if (newDL <= 0)
		{
			return 0;
		}
		if (newDL > 17)
		{
			Debug.LogError("Error_OldDL>17??");
			return 23;
		}
		if (newDL >= 6)
		{
			return newDL + 6;
		}
		return newDL * 2;
	}

	// Token: 0x040001C2 RID: 450
	public static GameData inst;

	// Token: 0x040001C3 RID: 451
	public static SaveFile saveFile;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private ObscuredLong starEnc = 1000L;

	// Token: 0x040001C5 RID: 453
	public ObscuredLong starUsed = 0L;

	// Token: 0x040001C6 RID: 454
	public ObscuredLong starTotal = 0L;

	// Token: 0x040001C7 RID: 455
	[Header("几何币")]
	[SerializeField]
	private ObscuredLong geometryCoinEnc = 19960614L;

	// Token: 0x040001C8 RID: 456
	public ObscuredLong geometryCoinUsed = 0L;

	// Token: 0x040001C9 RID: 457
	public ObscuredLong geometryCoinTotal = 0L;

	// Token: 0x040001CA RID: 458
	public bool ifFinished;

	// Token: 0x040001CB RID: 459
	public int maxEndless;

	// Token: 0x040001CC RID: 460
	[Header("角色")]
	[SerializeField]
	public File_Job[] jobs;

	// Token: 0x040001CD RID: 461
	[Header("符文")]
	[SerializeField]
	public List<Rune> runes = new List<Rune>();

	// Token: 0x040001CE RID: 462
	public int[] currentRunesIndex = new int[]
	{
		-1
	};

	// Token: 0x040001CF RID: 463
	public bool ifOnFusion;

	// Token: 0x040001D0 RID: 464
	public int[] runeFusion_MaterialIndexs = new int[]
	{
		-1,
		-1
	};

	// Token: 0x040001D1 RID: 465
	public Rune runeFusion_Result;

	// Token: 0x040001D2 RID: 466
	[Header("设置")]
	[SerializeField]
	public Setting setting = new Setting();

	// Token: 0x040001D3 RID: 467
	[Header("记录")]
	[SerializeField]
	public Record record = new Record();

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	public RuneStore runeStore = new RuneStore();
}
