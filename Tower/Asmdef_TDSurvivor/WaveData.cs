using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002E RID: 46
[Serializable]
public class WaveData
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000EA RID: 234 RVA: 0x00004D8F File Offset: 0x00002F8F
	public float DifficultyMultiplier
	{
		get
		{
			return this.difficultyMultiplier;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000EB RID: 235 RVA: 0x00004D97 File Offset: 0x00002F97
	public List<MonsterSpawnData> List_SpawnData
	{
		get
		{
			return this.list_SpawnData;
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00004D9F File Offset: 0x00002F9F
	public WaveData()
	{
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00004DB2 File Offset: 0x00002FB2
	public WaveData(float difficultyMultiplier, List<MonsterSpawnData> list_SpawnData)
	{
		this.difficultyMultiplier = difficultyMultiplier;
		this.list_SpawnData = list_SpawnData;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00004DD4 File Offset: 0x00002FD4
	public WaveData CloneData()
	{
		WaveData waveData = new WaveData();
		waveData.difficultyMultiplier = this.difficultyMultiplier;
		waveData.list_SpawnData = new List<MonsterSpawnData>();
		foreach (MonsterSpawnData monsterSpawnData in this.list_SpawnData)
		{
			waveData.list_SpawnData.Add(monsterSpawnData.CloneData());
		}
		return waveData;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00004E50 File Offset: 0x00003050
	private float CalculateInterval(MonsterSettingData data)
	{
		float result = 1f;
		float minMoveSpeed = data.GetMinMoveSpeed();
		switch (data.GetMonsterSize())
		{
		case eMonsterSize.SMALL:
			result = 1f / minMoveSpeed;
			break;
		case eMonsterSize.MEDIUM:
			result = 2f / minMoveSpeed;
			break;
		case eMonsterSize.LARGE:
			result = 5f / minMoveSpeed;
			break;
		}
		return result;
	}

	// Token: 0x040000A9 RID: 169
	[SerializeField]
	private float difficultyMultiplier = 0.1f;

	// Token: 0x040000AA RID: 170
	[SerializeField]
	[Header("每一波裡的怪物資料")]
	private List<MonsterSpawnData> list_SpawnData;
}
