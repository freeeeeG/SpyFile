using System;
using System.Runtime.CompilerServices;
using Characters.Projectiles.Movements.SubMovements;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BC RID: 1980
	public class LeapDuringDuration : Movement
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x00079FD4 File Offset: 0x000781D4
		public override void Initialize(IProjectile projectile, float direction)
		{
			if (this._finder.range != null)
			{
				this._finder.Initialize(projectile);
				Target target = this._finder.Find();
				if (target != null && target.character != null)
				{
					Vector2 vector = projectile.transform.position;
					RaycastHit2D hit = Physics2D.Raycast(target.character.transform.position, Vector2.down, float.PositiveInfinity, Layers.groundMask);
					Vector2 vector2;
					if (hit)
					{
						vector2 = hit.point;
					}
					else if (target.character.movement.controller.collisionState.lastStandingCollider != null)
					{
						Bounds bounds = target.character.movement.controller.collisionState.lastStandingCollider.bounds;
						vector2 = new Vector2(target.transform.position.x, bounds.center.y);
					}
					else
					{
						vector2 = target.transform.position;
					}
					this._directionVector = vector2 - vector;
					direction = Mathf.Atan2(this._directionVector.y, this._directionVector.x) * 57.29578f;
					this._distance = Vector2.Distance(vector, vector2);
				}
			}
			base.Initialize(projectile, direction);
			this._subMovement.Move(projectile);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x0007A148 File Offset: 0x00078348
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float item = (time > this._duration) ? 0f : (this._distance / this._duration);
			return new ValueTuple<Vector2, float>(this._directionVector.normalized, item);
		}

		// Token: 0x0400229F RID: 8863
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x040022A0 RID: 8864
		[SerializeField]
		private SubMovement _subMovement;

		// Token: 0x040022A1 RID: 8865
		[SerializeField]
		[Range(0.1f, 10f)]
		private float _duration = 1f;

		// Token: 0x040022A2 RID: 8866
		private Vector2 _directionVector;

		// Token: 0x040022A3 RID: 8867
		private float _distance;
	}
}
