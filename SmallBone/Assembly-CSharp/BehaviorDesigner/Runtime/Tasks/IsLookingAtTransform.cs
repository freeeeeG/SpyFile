using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B7 RID: 5303
	public sealed class IsLookingAtTransform : Conditional
	{
		// Token: 0x06006733 RID: 26419 RVA: 0x0012AD28 File Offset: 0x00128F28
		public override TaskStatus OnUpdate()
		{
			this._ownerValue = this._owner.Value;
			this._transformValue = this._transform.Value;
			this._direction = (this._transformValue.position - this._ownerValue.transform.position).normalized;
			if ((this._direction.x > 0f && this._ownerValue.lookingDirection == Character.LookingDirection.Left) || (this._direction.x < 0f && this._ownerValue.lookingDirection == Character.LookingDirection.Right))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005344 RID: 21316
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005345 RID: 21317
		[SerializeField]
		private SharedTransform _transform;

		// Token: 0x04005346 RID: 21318
		private Character _ownerValue;

		// Token: 0x04005347 RID: 21319
		private Transform _transformValue;

		// Token: 0x04005348 RID: 21320
		private Vector2 _direction;
	}
}
