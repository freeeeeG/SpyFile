using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007DB RID: 2011
	public class TrajectoryToPoint : Movement
	{
		// Token: 0x060028AD RID: 10413 RVA: 0x0007BA34 File Offset: 0x00079C34
		public void OnEnable()
		{
			if (!this._isInitialized)
			{
				return;
			}
			this.InitializedTrajectory();
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x0007BA48 File Offset: 0x00079C48
		public override void Initialize(IProjectile projectile, float direction)
		{
			if (this._finder.range != null)
			{
				this._finder.Initialize(projectile);
				this.InitializedTrajectory();
				Target target = this._finder.Find();
				Vector3 vector = target.collider.bounds.center - base.transform.position;
				this._targetDistance = Vector3.Distance(base.transform.position, target.collider.bounds.center);
				this._firingAngle = ((base.transform.position.x < target.transform.position.x) ? this._firingAngle : (this._firingAngle + 90f));
				Debug.Log(this._firingAngle);
				this._firingAngle = 135f;
				Debug.Log(this._firingAngle);
				float f = this._targetDistance / (Mathf.Sin(2f * this._firingAngle * 0.017453292f) / this._gravity);
				float num = Mathf.Sqrt(f) * Mathf.Cos(this._firingAngle * 0.017453292f);
				Mathf.Sqrt(f);
				Mathf.Sin(this._firingAngle * 0.017453292f);
				float num2 = this._targetDistance / num;
				if (target != null)
				{
					direction = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
					Debug.Log(direction);
				}
				this._elapseTime = 0f;
			}
			base.Initialize(projectile, direction);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0007BBE0 File Offset: 0x00079DE0
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			this._targetDistance = Vector3.Distance(base.transform.position, this._targetPosition);
			float f = this._targetDistance / (Mathf.Sin(2f * this._firingAngle * 0.017453292f) / this._gravity);
			float x = Mathf.Sqrt(f) * Mathf.Cos(this._firingAngle * 0.017453292f);
			float num = Mathf.Sqrt(f) * Mathf.Sin(this._firingAngle * 0.017453292f);
			this._elapseTime += Time.deltaTime;
			Vector2 vector;
			vector.x = x;
			vector.y = num - this._gravity * this._elapseTime;
			float magnitude = new Vector2(x, num - this._gravity * this._elapseTime).magnitude;
			return new ValueTuple<Vector2, float>(vector.normalized, vector.magnitude);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0007BCC0 File Offset: 0x00079EC0
		public bool InitializedTrajectory()
		{
			if (this._finder == null || this._finder.range == null)
			{
				return false;
			}
			this._target = this._finder.Find();
			this._elapseTime = 0f;
			Bounds bounds = this._finder.Find().collider.bounds;
			this._targetPosition = new Vector3((bounds.min.x + bounds.max.x) / 2f, bounds.min.y);
			this._firingAngle = ((this._target.transform.position.x > base.transform.position.x) ? this._firingAngle : (-this._firingAngle));
			return this._target != null;
		}

		// Token: 0x04002324 RID: 8996
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x04002325 RID: 8997
		[SerializeField]
		private float _easingTime;

		// Token: 0x04002326 RID: 8998
		[SerializeField]
		private float _gravity;

		// Token: 0x04002327 RID: 8999
		private float _elapseTime;

		// Token: 0x04002328 RID: 9000
		private float _targetDistance;

		// Token: 0x04002329 RID: 9001
		private Target _target;

		// Token: 0x0400232A RID: 9002
		private bool _isInitialized;

		// Token: 0x0400232B RID: 9003
		private float _firingAngle = 45f;

		// Token: 0x0400232C RID: 9004
		private Vector3 _targetPosition = Vector3.zero;
	}
}
