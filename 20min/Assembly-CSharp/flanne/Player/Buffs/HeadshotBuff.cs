using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000172 RID: 370
	public class HeadshotBuff : Buff
	{
		// Token: 0x06000936 RID: 2358 RVA: 0x000260FE File Offset: 0x000242FE
		public override void OnAttach()
		{
			this.player = PlayerController.Instance;
			this.AddObserver(new Action<object, object>(this.OnShoot), Gun.ShootEvent);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00026122 File Offset: 0x00024322
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnShoot), Gun.ShootEvent);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002613C File Offset: 0x0002433C
		private void OnShoot(object sender, object args)
		{
			float num = this.player.stats[StatType.ProjectileSpeed].Modify(this.chance);
			num = this.player.stats[StatType.MoveSpeed].Modify(num);
			if (Random.Range(0f, 1f) < num)
			{
				ProjectileRecipe projectileRecipe = args as ProjectileRecipe;
				projectileRecipe.damage *= this.damageMultplier;
				projectileRecipe.size *= this.sizeMultiplier;
				projectileRecipe.projectileSpeed *= this.speedMultiplier;
			}
		}

		// Token: 0x040006B8 RID: 1720
		[SerializeField]
		[Range(0f, 1f)]
		private float chance;

		// Token: 0x040006B9 RID: 1721
		[SerializeField]
		private float damageMultplier = 1f;

		// Token: 0x040006BA RID: 1722
		[SerializeField]
		private float sizeMultiplier = 1f;

		// Token: 0x040006BB RID: 1723
		[SerializeField]
		private float speedMultiplier = 1f;

		// Token: 0x040006BC RID: 1724
		private PlayerController player;
	}
}
