using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007DA RID: 2010
	public class Trajectory : Movement
	{
		// Token: 0x060028AA RID: 10410 RVA: 0x0007B8F8 File Offset: 0x00079AF8
		public override void Initialize(IProjectile projectile, float direction)
		{
			if (this._finder.range != null)
			{
				this._finder.Initialize(projectile);
				Target target = this._finder.Find();
				if (target != null)
				{
					Vector3 vector = target.collider.bounds.center - base.transform.position;
					direction = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				}
			}
			direction += this._extraAngle.value;
			base.Initialize(projectile, direction);
			this._ySpeed = 0f;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0007B998 File Offset: 0x00079B98
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float num;
			if (time < this._easingTime)
			{
				num = this._startSpeed + (this._targetSpeed - this._startSpeed) * this._curve.Evaluate(time / this._easingTime);
			}
			else
			{
				num = this._targetSpeed;
			}
			num *= base.projectile.speedMultiplier;
			Vector2 vector = num * base.directionVector;
			this._ySpeed -= this._gravity * deltaTime;
			vector.y += this._ySpeed;
			return new ValueTuple<Vector2, float>(vector.normalized, vector.magnitude);
		}

		// Token: 0x0400231C RID: 8988
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x0400231D RID: 8989
		[SerializeField]
		private float _startSpeed;

		// Token: 0x0400231E RID: 8990
		[SerializeField]
		private float _targetSpeed;

		// Token: 0x0400231F RID: 8991
		[SerializeField]
		private AnimationCurve _curve;

		// Token: 0x04002320 RID: 8992
		[SerializeField]
		private float _easingTime;

		// Token: 0x04002321 RID: 8993
		[SerializeField]
		private float _gravity;

		// Token: 0x04002322 RID: 8994
		[SerializeField]
		[Tooltip("이 값만큼 초기 발사각에 더해집니다. 주로 투사체에 노이즈를 추가하여 산발되게하는 식으로 사용할 수 있습니다.")]
		private CustomFloat _extraAngle;

		// Token: 0x04002323 RID: 8995
		private float _ySpeed;
	}
}
