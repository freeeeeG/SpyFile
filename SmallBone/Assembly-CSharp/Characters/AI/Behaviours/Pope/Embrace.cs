using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001351 RID: 4945
	public sealed class Embrace : Behaviour
	{
		// Token: 0x0600617B RID: 24955 RVA: 0x0011D1F8 File Offset: 0x0011B3F8
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			yield return this._pull.CRun(controller);
			yield return this._attack.CRun(controller);
			yield return this._end.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E9A RID: 20122
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _pull;

		// Token: 0x04004E9B RID: 20123
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004E9C RID: 20124
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _end;

		// Token: 0x04004E9D RID: 20125
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		private MoveHandler _moveHandler;
	}
}
