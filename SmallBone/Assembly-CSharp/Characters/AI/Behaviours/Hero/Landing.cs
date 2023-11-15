using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013BC RID: 5052
	public sealed class Landing : Behaviour
	{
		// Token: 0x060063A1 RID: 25505 RVA: 0x0012196E File Offset: 0x0011FB6E
		public override IEnumerator CRun(AIController controller)
		{
			this._jump.TryStart();
			while (this._jump.running)
			{
				yield return null;
			}
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			this._end.TryStart();
			while (this._end.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005056 RID: 20566
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _jump;

		// Token: 0x04005057 RID: 20567
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04005058 RID: 20568
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _end;
	}
}
