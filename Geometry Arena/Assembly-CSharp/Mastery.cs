using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[Serializable]
public class Mastery
{
	// Token: 0x06000223 RID: 547 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
	public int GetRank()
	{
		int a = 0;
		for (int i = 0; i < 100; i++)
		{
			long num = Mastery.ExpAllNeedTheRank(i);
			if (this.exps < num)
			{
				break;
			}
			a = i;
		}
		return Mathf.Min(a, 10);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000D0E0 File Offset: 0x0000B2E0
	public long GetRestExp()
	{
		long num = Mastery.ExpAllNeedTheRank(this.GetRank());
		return this.exps - num;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000D104 File Offset: 0x0000B304
	public float GetPercent()
	{
		long num = Mastery.ExpToUpgradeNeedToNextRank(this.GetRank());
		return (float)this.GetRestExp() / (float)num;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000D128 File Offset: 0x0000B328
	public string GetString_Progress()
	{
		int rank = this.GetRank();
		long num = Mastery.ExpToUpgradeNeedToNextRank(rank);
		long restExp = this.GetRestExp();
		if (rank >= 10)
		{
			return "MAX";
		}
		return restExp + "/" + num;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000D169 File Offset: 0x0000B369
	public static long ExpAllNeedTheRank(int rank)
	{
		return (Mastery.ExpToUpgradeNeedToNextRank(0) + Mastery.ExpToUpgradeNeedToNextRank(rank - 1)) * (long)rank / 2L;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000D180 File Offset: 0x0000B380
	public static long ExpToUpgradeNeedToNextRank(int rank)
	{
		return (long)(10 * (rank + 1));
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000D18C File Offset: 0x0000B38C
	public static void CurrentJobGainExps(int expNum)
	{
		int jobId = TempData.inst.jobId;
		GameData.inst.jobs[jobId].mastery.exps += (long)expNum;
		Debug.Log(DataBase.Inst.DataPlayerModels[jobId].dataName + "getExp" + expNum);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000D1EC File Offset: 0x0000B3EC
	public static int GetRankTotal()
	{
		File_Job[] jobs = GameData.inst.jobs;
		int num = 0;
		for (int i = 0; i < jobs.Length; i++)
		{
			num += jobs[i].mastery.GetRank();
		}
		return num;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000D228 File Offset: 0x0000B428
	public static long GetExpTotal()
	{
		File_Job[] jobs = GameData.inst.jobs;
		long num = 0L;
		for (int i = 0; i < jobs.Length; i++)
		{
			long num2 = jobs[i].mastery.exps;
			if (num2 < 0L)
			{
				num2 = 0L;
			}
			else if (num2 > 550L)
			{
				num2 = 550L;
			}
			num += num2;
		}
		return num;
	}

	// Token: 0x040001E1 RID: 481
	[SerializeField]
	public long exps;
}
