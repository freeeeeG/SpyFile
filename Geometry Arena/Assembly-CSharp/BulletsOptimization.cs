using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class BulletsOptimization
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x00011A72 File Offset: 0x0000FC72
	public static long maxBulletNum
	{
		get
		{
			return MyTool.DoubleToLong(270.0 * BulletsOptimization.SettingPercent());
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00011A88 File Offset: 0x0000FC88
	private static double SettingPercent()
	{
		double setFloat_BulletOptimizeVolume = Setting.Inst.SetFloat_BulletOptimizeVolume;
		double num;
		if (setFloat_BulletOptimizeVolume <= 0.5)
		{
			num = 10.0 * setFloat_BulletOptimizeVolume + 1.0;
		}
		else
		{
			num = 24.0 * setFloat_BulletOptimizeVolume - 6.0;
		}
		return num / 3.0;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00011AF0 File Offset: 0x0000FCF0
	public static long TheoryBulletNum()
	{
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		Battle inst = Battle.inst;
		float num = Mathf.Max(3f, playerFactorTotal.bulletRng) / math.min(90f, playerFactorTotal.bulletSpd);
		if (inst.ForShow_UpgradeNum[22] >= 1)
		{
			num += DataBase.Inst.Data_Upgrades[22].buffFacs[0];
		}
		double num2 = 1.0;
		if (TempData.inst.jobId == 10)
		{
			num2 = 1.0;
		}
		else
		{
			num2 *= (double)(playerFactorTotal.fireSpd * num * (float)playerFactorTotal.bulletNum);
		}
		if (TempData.inst.jobId != 10 || !TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			for (int i = 0; i < 5; i++)
			{
				if (inst.ForShow_UpgradeNum[i] >= 1)
				{
					num2 *= 2.0;
				}
			}
		}
		if (TempData.inst.jobId != 10)
		{
			if (inst.ForShow_UpgradeNum[50] >= 1)
			{
				num2 *= 2.0;
			}
			if (inst.ForShow_UpgradeNum[49] >= 1)
			{
				num2 *= 2.0;
			}
			if (inst.ForShow_UpgradeNum[52] >= 1)
			{
				num2 *= 2.0;
			}
			if (inst.ForShow_UpgradeNum[14] >= 1)
			{
				num2 *= 1.2999999523162842;
			}
			if (inst.ForShow_UpgradeNum[16] >= 1)
			{
				num2 *= 1.2999999523162842;
			}
			if (inst.ForShow_UpgradeNum[19] >= 1)
			{
				num2 *= 1.5;
			}
			if (inst.ForShow_UpgradeNum[10] >= 1)
			{
				num2 /= 1.25;
			}
			if (inst.ForShow_UpgradeNum[12] >= 1)
			{
				num2 /= 1.5;
			}
			if (inst.ForShow_UpgradeNum[13] >= 1)
			{
				num2 /= 1.25;
			}
			if (inst.ForShow_UpgradeNum[15] >= 1)
			{
				num2 /= 1.25;
			}
			if (inst.ForShow_UpgradeNum[20] >= 1)
			{
				num2 /= 1.2999999523162842;
			}
		}
		if (TempData.inst.jobId == 11)
		{
			if (Player_11_CPU.inst == null)
			{
				Debug.LogError("Error_CpuNull!");
			}
			else
			{
				int num3 = Player_11_CPU.inst.MainDroneNum;
				if (num3 == 0)
				{
					num3 = 1;
				}
				num2 *= (double)((float)num3 * 0.3f);
			}
		}
		return (long)math.round(num2);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00011D3A File Offset: 0x0000FF3A
	public static float ActualFireSpeed()
	{
		return BulletsOptimization.actualFireSpeed;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00011D41 File Offset: 0x0000FF41
	public static double ActualBulletDamage()
	{
		return BulletsOptimization.actualBulletDamage;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00011D48 File Offset: 0x0000FF48
	public static void Update_ActualFireSpeed()
	{
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		if (!Setting.Inst.Option_BulletOptimize)
		{
			BulletsOptimization.actualFireSpeed = playerFactorTotal.fireSpd;
			return;
		}
		if (BulletsOptimization.TheoryBulletNum() <= BulletsOptimization.maxBulletNum && BulletsOptimization.TheoryBulletNum() <= 50L)
		{
			BulletsOptimization.actualFireSpeed = Mathf.Min(50f, playerFactorTotal.fireSpd);
			return;
		}
		float num = Mathf.Min(1f, (float)BulletsOptimization.maxBulletNum / (float)BulletsOptimization.TheoryBulletNum()) * playerFactorTotal.fireSpd;
		num = Mathf.Clamp(num, 3f * (float)BulletsOptimization.SettingPercent(), 50f);
		if (num >= playerFactorTotal.fireSpd)
		{
			BulletsOptimization.actualFireSpeed = playerFactorTotal.fireSpd;
			return;
		}
		BulletsOptimization.actualFireSpeed = num;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00011DFC File Offset: 0x0000FFFC
	public static void Update_ActualBulletDamage()
	{
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		if (!Setting.Inst.Option_BulletOptimize)
		{
			BulletsOptimization.actualBulletDamage = playerFactorTotal.bulletDmg;
			return;
		}
		float num = (playerFactorTotal.fireSpd == 0f) ? 1f : (playerFactorTotal.fireSpd / BulletsOptimization.ActualFireSpeed());
		BulletsOptimization.actualBulletDamage = playerFactorTotal.bulletDmg * (double)num;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00011E60 File Offset: 0x00010060
	public static float ActualHitBack()
	{
		return Player.inst.unit.playerFactorTotal.repulse;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00011E76 File Offset: 0x00010076
	public static float ActualRecoil()
	{
		return Player.inst.unit.playerFactorTotal.recoil;
	}

	// Token: 0x0400029C RID: 668
	private static float actualFireSpeed;

	// Token: 0x0400029D RID: 669
	private static double actualBulletDamage;
}
