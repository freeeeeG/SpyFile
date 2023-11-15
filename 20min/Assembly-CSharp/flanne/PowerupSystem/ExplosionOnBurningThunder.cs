using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024A RID: 586
	public class ExplosionOnBurningThunder : MonoBehaviour
	{
		// Token: 0x06000CC7 RID: 3271 RVA: 0x0002E8A4 File Offset: 0x0002CAA4
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.explosionPrefab.name, this.explosionPrefab, 5, true);
			this.BurnSys = BurnSystem.SharedInstance;
			this.AddObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002E8FC File Offset: 0x0002CAFC
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0002E918 File Offset: 0x0002CB18
		private void OnThunderHit(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			if (this.BurnSys.IsBurning(gameObject))
			{
				GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.explosionPrefab.name);
				pooledObject.transform.position = gameObject.transform.position;
				pooledObject.SetActive(true);
				this.cameraShaker.Shake();
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x040008FF RID: 2303
		[SerializeField]
		private GameObject explosionPrefab;

		// Token: 0x04000900 RID: 2304
		[SerializeField]
		private ExplosionShake2D cameraShaker;

		// Token: 0x04000901 RID: 2305
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000902 RID: 2306
		private ObjectPooler OP;

		// Token: 0x04000903 RID: 2307
		private BurnSystem BurnSys;
	}
}
