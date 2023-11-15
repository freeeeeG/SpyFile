using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001342 RID: 4930
	public sealed class Consecration : Behaviour
	{
		// Token: 0x0600613E RID: 24894 RVA: 0x0011CBE7 File Offset: 0x0011ADE7
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Done;
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E68 RID: 20072
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;
	}
}
