using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001363 RID: 4963
	public sealed class SummonEscort : Behaviour
	{
		// Token: 0x060061C9 RID: 25033 RVA: 0x0011DD1F File Offset: 0x0011BF1F
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004EE2 RID: 20194
		[SerializeField]
		private RunAction _attack;

		// Token: 0x04004EE3 RID: 20195
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		private MoveHandler _moveHandler;
	}
}
