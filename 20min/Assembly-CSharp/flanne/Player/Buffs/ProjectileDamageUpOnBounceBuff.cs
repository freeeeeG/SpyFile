using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000177 RID: 375
	public class ProjectileDamageUpOnBounceBuff : Buff
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x0002633F File Offset: 0x0002453F
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnKill), Projectile.BounceEvent);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00026358 File Offset: 0x00024558
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnKill), Projectile.BounceEvent);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00026371 File Offset: 0x00024571
		private void OnKill(object sender, object args)
		{
			(sender as Projectile).damage *= 1f + this.damageUpPerBounce;
		}

		// Token: 0x040006BF RID: 1727
		[SerializeField]
		private float damageUpPerBounce;
	}
}
