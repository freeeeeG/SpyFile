using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012B2 RID: 4786
	public class EndBehaviour : Decorator
	{
		// Token: 0x06005ECC RID: 24268 RVA: 0x001161AE File Offset: 0x001143AE
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._beforeBehaviour.CRun(controller);
			yield return this._behaviour.CRun(controller);
			yield break;
		}

		// Token: 0x04004C27 RID: 19495
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004C28 RID: 19496
		[SerializeField]
		private Behaviour _beforeBehaviour;
	}
}
