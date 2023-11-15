using System;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C2 RID: 4546
	public class BehaviourResult : Condition
	{
		// Token: 0x06005977 RID: 22903 RVA: 0x0010A46D File Offset: 0x0010866D
		protected override bool Check(AIController controller)
		{
			return this._behaviour.result == Characters.AI.Behaviours.Behaviour.Result.Success;
		}

		// Token: 0x04004841 RID: 18497
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _behaviour;
	}
}
