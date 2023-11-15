using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A3 RID: 419
	public class ApplyCurse : Action
	{
		// Token: 0x060009E3 RID: 2531 RVA: 0x00027444 File Offset: 0x00025644
		public override void Activate(GameObject target)
		{
			CurseSystem.Instance.Curse(target);
		}
	}
}
