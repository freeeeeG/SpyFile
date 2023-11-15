using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001320 RID: 4896
	public class MoveToTargetWithFly : Move
	{
		// Token: 0x060060B2 RID: 24754 RVA: 0x0011B5B2 File Offset: 0x001197B2
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				if (controller.FindClosestPlayerBody(controller.stopTrigger))
				{
					base.result = Behaviour.Result.Fail;
					break;
				}
				if (controller.target == null)
				{
					base.result = Behaviour.Result.Fail;
					break;
				}
				yield return null;
				Vector3 vector = target.collider.bounds.center - character.collider.bounds.center;
				if (vector.magnitude < 0.1f || base.LookAround(controller))
				{
					yield return this.idle.CRun(controller);
					base.result = Behaviour.Result.Success;
					break;
				}
				float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				switch (this._rotateMethod)
				{
				case MoveToTargetWithFly.RotateMethod.Constant:
					this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * 100f * Time.deltaTime);
					break;
				case MoveToTargetWithFly.RotateMethod.Lerp:
					this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * Time.deltaTime);
					break;
				case MoveToTargetWithFly.RotateMethod.Slerp:
					this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * Time.deltaTime);
					break;
				}
				Vector3 eulerAngles = this._rotation.eulerAngles;
				Vector3 v = this._rotation * Vector2.right;
				controller.character.movement.move = v;
			}
			yield break;
		}

		// Token: 0x04004DE9 RID: 19945
		[SerializeField]
		private MoveToTargetWithFly.RotateMethod _rotateMethod;

		// Token: 0x04004DEA RID: 19946
		[SerializeField]
		private float _rotateSpeed = 2f;

		// Token: 0x04004DEB RID: 19947
		private Quaternion _rotation;

		// Token: 0x02001321 RID: 4897
		public enum RotateMethod
		{
			// Token: 0x04004DED RID: 19949
			Constant,
			// Token: 0x04004DEE RID: 19950
			Lerp,
			// Token: 0x04004DEF RID: 19951
			Slerp
		}
	}
}
