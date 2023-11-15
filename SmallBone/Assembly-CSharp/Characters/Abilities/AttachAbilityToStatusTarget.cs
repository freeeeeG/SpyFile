using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009CB RID: 2507
	[Serializable]
	public sealed class AttachAbilityToStatusTarget : Ability
	{
		// Token: 0x0600355E RID: 13662 RVA: 0x0009E3C8 File Offset: 0x0009C5C8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AttachAbilityToStatusTarget.Instance(owner, this);
		}

		// Token: 0x04002B01 RID: 11009
		[SerializeField]
		[Tooltip("제거는 따로 안시켜줌")]
		private bool _onRelease;

		// Token: 0x04002B02 RID: 11010
		[SerializeField]
		internal CharacterStatus.Kind _kind;

		// Token: 0x04002B03 RID: 11011
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x020009CC RID: 2508
		public class Instance : AbilityInstance<AttachAbilityToStatusTarget>
		{
			// Token: 0x06003560 RID: 13664 RVA: 0x0009E3D1 File Offset: 0x0009C5D1
			public Instance(Character owner, AttachAbilityToStatusTarget ability) : base(owner, ability)
			{
				ability._abilityComponent.Initialize();
			}

			// Token: 0x06003561 RID: 13665 RVA: 0x0009E3E8 File Offset: 0x0009C5E8
			protected override void OnAttach()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun += this.AttachAbility;
						this.owner.status.onReleaseStun += this.DetachAbility;
						return;
					}
					this.owner.status.onReleaseStun += this.AttachAbility;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze += this.AttachAbility;
						this.owner.status.onReleaseFreeze += this.DetachAbility;
						return;
					}
					this.owner.status.onReleaseFreeze += this.AttachAbility;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn += this.AttachAbility;
						this.owner.status.onReleaseBurn += this.DetachAbility;
						return;
					}
					this.owner.status.onReleaseBurn += this.DetachAbility;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound += this.AttachAbility;
						this.owner.status.onApplyBleed += this.DetachAbility;
						return;
					}
					this.owner.status.onApplyBleed += this.DetachAbility;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison += this.AttachAbility;
						this.owner.status.onReleasePoison += this.DetachAbility;
						return;
					}
					this.owner.status.onReleasePoison += this.AttachAbility;
					return;
				default:
					return;
				}
			}

			// Token: 0x06003562 RID: 13666 RVA: 0x0009E60C File Offset: 0x0009C80C
			protected override void OnDetach()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun -= this.AttachAbility;
						this.owner.status.onReleaseStun -= this.DetachAbility;
						return;
					}
					this.owner.status.onReleasePoison -= this.AttachAbility;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze -= this.AttachAbility;
						this.owner.status.onReleaseFreeze -= this.DetachAbility;
						return;
					}
					this.owner.status.onReleaseFreeze -= this.AttachAbility;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn -= this.AttachAbility;
						this.owner.status.onReleaseBurn -= this.DetachAbility;
						return;
					}
					this.owner.status.onReleaseBurn -= this.AttachAbility;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound -= this.AttachAbility;
						this.owner.status.onApplyBleed -= this.DetachAbility;
						return;
					}
					this.owner.status.onApplyBleed -= this.AttachAbility;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison -= this.AttachAbility;
						this.owner.status.onReleasePoison -= this.DetachAbility;
						return;
					}
					this.owner.status.onReleasePoison -= this.AttachAbility;
					return;
				default:
					return;
				}
			}

			// Token: 0x06003563 RID: 13667 RVA: 0x0009E82E File Offset: 0x0009CA2E
			private void AttachAbility(Character owner, Character target)
			{
				target.ability.Add(this.ability._abilityComponent.ability);
			}

			// Token: 0x06003564 RID: 13668 RVA: 0x0009E84C File Offset: 0x0009CA4C
			private void DetachAbility(Character owner, Character target)
			{
				if (target.health.dead)
				{
					return;
				}
				target.ability.Remove(this.ability._abilityComponent.ability);
			}
		}
	}
}
