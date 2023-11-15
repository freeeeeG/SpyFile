using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000713 RID: 1811
	[Serializable]
	public class PushForce
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x0006E3FB File Offset: 0x0006C5FB
		// (set) Token: 0x060024B1 RID: 9393 RVA: 0x0006E403 File Offset: 0x0006C603
		public CustomFloat angle
		{
			get
			{
				return this._angle;
			}
			set
			{
				this._angle = value;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x0006E40C File Offset: 0x0006C60C
		// (set) Token: 0x060024B3 RID: 9395 RVA: 0x0006E414 File Offset: 0x0006C614
		public CustomFloat power
		{
			get
			{
				return this._power;
			}
			set
			{
				this._power = value;
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0006E41D File Offset: 0x0006C61D
		public PushForce()
		{
			this._method = PushForce.Method.OwnerDirection;
			this._angle = new CustomFloat(0f);
			this._power = new CustomFloat(0f);
			this._directionMethod = PushForce.DirectionComputingMethod.XOnly;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0006E454 File Offset: 0x0006C654
		private Vector2 EvaluateDirection(Vector2 originalDirection)
		{
			PushForce.DirectionComputingMethod directionMethod = this._directionMethod;
			if (directionMethod != PushForce.DirectionComputingMethod.XOnly)
			{
				if (directionMethod == PushForce.DirectionComputingMethod.YOnly)
				{
					originalDirection.x = 0f;
				}
			}
			else
			{
				originalDirection.y = 0f;
			}
			return originalDirection.normalized;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0006E494 File Offset: 0x0006C694
		private Vector2 EvaluateOutsideCenter(Transform from, ITarget to, Vector2 force)
		{
			Vector3 b;
			if (this._centerTransform != null)
			{
				b = this._centerTransform.position;
			}
			else if (this._centerCollider != null)
			{
				b = this._centerCollider.bounds.center;
			}
			else
			{
				b = from.transform.position;
			}
			Vector2 vector = this.EvaluateDirection(to.transform.position - b);
			return new Vector2(force.x * vector.x - force.y * vector.y, force.x * vector.y + force.y * vector.x);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0006E545 File Offset: 0x0006C745
		private Vector2 RotateByDirection(Vector2 direction, Vector2 value)
		{
			return new Vector2(value.x * direction.x - value.y * direction.y, value.x * direction.y + value.y * direction.x);
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0006E584 File Offset: 0x0006C784
		public Vector2 Evaluate(Transform from, ITarget to)
		{
			float value = this._angle.value;
			Vector2 vector = new Vector2(Mathf.Cos(value * 0.017453292f), Mathf.Sin(value * 0.017453292f)) * this._power.value;
			switch (this._method)
			{
			case PushForce.Method.OwnerDirection:
			{
				bool flag = false;
				if (from.lossyScale.x < 0f)
				{
					flag = !flag;
				}
				if (from.rotation.eulerAngles.z == 180f)
				{
					flag = !flag;
				}
				if (flag)
				{
					vector.x *= -1f;
				}
				break;
			}
			case PushForce.Method.OutsideCenter:
				vector = this.EvaluateOutsideCenter(from.transform, to, vector);
				break;
			case PushForce.Method.LastSmashedDirection:
				if (to.character != null)
				{
					vector = this.RotateByDirection(this.EvaluateDirection(to.character.movement.push.direction), vector);
				}
				break;
			}
			return vector;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0006E680 File Offset: 0x0006C880
		public Vector2 Evaluate(IProjectile from, ITarget to)
		{
			float value = this._angle.value;
			Vector2 vector = new Vector2(Mathf.Cos(value * 0.017453292f), Mathf.Sin(value * 0.017453292f)) * this._power.value;
			switch (this._method)
			{
			case PushForce.Method.OwnerDirection:
			{
				Vector2 direction = from.direction;
				vector = new Vector2(vector.x * direction.x - vector.y * direction.y, vector.x * direction.y + vector.y * direction.x);
				break;
			}
			case PushForce.Method.OutsideCenter:
				vector = this.EvaluateOutsideCenter(from.transform, to, vector);
				break;
			case PushForce.Method.LastSmashedDirection:
				if (to.character != null)
				{
					vector = this.RotateByDirection(this.EvaluateDirection(to.character.movement.push.direction), vector);
				}
				break;
			}
			return vector;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0006E774 File Offset: 0x0006C974
		public Vector2 Evaluate(Character from, ITarget to)
		{
			float value = this._angle.value;
			Vector2 vector = new Vector2(Mathf.Cos(value * 0.017453292f), Mathf.Sin(value * 0.017453292f)) * this._power.value;
			switch (this._method)
			{
			case PushForce.Method.OwnerDirection:
				if (from.lookingDirection != Character.LookingDirection.Right)
				{
					vector.x *= -1f;
				}
				break;
			case PushForce.Method.OutsideCenter:
				vector = this.EvaluateOutsideCenter(from.transform, to, vector);
				break;
			case PushForce.Method.LastSmashedDirection:
				if (to.character != null)
				{
					vector = this.RotateByDirection(this.EvaluateDirection(to.character.movement.push.direction), vector);
				}
				break;
			}
			return vector;
		}

		// Token: 0x04001F25 RID: 7973
		[SerializeField]
		private PushForce.Method _method;

		// Token: 0x04001F26 RID: 7974
		[SerializeField]
		private CustomFloat _angle;

		// Token: 0x04001F27 RID: 7975
		[SerializeField]
		private CustomFloat _power;

		// Token: 0x04001F28 RID: 7976
		[SerializeField]
		private PushForce.DirectionComputingMethod _directionMethod;

		// Token: 0x04001F29 RID: 7977
		[SerializeField]
		private Transform _centerTransform;

		// Token: 0x04001F2A RID: 7978
		[SerializeField]
		private Collider2D _centerCollider;

		// Token: 0x04001F2B RID: 7979
		private Vector2? _force;

		// Token: 0x02000714 RID: 1812
		public enum Method
		{
			// Token: 0x04001F2D RID: 7981
			OwnerDirection,
			// Token: 0x04001F2E RID: 7982
			OutsideCenter,
			// Token: 0x04001F2F RID: 7983
			Constant,
			// Token: 0x04001F30 RID: 7984
			LastSmashedDirection
		}

		// Token: 0x02000715 RID: 1813
		public enum DirectionComputingMethod
		{
			// Token: 0x04001F32 RID: 7986
			XOnly,
			// Token: 0x04001F33 RID: 7987
			YOnly,
			// Token: 0x04001F34 RID: 7988
			Both
		}
	}
}
