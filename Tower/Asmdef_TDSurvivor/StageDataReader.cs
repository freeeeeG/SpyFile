using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class StageDataReader : Singleton<StageDataReader>
{
	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600051E RID: 1310 RVA: 0x00014A9B File Offset: 0x00012C9B
	public StageSettingData CurrentUsingData
	{
		get
		{
			return this.stageData;
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00014AA3 File Offset: 0x00012CA3
	private void OnEnable()
	{
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterDespawn, new Action<AMonsterBase>(this.OnMonsterDespawn));
		EventMgr.Register(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00014AD5 File Offset: 0x00012CD5
	private void OnDisable()
	{
		EventMgr.Remove<AMonsterBase>(eGameEvents.MonsterDespawn, new Action<AMonsterBase>(this.OnMonsterDespawn));
		EventMgr.Remove(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00014B07 File Offset: 0x00012D07
	private void OnMonsterDespawn(AMonsterBase monster)
	{
		if (monster.RemainingDistance < this.minDistanceLastWave)
		{
			this.minDistanceLastWave = monster.RemainingDistance;
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00014B24 File Offset: 0x00012D24
	private void OnBattleEnd()
	{
		float t = Mathf.InverseLerp(3f, 30f, this.minDistanceLastWave);
		this.difficultyAdjustmentByPerformance = Mathf.Lerp(0.8f, 1.3f, t);
		Debug.Log(string.Format("上一波最短距離 {0}, 下一波難度調整: x{1}", this.minDistanceLastWave, this.difficultyAdjustmentByPerformance));
		this.difficultyAdjustmentByPerformance = 1f;
		this.minDistanceLastWave = float.PositiveInfinity;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00014B98 File Offset: 0x00012D98
	public void LoadStageData(StageSettingData stageData)
	{
		if (stageData == null)
		{
			if (this.testData != null)
			{
				int difficulty = GameDataManager.instance.IntermediateData.difficulty;
				int waveCount;
				if (difficulty <= 1)
				{
					waveCount = 3;
				}
				else if (difficulty <= 3)
				{
					waveCount = 5;
				}
				else if (difficulty <= 7)
				{
					waveCount = 7;
				}
				else
				{
					waveCount = 10;
				}
				this.testData.SetBaseDifficulty(difficulty);
				this.testData.SetWaveCount(waveCount);
				this.testData.RandomSeedGenerate();
				this.stageData = this.testData;
			}
		}
		else
		{
			this.stageData = stageData;
		}
		this.ResetStage();
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00014C27 File Offset: 0x00012E27
	private void ResetStage()
	{
		this.currentWaveIndex = -1;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00014C30 File Offset: 0x00012E30
	public bool HasNextWave()
	{
		return !this.stageData.IsFinalWave(this.currentWaveIndex);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00014C46 File Offset: 0x00012E46
	public WaveInfoData GetNextWaveMonsterInfo()
	{
		return this.stageData.GetWaveInfoData(this.currentWaveIndex + 1);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00014C5C File Offset: 0x00012E5C
	public List<int> GetNextWaveSpawnIndexs()
	{
		List<int> list = new List<int>();
		if (!this.CheckStageDataExist())
		{
			return list;
		}
		if (!this.HasNextWave())
		{
			return list;
		}
		int waveIndex = this.currentWaveIndex + 1;
		foreach (MonsterSpawnData monsterSpawnData in this.stageData.GetWaveData(waveIndex).List_SpawnData)
		{
			if (!list.Contains(monsterSpawnData.SpawnNodeIndex))
			{
				list.Add(monsterSpawnData.SpawnNodeIndex);
			}
		}
		return list;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00014CF4 File Offset: 0x00012EF4
	public int GetTotalWaveCount()
	{
		return this.stageData.GetTotalWaveCount();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00014D01 File Offset: 0x00012F01
	public float GetNextWaveDifficulty()
	{
		if (!this.HasNextWave())
		{
			return -1f;
		}
		return this.stageData.GetDifficultyMultiplier(this.currentWaveIndex + 1) * this.difficultyAdjustmentByPerformance;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00014D2B File Offset: 0x00012F2B
	public float GetCurrentDifficulty()
	{
		return this.stageData.GetDifficultyMultiplier(this.currentWaveIndex) * this.difficultyAdjustmentByPerformance;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00014D45 File Offset: 0x00012F45
	private bool CheckStageDataExist()
	{
		if (this.stageData == null)
		{
			Debug.LogError("沒有讀取關卡檔案");
			return false;
		}
		return true;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00014D64 File Offset: 0x00012F64
	public void StartNextWave(Action waveFinishCallback = null)
	{
		if (!this.CheckStageDataExist())
		{
			return;
		}
		Debug.Log(string.Format("開始第#{0}波怪物", this.currentWaveIndex + 1));
		this.currentWaveIndex++;
		EventMgr.SendEvent<int, bool>(eGameEvents.OnWaveIndexChanged, this.currentWaveIndex, this.currentWaveIndex == this.stageData.GetTotalWaveCount() - 1);
		WaveData waveData = this.stageData.GetWaveData(this.currentWaveIndex).CloneData();
		this.stageData.IsFinalWave(this.currentWaveIndex);
		base.StartCoroutine(this.CR_WaveProc(waveData, waveFinishCallback));
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00014E04 File Offset: 0x00013004
	private IEnumerator CR_WaveProc(WaveData waveData, Action waveFinishCallback)
	{
		float time = 0f;
		int executingCoroutine = 0;
		List<Coroutine> list_PlayingMonsterWaves = new List<Coroutine>();
		yield return new WaitForSeconds(2f);
		Action <>9__0;
		while (waveData.List_SpawnData.Count > 0)
		{
			time += Time.deltaTime;
			yield return null;
			for (int i = waveData.List_SpawnData.Count - 1; i >= 0; i--)
			{
				MonsterSpawnData monsterSpawnData = waveData.List_SpawnData[i];
				if (time >= monsterSpawnData.StartTime)
				{
					int executingCoroutine3 = executingCoroutine;
					executingCoroutine = executingCoroutine3 + 1;
					MonsterSpawnData data = monsterSpawnData;
					Action onCompleteCallback;
					if ((onCompleteCallback = <>9__0) == null)
					{
						onCompleteCallback = (<>9__0 = delegate()
						{
							int executingCoroutine2 = executingCoroutine;
							executingCoroutine = executingCoroutine2 - 1;
						});
					}
					Coroutine item = base.StartCoroutine(this.CR_MonsterWaveProc(data, onCompleteCallback));
					list_PlayingMonsterWaves.Add(item);
					waveData.List_SpawnData.RemoveAt(i);
				}
			}
		}
		Debug.Log("所有怪物資料都啟動了");
		bool flag;
		do
		{
			yield return null;
			flag = (executingCoroutine != 0);
		}
		while (!flag);
		DebugManager.Log(eDebugKey.STAGE, "所有怪物資料播放完成, 等待清場...", null);
		yield return null;
		if (this.stageData.IsFinalWave(this.currentWaveIndex))
		{
			while (Singleton<MonsterManager>.Instance.GetMonsterOnFieldCount() != 0)
			{
				if (Singleton<MonsterManager>.Instance.GetMonsterOnFieldCountWithoutBoss() == 0 && Singleton<MonsterManager>.Instance.GetBoss().IsDead())
				{
					break;
				}
				yield return null;
			}
		}
		else
		{
			while (Singleton<MonsterManager>.Instance.GetMonsterOnFieldCountWithoutBoss() != 0)
			{
				yield return null;
			}
		}
		if (waveFinishCallback != null)
		{
			waveFinishCallback();
		}
		DebugManager.Log(eDebugKey.STAGE, string.Format("Wave #{0} 結束", this.currentWaveIndex), null);
		yield break;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00014E21 File Offset: 0x00013021
	private IEnumerator CR_MonsterWaveProc(MonsterSpawnData data, Action onCompleteCallback = null)
	{
		int num;
		for (int i = 0; i < data.WaveCount; i = num + 1)
		{
			MonsterSettingData monsterDataByType = Singleton<ResourceManager>.Instance.GetMonsterDataByType(data.MonsterType);
			MonsterSpawnRequest arg = new MonsterSpawnRequest
			{
				spawnNodeIndex = data.SpawnNodeIndex,
				type = data.MonsterType,
				count = data.CountForEachSpawn,
				isCorrupted = GameDataManager.instance.IntermediateData.isCorrupted
			};
			EventMgr.SendEvent<MonsterSpawnRequest>(eGameEvents.RequestSpawnMonster, arg);
			float seconds = 1f;
			float averageMoveSpeed = monsterDataByType.GetAverageMoveSpeed();
			switch (monsterDataByType.GetMonsterSize())
			{
			case eMonsterSize.SMALL:
				seconds = 1.25f / averageMoveSpeed;
				break;
			case eMonsterSize.MEDIUM:
				seconds = 3.25f / averageMoveSpeed;
				break;
			case eMonsterSize.LARGE:
				seconds = 6f / averageMoveSpeed;
				break;
			}
			yield return new WaitForSeconds(seconds);
			num = i;
		}
		if (onCompleteCallback != null)
		{
			onCompleteCallback();
		}
		yield break;
	}

	// Token: 0x040004D5 RID: 1237
	[SerializeField]
	private StageSettingData stageData;

	// Token: 0x040004D6 RID: 1238
	[SerializeField]
	private StageSettingData testData;

	// Token: 0x040004D7 RID: 1239
	private int currentWaveIndex;

	// Token: 0x040004D8 RID: 1240
	[SerializeField]
	private float difficultyAdjustmentByPerformance = 1f;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	private float minDistanceLastWave = float.PositiveInfinity;
}
