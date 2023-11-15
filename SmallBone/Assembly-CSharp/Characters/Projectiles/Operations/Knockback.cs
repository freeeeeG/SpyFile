using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000787 RID: 1927
	public class Knockback : CharacterHitOperation
	{
		// Token: 0x06002791 RID: 10129 RVA: 0x00076E4D File Offset: 0x0007504D
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			character.movement.push.ApplyKnockback(projectile, this._pushInfo);
		}

		// Token: 0x040021B4 RID: 8628
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);
	}
}
