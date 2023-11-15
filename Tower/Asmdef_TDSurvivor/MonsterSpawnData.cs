using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
[Serializable]
public class MonsterSpawnData
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004F45 File Offset: 0x00003145
	public eMonsterType MonsterType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004F4D File Offset: 0x0000314D
	public int SpawnNodeIndex
	{
		get
		{
			return this.spawnNodeIndex;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004F55 File Offset: 0x00003155
	public float StartTime
	{
		get
		{
			return this.startTime;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004F5D File Offset: 0x0000315D
	public int WaveCount
	{
		get
		{
			return this.waveCount;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004F65 File Offset: 0x00003165
	public int CountForEachSpawn
	{
		get
		{
			return this.countForEachSpawn;
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00004F6D File Offset: 0x0000316D
	public MonsterSpawnData()
	{
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00004F83 File Offset: 0x00003183
	public MonsterSpawnData(eMonsterType type, int spawnNodeIndex, float startTime, float interval, int waveCount, int countForEachSpawn)
	{
		this.type = type;
		this.spawnNodeIndex = spawnNodeIndex;
		this.startTime = startTime;
		this.waveCount = waveCount;
		this.countForEachSpawn = countForEachSpawn;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00004FC0 File Offset: 0x000031C0
	public MonsterSpawnData CloneData()
	{
		return new MonsterSpawnData
		{
			type = this.type,
			spawnNodeIndex = this.spawnNodeIndex,
			startTime = this.startTime,
			waveCount = this.waveCount,
			countForEachSpawn = this.countForEachSpawn
		};
	}

	// Token: 0x040000AE RID: 174
	[SerializeField]
	[Header("$GetDifficultyInfo")]
	private eMonsterType type;

	// Token: 0x040000AF RID: 175
	[SerializeField]
	[Header("在哪個地方出現")]
	private int spawnNodeIndex;

	// Token: 0x040000B0 RID: 176
	[SerializeField]
	[Header("開始時間")]
	private float startTime;

	// Token: 0x040000B1 RID: 177
	[SerializeField]
	[Header("總共幾次")]
	private int waveCount = 1;

	// Token: 0x040000B2 RID: 178
	[SerializeField]
	[Header("每次幾隻")]
	private int countForEachSpawn = 1;
}
