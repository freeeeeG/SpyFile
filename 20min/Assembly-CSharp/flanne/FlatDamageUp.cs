using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E2 RID: 226
	public class FlatDamageUp : MonoBehaviour
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0001E2B1 File Offset: 0x0001C4B1
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001E2F8 File Offset: 0x0001C4F8
		private void OnImpact(object sender, object args)
		{
			if ((sender as MonoBehaviour).gameObject.tag == "Bullet")
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					Health component = gameObject.gameObject.GetComponent<Health>();
					if (component == null)
					{
						return;
					}
					component.HPChange(-1 * this.damage);
				}
			}
		}

		// Token: 0x04000488 RID: 1160
		public int damage;
	}
}
