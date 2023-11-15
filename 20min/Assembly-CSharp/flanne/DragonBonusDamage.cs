using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000DF RID: 223
	public class DragonBonusDamage : MonoBehaviour
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001DFC6 File Offset: 0x0001C1C6
		private int damage
		{
			get
			{
				return Mathf.FloorToInt((float)this._dragon.baseDamage * this.percentOfDragonsDamage);
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
		private void Start()
		{
			foreach (ShootingSummon shootingSummon in base.GetComponentInParent<PlayerController>().GetComponentsInChildren<ShootingSummon>(true))
			{
				if (shootingSummon.SummonTypeID == "Dragon")
				{
					this._dragon = shootingSummon;
				}
			}
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001E046 File Offset: 0x0001C246
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001E06C File Offset: 0x0001C26C
		private void OnImpact(object sender, object args)
		{
			if (this._dragon == null)
			{
				return;
			}
			if ((sender as MonoBehaviour).gameObject.tag == "Bullet")
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					Health component = gameObject.GetComponent<Health>();
					if (component == null)
					{
						return;
					}
					component.HPChange(-1 * this.damage);
				}
			}
		}

		// Token: 0x0400047B RID: 1147
		[Range(0f, 1f)]
		[SerializeField]
		private float percentOfDragonsDamage;

		// Token: 0x0400047C RID: 1148
		private ShootingSummon _dragon;
	}
}
