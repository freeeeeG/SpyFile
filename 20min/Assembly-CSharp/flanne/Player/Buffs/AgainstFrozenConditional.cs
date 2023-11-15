using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000167 RID: 359
	public class AgainstFrozenConditional : IBuffConditional
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x00025DEF File Offset: 0x00023FEF
		public bool ConditionMet(GameObject target)
		{
			return FreezeSystem.SharedInstance.IsFrozen(target);
		}
	}
}
