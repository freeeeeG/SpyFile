using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x0200139B RID: 5019
	public class LightSwordFieldAction : Behaviour
	{
		// Token: 0x0600630B RID: 25355 RVA: 0x0012068A File Offset: 0x0011E88A
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._move.CRun(controller);
			yield return this._attack.CRun(controller);
			yield break;
		}

		// Token: 0x04004FE0 RID: 20448
		[SerializeField]
		private LightSwordFieldMove _move;

		// Token: 0x04004FE1 RID: 20449
		[SerializeField]
		private Behaviour _attack;
	}
}
