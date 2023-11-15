using System;
using System.Collections;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200017B RID: 379
	public class TemporaryStatBuff : Buff
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x000264F0 File Offset: 0x000246F0
		public override void OnAttach()
		{
			PlayerController instance = PlayerController.Instance;
			StatsHolder stats = instance.stats;
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					stats[statChange.type].AddFlatBonus(statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					stats[statChange.type].AddMultiplierBonus(statChange.value);
				}
				else if (statChange.value < 0f)
				{
					stats[statChange.type].AddMultiplierReduction(1f + statChange.value);
				}
			}
			instance.StartCoroutine(this.WaitDurationCR());
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000265B8 File Offset: 0x000247B8
		public override void OnUnattach()
		{
			StatsHolder stats = PlayerController.Instance.stats;
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					stats[statChange.type].AddFlatBonus(-1 * statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					stats[statChange.type].AddMultiplierBonus(-1f * statChange.value);
				}
				else if (statChange.value < 0f)
				{
					stats[statChange.type].AddMultiplierReduction(1f - statChange.value);
				}
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002666C File Offset: 0x0002486C
		private IEnumerator WaitDurationCR()
		{
			yield return new WaitForSeconds(this.duration);
			this.owner.Remove(this);
			yield break;
		}

		// Token: 0x040006C9 RID: 1737
		[SerializeField]
		private StatChange[] statChanges;

		// Token: 0x040006CA RID: 1738
		[SerializeField]
		private float duration;
	}
}
