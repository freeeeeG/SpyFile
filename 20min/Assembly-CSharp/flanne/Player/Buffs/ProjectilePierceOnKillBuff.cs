using System;

namespace flanne.Player.Buffs
{
	// Token: 0x02000178 RID: 376
	public class ProjectilePierceOnKillBuff : Buff
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x00026391 File Offset: 0x00024591
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnKill), Projectile.KillEvent);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000263AA File Offset: 0x000245AA
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnKill), Projectile.KillEvent);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000263C3 File Offset: 0x000245C3
		private void OnKill(object sender, object args)
		{
			(sender as Projectile).piercing++;
		}
	}
}
