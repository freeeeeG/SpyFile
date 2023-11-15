using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200076D RID: 1901
	public class AttachAbility : CharacterHitOperation
	{
		// Token: 0x0600274D RID: 10061 RVA: 0x00075F17 File Offset: 0x00074117
		private void Awake()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x00075F24 File Offset: 0x00074124
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			character.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0400216E RID: 8558
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;
	}
}
