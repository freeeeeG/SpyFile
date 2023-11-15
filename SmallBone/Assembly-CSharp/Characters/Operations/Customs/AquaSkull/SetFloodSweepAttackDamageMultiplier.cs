using System;
using Characters.Operations.Attack;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull
{
	// Token: 0x0200102D RID: 4141
	public class SetFloodSweepAttackDamageMultiplier : Operation
	{
		// Token: 0x06004FC3 RID: 20419 RVA: 0x000F0B44 File Offset: 0x000EED44
		public override void Run()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._damageMultiplierByCount.Length - 1);
			this._sweepAttack.hitInfo.damageMultiplier = this._damageMultiplierByCount[num2];
		}

		// Token: 0x0400403E RID: 16446
		[SerializeField]
		private SweepAttack _sweepAttack;

		// Token: 0x0400403F RID: 16447
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x04004040 RID: 16448
		[SerializeField]
		private float[] _damageMultiplierByCount;
	}
}
