using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013B4 RID: 5044
	public sealed class DashBreakAway : Behaviour
	{
		// Token: 0x0600637A RID: 25466 RVA: 0x00121548 File Offset: 0x0011F748
		public override IEnumerator CRun(AIController controller)
		{
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400503A RID: 20538
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
