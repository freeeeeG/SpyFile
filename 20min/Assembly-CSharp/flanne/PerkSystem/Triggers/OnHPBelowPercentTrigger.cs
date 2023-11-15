using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018D RID: 397
	public class OnHPBelowPercentTrigger : Trigger
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x00026C9B File Offset: 0x00024E9B
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnTookDamage), Health.TookDamageEvent);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00026CB4 File Offset: 0x00024EB4
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnTookDamage), Health.TookDamageEvent);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00026CD0 File Offset: 0x00024ED0
		private void OnTookDamage(object sender, object args)
		{
			Health health = sender as Health;
			if ((float)health.HP / (float)health.maxHP <= this.hpPrecentTheshold && health.HP != 0)
			{
				base.RaiseTrigger(health.gameObject);
			}
		}

		// Token: 0x040006EA RID: 1770
		[Range(0f, 1f)]
		[SerializeField]
		private float hpPrecentTheshold;
	}
}
