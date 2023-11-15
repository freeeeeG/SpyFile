using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018F RID: 399
	public class OnKillTrigger : Trigger
	{
		// Token: 0x060009A2 RID: 2466 RVA: 0x00026DC4 File Offset: 0x00024FC4
		public override void OnEquip(PlayerController player)
		{
			if (this.anyDamageType)
			{
				this.AddObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);
				return;
			}
			this.AddObserver(new Action<object, object>(this.OnKill), string.Format("Health.{0}DamageKill", this.damageType.ToString()));
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00026E20 File Offset: 0x00025020
		public override void OnUnEquip(PlayerController player)
		{
			if (this.anyDamageType)
			{
				this.RemoveObserver(new Action<object, object>(this.OnKill), Health.DeathEvent);
				return;
			}
			this.RemoveObserver(new Action<object, object>(this.OnKill), string.Format("Health.{0}DamageKill", this.damageType.ToString()));
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00026E7C File Offset: 0x0002507C
		private void OnKill(object sender, object args)
		{
			this._killCounter++;
			if (this._killCounter >= this.killsToTrigger)
			{
				this._killCounter = 0;
				if (this.actionTargetPlayer)
				{
					base.RaiseTrigger(PlayerController.Instance.gameObject);
					return;
				}
				GameObject gameObject = (sender as Health).gameObject;
				base.RaiseTrigger(gameObject);
			}
		}

		// Token: 0x040006ED RID: 1773
		[SerializeField]
		private bool anyDamageType;

		// Token: 0x040006EE RID: 1774
		[SerializeField]
		private DamageType damageType;

		// Token: 0x040006EF RID: 1775
		[SerializeField]
		private int killsToTrigger;

		// Token: 0x040006F0 RID: 1776
		[SerializeField]
		private bool actionTargetPlayer;

		// Token: 0x040006F1 RID: 1777
		[NonSerialized]
		private int _killCounter;
	}
}
