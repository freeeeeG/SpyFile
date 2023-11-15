using System;
using System.Runtime.CompilerServices;
using Characters.Projectiles.Movements.SubMovements;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BB RID: 1979
	public class Leap : Movement
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x00079DEC File Offset: 0x00077FEC
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
					vector2.x += this._offsetX.value;
					vector2.y += this._offsetY.value;
					this._directionVector = vector2 - vector;
					direction = Mathf.Atan2(this._directionVector.y, this._directionVector.x) * 57.29578f;
					this._distance = Vector2.Distance(vector, vector2);
				}
			}
			base.Initialize(projectile, direction);
			this._subMovement.Move(projectile);
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x00079F8C File Offset: 0x0007818C
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float item = (time > this._duration) ? 0f : this._distance;
			return new ValueTuple<Vector2, float>(this._directionVector.normalized, item);
		}

		// Token: 0x04002298 RID: 8856
		[SerializeField]
		private TargetFinder _finder;

		// Token: 0x04002299 RID: 8857
		[SerializeField]
		private SubMovement _subMovement;

		// Token: 0x0400229A RID: 8858
		[SerializeField]
		private float _duration = 1f;

		// Token: 0x0400229B RID: 8859
		private Vector2 _directionVector;

		// Token: 0x0400229C RID: 8860
		private float _distance;

		// Token: 0x0400229D RID: 8861
		[SerializeField]
		private CustomFloat _offsetX;

		// Token: 0x0400229E RID: 8862
		[SerializeField]
		private CustomFloat _offsetY;
	}
}
