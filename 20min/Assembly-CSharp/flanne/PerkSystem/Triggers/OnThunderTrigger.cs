using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000198 RID: 408
	public class OnThunderTrigger : Trigger
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x00027113 File Offset: 0x00025313
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002712C File Offset: 0x0002532C
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00027148 File Offset: 0x00025348
		private void OnThunderHit(object sender, object args)
		{
			this._thunderCounter++;
			if (this._thunderCounter >= this.thunderHitsToTrigger)
			{
				this._thunderCounter = 0;
				if (this.actionTargetPlayer)
				{
					base.RaiseTrigger(PlayerController.Instance.gameObject);
					return;
				}
				GameObject target = args as GameObject;
				base.RaiseTrigger(target);
			}
		}

		// Token: 0x040006F4 RID: 1780
		[SerializeField]
		private int thunderHitsToTrigger;

		// Token: 0x040006F5 RID: 1781
		[NonSerialized]
		private int _thunderCounter;

		// Token: 0x040006F6 RID: 1782
		[SerializeField]
		private bool actionTargetPlayer;
	}
}
