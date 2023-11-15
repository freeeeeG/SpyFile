using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018A RID: 394
	public class OnBurnTrigger : Trigger
	{
		// Token: 0x0600098E RID: 2446 RVA: 0x00026B5D File Offset: 0x00024D5D
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00026B76 File Offset: 0x00024D76
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00026B90 File Offset: 0x00024D90
		private void OnInflictBurn(object sender, object args)
		{
			GameObject target = args as GameObject;
			if (Random.Range(0f, 1f) < this.chanceToTrigger)
			{
				base.RaiseTrigger(target);
			}
		}

		// Token: 0x040006E8 RID: 1768
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToTrigger = 1f;
	}
}
