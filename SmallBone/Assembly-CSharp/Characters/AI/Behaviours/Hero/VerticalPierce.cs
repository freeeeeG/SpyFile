using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C2 RID: 5058
	public class VerticalPierce : SequentialCombo
	{
		// Token: 0x060063B9 RID: 25529 RVA: 0x00121C1A File Offset: 0x0011FE1A
		public override IEnumerator CRun(AIController controller)
		{
			this._readyAction.TryStart();
			while (this._readyAction.running)
			{
				yield return null;
			}
			this._jumpAction.TryStart();
			while (this._jumpAction.running)
			{
				yield return null;
			}
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005067 RID: 20583
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _readyAction;

		// Token: 0x04005068 RID: 20584
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _jumpAction;

		// Token: 0x04005069 RID: 20585
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _attackAction;
	}
}
