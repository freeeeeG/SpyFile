using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024D RID: 589
	public class IceShatterOnDeath : MonoBehaviour
	{
		// Token: 0x06000CD7 RID: 3287 RVA: 0x0002EB50 File Offset: 0x0002CD50
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.shatterPrefab.name, this.shatterPrefab, 25, true);
			this.FreezeSys = FreezeSystem.SharedInstance;
			this.player = base.GetComponentInParent<PlayerController>();
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002EBB5 File Offset: 0x0002CDB5
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002EBD0 File Offset: 0x0002CDD0
		private void OnDeath(object sender, object args)
		{
			Health health = sender as Health;
			GameObject gameObject = health.gameObject;
			if (gameObject.tag == "Enemy" && this.FreezeSys.IsFrozen(gameObject))
			{
				GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.shatterPrefab.name);
				pooledObject.transform.position = gameObject.transform.position;
				pooledObject.GetComponent<Harmful>().damageAmount = Mathf.FloorToInt((float)health.maxHP * this.shatterPercentDamage);
				pooledObject.SetActive(true);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x04000910 RID: 2320
		[SerializeField]
		private GameObject shatterPrefab;

		// Token: 0x04000911 RID: 2321
		[Range(0f, 1f)]
		[SerializeField]
		private float shatterPercentDamage;

		// Token: 0x04000912 RID: 2322
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000913 RID: 2323
		private ObjectPooler OP;

		// Token: 0x04000914 RID: 2324
		private FreezeSystem FreezeSys;

		// Token: 0x04000915 RID: 2325
		private PlayerController player;
	}
}
