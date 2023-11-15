using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FF1 RID: 4081
	public class ChaseTarget : CharacterOperation
	{
		// Token: 0x06004ED1 RID: 20177 RVA: 0x000ECB20 File Offset: 0x000EAD20
		public override void Run(Character owner)
		{
			Character target;
			if (this._ai != null)
			{
				target = this._ai.target;
			}
			else
			{
				target = this._communicator.GetVariable<SharedCharacter>(this._targetKey).Value;
			}
			this._cExpire = base.StartCoroutine(this.CRun(owner, target));
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x000ECB74 File Offset: 0x000EAD74
		private IEnumerator CRun(Character owner, Character target)
		{
			float elpased = 0f;
			while (elpased <= this._duration)
			{
				float num = owner.transform.position.x - target.transform.position.x;
				if (Mathf.Abs(num) > 1f)
				{
					owner.movement.MoveHorizontal((num > 0f) ? Vector2.left : Vector2.right);
				}
				if (this._lookTarget)
				{
					owner.DesireToLookAt(target.transform.position.x);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x000ECB91 File Offset: 0x000EAD91
		public override void Stop()
		{
			base.Stop();
			if (this._cExpire != null)
			{
				base.StopCoroutine(this._cExpire);
			}
		}

		// Token: 0x04003EEE RID: 16110
		[SerializeField]
		private AIController _ai;

		// Token: 0x04003EEF RID: 16111
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003EF0 RID: 16112
		[SerializeField]
		private string _targetKey = "Target";

		// Token: 0x04003EF1 RID: 16113
		[SerializeField]
		private float _duration;

		// Token: 0x04003EF2 RID: 16114
		[SerializeField]
		private bool _lookTarget;

		// Token: 0x04003EF3 RID: 16115
		private Coroutine _cExpire;

		// Token: 0x04003EF4 RID: 16116
		private const float epsilon = 1f;
	}
}
