using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007C7 RID: 1991
	public class Homing : Movement
	{
		// Token: 0x06002869 RID: 10345 RVA: 0x0007AC5C File Offset: 0x00078E5C
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._finder.Initialize(projectile);
			this._target = null;
			this._doNotFound = false;
			this._rotation = Quaternion.Euler(0f, 0f, direction);
			this._currentRotateSpeed = this._rotateSpeed;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x0007ACB0 File Offset: 0x00078EB0
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (time >= this._delay)
			{
				this.UpdateTarget();
				this.UpdateDirection(deltaTime);
			}
			if (this._timeToStopChasing > 0f && time > this._timeToStopChasing)
			{
				this._target = null;
				this._doNotFound = true;
			}
			float num = this._startSpeed + (this._targetSpeed - this._startSpeed) * this._curve.Evaluate(time / this._easingTime);
			return new ValueTuple<Vector2, float>(base.directionVector, num * base.projectile.speedMultiplier);
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x0007AD38 File Offset: 0x00078F38
		private void UpdateTarget()
		{
			if ((this._target != null && (this._target.character == null || !this._target.character.health.dead)) || this._finder.range == null)
			{
				return;
			}
			if (this._doNotFound)
			{
				return;
			}
			Target target = this._finder.Find();
			if (target != null && target.character != null)
			{
				this._target = target;
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x0007ADC4 File Offset: 0x00078FC4
		private void UpdateDirection(float deltaTime)
		{
			if (this._target == null)
			{
				return;
			}
			if (this._target.character == null)
			{
				return;
			}
			Vector3 vector = this._target.collider.bounds.center - base.transform.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._currentRotateSpeed += this._rotateSpeedAcc * deltaTime;
			switch (this._rotateMethod)
			{
			case Homing.RotateMethod.Constant:
				this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * 100f * deltaTime);
				break;
			case Homing.RotateMethod.Lerp:
				this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * deltaTime);
				break;
			case Homing.RotateMethod.Slerp:
				this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * deltaTime);
				break;
			}
			base.direction = this._rotation.eulerAngles.z;
			base.directionVector = this._rotation * Vector3.right;
		}

		// Token: 0x040022D1 RID: 8913
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x040022D2 RID: 8914
		[SerializeField]
		private float _delay;

		// Token: 0x040022D3 RID: 8915
		[SerializeField]
		private Homing.RotateMethod _rotateMethod;

		// Token: 0x040022D4 RID: 8916
		[SerializeField]
		private float _rotateSpeed = 2f;

		// Token: 0x040022D5 RID: 8917
		[SerializeField]
		private float _rotateSpeedAcc;

		// Token: 0x040022D6 RID: 8918
		[SerializeField]
		private float _startSpeed;

		// Token: 0x040022D7 RID: 8919
		[SerializeField]
		private float _targetSpeed;

		// Token: 0x040022D8 RID: 8920
		[SerializeField]
		private AnimationCurve _curve;

		// Token: 0x040022D9 RID: 8921
		[SerializeField]
		private float _easingTime;

		// Token: 0x040022DA RID: 8922
		private Target _target;

		// Token: 0x040022DB RID: 8923
		private Quaternion _rotation;

		// Token: 0x040022DC RID: 8924
		private float _currentRotateSpeed;

		// Token: 0x040022DD RID: 8925
		[SerializeField]
		private float _timeToStopChasing;

		// Token: 0x040022DE RID: 8926
		private bool _doNotFound;

		// Token: 0x020007C8 RID: 1992
		public enum RotateMethod
		{
			// Token: 0x040022E0 RID: 8928
			Constant,
			// Token: 0x040022E1 RID: 8929
			Lerp,
			// Token: 0x040022E2 RID: 8930
			Slerp
		}
	}
}
