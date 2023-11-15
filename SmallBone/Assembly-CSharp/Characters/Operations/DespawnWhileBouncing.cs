using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD9 RID: 3545
	public class DespawnWhileBouncing : CharacterOperation
	{
		// Token: 0x06004719 RID: 18201 RVA: 0x000CE6D0 File Offset: 0x000CC8D0
		public override void Run(Character owner)
		{
			this._bouncyProjectile.speedMultiplier *= 1f - this._speedReductionRate;
			if (this._bouncyProjectile.speedMultiplier < this._minimumSpeedMultiplier)
			{
				this._bouncyProjectile.Despawn();
			}
		}

		// Token: 0x040035FB RID: 13819
		[SerializeField]
		private BouncyProjectile _bouncyProjectile;

		// Token: 0x040035FC RID: 13820
		[SerializeField]
		[Range(0f, 1f)]
		private float _speedReductionRate = 0.3f;

		// Token: 0x040035FD RID: 13821
		[SerializeField]
		private float _minimumSpeedMultiplier = 0.2f;
	}
}
