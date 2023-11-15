using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Movements.SubMovements
{
	// Token: 0x020007E2 RID: 2018
	public class VerticalMove2 : SubMovement
	{
		// Token: 0x060028C8 RID: 10440 RVA: 0x0007C0F8 File Offset: 0x0007A2F8
		public override void Move(IProjectile projectile)
		{
			this._projectile = projectile;
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0007BEE9 File Offset: 0x0007A0E9
		private void OnEnable()
		{
			base.transform.localPosition = Vector2.zero;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x0007BF00 File Offset: 0x0007A100
		private void OnDisable()
		{
			base.transform.localPosition = Vector2.zero;
			base.StopAllCoroutines();
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x0007C10E File Offset: 0x0007A30E
		private IEnumerator CMove()
		{
			float elpased = 0f;
			float startY = 0f;
			int lookingDirection = (this._projectile.owner.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			float destinationY = this._height * (float)lookingDirection;
			while (elpased < this._curve.duration)
			{
				yield return null;
				elpased += Chronometer.global.deltaTime;
				float y = Mathf.Lerp(startY, destinationY, this._curve.Evaluate(elpased));
				Vector2 v = new Vector2(0f, y);
				Vector2 a = this._projectile.firedDirection;
				if (elpased >= this._curve.duration / 2f)
				{
					a += Vector2.down * (float)lookingDirection;
				}
				else
				{
					a += Vector2.up * (float)lookingDirection;
				}
				this._projectile.DetectCollision(base.transform.position, a.normalized, this._projectile.owner.chronometer.projectile.deltaTime);
				base.transform.localPosition = v;
			}
			yield break;
		}

		// Token: 0x0400233F RID: 9023
		[SerializeField]
		private float _height = 3f;

		// Token: 0x04002340 RID: 9024
		[SerializeField]
		private Curve _curve;

		// Token: 0x04002341 RID: 9025
		private IProjectile _projectile;
	}
}
