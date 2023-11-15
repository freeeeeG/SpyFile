using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public class EnemyModel
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000B499 File Offset: 0x00009699
	public string Language_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000B4AC File Offset: 0x000096AC
	public EnemyModel(EnemyModel source)
	{
		if (source == null)
		{
			return;
		}
		this.no = source.no;
		this.name = source.name;
		this.rank = source.rank;
		this.type = source.type;
		this.ifAvailable = source.ifAvailable;
		this.enemyGeneration = source.enemyGeneration;
		this.factorMultis = source.factorMultis;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000B558 File Offset: 0x00009758
	public int CoinNum(float multi)
	{
		if (multi == 0f)
		{
			return 0;
		}
		GameParameters inst = GameParameters.Inst;
		return MyTool.DecimalToInt(inst.Coin_ChanceBasic * inst.Coin_ChanceFixPerRank[(int)this.rank] * multi * Battle.inst.factorBattleTotal.FragDrop);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000B5A0 File Offset: 0x000097A0
	public float Score(float multi)
	{
		if (multi == 0f)
		{
			return 0f;
		}
		GameParameters.ScoreSetting scoreSetting = GameParameters.Inst.scoreSetting;
		float scoreBasic = scoreSetting.ScoreBasic;
		float num = scoreSetting.Score_FixPerRank[(int)this.rank];
		return scoreBasic * num * multi;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000B5E0 File Offset: 0x000097E0
	private bool CanGenerate(int levelType, int wave)
	{
		EnemyGeneration enemyGeneration = this.enemyGeneration[levelType];
		return wave >= enemyGeneration.waveStart && wave <= enemyGeneration.waveEnd;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000B60C File Offset: 0x0000980C
	public int GetWeightTheWave(EnumLevelType levelType, int wave)
	{
		int result = 0;
		if (this.CanGenerate((int)levelType, wave))
		{
			result = this.enemyGeneration[(int)levelType].weight;
		}
		return result;
	}

	// Token: 0x040001AE RID: 430
	public string name = "Uninited";

	// Token: 0x040001AF RID: 431
	public int no = -1;

	// Token: 0x040001B0 RID: 432
	public EnumRank rank = EnumRank.UNINTED;

	// Token: 0x040001B1 RID: 433
	public EnemyModel.EnumEnemyType type;

	// Token: 0x040001B2 RID: 434
	public bool ifAvailable;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	public EnemyGeneration[] enemyGeneration;

	// Token: 0x040001B4 RID: 436
	public FactorMultis factorMultis = new FactorMultis();

	// Token: 0x040001B5 RID: 437
	public int splitType = -1;

	// Token: 0x040001B6 RID: 438
	public int summonType = -1;

	// Token: 0x040001B7 RID: 439
	public string[] names = new string[3];

	// Token: 0x02000143 RID: 323
	public enum EnumEnemyType
	{
		// Token: 0x0400098F RID: 2447
		UNINITED = -1,
		// Token: 0x04000990 RID: 2448
		ORDINARY,
		// Token: 0x04000991 RID: 2449
		CANSHOOT
	}
}
