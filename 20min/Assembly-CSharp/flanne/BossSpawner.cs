using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000095 RID: 149
	public class BossSpawner : MonoBehaviour
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x00019E90 File Offset: 0x00018090
		public void LoadSpawners(List<BossSpawn> spawners)
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.arenaMonsterPrefab.name, this.arenaMonsterPrefab, 1, true);
			foreach (BossSpawn bossSpawn in spawners)
			{
				this.OP.AddObject(bossSpawn.bossPrefab.name, bossSpawn.bossPrefab, 1, true);
				base.StartCoroutine(this.WaitToSpawnCR(bossSpawn));
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00019F2C File Offset: 0x0001812C
		private IEnumerator WaitToSpawnCR(BossSpawn spawner)
		{
			yield return new WaitForSeconds(spawner.timeToSpawn);
			if (spawner.killAllOnSpawn)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
				for (int i = 0; i < array.Length; i++)
				{
					Health component = array[i].GetComponent<Health>();
					if (component != null)
					{
						component.AutoKill(true);
					}
				}
			}
			GameObject pooledObject = this.OP.GetPooledObject(spawner.bossPrefab.name);
			pooledObject.transform.position = this.playerTransform.position + spawner.spawnPosition;
			Health component2 = pooledObject.GetComponent<Health>();
			if (component2 != null)
			{
				component2.maxHP = Mathf.FloorToInt((float)spawner.maxHP * this.healthMultiplier);
			}
			AIComponent component3 = pooledObject.GetComponent<AIComponent>();
			if (component3 != null)
			{
				component3.damageToPlayer = this.enemyDamage;
				component3.specialCooldown /= this.cooldownRate;
			}
			pooledObject.SetActive(true);
			if (!spawner.dontSpawnArena)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.arenaMonsterPrefab);
				gameObject.transform.position = this.playerTransform.position;
				gameObject.SetActive(true);
			}
			yield break;
		}

		// Token: 0x0400034B RID: 843
		[SerializeField]
		private Transform playerTransform;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		private GameObject arenaMonsterPrefab;

		// Token: 0x0400034D RID: 845
		[NonSerialized]
		public float healthMultiplier = 1f;

		// Token: 0x0400034E RID: 846
		[NonSerialized]
		public int enemyDamage = 1;

		// Token: 0x0400034F RID: 847
		[NonSerialized]
		public float cooldownRate = 1f;

		// Token: 0x04000350 RID: 848
		private ObjectPooler OP;
	}
}
