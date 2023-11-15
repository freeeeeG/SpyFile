using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A7 RID: 5031
	public class BackSlashA : Behaviour
	{
		// Token: 0x0600633A RID: 25402 RVA: 0x00120C30 File Offset: 0x0011EE30
		public override IEnumerator CRun(AIController controller)
		{
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005007 RID: 20487
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _attackAction;
	}
}
