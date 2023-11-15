using System;

namespace flanne.Player.Buffs
{
	// Token: 0x02000165 RID: 357
	public class AddBasePiercingToBounceBuff : Buff
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x00025D79 File Offset: 0x00023F79
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnTweakPierceBounce), Projectile.TweakPierceBounce);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00025D92 File Offset: 0x00023F92
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakPierceBounce), Projectile.TweakPierceBounce);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00025DAC File Offset: 0x00023FAC
		private void OnTweakPierceBounce(object sender, object args)
		{
			Projectile projectile = sender as Projectile;
			int piercing = projectile.piercing;
			projectile.piercing = 0;
			projectile.bounce += piercing;
		}
	}
}
