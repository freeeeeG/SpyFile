using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CF RID: 463
	public class ShatterFrozenAction : Action
	{
		// Token: 0x06000A4D RID: 2637 RVA: 0x00028110 File Offset: 0x00026310
		public override void Activate(GameObject target)
		{
			ObjectPooler sharedInstance = ObjectPooler.SharedInstance;
			sharedInstance.AddObject(this.shatterPrefab.name, this.shatterPrefab, 25, true);
			Health component = target.GetComponent<Health>();
			if (FreezeSystem.SharedInstance.IsFrozen(component.gameObject))
			{
				GameObject pooledObject = sharedInstance.GetPooledObject(this.shatterPrefab.name);
				pooledObject.transform.position = component.transform.position;
				pooledObject.GetComponent<Harmful>().damageAmount = Mathf.FloorToInt((float)component.maxHP * this.shatterPercentDamage);
				pooledObject.SetActive(true);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x04000747 RID: 1863
		[SerializeField]
		private GameObject shatterPrefab;

		// Token: 0x04000748 RID: 1864
		[Range(0f, 1f)]
		[SerializeField]
		private float shatterPercentDamage;

		// Token: 0x04000749 RID: 1865
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
