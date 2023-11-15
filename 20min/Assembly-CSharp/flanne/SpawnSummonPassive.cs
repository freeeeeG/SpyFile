using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000068 RID: 104
	public class SpawnSummonPassive : MonoBehaviour
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x00017258 File Offset: 0x00015458
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.summonPrefab.name, this.summonPrefab, 30, true);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00017284 File Offset: 0x00015484
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer > this.cooldown)
			{
				this._timer -= this.cooldown;
				GameObject pooledObject = this.OP.GetPooledObject(this.summonPrefab.name);
				Vector3 normalized = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
				Vector3 position = base.transform.position + normalized * this.spawnDistanceAway;
				pooledObject.transform.position = position;
				pooledObject.GetComponent<Spawn>();
				pooledObject.SetActive(true);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x04000285 RID: 645
		[SerializeField]
		private GameObject summonPrefab;

		// Token: 0x04000286 RID: 646
		[SerializeField]
		private float spawnDistanceAway;

		// Token: 0x04000287 RID: 647
		[SerializeField]
		private float cooldown;

		// Token: 0x04000288 RID: 648
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000289 RID: 649
		private float _timer;

		// Token: 0x0400028A RID: 650
		private ObjectPooler OP;
	}
}
