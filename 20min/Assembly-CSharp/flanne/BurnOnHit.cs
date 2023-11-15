using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D5 RID: 213
	public class BurnOnHit : MonoBehaviour
	{
		// Token: 0x06000684 RID: 1668 RVA: 0x0001D858 File Offset: 0x0001BA58
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
			this.BurnSys = BurnSystem.SharedInstance;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001D886 File Offset: 0x0001BA86
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001D8AC File Offset: 0x0001BAAC
		private void OnImpact(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToHit)
			{
				GameObject gameObject = args as GameObject;
				if (gameObject.tag.Contains("Enemy"))
				{
					this.BurnSys.Burn(gameObject, this.burnDamge);
				}
			}
		}

		// Token: 0x04000450 RID: 1104
		[Range(0f, 1f)]
		public float chanceToHit;

		// Token: 0x04000451 RID: 1105
		public int burnDamge;

		// Token: 0x04000452 RID: 1106
		private BurnSystem BurnSys;
	}
}
