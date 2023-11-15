using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018E RID: 398
	public class OnHitTrigger : Trigger
	{
		// Token: 0x0600099E RID: 2462 RVA: 0x00026D0F File Offset: 0x00024F0F
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00026D32 File Offset: 0x00024F32
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00026D58 File Offset: 0x00024F58
		private void OnImpact(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			PlayerController instance = PlayerController.Instance;
			if (gameObject == instance)
			{
				return;
			}
			if (Random.Range(0f, 1f) < this.triggerChance)
			{
				if (this.actionTargetPlayer)
				{
					base.RaiseTrigger(instance.gameObject);
					return;
				}
				base.RaiseTrigger(gameObject);
			}
		}

		// Token: 0x040006EB RID: 1771
		[Range(0f, 1f)]
		[SerializeField]
		private float triggerChance = 1f;

		// Token: 0x040006EC RID: 1772
		[SerializeField]
		private bool actionTargetPlayer;
	}
}
