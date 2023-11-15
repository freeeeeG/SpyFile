using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002D RID: 45
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/StageSettingData", order = 1)]
public class StageSettingData : ABaseStageSettingData
{
	// Token: 0x060000DB RID: 219 RVA: 0x000048C8 File Offset: 0x00002AC8
	public WaveData GetWaveData(int waveIndex)
	{
		if (waveIndex >= this.list_WaveData.Count)
		{
			return null;
		}
		return this.list_WaveData[waveIndex];
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000048E8 File Offset: 0x00002AE8
	public WaveInfoData GetWaveInfoData(int waveIndex)
	{
		WaveData waveData = this.GetWaveData(waveIndex);
		if (waveData == null)
		{
			return null;
		}
		WaveInfoData waveInfoData = new WaveInfoData();
		foreach (MonsterSpawnData monsterSpawnData in waveData.List_SpawnData)
		{
			int count = monsterSpawnData.CountForEachSpawn * monsterSpawnData.WaveCount;
			waveInfoData.AddData(monsterSpawnData.MonsterType, count);
		}
		return waveInfoData;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00004968 File Offset: 0x00002B68
	public float GetDifficultyMultiplier(int waveIndex)
	{
		float num = 1f;
		for (int i = 0; i <= waveIndex; i++)
		{
			num *= 1f + this.list_WaveData[i].DifficultyMultiplier;
		}
		return num;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000049A2 File Offset: 0x00002BA2
	public void SetBaseDifficulty(int value)
	{
		this.difficulty = value;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000049AB File Offset: 0x00002BAB
	public int GetTotalWaveCount()
	{
		return this.list_WaveData.Count;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000049B8 File Offset: 0x00002BB8
	public void SetWaveCount(int value)
	{
		this.totalWaves = value;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000049C1 File Offset: 0x00002BC1
	public bool IsFinalWave(int waveIndex)
	{
		return waveIndex == this.list_WaveData.Count - 1;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000049D3 File Offset: 0x00002BD3
	public void RandomSeedGenerate()
	{
		this.seed = Random.Range(0, 999999999);
		this.Generate(eWorldType.WORLD_1_FOREST);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000049F0 File Offset: 0x00002BF0
	public void Generate(eWorldType worldType)
	{
		if (this.list_WaveData == null)
		{
			this.list_WaveData = new List<WaveData>();
		}
		this.list_WaveData.Clear();
		if (this.difficulty < 1 || this.difficulty > 10)
		{
			Debug.LogError("難度必須在1到10之間");
			return;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < this.maxPathCount; i++)
		{
			list.Add(i);
		}
		list.Shuffle<int>();
		Random.InitState(this.seed);
		List<WaveData> list2 = new List<WaveData>();
		for (int j = 0; j < this.totalWaves; j++)
		{
			List<MonsterSpawnData> list3 = new List<MonsterSpawnData>();
			int pathCount = this.GetPathCount(j);
			for (int k = 0; k < pathCount; k++)
			{
				float num = 0f;
				int num2 = Mathf.Clamp(Random.Range(1, 3), 1, Enum.GetValues(typeof(eMonsterType)).Length);
				int num3 = Mathf.Max(1, Mathf.CeilToInt((float)(j + 3) * ((float)this.difficulty / 3f)));
				int num4 = 1;
				for (int l = 0; l < num2; l++)
				{
					eMonsterType randomMonsterType = this.GetRandomMonsterType(this.difficulty, worldType, j, l);
					MonsterSettingData monsterSettingDataByType = StageSettingData.GetMonsterSettingDataByType(randomMonsterType);
					float spawnCountModifier = monsterSettingDataByType.GetSpawnCountModifier();
					float spawnIntervalModifier = monsterSettingDataByType.GetSpawnIntervalModifier();
					float startTime = num;
					float minMoveSpeed = monsterSettingDataByType.GetMinMoveSpeed();
					float num5 = 1f;
					switch (monsterSettingDataByType.GetMonsterSize())
					{
					case eMonsterSize.SMALL:
						num5 = 1f / minMoveSpeed;
						break;
					case eMonsterSize.MEDIUM:
						num5 = 2f / minMoveSpeed;
						break;
					case eMonsterSize.LARGE:
						num5 = 5f / minMoveSpeed;
						break;
					}
					int num6 = 1;
					int countForEachSpawn = 1;
					switch (monsterSettingDataByType.GetMonsterSize())
					{
					case eMonsterSize.SMALL:
						num6 = Mathf.CeilToInt((float)num3 * spawnIntervalModifier);
						countForEachSpawn = Mathf.CeilToInt((float)num4 * spawnCountModifier);
						break;
					case eMonsterSize.MEDIUM:
						num6 = Mathf.CeilToInt((float)num3 * spawnIntervalModifier) / 2;
						countForEachSpawn = Mathf.CeilToInt((float)num4 * spawnCountModifier);
						break;
					case eMonsterSize.LARGE:
						num6 = Mathf.CeilToInt((float)num3 * spawnIntervalModifier) / 5;
						countForEachSpawn = 1;
						break;
					}
					num += num5 * (float)num6;
					MonsterSpawnData item = new MonsterSpawnData(randomMonsterType, list[k], startTime, num5, num6, countForEachSpawn);
					list3.Add(item);
				}
			}
			WaveData item2 = new WaveData(0.05f + 0.025f * (float)this.difficulty, list3);
			list2.Add(item2);
		}
		this.list_WaveData = list2;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00004C58 File Offset: 0x00002E58
	private int GetPathCount(int waveIndex)
	{
		if (waveIndex <= 3)
		{
			return 1;
		}
		return Mathf.Min(Random.Range(2, this.maxPathCount + 1), waveIndex);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00004C74 File Offset: 0x00002E74
	private static MonsterSettingData GetMonsterSettingDataByType(eMonsterType monsterType)
	{
		return Singleton<ResourceManager>.Instance.GetMonsterDataByType(monsterType);
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00004C84 File Offset: 0x00002E84
	private eMonsterType GetRandomMonsterType(int difficulty, eWorldType worldType, int waveIndex, int monsterTypeIndex)
	{
		List<MonsterSettingData> monsterDataByWorld = Singleton<ResourceManager>.Instance.GetMonsterDataByWorld(worldType);
		if (monsterDataByWorld.Count == 0)
		{
			Debug.LogError(string.Format("在指定的世界{0}找不到可以使用的怪!! (第{1}波)", worldType, waveIndex));
		}
		if (difficulty < 3 || waveIndex < 2)
		{
			monsterDataByWorld.RemoveAll((MonsterSettingData a) => a.GetMonsterSize() == eMonsterSize.LARGE);
		}
		else if (waveIndex % 2 == 0 && monsterTypeIndex == 0)
		{
			monsterDataByWorld.RemoveAll((MonsterSettingData a) => a.GetMonsterSize() != eMonsterSize.LARGE);
		}
		if (monsterDataByWorld.Count == 0)
		{
			Debug.LogError(string.Format("在指定的世界{0}找不到可以使用的怪!! (第{1}波)", worldType, waveIndex));
			return eMonsterType.NONE;
		}
		return monsterDataByWorld.RandomItem<MonsterSettingData>().GetMonsterType();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00004D4F File Offset: 0x00002F4F
	public override string GetLocalization_Title()
	{
		return LocalizationManager.Instance.GetString("StageData", this.stageNameLoc, Array.Empty<object>());
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00004D6B File Offset: 0x00002F6B
	public override string GetLocalization_Description()
	{
		return LocalizationManager.Instance.GetString("StageData", this.stageDescriptionLoc, Array.Empty<object>());
	}

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	[Header("屬於哪個世界的關卡")]
	protected eWorldType worldType;

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	[Header("關卡名稱的多國key")]
	private string stageNameLoc;

	// Token: 0x040000A2 RID: 162
	[SerializeField]
	[Header("關卡介紹的多國key")]
	private string stageDescriptionLoc;

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	[Header("關卡特殊成就的多國key")]
	private string achievementLoc;

	// Token: 0x040000A4 RID: 164
	[SerializeField]
	private int seed;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	[Header("難度")]
	private int difficulty;

	// Token: 0x040000A6 RID: 166
	[SerializeField]
	[Header("總共波數")]
	private int totalWaves;

	// Token: 0x040000A7 RID: 167
	[SerializeField]
	[Header("最大路徑數量")]
	private int maxPathCount;

	// Token: 0x040000A8 RID: 168
	[SerializeField]
	private List<WaveData> list_WaveData;
}
