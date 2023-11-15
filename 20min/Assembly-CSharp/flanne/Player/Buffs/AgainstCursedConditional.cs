using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000166 RID: 358
	public class AgainstCursedConditional : IBuffConditional
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x00025DE2 File Offset: 0x00023FE2
		public bool ConditionMet(GameObject target)
		{
			return CurseSystem.Instance.IsCursed(target);
		}
	}
}
