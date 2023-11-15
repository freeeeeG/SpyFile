using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012A6 RID: 4774
	public class BeginBehaviour : Decorator
	{
		// Token: 0x06005E9B RID: 24219 RVA: 0x00115D5F File Offset: 0x00113F5F
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._behaviour.CRun(controller);
			yield return this._nextBehaviour.CRun(controller);
			yield break;
		}

		// Token: 0x04004C02 RID: 19458
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004C03 RID: 19459
		[SerializeField]
		private Behaviour _nextBehaviour;
	}
}
