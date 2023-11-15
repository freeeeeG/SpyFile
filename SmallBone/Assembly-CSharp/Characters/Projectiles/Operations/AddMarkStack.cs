using System;
using Characters.Marks;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076B RID: 1899
	public class AddMarkStack : CharacterHitOperation
	{
		// Token: 0x06002749 RID: 10057 RVA: 0x00075E74 File Offset: 0x00074074
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
		{
			if (!MMMaths.PercentChance(this._chance) || target == null || target.health == null || target.health.dead)
			{
				return;
			}
			target.mark.AddStack(this._mark, this._count);
		}

		// Token: 0x04002169 RID: 8553
		[SerializeField]
		private MarkInfo _mark;

		// Token: 0x0400216A RID: 8554
		[SerializeField]
		[Range(1f, 100f)]
		private int _chance = 100;

		// Token: 0x0400216B RID: 8555
		[SerializeField]
		private float _count = 1f;
	}
}
