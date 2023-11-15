using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013B6 RID: 5046
	public class DashSlash : SequentialCombo
	{
		// Token: 0x06006382 RID: 25474 RVA: 0x001215CE File Offset: 0x0011F7CE
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
			yield break;
		}

		// Token: 0x0400503E RID: 20542
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _readyAction;

		// Token: 0x0400503F RID: 20543
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
