using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024E RID: 590
	public class IcicleOnLastAmmo : MonoBehaviour
	{
		// Token: 0x06000CDB RID: 3291 RVA: 0x0002EC6C File Offset: 0x0002CE6C
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.iciclePrefab.name, this.iciclePrefab, 10, true);
			this.player = base.GetComponentInParent<PlayerController>();
			this.ammo = this.player.ammo;
			this.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0002ECDC File Offset: 0x0002CEDC
		private void OnDestroy()
		{
			this.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0002ECFA File Offset: 0x0002CEFA
		private void OnAmmoChanged(int ammoAmount)
		{
			if (ammoAmount == 0)
			{
				this.SpawnIcicle();
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002ED08 File Offset: 0x0002CF08
		private void SpawnIcicle()
		{
			for (int i = 0; i < this.numIcicles; i++)
			{
				GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.iciclePrefab.name);
				pooledObject.transform.position = this.player.transform.position;
				SeekEnemy component = pooledObject.GetComponent<SeekEnemy>();
				if (component != null)
				{
					component.player = this.player.transform;
				}
				MoveComponent2D component2 = pooledObject.GetComponent<MoveComponent2D>();
				if (component2 != null)
				{
					component2.vector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
				}
				pooledObject.SetActive(true);
			}
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x04000916 RID: 2326
		[SerializeField]
		private GameObject iciclePrefab;

		// Token: 0x04000917 RID: 2327
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000918 RID: 2328
		[SerializeField]
		private int numIcicles;

		// Token: 0x04000919 RID: 2329
		private ObjectPooler OP;

		// Token: 0x0400091A RID: 2330
		private PlayerController player;

		// Token: 0x0400091B RID: 2331
		private Ammo ammo;
	}
}
