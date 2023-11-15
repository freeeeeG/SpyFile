using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200134F RID: 4943
	public sealed class DivineLight : Behaviour
	{
		// Token: 0x06006173 RID: 24947 RVA: 0x0011D0FF File Offset: 0x0011B2FF
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				yield return this._hardmodeAttack.CRun(controller);
			}
			else
			{
				yield return this._attack.CRun(controller);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E93 RID: 20115
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004E94 RID: 20116
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _hardmodeAttack;

		// Token: 0x04004E95 RID: 20117
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		[SerializeField]
		private MoveHandler _moveHandler;
	}
}
