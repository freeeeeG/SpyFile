using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076E RID: 1902
	public sealed class AttachAbilityToOwner : Operation
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x00075F45 File Offset: 0x00074145
		private void Awake()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x00075F54 File Offset: 0x00074154
		public override void Run(IProjectile projectile)
		{
			Character owner = projectile.owner;
			if (owner != null)
			{
				owner.ability.Add(this._abilityComponent.ability);
			}
		}

		// Token: 0x0400216F RID: 8559
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
