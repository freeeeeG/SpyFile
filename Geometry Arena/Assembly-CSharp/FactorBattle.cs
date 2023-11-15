using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000026 RID: 38
[Serializable]
public class FactorBattle
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000BC84 File Offset: 0x00009E84
	public float StarGain
	{
		get
		{
			return (float)this.factors[0];
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000BC8F File Offset: 0x00009E8F
	public float EnemyAmount
	{
		get
		{
			return (float)this.factors[1];
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000BC9A File Offset: 0x00009E9A
	public float EnemyGeneSpd
	{
		get
		{
			return (float)this.factors[2];
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060001CA RID: 458 RVA: 0x0000BCA5 File Offset: 0x00009EA5
	public float FragDrop
	{
		get
		{
			return (float)this.factors[3];
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060001CB RID: 459 RVA: 0x0000BCB0 File Offset: 0x00009EB0
	public float Enemy_NormalRate
	{
		get
		{
			return (float)this.factors[4];
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000BCBB File Offset: 0x00009EBB
	public float Enemy_EliteRate
	{
		get
		{
			return (float)this.factors[5];
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060001CD RID: 461 RVA: 0x0000BCC6 File Offset: 0x00009EC6
	public float Enemy_ModSpeed
	{
		get
		{
			return (float)this.factors[6];
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060001CE RID: 462 RVA: 0x0000BCD1 File Offset: 0x00009ED1
	public double Enemy_ModLife
	{
		get
		{
			return this.factors[7];
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060001CF RID: 463 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public float Shop_NormalRate
	{
		get
		{
			return (float)this.factors[8];
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000BCE6 File Offset: 0x00009EE6
	public float Shop_RareRate
	{
		get
		{
			return (float)this.factors[9];
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000BCF2 File Offset: 0x00009EF2
	public float Shop_EpicRate
	{
		get
		{
			return (float)this.factors[10];
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000BCFE File Offset: 0x00009EFE
	public float Shop_LegendaryRate
	{
		get
		{
			return (float)this.factors[11];
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000BD0A File Offset: 0x00009F0A
	public float Player_InvincibleTime
	{
		get
		{
			return (float)this.factors[12];
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000BD18 File Offset: 0x00009F18
	public static FactorBattle operator *(FactorBattle f1, FactorBattle f2)
	{
		FactorBattle factorBattle = new FactorBattle();
		for (int i = 0; i <= 12; i++)
		{
			factorBattle.factors[i] = f1.factors[i] * f2.factors[i];
		}
		return factorBattle;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000BD54 File Offset: 0x00009F54
	public static FactorBattle FactorBattleThisLevel()
	{
		int num = Battle.inst.SequalWave;
		if (TempData.inst.modeType != EnumModeType.NORMAL)
		{
			EnumModeType modeType = TempData.inst.modeType;
			if (modeType != EnumModeType.ENDLESS)
			{
				if (modeType == EnumModeType.WANDER)
				{
					num = (num - 1) * 4 + 1;
				}
				else
				{
					Debug.LogError("模式错误");
				}
			}
		}
		FactorBattle factorBattle = new FactorBattle();
		if (TempData.inst.modeType != EnumModeType.NORMAL)
		{
			factorBattle.factors[1] = 1.0;
			factorBattle.factors[2] = 1.0;
			factorBattle.factors[5] = 1.0;
			factorBattle.factors[6] = 1.0;
			if (num <= 1)
			{
				factorBattle.factors[0] = 1.0;
				factorBattle.factors[7] = 1.0;
			}
			else
			{
				float p = Mathf.Pow((float)(num - 1), 0.63f);
				factorBattle.factors[0] = (double)Mathf.Clamp(Mathf.Pow(1.06f, p), 1f, 3f);
				factorBattle.factors[7] = (math.pow(1.09, (double)(num - 1)) - 1.0) / 0.09 * 0.12 + 1.0;
			}
		}
		else
		{
			factorBattle.factors[0] = 1.0;
			num--;
			int num2 = Mathf.CeilToInt((float)num / 5f);
			factorBattle.factors[7] = ((double)(num2 * num) - 2.5 * (double)num2 * (double)(num2 - 1)) * 0.10000000149011612 + 1.0;
		}
		return factorBattle;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
	public static float GetFloat_EnemyAmout_Basic()
	{
		return 15f;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000BEFA File Offset: 0x0000A0FA
	public static float GetFloat_EnemyAmount_Total()
	{
		return FactorBattle.GetFloat_EnemyAmout_Basic() * Battle.inst.factorBattleTotal.EnemyAmount;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000BF14 File Offset: 0x0000A114
	public static double GetDouble_FactorBattleShow_Basic(int fbID)
	{
		switch (fbID)
		{
		case 1:
			return (double)FactorBattle.GetFloat_EnemyAmout_Basic();
		case 5:
			return (double)(FactorBattle.GetFloat_EliteProp_Basic() / 100f);
		case 8:
		case 9:
		case 10:
		case 11:
			return (double)FactorBattle.GetFloat_UpgradeWeightWithRank_Basic((EnumRank)(fbID - 8));
		case 12:
			return (double)FactorBattle.GetFloat_InvincibleTime_Basic();
		}
		return 1.0;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000BF8C File Offset: 0x0000A18C
	public double GetDouble_FactorBattleShow_Total(int fbID)
	{
		switch (fbID)
		{
		case 1:
			return (double)FactorBattle.GetFloat_EnemyAmount_Total();
		case 5:
			return (double)FactorBattle.GetFloat_EliteProp_Total();
		case 8:
		case 9:
		case 10:
		case 11:
			return (double)(FactorBattle.GetFloat_UpgradeWeightWithRank_Total((EnumRank)(fbID - 8)) / FactorBattle.GetFloat_UpgradeWeight_Total_AllRank());
		case 12:
			return (double)FactorBattle.GetFloat_InvincibleTime_Total();
		}
		return this.factors[fbID];
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000C000 File Offset: 0x0000A200
	public static float GetFloat_EliteProp_Basic()
	{
		return (float)GameParameters.Inst.enemySet.rankWeight_Elite;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000C014 File Offset: 0x0000A214
	public static float GetFloat_EliteProp_Total()
	{
		GameParameters.EnemySetting enemySet = GameParameters.Inst.enemySet;
		FactorBattle factorBattleTotal = Battle.inst.factorBattleTotal;
		return (float)enemySet.rankWeight_Elite * factorBattleTotal.Enemy_EliteRate / 100f;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000C049 File Offset: 0x0000A249
	public static float GetFloat_UpgradeWeightWithRank_Basic(EnumRank rank)
	{
		return (float)GameParameters.Inst.upgrade.weight[(int)rank];
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000C060 File Offset: 0x0000A260
	public static float GetFloat_UpgradeWeightWithRank_Total(EnumRank rank)
	{
		FactorBattle factorBattleTotal = Battle.inst.factorBattleTotal;
		return FactorBattle.GetFloat_UpgradeWeightWithRank_Basic(rank) * (float)factorBattleTotal.factors[(int)(rank + 8)];
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000C08C File Offset: 0x0000A28C
	public static float GetFloat_UpgradeWeight_Total_AllRank()
	{
		float num = 0f;
		for (int i = 0; i < 4; i++)
		{
			num += FactorBattle.GetFloat_UpgradeWeightWithRank_Total((EnumRank)i);
		}
		return num;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
	public static string GetString_NumberFormated_Basic(int fbID)
	{
		double double_FactorBattleShow_Basic = FactorBattle.GetDouble_FactorBattleShow_Basic(fbID);
		return FactorBattle.GetString_NumberToFormat(fbID, double_FactorBattleShow_Basic, false);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
	public static string GetString_NumberFormated_Total(int fbID)
	{
		double double_FactorBattleShow_Total = Battle.inst.factorBattleTotal.GetDouble_FactorBattleShow_Total(fbID);
		return FactorBattle.GetString_NumberToFormat(fbID, double_FactorBattleShow_Total, true);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000C0FA File Offset: 0x0000A2FA
	public static float GetFloat_InvincibleTime_Basic()
	{
		return 0.8f;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000C101 File Offset: 0x0000A301
	public static float GetFloat_InvincibleTime_Total()
	{
		return FactorBattle.GetFloat_InvincibleTime_Basic() * Battle.inst.factorBattleTotal.Player_InvincibleTime;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000C118 File Offset: 0x0000A318
	public static string GetString_NumberToFormat(int fbID, double numIn, bool ifTotal)
	{
		if (fbID == 1)
		{
			return numIn.ToSmartString_Int();
		}
		if (fbID - 8 > 3)
		{
			if (fbID != 12)
			{
				return numIn.ToSmartString_Percent();
			}
			return numIn.ToString("0.####");
		}
		else
		{
			if (ifTotal)
			{
				return numIn.ToSmartString_Percent();
			}
			return numIn.ToSmartString_Int();
		}
	}

	// Token: 0x040001BB RID: 443
	public string name = "FactorBattle";

	// Token: 0x040001BC RID: 444
	public double[] factors = new double[]
	{
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0,
		1.0
	};
}
