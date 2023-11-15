using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000778 RID: 1912
	public class DropParts : HitOperation
	{
		// Token: 0x06002767 RID: 10087 RVA: 0x00076374 File Offset: 0x00074574
		private void OnDestroy()
		{
			this._particleEffectInfo = null;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x0007637D File Offset: 0x0007457D
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			this._particleEffectInfo.Emit(raycastHit.point, this._range.bounds, this._direction * this._power, this._interpolation);
		}

		// Token: 0x0400217D RID: 8573
		[SerializeField]
		private Collider2D _range;

		// Token: 0x0400217E RID: 8574
		[SerializeField]
		private ParticleEffectInfo _particleEffectInfo;

		// Token: 0x0400217F RID: 8575
		[SerializeField]
		private Vector2 _direction;

		// Token: 0x04002180 RID: 8576
		[SerializeField]
		private float _power = 3f;

		// Token: 0x04002181 RID: 8577
		[SerializeField]
		private bool _interpolation;
	}
}
