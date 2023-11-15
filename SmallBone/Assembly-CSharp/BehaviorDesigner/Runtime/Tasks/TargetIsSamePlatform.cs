using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BF RID: 5311
	[TaskDescription("행동 주체와 타겟이 같은 플랫폼인가?")]
	public class TargetIsSamePlatform : Conditional
	{
		// Token: 0x06006748 RID: 26440 RVA: 0x0012AFE8 File Offset: 0x001291E8
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x0012AFFC File Offset: 0x001291FC
		public override TaskStatus OnUpdate()
		{
			Character value = this._target.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return TaskStatus.Failure;
			}
			Collider2D lastStandingCollider2 = this._ownerValue.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider2 == null)
			{
				return TaskStatus.Failure;
			}
			if (lastStandingCollider != lastStandingCollider2)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0400535F RID: 21343
		[Tooltip("행동 주체")]
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005360 RID: 21344
		[Tooltip("타겟")]
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x04005361 RID: 21345
		private Character _ownerValue;
	}
}
