using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076F RID: 1903
	public sealed class AttachCurseOfLight : CharacterHitOperation
	{
		// Token: 0x06002753 RID: 10067 RVA: 0x00075F90 File Offset: 0x00074190
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
		{
			if (target.playerComponents == null)
			{
				return;
			}
			target.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}
	}
}
