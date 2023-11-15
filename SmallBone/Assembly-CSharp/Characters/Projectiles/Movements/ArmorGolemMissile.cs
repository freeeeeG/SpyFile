using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007B7 RID: 1975
	public sealed class ArmorGolemMissile : Movement
	{
		// Token: 0x06002839 RID: 10297 RVA: 0x000798D4 File Offset: 0x00077AD4
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._finder.Initialize(projectile);
			this._target = null;
			this._rotationCache = Quaternion.Euler(0f, 0f, direction);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x00079908 File Offset: 0x00077B08
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (time < this._wait)
			{
				this.UpdateTarget();
			}
			this.UpdateLookDirection(deltaTime, (time < this._delay) ? this._appearRotateSpeed : this._targetingRotateSpeed);
			if (time >= this._delay || this._startRotationOnAppear)
			{
				this.UpdateMoveDirection();
			}
			float num = (time < this._delay) ? this.GetSpeed(this._onDelay, time) : this.GetSpeed(this._onTarget, time - this._delay);
			if (time >= this._delay && time < this._wait)
			{
				num *= 1f - this._waitCurve.Evaluate(time / this._wait);
			}
			return new ValueTuple<Vector2, float>(base.directionVector, num * base.projectile.speedMultiplier);
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000799CC File Offset: 0x00077BCC
		private float GetSpeed(ArmorGolemMissile.SpeedInfo info, float time)
		{
			return info.startSpeed + (info.targetSpeed - info.startSpeed) * info.curve.Evaluate(time / info.easingTime);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000799F8 File Offset: 0x00077BF8
		private void UpdateTarget()
		{
			if (this._finder.range != null)
			{
				this._target = this._finder.Find();
				if (this._target != null && this._target.character != null)
				{
					RaycastHit2D hit = Physics2D.Raycast(this._target.character.transform.position, Vector2.down, float.PositiveInfinity, Layers.groundMask);
					if (hit)
					{
						this._targetGroundPosition = hit.point;
					}
					else if (this._target.character.movement.controller.collisionState.lastStandingCollider != null)
					{
						Bounds bounds = this._target.character.movement.controller.collisionState.lastStandingCollider.bounds;
						this._targetGroundPosition = new Vector2(this._target.transform.position.x, bounds.center.y);
					}
					else
					{
						this._targetGroundPosition = this._target.transform.position;
					}
					this._targetGroundPosition = new Vector3(this._targetGroundPosition.x + this._targetOffset.x, this._targetGroundPosition.y + this._targetOffset.y, 0f);
				}
			}
			Vector3 vector = this._targetGroundPosition - base.transform.position;
			this._lookTargetAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00079BAD File Offset: 0x00077DAD
		private void UpdateMoveDirection()
		{
			base.direction = this._rotationCache.eulerAngles.z;
			base.directionVector = this._rotationCache * Vector3.right;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00079BE0 File Offset: 0x00077DE0
		private void UpdateLookDirection(float deltaTime, float rotateSpeed)
		{
			switch (this._rotateMethod)
			{
			case ArmorGolemMissile.RotateMethod.Constant:
				this._rotationCache = Quaternion.RotateTowards(this._rotationCache, Quaternion.AngleAxis(this._lookTargetAngle, Vector3.forward), rotateSpeed * 100f * deltaTime);
				break;
			case ArmorGolemMissile.RotateMethod.Lerp:
				this._rotationCache = Quaternion.Lerp(this._rotationCache, Quaternion.AngleAxis(this._lookTargetAngle, Vector3.forward), rotateSpeed * deltaTime);
				break;
			case ArmorGolemMissile.RotateMethod.Slerp:
				this._rotationCache = Quaternion.Slerp(this._rotationCache, Quaternion.AngleAxis(this._lookTargetAngle, Vector3.forward), rotateSpeed * deltaTime);
				break;
			}
			this._missileTransform.rotation = this._rotationCache;
		}

		// Token: 0x04002278 RID: 8824
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x04002279 RID: 8825
		[SerializeField]
		private float _delay;

		// Token: 0x0400227A RID: 8826
		[SerializeField]
		private Vector2 _targetOffset;

		// Token: 0x0400227B RID: 8827
		[Header("Wait For Targeting")]
		[SerializeField]
		private AnimationCurve _waitCurve;

		// Token: 0x0400227C RID: 8828
		[SerializeField]
		private float _wait;

		// Token: 0x0400227D RID: 8829
		[Header("Targeting Roatation")]
		[SerializeField]
		private bool _startRotationOnAppear;

		// Token: 0x0400227E RID: 8830
		[SerializeField]
		private ArmorGolemMissile.RotateMethod _rotateMethod;

		// Token: 0x0400227F RID: 8831
		[SerializeField]
		private float _appearRotateSpeed = 300f;

		// Token: 0x04002280 RID: 8832
		[SerializeField]
		private float _targetingRotateSpeed = 300f;

		// Token: 0x04002281 RID: 8833
		[Header("Speed")]
		[SerializeField]
		private ArmorGolemMissile.SpeedInfo _onDelay;

		// Token: 0x04002282 RID: 8834
		[SerializeField]
		private ArmorGolemMissile.SpeedInfo _onTarget;

		// Token: 0x04002283 RID: 8835
		[SerializeField]
		private Transform _missileTransform;

		// Token: 0x04002284 RID: 8836
		private float _lookTargetAngle;

		// Token: 0x04002285 RID: 8837
		private Quaternion _rotationCache;

		// Token: 0x04002286 RID: 8838
		private Target _target;

		// Token: 0x04002287 RID: 8839
		private Vector3 _targetGroundPosition;

		// Token: 0x020007B8 RID: 1976
		[Serializable]
		private class SpeedInfo
		{
			// Token: 0x04002288 RID: 8840
			[SerializeField]
			internal float startSpeed = 1f;

			// Token: 0x04002289 RID: 8841
			[SerializeField]
			internal float targetSpeed;

			// Token: 0x0400228A RID: 8842
			[SerializeField]
			internal AnimationCurve curve;

			// Token: 0x0400228B RID: 8843
			[SerializeField]
			internal float easingTime;
		}

		// Token: 0x020007B9 RID: 1977
		public enum RotateMethod
		{
			// Token: 0x0400228D RID: 8845
			Constant,
			// Token: 0x0400228E RID: 8846
			Lerp,
			// Token: 0x0400228F RID: 8847
			Slerp
		}
	}
}
