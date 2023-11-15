using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200128C RID: 4748
	public class BackStep : Behaviour
	{
		// Token: 0x06005E2E RID: 24110 RVA: 0x00114F54 File Offset: 0x00113154
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._jump.TryStart();
			yield return this.CWaitJumpEnd();
			yield return this._idle.CRun(controller);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x00114F6A File Offset: 0x0011316A
		private IEnumerator CWaitJumpEnd()
		{
			while (this._jump.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00114F79 File Offset: 0x00113179
		public bool CanUse()
		{
			return this._jump.cooldown.canUse;
		}

		// Token: 0x04004BAF RID: 19375
		[SerializeField]
		private Characters.Actions.Action _jump;

		// Token: 0x04004BB0 RID: 19376
		[UnityEditor.Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;
	}
}
