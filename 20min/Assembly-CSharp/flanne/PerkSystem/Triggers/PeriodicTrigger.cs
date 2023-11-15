using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200019A RID: 410
	public class PeriodicTrigger : Trigger
	{
		// Token: 0x060009CE RID: 2510 RVA: 0x000271FA File Offset: 0x000253FA
		public override void OnEquip(PlayerController player)
		{
			player.StartCoroutine(this.PeriodicTriggerCR(player.gameObject));
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002720F File Offset: 0x0002540F
		private IEnumerator PeriodicTriggerCR(GameObject target)
		{
			for (;;)
			{
				yield return new WaitForSeconds(this.triggerIntervalSeconds);
				base.RaiseTrigger(target);
			}
			yield break;
		}

		// Token: 0x040006F8 RID: 1784
		[SerializeField]
		private float triggerIntervalSeconds;
	}
}
