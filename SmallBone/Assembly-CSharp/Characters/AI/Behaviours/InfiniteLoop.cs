using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012B4 RID: 4788
	public class InfiniteLoop : Decorator
	{
		// Token: 0x06005ED4 RID: 24276 RVA: 0x00116261 File Offset: 0x00114461
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			for (;;)
			{
				yield return this._behaviour.CRun(controller);
			}
			yield break;
		}

		// Token: 0x04004C2D RID: 19501
		[SerializeField]
		[Behaviour.SubcomponentAttribute(true)]
		private Behaviour _behaviour;
	}
}
