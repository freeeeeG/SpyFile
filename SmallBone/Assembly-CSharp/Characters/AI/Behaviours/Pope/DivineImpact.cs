using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200134D RID: 4941
	public sealed class DivineImpact : Behaviour
	{
		// Token: 0x0600616B RID: 24939 RVA: 0x0011D03D File Offset: 0x0011B23D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E8D RID: 20109
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004E8E RID: 20110
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		[SerializeField]
		private MoveHandler _moveHandler;
	}
}
