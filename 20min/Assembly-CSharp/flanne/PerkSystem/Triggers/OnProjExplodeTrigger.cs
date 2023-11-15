using System;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000193 RID: 403
	public class OnProjExplodeTrigger : Trigger
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x00026FB3 File Offset: 0x000251B3
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnExplode), ExplosiveProjectile.ProjExplodeEvent);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00026FCC File Offset: 0x000251CC
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnExplode), ExplosiveProjectile.ProjExplodeEvent);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00026FE8 File Offset: 0x000251E8
		private void OnExplode(object sender, object args)
		{
			ExplosiveProjectile explosiveProjectile = sender as ExplosiveProjectile;
			base.RaiseTrigger(explosiveProjectile.gameObject);
		}
	}
}
