using System;
using System.Collections;
using Characters.AI.Conditions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012AA RID: 4778
	public class Conditional : Decorator
	{
		// Token: 0x06005EAB RID: 24235 RVA: 0x00115EB7 File Offset: 0x001140B7
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._condition.IsSatisfied(controller))
			{
				yield return this._behaviour.CRun(controller);
				base.result = this._behaviour.result;
				yield break;
			}
			base.result = Behaviour.Result.Fail;
			yield break;
		}

		// Token: 0x04004C0E RID: 19470
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _condition;

		// Token: 0x04004C0F RID: 19471
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _behaviour;
	}
}
