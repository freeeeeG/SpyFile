using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000788 RID: 1928
	public class KnockbackTo : CharacterHitOperation
	{
		// Token: 0x06002793 RID: 10131 RVA: 0x00076E7C File Offset: 0x0007507C
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
		{
			Vector2 a;
			if (this._targetPlace != null)
			{
				a = MMMaths.RandomPointWithinBounds(this._targetPlace.bounds);
			}
			else
			{
				a = this._targetPoint.position;
			}
			Vector2 force = a - target.transform.position;
			target.movement.push.ApplyKnockback(projectile.owner, force, this._curve, this._ignoreOtherForce, this._expireOnGround);
		}

		// Token: 0x040021B5 RID: 8629
		[Header("Destination")]
		[SerializeField]
		private Collider2D _targetPlace;

		// Token: 0x040021B6 RID: 8630
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x040021B7 RID: 8631
		[Header("Force")]
		[SerializeField]
		private Curve _curve;

		// Token: 0x040021B8 RID: 8632
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x040021B9 RID: 8633
		[SerializeField]
		private bool _expireOnGround;
	}
}
