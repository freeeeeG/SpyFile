using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword
{
	// Token: 0x02000BF9 RID: 3065
	[Serializable]
	public class Skeleton_SwordPassiveTetanus : Ability
	{
		// Token: 0x06003EF5 RID: 16117 RVA: 0x000B6FFD File Offset: 0x000B51FD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Skeleton_SwordPassiveTetanus.Instance(owner, this);
		}

		// Token: 0x04003095 RID: 12437
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private Skeleton_SwordTetanusDamageComponent _damageComponent;

		// Token: 0x02000BFA RID: 3066
		public class Instance : AbilityInstance<Skeleton_SwordPassiveTetanus>
		{
			// Token: 0x06003EF7 RID: 16119 RVA: 0x000B7006 File Offset: 0x000B5206
			public Instance(Character owner, Skeleton_SwordPassiveTetanus ability) : base(owner, ability)
			{
			}

			// Token: 0x06003EF8 RID: 16120 RVA: 0x000B7010 File Offset: 0x000B5210
			protected override void OnAttach()
			{
				this.owner.status.onApplyBleed += this.OnApplyBleed;
				this.ability._damageComponent.baseAbility.attacker = this.owner;
			}

			// Token: 0x06003EF9 RID: 16121 RVA: 0x000B7049 File Offset: 0x000B5249
			protected override void OnDetach()
			{
				this.owner.status.onApplyBleed -= this.OnApplyBleed;
			}

			// Token: 0x06003EFA RID: 16122 RVA: 0x000B7067 File Offset: 0x000B5267
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
			}

			// Token: 0x06003EFB RID: 16123 RVA: 0x000B7070 File Offset: 0x000B5270
			private void OnApplyBleed(Character attacker, Character target)
			{
				if (target.health.dead)
				{
					return;
				}
				target.ability.Add(this.ability._damageComponent.ability);
			}
		}
	}
}
