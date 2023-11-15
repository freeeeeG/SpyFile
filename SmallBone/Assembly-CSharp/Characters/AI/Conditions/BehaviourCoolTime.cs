using System;
using Characters.AI.Behaviours.Hero;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C1 RID: 4545
	public class BehaviourCoolTime : Condition
	{
		// Token: 0x06005975 RID: 22901 RVA: 0x0010A458 File Offset: 0x00108658
		protected override bool Check(AIController controller)
		{
			return this._behaviour.canUse;
		}

		// Token: 0x04004840 RID: 18496
		[SerializeField]
		private BehaviourTemplate _behaviour;
	}
}
