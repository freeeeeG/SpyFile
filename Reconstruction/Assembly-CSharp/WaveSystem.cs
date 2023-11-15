using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class WaveSystem : IGameSystem
{
	// Token: 0x060010B3 RID: 4275 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
	private void GenerateWave()
	{
		this.RunningSequence = this.TestSequenceSet();
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0002E502 File Offset: 0x0002C702
	// (set) Token: 0x060010B5 RID: 4277 RVA: 0x0002E50A File Offset: 0x0002C70A
	public List<EnemySequence> RunningSequence
	{
		get
		{
			return this.runningSequence;
		}
		set
		{
			this.runningSequence = value;
		}
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0002E514 File Offset: 0x0002C714
	public override void Initialize()
	{
		this.LevelAttribute = Singleton<LevelManager>.Instance.CurrentLevel;
		Singleton<GameEvents>.Instance.onEnemyReach += this.EnemyReach;
		Singleton<GameEvents>.Instance.onEnemyDie += this.EnemyDie;
		this.bossWarningUIAnim.Initialize();
		this.goldKeeperUIAnim.Initialize();
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0002E573 File Offset: 0x0002C773
	public override void Release()
	{
		base.Release();
		Singleton<GameEvents>.Instance.onEnemyReach -= this.EnemyReach;
		Singleton<GameEvents>.Instance.onEnemyDie -= this.EnemyDie;
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0002E5A7 File Offset: 0x0002C7A7
	private void EnemyReach(Enemy enemy)
	{
		GameRes.LostLifeBattleTurn += Mathf.Min(GameRes.Life - 1, enemy.ReachDamage);
		GameRes.Life -= enemy.ReachDamage;
		GameRes.EnemyRemain--;
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0002E5E3 File Offset: 0x0002C7E3
	private void EnemyDie(Enemy enemy)
	{
		GameRes.EnemyRemain--;
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0002E5F4 File Offset: 0x0002C7F4
	public override void GameUpdate()
	{
		if (this.RunningSpawn)
		{
			for (int i = 0; i < this.RunningSequence.Count; i++)
			{
				if (!this.RunningSequence[i].Progress())
				{
					this.RunningSpawn = false;
					GameRes.EnemyRemain = GameRes.EnemyRemain;
				}
			}
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0002E644 File Offset: 0x0002C844
	public void LoadSaveWave()
	{
		WaveSystem.LevelSequence.Clear();
		foreach (EnemySequenceStruct enemySequenceStruct in Singleton<LevelManager>.Instance.LastGameSave.SaveSequences)
		{
			WaveSystem.LevelSequence.Add(enemySequenceStruct.SequencesList);
		}
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0002E6B4 File Offset: 0x0002C8B4
	public void LoadChallengeWave()
	{
		WaveSystem.LevelSequence.Clear();
		foreach (EnemySequenceStruct enemySequenceStruct in Singleton<LevelManager>.Instance.CurrentLevel.SaveSequences)
		{
			WaveSystem.LevelSequence.Add(enemySequenceStruct.SequencesList);
		}
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x0002E724 File Offset: 0x0002C924
	public void LevelInitialize()
	{
		WaveSystem.LevelSequence.Clear();
		float num = 1f;
		int num2 = 0;
		float dmgResist = 0f;
		for (int i = 0; i < this.LevelAttribute.Wave; i++)
		{
			for (int j = 0; j < this.LevelAttribute.WaveSets.Length; j++)
			{
				if (i < this.LevelAttribute.WaveSets[j].waveIndex)
				{
					num2 = j;
					break;
				}
			}
			if (i <= this.LevelAttribute.WaveSets[this.LevelAttribute.WaveSets.Length - 1].waveIndex)
			{
				num += this.LevelAttribute.LevelIntensify * (this.LevelAttribute.WaveSets[num2].baseNum * Mathf.Pow((float)i, this.LevelAttribute.WaveSets[num2].powerNum) + 1f);
			}
			else
			{
				int num3 = i - this.LevelAttribute.WaveSets[this.LevelAttribute.WaveSets.Length - 1].waveIndex;
				float num4 = Mathf.Pow((float)num3, 1f + (float)num3 / 15f);
				dmgResist = num4 / (num4 + 4f);
			}
			List<EnemySequence> item;
			if (i < 3)
			{
				num = (float)(i + 1) * 0.5f;
				item = this.GenerateRandomSequence(this.LevelAttribute.NormalEnemies, 1, num, i, dmgResist);
			}
			else if (i == 9)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss1, 1, num, i, dmgResist);
			}
			else if (i == 19)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss2, 1, num, i, dmgResist);
			}
			else if (i == 29)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss3, 1, num, i, dmgResist);
			}
			else if (i > 29 && i < 49 && (i + 1) % 5 == 0)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss3, 1, num, i, dmgResist);
			}
			else if (i >= 49 && i < this.LevelAttribute.WaveSets[this.LevelAttribute.WaveSets.Length - 1].waveIndex && (i + 1) % 5 == 0)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss4, 1, num, i, dmgResist);
			}
			else if (i >= this.LevelAttribute.WaveSets[this.LevelAttribute.WaveSets.Length - 1].waveIndex)
			{
				item = this.GenerateRandomSequence(this.LevelAttribute.Boss4, 1, num, i, dmgResist);
			}
			else if ((i + 4) % 10 == 0 && i < 99)
			{
				item = this.GenerateSpecificSequence(EnemyType.GoldKeeper, num, i, false, dmgResist);
			}
			else if (((i + 10) % 10 == 0 || (i + 9) % 10 == 0) && i > 9)
			{
				item = this.GenerateRandomSequence((i > this.LevelAttribute.EliteWave) ? this.LevelAttribute.EliteEnemies : this.LevelAttribute.NormalEnemies, 2, num, i, dmgResist);
			}
			else if (((i + 8) % 10 == 0 || (i + 3) % 10 == 0) && i > 9)
			{
				item = this.GenerateRandomSequence((i > this.LevelAttribute.EliteWave) ? this.LevelAttribute.EliteEnemies : this.LevelAttribute.NormalEnemies, 3, num, i, dmgResist);
			}
			else
			{
				item = this.GenerateRandomSequence((i > this.LevelAttribute.EliteWave) ? this.LevelAttribute.EliteEnemies : this.LevelAttribute.NormalEnemies, 1, num, i, dmgResist);
			}
			WaveSystem.LevelSequence.Add(item);
		}
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0002EAAC File Offset: 0x0002CCAC
	private List<EnemySequence> GenerateRandomSequence(List<EnemyAttribute> enemyList, int genres, float stage, int wave, float dmgResist)
	{
		List<EnemySequence> list = new List<EnemySequence>();
		foreach (int index in StaticData.SelectNoRepeat(enemyList.Count, genres))
		{
			EnemySequence item = this.SequenceInfoSet(genres, stage, wave, enemyList[index].EnemyType, enemyList[index].IsBoss, dmgResist);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0002EB34 File Offset: 0x0002CD34
	private List<EnemySequence> GenerateSpecificSequence(EnemyType type, float stage, int wave, bool isBoss, float dmgResist)
	{
		List<EnemySequence> list = new List<EnemySequence>();
		EnemySequence item = this.SequenceInfoSet(1, stage, wave, type, isBoss, dmgResist);
		list.Add(item);
		return list;
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x0002EB5C File Offset: 0x0002CD5C
	private EnemySequence SequenceInfoSet(int genres, float intensify, int wave, EnemyType type, bool isBoss, float dmgResist)
	{
		EnemyAttribute enemyAttribute = Singleton<StaticData>.Instance.EnemyFactory.Get(type);
		int amount = Mathf.CeilToInt(Mathf.Min((float)(enemyAttribute.MaxAmount / genres) * GameRes.EnemyAmoundAdjust, (float)Mathf.RoundToInt((float)enemyAttribute.InitCount + (float)wave / 5f * enemyAttribute.CountIncrease / (float)genres)) * GameRes.EnemyAmoundAdjust);
		float cooldown = enemyAttribute.CoolDown * (float)genres;
		intensify *= GameRes.EnemyIntensifyAdjust;
		return new EnemySequence(wave, type, amount, cooldown, intensify, isBoss, dmgResist);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x0002EBE0 File Offset: 0x0002CDE0
	private List<EnemySequence> TestSequenceSet()
	{
		List<EnemySequence> list = new List<EnemySequence>();
		EnemyAttribute enemyAttribute = Singleton<StaticData>.Instance.EnemyFactory.Get(this.TestType);
		EnemySequence item = new EnemySequence(1, this.TestType, this.TestEnemyCount, this.TestCoolDown, this.testIntensify, enemyAttribute.IsBoss, 0f);
		list.Add(item);
		return list;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0002EC39 File Offset: 0x0002CE39
	public void ManualSetSequence(EnemyType type, float stage, int wave)
	{
		WaveSystem.LevelSequence[wave] = this.GenerateSpecificSequence(type, stage, wave, false, 0f);
		this.PrepareNextWave();
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x0002EC5C File Offset: 0x0002CE5C
	public void PrepareNextWave()
	{
		this.RunningSequence = WaveSystem.LevelSequence[GameRes.CurrentWave - 1];
		foreach (EnemySequence enemySequence in this.RunningSequence)
		{
			enemySequence.Initialize();
		}
		if (this.RunningSequence[0].IsBoss)
		{
			this.bossWarningUIAnim.Show();
		}
		else if (this.RunningSequence[0].EnemyType == EnemyType.GoldKeeper)
		{
			this.goldKeeperUIAnim.Show();
		}
		for (int i = GameRes.CurrentWave - 1; i < WaveSystem.LevelSequence.Count; i++)
		{
			if (WaveSystem.LevelSequence[i][0].IsBoss)
			{
				this.NextBoss = WaveSystem.LevelSequence[i][0].EnemyType;
				this.NextBossWave = i - GameRes.CurrentWave + 1;
				return;
			}
		}
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x0002ED64 File Offset: 0x0002CF64
	public Enemy SpawnEnemy(EnemyType eType, int pathIndex, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		EnemyAttribute enemyAttribute = Singleton<StaticData>.Instance.EnemyFactory.Get(eType);
		GameRes.EnemyRemain++;
		Enemy enemy = Singleton<ObjectPool>.Instance.Spawn(enemyAttribute.Prefab) as Enemy;
		enemy.Initialize(pathIndex, enemyAttribute, Random.Range(-0.19f, 0.19f), intensify, dmgResist, pathPoints);
		Singleton<GameManager>.Instance.enemies.Add(enemy);
		return enemy;
	}

	// Token: 0x040008E8 RID: 2280
	[Header("测试用")]
	[SerializeField]
	private float testIntensify;

	// Token: 0x040008E9 RID: 2281
	[SerializeField]
	private EnemyType TestType = EnemyType.Knight;

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private int TestEnemyCount;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private float TestCoolDown;

	// Token: 0x040008EC RID: 2284
	[Space]
	public bool RunningSpawn;

	// Token: 0x040008ED RID: 2285
	public static List<List<EnemySequence>> LevelSequence = new List<List<EnemySequence>>();

	// Token: 0x040008EE RID: 2286
	private LevelAttribute LevelAttribute;

	// Token: 0x040008EF RID: 2287
	private List<EnemySequence> runningSequence;

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private BossComeAnim bossWarningUIAnim;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private GoldKeeperAnim goldKeeperUIAnim;

	// Token: 0x040008F2 RID: 2290
	public EnemyType NextBoss;

	// Token: 0x040008F3 RID: 2291
	public int NextBossWave;
}
