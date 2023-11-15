using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007C9 RID: 1993
	public class Missile : Movement
	{
		// Token: 0x0600286E RID: 10350 RVA: 0x0007AF1F File Offset: 0x0007911F
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._finder.Initialize(projectile);
			this._target = null;
			this._speed = Vector2.zero;
			this._rotation = Quaternion.Euler(0f, 0f, direction);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x0007AF60 File Offset: 0x00079160
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
			this._speed += this._acceleration * base.directionVector * deltaTime;
			Vector2 normalized = this._speed.normalized;
			float magnitude = this._speed.magnitude;
			if (magnitude > this._maxSpeed)
			{
				this._speed = this._maxSpeed * normalized;
			}
			return new ValueTuple<Vector2, float>(normalized, magnitude * base.projectile.speedMultiplier);
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x0007AFF4 File Offset: 0x000791F4
		private void UpdateTarget()
		{
			if ((this._target != null && (this._target.character == null || !this._target.character.health.dead)) || this._finder.range == null)
			{
				return;
			}
			this._target = this._finder.Find();
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x0007B060 File Offset: 0x00079260
		private void UpdateDirection(float deltaTime)
		{
			if (this._target == null)
			{
				return;
			}
			Vector3 vector = this._target.collider.bounds.center - base.transform.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			switch (this._rotateMethod)
			{
			case Missile.RotateMethod.Constant:
				this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * 100f * deltaTime);
				break;
			case Missile.RotateMethod.Lerp:
				this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * deltaTime);
				break;
			case Missile.RotateMethod.Slerp:
				this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._rotateSpeed * deltaTime);
				break;
			}
			base.direction = this._rotation.eulerAngles.z;
			base.directionVector = this._rotation * Vector3.right;
		}

		// Token: 0x040022E3 RID: 8931
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x040022E4 RID: 8932
		[SerializeField]
		private float _delay;

		// Token: 0x040022E5 RID: 8933
		[Header("Roatation")]
		[SerializeField]
		private Missile.RotateMethod _rotateMethod;

		// Token: 0x040022E6 RID: 8934
		[SerializeField]
		private float _rotateSpeed = 300f;

		// Token: 0x040022E7 RID: 8935
		[Header("Speed")]
		[SerializeField]
		private float _initialSpeed = 1f;

		// Token: 0x040022E8 RID: 8936
		[SerializeField]
		private float _acceleration = 2f;

		// Token: 0x040022E9 RID: 8937
		[SerializeField]
		private float _maxSpeed = 3f;

		// Token: 0x040022EA RID: 8938
		private Target _target;

		// Token: 0x040022EB RID: 8939
		private Vector2 _speed;

		// Token: 0x040022EC RID: 8940
		private Quaternion _rotation;

		// Token: 0x020007CA RID: 1994
		public enum RotateMethod
		{
			// Token: 0x040022EE RID: 8942
			Constant,
			// Token: 0x040022EF RID: 8943
			Lerp,
			// Token: 0x040022F0 RID: 8944
			Slerp
		}
	}
}
