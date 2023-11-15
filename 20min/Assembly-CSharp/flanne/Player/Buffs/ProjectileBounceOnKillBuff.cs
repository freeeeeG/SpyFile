using System;

namespace flanne.Player.Buffs
{
	// Token: 0x02000176 RID: 374
	public class ProjectileBounceOnKillBuff : Buff
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x000262F8 File Offset: 0x000244F8
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnKill), Projectile.KillEvent);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00026311 File Offset: 0x00024511
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnKill), Projectile.KillEvent);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002632A File Offset: 0x0002452A
		private void OnKill(object sender, object args)
		{
			(sender as Projectile).bounce++;
		}
	}
}
