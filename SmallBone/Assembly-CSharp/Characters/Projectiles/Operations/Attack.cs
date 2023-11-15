using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000770 RID: 1904
	public class Attack : CharacterHitOperation
	{
		// Token: 0x06002756 RID: 10070 RVA: 0x00075FAC File Offset: 0x000741AC
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			Damage damage = projectile.owner.stat.GetDamage((double)projectile.baseDamage, raycastHit.point, this._hitInfo);
			projectile.owner.Attack(character, ref damage);
			this._chrono.ApplyTo(character);
		}

		// Token: 0x04002170 RID: 8560
		[SerializeField]
		protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Ranged);

		// Token: 0x04002171 RID: 8561
		[SerializeField]
		protected ChronoInfo _chrono;
	}
}
