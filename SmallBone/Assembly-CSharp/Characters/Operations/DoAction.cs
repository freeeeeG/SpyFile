using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E17 RID: 3607
	public class DoAction : Operation
	{
		// Token: 0x060047FF RID: 18431 RVA: 0x000D1B7F File Offset: 0x000CFD7F
		public override void Run()
		{
			if (!this._action.TryStart() && this._repeatUntilSuccess)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.CRun());
			}
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x000D1BA9 File Offset: 0x000CFDA9
		private IEnumerator CRun()
		{
			do
			{
				yield return null;
			}
			while (!this._action.TryStart());
			yield break;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x000D1BB8 File Offset: 0x000CFDB8
		public override void Stop()
		{
			base.Stop();
			base.StopAllCoroutines();
		}

		// Token: 0x04003722 RID: 14114
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04003723 RID: 14115
		[Tooltip("Constraints, Cooldown 등에 의해 action 실행에 실패할 경우 성공할 때까지 혹은 stop될 때까지 반복합니다. 예를 들어 IdleConstraint와 함께 사용하면 Idle 상태가 되자마자 실행됩니다.")]
		[SerializeField]
		private bool _repeatUntilSuccess;
	}
}
