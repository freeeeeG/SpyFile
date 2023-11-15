using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001480 RID: 5248
	public class SetFlyDirectionFromTarget : Action
	{
		// Token: 0x0600664E RID: 26190 RVA: 0x00127E60 File Offset: 0x00126060
		public override TaskStatus OnUpdate()
		{
			if (this._owner == null || this._target == null)
			{
				return TaskStatus.Failure;
			}
			Character value = this._owner.Value;
			Vector3 vector = this._target.Value.collider.bounds.center - value.collider.bounds.center;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			switch (this._rotateMethod)
			{
			case SetFlyDirectionFromTarget.RotateMethod.Constant:
				this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * 100f * Time.deltaTime);
				break;
			case SetFlyDirectionFromTarget.RotateMethod.Lerp:
				this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * Time.deltaTime);
				break;
			case SetFlyDirectionFromTarget.RotateMethod.Slerp:
				this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * Time.deltaTime);
				break;
			}
			Vector2 vector2 = this._rotation * Vector2.right;
			this._moveDirection.SetValue(vector2);
			return TaskStatus.Success;
		}

		// Token: 0x0400525B RID: 21083
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x0400525C RID: 21084
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400525D RID: 21085
		[SerializeField]
		private SharedVector2 _moveDirection;

		// Token: 0x0400525E RID: 21086
		[SerializeField]
		private SetFlyDirectionFromTarget.RotateMethod _rotateMethod;

		// Token: 0x0400525F RID: 21087
		[SerializeField]
		private float _rotateSpeed = 2f;

		// Token: 0x04005260 RID: 21088
		private Quaternion _rotation;

		// Token: 0x02001481 RID: 5249
		public enum RotateMethod
		{
			// Token: 0x04005262 RID: 21090
			Constant,
			// Token: 0x04005263 RID: 21091
			Lerp,
			// Token: 0x04005264 RID: 21092
			Slerp
		}
	}
}
