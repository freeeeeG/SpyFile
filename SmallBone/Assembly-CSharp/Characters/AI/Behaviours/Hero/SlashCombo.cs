using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013BE RID: 5054
	public class SlashCombo : Behaviour
	{
		// Token: 0x060063A9 RID: 25513 RVA: 0x00121A65 File Offset: 0x0011FC65
		public override IEnumerator CRun(AIController controller)
		{
			this._readyAction.TryStart();
			while (this._readyAction.running)
			{
				yield return null;
			}
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			this._fall.TryStart();
			while (this._fall.running)
			{
				yield return null;
			}
			this._endAction.TryStart();
			while (this._endAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400505C RID: 20572
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _readyAction;

		// Token: 0x0400505D RID: 20573
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x0400505E RID: 20574
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _fall;

		// Token: 0x0400505F RID: 20575
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _endAction;
	}
}
