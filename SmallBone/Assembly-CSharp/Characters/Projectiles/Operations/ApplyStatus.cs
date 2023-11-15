using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076C RID: 1900
	public sealed class ApplyStatus : CharacterHitOperation
	{
		// Token: 0x0600274B RID: 10059 RVA: 0x00075EE5 File Offset: 0x000740E5
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
		{
			if (MMMaths.PercentChance(this._chance))
			{
				projectile.owner.GiveStatus(target, this._status);
			}
		}

		// Token: 0x0400216C RID: 8556
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x0400216D RID: 8557
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;
	}
}
