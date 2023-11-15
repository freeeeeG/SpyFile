using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Movements.SubMovements
{
	// Token: 0x020007E0 RID: 2016
	public class VerticalMove : SubMovement
	{
		// Token: 0x060028BD RID: 10429 RVA: 0x0007BED3 File Offset: 0x0007A0D3
		public override void Move(IProjectile projectile)
		{
			this._projectile = projectile;
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x0007BEE9 File Offset: 0x0007A0E9
		private void OnEnable()
		{
			base.transform.localPosition = Vector2.zero;
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x0007BF00 File Offset: 0x0007A100
		private void OnDisable()
		{
			base.transform.localPosition = Vector2.zero;
			base.StopAllCoroutines();
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0007BF1D File Offset: 0x0007A11D
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
				float y = Mathf.Lerp(startY, destinationY, this._curve.Evaluate(elpased / this._curve.duration));
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

		// Token: 0x04002335 RID: 9013
		[SerializeField]
		private float _height = 3f;

		// Token: 0x04002336 RID: 9014
		[SerializeField]
		private Curve _curve;

		// Token: 0x04002337 RID: 9015
		private IProjectile _projectile;
	}
}
