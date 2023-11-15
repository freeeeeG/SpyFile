using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A9 RID: 425
	public class AutoKillAction : Action
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x000276CA File Offset: 0x000258CA
		public override void Activate(GameObject target)
		{
			if (!this.affectChampions && target.tag.Contains("Champion"))
			{
				return;
			}
			Health component = target.GetComponent<Health>();
			if (component == null)
			{
				return;
			}
			component.AutoKill(true);
		}

		// Token: 0x0400070A RID: 1802
		[SerializeField]
		private bool affectChampions;
	}
}
