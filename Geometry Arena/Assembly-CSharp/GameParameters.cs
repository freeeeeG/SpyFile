using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class GameParameters : ScriptableObject
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000024 RID: 36 RVA: 0x000037C0 File Offset: 0x000019C0
	public static GameParameters Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.gameParameters;
			}
			return Resources.Load<GameParameters>("Assets/GameParameters");
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000025 RID: 37 RVA: 0x000037E4 File Offset: 0x000019E4
	public float Talent_BasicPrice
	{
		get
		{
			return this.talentBasicPrice;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000026 RID: 38 RVA: 0x000037EC File Offset: 0x000019EC
	public Factor DefaultFactor
	{
		get
		{
			return this.defaultFactor;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000027 RID: 39 RVA: 0x000037F4 File Offset: 0x000019F4
	public Factor DefaultFactorEnemy
	{
		get
		{
			return this.defaultFactorEnemy;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000028 RID: 40 RVA: 0x000037FC File Offset: 0x000019FC
	public FactorBattle BattleFactorLevelGrow
	{
		get
		{
			return this.battleFactorLevelGrow;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000029 RID: 41 RVA: 0x00003804 File Offset: 0x00001A04
	public float FactorInfinityJump
	{
		get
		{
			return this.factorInfinityJump;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600002A RID: 42 RVA: 0x0000380C File Offset: 0x00001A0C
	public float FactorUpgradeInflation
	{
		get
		{
			return this.factorUpgradeInflation;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600002B RID: 43 RVA: 0x00003814 File Offset: 0x00001A14
	public float BasicRecoilForce
	{
		get
		{
			return this.basicRecoilForce;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600002C RID: 44 RVA: 0x0000381C File Offset: 0x00001A1C
	public float RepulseForceByEnemy
	{
		get
		{
			return this.repulseForceByEnemy;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00003824 File Offset: 0x00001A24
	public float RepulseForceByWall
	{
		get
		{
			return this.repulseForceByWall;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600002E RID: 46 RVA: 0x0000382C File Offset: 0x00001A2C
	public float ForceEnemyShootOrigin
	{
		get
		{
			return this.forceEnemyShootOrigin;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600002F RID: 47 RVA: 0x00003834 File Offset: 0x00001A34
	public float ForceEnemySplitOrigin
	{
		get
		{
			return this.forceEnemySplitOrigin;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000030 RID: 48 RVA: 0x0000383C File Offset: 0x00001A3C
	public float Coin_ChanceBasic
	{
		get
		{
			return this.coinChanceBasic;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000031 RID: 49 RVA: 0x00003844 File Offset: 0x00001A44
	public float[] Coin_ChanceFixPerRank
	{
		get
		{
			return this.coinChanceFixPerRank;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000032 RID: 50 RVA: 0x0000384C File Offset: 0x00001A4C
	public float InteractiveObjectPickUpDist
	{
		get
		{
			return this.interactiveObjectPickUpDist;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000033 RID: 51 RVA: 0x00003854 File Offset: 0x00001A54
	public float EnemyGeneDistMulti
	{
		get
		{
			return this.enemyGeneDistMulti;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000034 RID: 52 RVA: 0x0000385C File Offset: 0x00001A5C
	public float[] FacStandards
	{
		get
		{
			return this.facStandards;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000035 RID: 53 RVA: 0x00003864 File Offset: 0x00001A64
	public float WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000036 RID: 54 RVA: 0x0000386C File Offset: 0x00001A6C
	public float GunFireBack_SmoothTime
	{
		get
		{
			return this.gunFireBackSmoothTime;
		}
	}

	// Token: 0x04000012 RID: 18
	[Range(0f, 100f)]
	public int NoUse;

	// Token: 0x04000013 RID: 19
	public ObscuredInt version = 3;

	// Token: 0x04000014 RID: 20
	public ObscuredBool ifDemo = false;

	// Token: 0x04000015 RID: 21
	public bool ifSkipSteamDetect;

	// Token: 0x04000016 RID: 22
	[SerializeField]
	private float talentBasicPrice = 1000f;

	// Token: 0x04000017 RID: 23
	[Header("Factors")]
	[SerializeField]
	private Factor defaultFactor = new Factor(null, false);

	// Token: 0x04000018 RID: 24
	[SerializeField]
	private Factor defaultFactorEnemy = new Factor(null, false);

	// Token: 0x04000019 RID: 25
	[SerializeField]
	private FactorBattle battleFactorLevelGrow = new FactorBattle();

	// Token: 0x0400001A RID: 26
	[SerializeField]
	private float factorInfinityJump = 0.54f;

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private float factorUpgradeInflation = 0.05f;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private float basicRecoilForce = 50f;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	private float repulseForceByEnemy = 1000f;

	// Token: 0x0400001E RID: 30
	[SerializeField]
	private float repulseForceByWall = 1000f;

	// Token: 0x0400001F RID: 31
	[SerializeField]
	private float forceEnemyShootOrigin = 1000f;

	// Token: 0x04000020 RID: 32
	[SerializeField]
	private float forceEnemySplitOrigin = 500f;

	// Token: 0x04000021 RID: 33
	[Header("Coin")]
	[SerializeField]
	private float coinChanceBasic = 0.5f;

	// Token: 0x04000022 RID: 34
	[SerializeField]
	private float[] coinChanceFixPerRank;

	// Token: 0x04000023 RID: 35
	[Header("Upgrade")]
	[SerializeField]
	public GameParameters.Upgrade upgrade = new GameParameters.Upgrade();

	// Token: 0x04000024 RID: 36
	[Header("Score")]
	[SerializeField]
	public GameParameters.ScoreSetting scoreSetting = new GameParameters.ScoreSetting();

	// Token: 0x04000025 RID: 37
	[Header("Others")]
	[SerializeField]
	private float interactiveObjectPickUpDist = 2f;

	// Token: 0x04000026 RID: 38
	[SerializeField]
	private float enemyGeneDistMulti = 2.5f;

	// Token: 0x04000027 RID: 39
	[SerializeField]
	private float[] facStandards = new float[16];

	// Token: 0x04000028 RID: 40
	[SerializeField]
	private float worldSize = 1.5f;

	// Token: 0x04000029 RID: 41
	[SerializeField]
	private float gunFireBackSmoothTime = 0.1f;

	// Token: 0x0400002A RID: 42
	[SerializeField]
	public GameParameters.EnemySetting enemySet = new GameParameters.EnemySetting();

	// Token: 0x0400002B RID: 43
	public FactorBattle[] difficultyLevel_FactorBattle = new FactorBattle[8];

	// Token: 0x0400002C RID: 44
	[Header("Settings")]
	[SerializeField]
	public Settings_IntSet[] settings_IntSets = new Settings_IntSet[0];

	// Token: 0x020000FD RID: 253
	[Serializable]
	public class ScoreSetting
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00035262 File Offset: 0x00033462
		public float ScoreBasic
		{
			get
			{
				return this.scoreBasic;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0003526A File Offset: 0x0003346A
		public float[] Score_FixPerRank
		{
			get
			{
				return this.scoreFixPerRank;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00035272 File Offset: 0x00033472
		public float Score_SummonPercent
		{
			get
			{
				return this.scoreSummonPercent;
			}
		}

		// Token: 0x040007A5 RID: 1957
		[SerializeField]
		private float scoreBasic = 10f;

		// Token: 0x040007A6 RID: 1958
		[SerializeField]
		private float[] scoreFixPerRank;

		// Token: 0x040007A7 RID: 1959
		[SerializeField]
		private float scoreSummonPercent = 0.3f;
	}

	// Token: 0x020000FE RID: 254
	[Serializable]
	public class EnemySetting
	{
		// Token: 0x040007A8 RID: 1960
		public int rankWeight_Normal = 90;

		// Token: 0x040007A9 RID: 1961
		public int rankWeight_Elite = 10;

		// Token: 0x040007AA RID: 1962
		public float speedMod_Tracking = 1f;

		// Token: 0x040007AB RID: 1963
		public float speedMod_Bounce = 1f;

		// Token: 0x040007AC RID: 1964
		public float speedMod_Dash = 1f;

		// Token: 0x040007AD RID: 1965
		public float speedMod_Jump = 1f;

		// Token: 0x040007AE RID: 1966
		public float jump_AngleDelta = 90f;

		// Token: 0x040007AF RID: 1967
		public float basicPara_Tracking_AngleSpeed = 50f;

		// Token: 0x040007B0 RID: 1968
		public float basicPara_Dash_WaitTime = 2f;

		// Token: 0x040007B1 RID: 1969
		public float basicPara_Jump_IntervalTime = 1f;

		// Token: 0x040007B2 RID: 1970
		public float basicPara_Jump_DurationTime = 0.5f;

		// Token: 0x040007B3 RID: 1971
		public float basicPara_Rotate_AngleSpeed = 720f;
	}

	// Token: 0x020000FF RID: 255
	[Serializable]
	public class Upgrade
	{
		// Token: 0x040007B4 RID: 1972
		public int basicPrice = 10;

		// Token: 0x040007B5 RID: 1973
		public Vector2 priceFloatRange = new Vector2(0.7f, 1.2f);

		// Token: 0x040007B6 RID: 1974
		public int[] weight = new int[]
		{
			60,
			30,
			10
		};

		// Token: 0x040007B7 RID: 1975
		public float[] pricePerRank = new float[]
		{
			1f,
			1.6f,
			2.4f
		};
	}
}
