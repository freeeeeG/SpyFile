using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200019B RID: 411
	public class SummonOnHitTrigger : Trigger
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x00027225 File Offset: 0x00025425
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnHit), Summon.SummonOnHitNotification);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002723E File Offset: 0x0002543E
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnHit), Summon.SummonOnHitNotification);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00027258 File Offset: 0x00025458
		private void OnHit(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			if ((sender as Summon).SummonTypeID != this.summonTypeID && this.summonTypeID != "")
			{
				return;
			}
			PlayerController instance = PlayerController.Instance;
			if (gameObject == instance)
			{
				return;
			}
			if (Random.Range(0f, 1f) < this.triggerChance)
			{
				base.RaiseTrigger(gameObject);
			}
		}

		// Token: 0x040006F9 RID: 1785
		[SerializeField]
		private string summonTypeID;

		// Token: 0x040006FA RID: 1786
		[Range(0f, 1f)]
		[SerializeField]
		private float triggerChance = 1f;
	}
}
