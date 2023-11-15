using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000148 RID: 328
	public class BurnFreezeOnHitRune : Rune
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x00023D9E File Offset: 0x00021F9E
		protected override void Init()
		{
			PlayerController.Instance.gameObject.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent);
			this.BurnSys = BurnSystem.SharedInstance;
			this.FreezeSys = FreezeSystem.SharedInstance;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00023DD6 File Offset: 0x00021FD6
		private void OnDestroy()
		{
			PlayerController.Instance.gameObject.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00023DF8 File Offset: 0x00021FF8
		private void OnImpact(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			if (gameObject.tag.Contains("Enemy"))
			{
				if (Random.Range(0f, 1f) < this.chancePerLevel * (float)this.level)
				{
					this.BurnSys.Burn(gameObject, this.burnDamge);
				}
				if (Random.Range(0f, 1f) < this.chancePerLevel * (float)this.level)
				{
					this.FreezeSys.Freeze(gameObject);
				}
			}
		}

		// Token: 0x04000647 RID: 1607
		[Range(0f, 1f)]
		public float chancePerLevel;

		// Token: 0x04000648 RID: 1608
		public int burnDamge;

		// Token: 0x04000649 RID: 1609
		public float freezeDuration;

		// Token: 0x0400064A RID: 1610
		private BurnSystem BurnSys;

		// Token: 0x0400064B RID: 1611
		private FreezeSystem FreezeSys;
	}
}
