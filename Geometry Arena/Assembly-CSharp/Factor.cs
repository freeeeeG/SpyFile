using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Unity.Mathematics;

// Token: 0x02000025 RID: 37
[Serializable]
public class Factor
{
	// Token: 0x060001AF RID: 431 RVA: 0x0000B634 File Offset: 0x00009834
	public double ActualFactor(int id)
	{
		return math.max(this.factor[id], Factor.factorMin[id]);
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000B65C File Offset: 0x0000985C
	public double lifeMaxEnemy
	{
		get
		{
			return this.ActualFactor(0);
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000B665 File Offset: 0x00009865
	public int lifeMaxPlayer
	{
		get
		{
			return this.ActualFactor(1).RoundToInt();
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000B673 File Offset: 0x00009873
	public float moveSpd
	{
		get
		{
			return (float)this.ActualFactor(2);
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000B67D File Offset: 0x0000987D
	public float fireSpd
	{
		get
		{
			return (float)this.ActualFactor(3);
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000B687 File Offset: 0x00009887
	public double bulletDmg
	{
		get
		{
			return this.ActualFactor(4);
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000B690 File Offset: 0x00009890
	public int bulletNum
	{
		get
		{
			return this.ActualFactor(5).RoundToInt();
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000B69E File Offset: 0x0000989E
	public float bulletSpd
	{
		get
		{
			return (float)this.ActualFactor(6);
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000B6A8 File Offset: 0x000098A8
	public float bulletRng
	{
		get
		{
			return (float)this.ActualFactor(7);
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000B6B2 File Offset: 0x000098B2
	public float bodySize
	{
		get
		{
			return (float)this.ActualFactor(8);
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000B6BC File Offset: 0x000098BC
	public float bulletSize
	{
		get
		{
			return (float)this.ActualFactor(9);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060001BA RID: 442 RVA: 0x0000B6C7 File Offset: 0x000098C7
	public float critChc
	{
		get
		{
			return (float)this.ActualFactor(10);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060001BB RID: 443 RVA: 0x0000B6D2 File Offset: 0x000098D2
	public float critDmg
	{
		get
		{
			return (float)this.ActualFactor(11);
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060001BC RID: 444 RVA: 0x0000B6DD File Offset: 0x000098DD
	public float lifeRec
	{
		get
		{
			return (float)this.ActualFactor(12);
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B6E8 File Offset: 0x000098E8
	public float accuracy
	{
		get
		{
			return (float)this.ActualFactor(13);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060001BE RID: 446 RVA: 0x0000B6F3 File Offset: 0x000098F3
	public float recoil
	{
		get
		{
			return (float)this.ActualFactor(14);
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060001BF RID: 447 RVA: 0x0000B6FE File Offset: 0x000098FE
	public float repulse
	{
		get
		{
			return (float)this.ActualFactor(15);
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000B70C File Offset: 0x0000990C
	public Factor(FactorMultis multis, bool ifEnemy)
	{
		if (multis == null)
		{
			return;
		}
		this.factorMultis = multis;
		Factor factor;
		if (!ifEnemy)
		{
			factor = GameParameters.Inst.DefaultFactor;
		}
		else
		{
			factor = GameParameters.Inst.DefaultFactorEnemy;
		}
		for (int i = 1; i <= 9; i++)
		{
			this.factor[i] = factor.factor[i] * (double)multis.factorMultis[i];
		}
		for (int j = 10; j <= 13; j++)
		{
			this.factor[j] = factor.factor[j] + (double)multis.factorMultis[j];
		}
		for (int k = 14; k <= 15; k++)
		{
			this.factor[k] = factor.factor[k] * (double)multis.factorMultis[k];
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000B80C File Offset: 0x00009A0C
	public string FloatToPercentString(float num)
	{
		return (num * 100f).ToString() + "%";
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000B834 File Offset: 0x00009A34
	public static Factor operator *(Factor factor, FactorMultis multis)
	{
		Factor factor2 = new Factor(null, false);
		factor2.factor[0] = factor.factor[0] * (double)multis.factorMultis[0];
		factor2.factor[1] = factor.factor[1] + (double)multis.factorMultis[1];
		for (int i = 2; i <= 9; i++)
		{
			factor2.factor[i] = factor.factor[i] * (double)multis.factorMultis[i];
		}
		for (int j = 10; j <= 13; j++)
		{
			factor2.factor[j] = factor.factor[j] + (double)multis.factorMultis[j];
		}
		for (int k = 14; k <= 15; k++)
		{
			factor2.factor[k] = factor.factor[k] * (double)multis.factorMultis[k];
		}
		return factor2;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000B950 File Offset: 0x00009B50
	public static Factor operator *(Factor factor, double[] multis)
	{
		Factor factor2 = new Factor(null, false);
		factor2.factor[0] = factor.factor[0] * multis[0];
		factor2.factor[1] = factor.factor[1] + multis[1];
		for (int i = 2; i <= 9; i++)
		{
			factor2.factor[i] = factor.factor[i] * multis[i];
		}
		for (int j = 10; j <= 13; j++)
		{
			factor2.factor[j] = factor.factor[j] + multis[j];
		}
		for (int k = 14; k <= 15; k++)
		{
			factor2.factor[k] = factor.factor[k] * multis[k];
		}
		return factor2;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000BA50 File Offset: 0x00009C50
	public string GetFactorInfo(int i)
	{
		if (i == 0 || i == 5 || i == 12)
		{
			return null;
		}
		if (i <= 15 && i >= 8)
		{
			return MyTool.DecimalToUnsignedPercentString(this.factor[i]);
		}
		if (i == 1)
		{
			return this.factor[i].ToString();
		}
		return this.factor[i].ToString("0.0");
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000BAC0 File Offset: 0x00009CC0
	public string GetActualFactorInfo(int i)
	{
		if (i == 0 || i == 5 || i == 12)
		{
			return null;
		}
		if (i <= 15 && i >= 8)
		{
			return MyTool.DecimalToUnsignedPercentString(this.ActualFactor(i));
		}
		if (i == 1)
		{
			return this.ActualFactor(i).ToString();
		}
		return this.ActualFactor(i).ToSmartString();
	}

	// Token: 0x040001B8 RID: 440
	public ObscuredDouble[] factor = new ObscuredDouble[16];

	// Token: 0x040001B9 RID: 441
	public static ObscuredDouble[] factorMin = new ObscuredDouble[]
	{
		0.0,
		1.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.10000000149011612,
		0.009999999776482582,
		0.009999999776482582,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0
	};

	// Token: 0x040001BA RID: 442
	private FactorMultis factorMultis = new FactorMultis();
}
