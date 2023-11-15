using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000027 RID: 39
[Serializable]
public class FactorMultis
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000C181 File Offset: 0x0000A381
	public float mod_LifeMaxEnemy
	{
		get
		{
			return this.factorMultis[0];
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000C18B File Offset: 0x0000A38B
	public int mod_LifeMaxPlayer
	{
		get
		{
			return this.factorMultis[1].RoundToInt();
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000C19A File Offset: 0x0000A39A
	public float mod_MoveSpd
	{
		get
		{
			return this.factorMultis[2];
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
	public float mod_FireSpd
	{
		get
		{
			return this.factorMultis[3];
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000C1AE File Offset: 0x0000A3AE
	public float mod_BulletDmg
	{
		get
		{
			return this.factorMultis[4];
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060001EA RID: 490 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
	public int mod_BulletNum
	{
		get
		{
			return Mathf.RoundToInt(this.factorMultis[5]);
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060001EB RID: 491 RVA: 0x0000C1C7 File Offset: 0x0000A3C7
	public float mod_BulletSpd
	{
		get
		{
			return this.factorMultis[6];
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060001EC RID: 492 RVA: 0x0000C1D1 File Offset: 0x0000A3D1
	public float mod_BulletRng
	{
		get
		{
			return this.factorMultis[7];
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060001ED RID: 493 RVA: 0x0000C1DB File Offset: 0x0000A3DB
	public float mod_BodySize
	{
		get
		{
			return this.factorMultis[8];
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060001EE RID: 494 RVA: 0x0000C1E5 File Offset: 0x0000A3E5
	public float mod_BulletSize
	{
		get
		{
			return this.factorMultis[9];
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060001EF RID: 495 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
	public float add_CritChc
	{
		get
		{
			return this.factorMultis[10];
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000C1FB File Offset: 0x0000A3FB
	public float add_CritDmg
	{
		get
		{
			return this.factorMultis[11];
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000C206 File Offset: 0x0000A406
	public float add_LifeRec
	{
		get
		{
			return this.factorMultis[12];
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000C211 File Offset: 0x0000A411
	public float mod_Accuracy
	{
		get
		{
			return this.factorMultis[13];
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000C21C File Offset: 0x0000A41C
	public float mod_Recoil
	{
		get
		{
			return this.factorMultis[14];
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000C227 File Offset: 0x0000A427
	public void MultiOneFactor(int type, float num)
	{
		if (type < 0 || type > 15 || type == 5)
		{
			Debug.LogError("TypeError!");
			return;
		}
		this.factorMultis[type] += num;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000C254 File Offset: 0x0000A454
	public static FactorMultis operator +(FactorMultis mul1, FactorMultis mul2)
	{
		if (mul1.factorMultis.Length != mul2.factorMultis.Length)
		{
			Debug.LogError("Length!=!");
			return null;
		}
		for (int i = 0; i < mul1.factorMultis.Length; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				mul1.factorMultis[i] += mul2.factorMultis[i];
			}
			else
			{
				mul1.factorMultis[i] += mul2.factorMultis[i] - 1f;
			}
		}
		return mul1;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000C2DC File Offset: 0x0000A4DC
	public static FactorMultis operator *(FactorMultis mul1, FactorMultis mul2)
	{
		if (mul1.factorMultis.Length != mul2.factorMultis.Length)
		{
			Debug.LogError("Length!=!");
			return null;
		}
		for (int i = 0; i < mul1.factorMultis.Length; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				mul1.factorMultis[i] += mul2.factorMultis[i];
			}
			else
			{
				mul1.factorMultis[i] *= mul2.factorMultis[i];
			}
		}
		return mul1;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000C35C File Offset: 0x0000A55C
	public static double[]operator *(double[] dbs, FactorMultis mul)
	{
		if (dbs.Length != mul.factorMultis.Length)
		{
			Debug.LogError("Length!=!");
			return dbs;
		}
		for (int i = 0; i < mul.factorMultis.Length; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				dbs[i] += (double)mul.factorMultis[i];
			}
			else
			{
				dbs[i] *= (double)mul.factorMultis[i];
			}
		}
		return dbs;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
	public FactorMultis GetFactorMultis_Power(int power)
	{
		FactorMultis factorMultis = new FactorMultis();
		for (int i = 0; i < this.factorMultis.Length; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				factorMultis.factorMultis[i] += this.factorMultis[i] * (float)power;
			}
			else
			{
				factorMultis.factorMultis[i] *= Mathf.Pow(this.factorMultis[i], (float)power);
			}
		}
		return factorMultis;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000C444 File Offset: 0x0000A644
	public double[] GetDoubles_Power(int power)
	{
		double[] array = new double[]
		{
			1.0,
			0.0,
			1.0,
			1.0,
			1.0,
			1.0,
			1.0,
			1.0,
			1.0,
			1.0,
			0.0,
			0.0,
			0.0,
			0.0,
			1.0,
			1.0
		};
		for (int i = 0; i < this.factorMultis.Length; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				array[i] += (double)(this.factorMultis[i] * (float)power);
			}
			else
			{
				array[i] *= math.pow((double)this.factorMultis[i], (double)power);
			}
		}
		return array;
	}

	// Token: 0x040001BD RID: 445
	public string name = "UNITED";

	// Token: 0x040001BE RID: 446
	public float[] factorMultis = new float[]
	{
		1f,
		0f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		0f,
		0f,
		0f,
		0f,
		1f,
		1f
	};
}
