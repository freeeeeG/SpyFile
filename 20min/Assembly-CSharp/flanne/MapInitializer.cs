using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000CA RID: 202
	public class MapInitializer : MonoBehaviour
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x0001D1B4 File Offset: 0x0001B3B4
		private void Start()
		{
			MapData mapData = SelectedMap.MapData;
			if (mapData == null)
			{
				mapData = this.defaultMap;
			}
			if (mapData.endless)
			{
				base.StartCoroutine(this.EndlessLoop(mapData));
				this.gameTimer.endless = true;
			}
			else
			{
				this.hordeSpawner.LoadSpawners(mapData.spawnSessions);
				this.bossSpawner.LoadSpawners(mapData.bossSpawns);
				this.gameTimer.timeLimit = mapData.timeLimit;
			}
			PowerupGenerator.Instance.InitPowerupPool(mapData.numPowerupsRepeat);
			Object.Instantiate<GameObject>(mapData.mapPrefab);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001D24A File Offset: 0x0001B44A
		private IEnumerator EndlessLoop(MapData mapData)
		{
			this.hordeSpawner.LoadSpawners(mapData.spawnSessions);
			this.bossSpawner.LoadSpawners(mapData.bossSpawns);
			yield return new WaitForSeconds(mapData.timeLimit);
			int cycle = 1;
			for (;;)
			{
				this.hordeSpawner.healthMultiplier = 1f + Mathf.Pow((float)cycle, 3f);
				this.hordeSpawner.eliteHealthMultiplier = 1f + Mathf.Pow((float)cycle, 2f);
				this.hordeSpawner.speedMultiplier += 0.2f;
				this.bossSpawner.healthMultiplier += 3f;
				this.bossSpawner.cooldownRate += 0.25f;
				this.hordeSpawner.LoadSpawners(mapData.endlessSpawnSessions);
				this.bossSpawner.LoadSpawners(mapData.endlessBossSpawn);
				int num = cycle;
				cycle = num + 1;
				yield return new WaitForSeconds(mapData.endlessLoopTime);
			}
			yield break;
		}

		// Token: 0x04000432 RID: 1074
		[SerializeField]
		private HordeSpawner hordeSpawner;

		// Token: 0x04000433 RID: 1075
		[SerializeField]
		private BossSpawner bossSpawner;

		// Token: 0x04000434 RID: 1076
		[SerializeField]
		private MapData defaultMap;

		// Token: 0x04000435 RID: 1077
		[SerializeField]
		private GameTimer gameTimer;
	}
}
