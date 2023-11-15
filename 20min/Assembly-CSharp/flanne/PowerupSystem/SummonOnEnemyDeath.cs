using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000255 RID: 597
	public class SummonOnEnemyDeath : MonoBehaviour
	{
		// Token: 0x06000CFA RID: 3322 RVA: 0x0002F4AC File Offset: 0x0002D6AC
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.summonPrefab.name, this.summonPrefab, 200, true);
			this.player = base.GetComponentInParent<PlayerController>();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002F509 File Offset: 0x0002D709
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002F524 File Offset: 0x0002D724
		private void OnDeath(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (gameObject.tag == "Enemy")
			{
				GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.summonPrefab.name);
				pooledObject.transform.SetParent(this.player.transform);
				pooledObject.transform.position = gameObject.transform.position;
				pooledObject.SetActive(true);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x04000942 RID: 2370
		[SerializeField]
		private GameObject summonPrefab;

		// Token: 0x04000943 RID: 2371
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000944 RID: 2372
		private ObjectPooler OP;

		// Token: 0x04000945 RID: 2373
		private PlayerController player;
	}
}
