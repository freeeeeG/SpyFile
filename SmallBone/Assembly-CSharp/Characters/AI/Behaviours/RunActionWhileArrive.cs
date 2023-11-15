using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012F8 RID: 4856
	public sealed class RunActionWhileArrive : Behaviour
	{
		// Token: 0x06006004 RID: 24580 RVA: 0x001190DA File Offset: 0x001172DA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this.CUpdate(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x001190F0 File Offset: 0x001172F0
		private IEnumerator CUpdate(AIController controller)
		{
			float elapsed = 0f;
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running && base.result == Behaviour.Result.Doing && elapsed <= this._maxTime && !this.TryMove(controller.character))
			{
				yield return null;
				elapsed += controller.character.chronometer.master.deltaTime;
			}
			yield break;
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x00119108 File Offset: 0x00117308
		private bool TryMove(Character character)
		{
			float num = this._dest.position.x - character.transform.position.x;
			if (Mathf.Abs(num) < this._epsilon)
			{
				return false;
			}
			int num2 = (num > 0f) ? 0 : 180;
			character.movement.Move((float)num2);
			return true;
		}

		// Token: 0x04004D40 RID: 19776
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004D41 RID: 19777
		[SerializeField]
		private Transform _dest;

		// Token: 0x04004D42 RID: 19778
		[SerializeField]
		private float _maxTime = 10f;

		// Token: 0x04004D43 RID: 19779
		[SerializeField]
		private float _epsilon = 0.5f;
	}
}
