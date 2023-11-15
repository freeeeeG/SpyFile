using System;
using flanne.Player;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A5 RID: 421
	public class ApplyPlayerBuffAction : Action
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0002745E File Offset: 0x0002565E
		public override void Activate(GameObject target)
		{
			PlayerBuffs componentInChildren = target.GetComponentInChildren<PlayerBuffs>();
			if (componentInChildren == null)
			{
				Debug.LogWarning("No PlayerBuff component found on target.");
			}
			componentInChildren.Add(this.buff);
		}

		// Token: 0x04000702 RID: 1794
		[SerializeReference]
		private Buff buff;
	}
}
