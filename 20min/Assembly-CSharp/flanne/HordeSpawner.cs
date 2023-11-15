using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000096 RID: 150
	public class HordeSpawner : MonoBehaviour
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00019F67 File Offset: 0x00018167
		private void Awake()
		{
			this.activeSpawners = new List<SpawnSession>();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00019F74 File Offset: 0x00018174
		private void Update()
		{
			for (int i = 0; i < this.activeSpawners.Count; i++)
			{
				this.activeSpawners[i].timer -= Time.deltaTime;
				if (this.activeSpawners[i].timer < 0f)
				{
					if (this.CountActiveObjects(this.activeSpawners[i].monsterPrefab.name) < this.activeSpawners[i].maximum)
					{
						int num = Mathf.FloorToInt((float)this.activeSpawners[i].numPerSpawn * this.spawnRateMulitplier);
						for (int j = 0; j < num; j++)
						{
							this.Spawn(this.activeSpawners[i].monsterPrefab.name, this.activeSpawners[i].HP, this.activeSpawners[i].isElite);
						}
					}
					this.activeSpawners[i].timer += this.activeSpawners[i].spawnCooldown;
				}
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A098 File Offset: 0x00018298
		public void LoadSpawners(List<SpawnSession> spawnSessions)
		{
			this.OP = ObjectPooler.SharedInstance;
			foreach (SpawnSession spawner in spawnSessions)
			{
				base.StartCoroutine(this.SpawnerLifeCycleCR(spawner));
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A0F8 File Offset: 0x000182F8
		private void Spawn(string objectPoolTag, int HP, bool isElite)
		{
			GameObject pooledObject = this.OP.GetPooledObject(objectPoolTag);
			Vector2 vector = Random.insideUnitCircle.normalized * this.spawnRadius;
			pooledObject.transform.position = this.playerTransform.position + new Vector3(vector.x, vector.y, 0f);
			Health component = pooledObject.GetComponent<Health>();
			if (component != null)
			{
				if (isElite)
				{
					component.maxHP = Mathf.FloorToInt((float)HP * this.eliteHealthMultiplier);
				}
				else
				{
					component.maxHP = Mathf.FloorToInt((float)HP * this.healthMultiplier);
				}
			}
			AIComponent component2 = pooledObject.GetComponent<AIComponent>();
			if (component2 != null)
			{
				if (isElite)
				{
					component2.damageToPlayer = this.eliteDamage;
					component2.maxMoveSpeed = component2.baseMaxMoveSpeed * this.eliteSpeedMultiplier;
					component2.acceleration = component2.baseAcceleration * this.eliteSpeedMultiplier;
				}
				else
				{
					component2.damageToPlayer = this.enemyDamage;
					component2.maxMoveSpeed = component2.baseMaxMoveSpeed * this.speedMultiplier;
					component2.acceleration = component2.baseAcceleration * this.speedMultiplier;
				}
			}
			pooledObject.SetActive(true);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A218 File Offset: 0x00018418
		private int CountActiveObjects(string objectPoolTag)
		{
			int num = 0;
			List<GameObject> allPooledObjects = this.OP.GetAllPooledObjects(objectPoolTag);
			for (int i = 0; i < allPooledObjects.Count; i++)
			{
				if (allPooledObjects[i].activeInHierarchy)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A258 File Offset: 0x00018458
		private IEnumerator SpawnerLifeCycleCR(SpawnSession spawner)
		{
			if (spawner.isElite)
			{
				this.OP.AddObject(spawner.monsterPrefab.name, spawner.monsterPrefab, 1, true);
			}
			else
			{
				this.OP.AddObject(spawner.monsterPrefab.name, spawner.monsterPrefab, 1000, true);
			}
			yield return new WaitForSeconds(spawner.startTime);
			this.activeSpawners.Add(spawner);
			spawner.timer = 0f;
			yield return new WaitForSeconds(spawner.duration);
			this.activeSpawners.Remove(spawner);
			yield break;
		}

		// Token: 0x04000351 RID: 849
		[SerializeField]
		private Transform playerTransform;

		// Token: 0x04000352 RID: 850
		[SerializeField]
		private float spawnRadius;

		// Token: 0x04000353 RID: 851
		[NonSerialized]
		public float spawnRateMulitplier = 1f;

		// Token: 0x04000354 RID: 852
		[NonSerialized]
		public float healthMultiplier = 1f;

		// Token: 0x04000355 RID: 853
		[NonSerialized]
		public float eliteHealthMultiplier = 1f;

		// Token: 0x04000356 RID: 854
		[NonSerialized]
		public int enemyDamage = 1;

		// Token: 0x04000357 RID: 855
		[NonSerialized]
		public int eliteDamage = 1;

		// Token: 0x04000358 RID: 856
		[NonSerialized]
		public float speedMultiplier = 1f;

		// Token: 0x04000359 RID: 857
		[NonSerialized]
		public float eliteSpeedMultiplier = 1f;

		// Token: 0x0400035A RID: 858
		private List<SpawnSession> activeSpawners;

		// Token: 0x0400035B RID: 859
		private ObjectPooler OP;
	}
}
