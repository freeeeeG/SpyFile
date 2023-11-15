using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000242 RID: 578
	public class AmmoOnThunder : MonoBehaviour
	{
		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002E200 File Offset: 0x0002C400
		private void Start()
		{
			this.ammo = base.transform.parent.GetComponentInChildren<Ammo>();
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.staticObjectPoolTag, this.staticPrefab, 5, true);
			base.transform.localPosition = Vector3.zero;
			this.AddObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002E26E File Offset: 0x0002C46E
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002E288 File Offset: 0x0002C488
		private void OnThunderHit(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToActivate)
			{
				if (this.ammo != null)
				{
					this.ammo.GainAmmo(this.ammoRefillAmount);
					GameObject pooledObject = this.OP.GetPooledObject(this.staticObjectPoolTag);
					pooledObject.transform.SetParent(base.transform);
					pooledObject.transform.localPosition = Vector3.zero;
					pooledObject.SetActive(true);
					this.sfx.Play(null);
					return;
				}
				Debug.LogWarning("No ammo component found");
			}
		}

		// Token: 0x040008D9 RID: 2265
		[SerializeField]
		private GameObject staticPrefab;

		// Token: 0x040008DA RID: 2266
		[SerializeField]
		private string staticObjectPoolTag;

		// Token: 0x040008DB RID: 2267
		[SerializeField]
		private SoundEffectSO sfx;

		// Token: 0x040008DC RID: 2268
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToActivate;

		// Token: 0x040008DD RID: 2269
		[SerializeField]
		private int ammoRefillAmount;

		// Token: 0x040008DE RID: 2270
		private ObjectPooler OP;

		// Token: 0x040008DF RID: 2271
		private Ammo ammo;
	}
}
