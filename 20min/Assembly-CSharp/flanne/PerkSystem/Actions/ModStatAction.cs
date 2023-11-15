using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C4 RID: 452
	public class ModStatAction : Action
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x00027DB4 File Offset: 0x00025FB4
		public override void Activate(GameObject target)
		{
			StatsHolder componentInChildren = target.GetComponentInChildren<StatsHolder>();
			if (componentInChildren == null)
			{
				Debug.LogWarning("Cannot apply stat mods. No stats holder on target game object.");
				return;
			}
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					componentInChildren[statChange.type].AddFlatBonus(statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					componentInChildren[statChange.type].AddMultiplierBonus(statChange.value);
				}
				else if (statChange.value < 0f)
				{
					componentInChildren[statChange.type].AddMultiplierReduction(1f + statChange.value);
				}
			}
		}

		// Token: 0x0400072D RID: 1837
		[SerializeField]
		private StatChange[] statChanges;
	}
}
