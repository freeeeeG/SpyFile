using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000175 RID: 373
	public class ModSummonAttackCDByFireRateMod : Buff
	{
		// Token: 0x0600093F RID: 2367 RVA: 0x00026267 File Offset: 0x00024467
		public override void OnAttach()
		{
			this.player = PlayerController.Instance;
			this.AddObserver(new Action<object, object>(this.OnTweakSummonAttackCD), AttackingSummon.TweakAttackCDNotification);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002628B File Offset: 0x0002448B
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakSummonAttackCD), AttackingSummon.TweakAttackCDNotification);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000262A4 File Offset: 0x000244A4
		private void OnTweakSummonAttackCD(object sender, object args)
		{
			if ((sender as AttackingSummon).SummonTypeID != this.summonTypeID)
			{
				return;
			}
			float toMultiply = this.player.stats[StatType.FireRate].ModifyInverse(1f);
			(args as List<ValueModifier>).Add(new MultValueModifier(0, toMultiply));
		}

		// Token: 0x040006BD RID: 1725
		[SerializeField]
		private string summonTypeID;

		// Token: 0x040006BE RID: 1726
		private PlayerController player;
	}
}
