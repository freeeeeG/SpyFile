using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001367 RID: 4967
	public sealed class TentacleSummon : Behaviour
	{
		// Token: 0x060061D9 RID: 25049 RVA: 0x0011DE9F File Offset: 0x0011C09F
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._attack.CRun(controller);
			yield break;
		}

		// Token: 0x04004EEE RID: 20206
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;
	}
}
