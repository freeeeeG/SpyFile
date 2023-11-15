using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200133D RID: 4925
	public sealed class Baptism : Behaviour
	{
		// Token: 0x06006127 RID: 24871 RVA: 0x0011C99B File Offset: 0x0011AB9B
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E57 RID: 20055
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;
	}
}
