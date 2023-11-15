using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C4 RID: 5060
	public class VerticalSlash : Behaviour
	{
		// Token: 0x060063C1 RID: 25537 RVA: 0x00121D11 File Offset: 0x0011FF11
		public override IEnumerator CRun(AIController controller)
		{
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400506D RID: 20589
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
