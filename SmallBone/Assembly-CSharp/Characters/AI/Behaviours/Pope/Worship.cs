using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001348 RID: 4936
	public sealed class Worship : Behaviour
	{
		// Token: 0x06006158 RID: 24920 RVA: 0x0011CE16 File Offset: 0x0011B016
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E7C RID: 20092
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;
	}
}
