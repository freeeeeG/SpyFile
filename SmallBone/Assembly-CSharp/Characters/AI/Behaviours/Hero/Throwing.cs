using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C0 RID: 5056
	public class Throwing : Behaviour
	{
		// Token: 0x060063B1 RID: 25521 RVA: 0x00121B93 File Offset: 0x0011FD93
		public override IEnumerator CRun(AIController controller)
		{
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005063 RID: 20579
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
