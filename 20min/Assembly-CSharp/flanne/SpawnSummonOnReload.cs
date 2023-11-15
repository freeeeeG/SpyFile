using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000067 RID: 103
	public class SpawnSummonOnReload : MonoBehaviour
	{
		// Token: 0x06000484 RID: 1156 RVA: 0x0001712D File Offset: 0x0001532D
		private void OnReload()
		{
			this.Spawn();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00017138 File Offset: 0x00015338
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.summonPrefab.name, this.summonPrefab, 30, true);
			this.ammo = PlayerController.Instance.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001719B File Offset: 0x0001539B
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000171BC File Offset: 0x000153BC
		private void Spawn()
		{
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

		// Token: 0x0400027F RID: 639
		[SerializeField]
		private GameObject summonPrefab;

		// Token: 0x04000280 RID: 640
		[SerializeField]
		private float spawnDistanceAway;

		// Token: 0x04000281 RID: 641
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000282 RID: 642
		private float _timer;

		// Token: 0x04000283 RID: 643
		private ObjectPooler OP;

		// Token: 0x04000284 RID: 644
		private Ammo ammo;
	}
}
