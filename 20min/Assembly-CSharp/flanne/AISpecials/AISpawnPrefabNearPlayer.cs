using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000268 RID: 616
	[CreateAssetMenu(fileName = "AISpawnPrefabNearPlayer", menuName = "AISpecials/AISpawnPrefabNearPlayer")]
	public class AISpawnPrefabNearPlayer : AISpecial
	{
		// Token: 0x06000D44 RID: 3396 RVA: 0x000303DD File Offset: 0x0002E5DD
		public override void Use(AIComponent ai, Transform target)
		{
			ai.StartCoroutine(this.SpawnCR(ai, target));
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000303EE File Offset: 0x0002E5EE
		private IEnumerator SpawnCR(AIComponent ai, Transform target)
		{
			ObjectPooler OP = ObjectPooler.SharedInstance;
			OP.AddObject(this.enemyPrefab.name, this.enemyPrefab, 10, true);
			int num;
			for (int i = 0; i < this.numToSpawn; i = num + 1)
			{
				GameObject pooledObject = OP.GetPooledObject(this.enemyPrefab.name);
				Vector3 normalized = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
				Vector3 position = target.position + normalized * this.spawnDistanceFromPlayer;
				pooledObject.transform.position = position;
				pooledObject.SetActive(true);
				yield return new WaitForSeconds(this.delayBetweenSpawn);
				num = i;
			}
			yield break;
		}

		// Token: 0x040009A6 RID: 2470
		[SerializeField]
		private GameObject enemyPrefab;

		// Token: 0x040009A7 RID: 2471
		[SerializeField]
		private float spawnDistanceFromPlayer;

		// Token: 0x040009A8 RID: 2472
		[SerializeField]
		private int numToSpawn = 1;

		// Token: 0x040009A9 RID: 2473
		[SerializeField]
		private float delayBetweenSpawn;
	}
}
