using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B0A RID: 2826
	[Serializable]
	public sealed class StatusRegrant : Ability
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x000A9A0F File Offset: 0x000A7C0F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatusRegrant.Instance(owner, this);
		}

		// Token: 0x04002D9C RID: 11676
		[SerializeField]
		[Range(0f, 100f)]
		private int _chance;

		// Token: 0x02000B0B RID: 2827
		public sealed class Instance : AbilityInstance<StatusRegrant>
		{
			// Token: 0x06003985 RID: 14725 RVA: 0x000A9A18 File Offset: 0x000A7C18
			public Instance(Character owner, StatusRegrant ability) : base(owner, ability)
			{
				this._burn = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Burn);
				this._freeze = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Freeze);
				this._poison = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Poison);
				this._stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
				this._wound = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Wound);
			}

			// Token: 0x06003986 RID: 14726 RVA: 0x000A9A6C File Offset: 0x000A7C6C
			protected override void OnAttach()
			{
				this.owner.status.onReleaseBurn += this.HandleOnReleaseBurn;
				this.owner.status.onReleaseFreeze += this.HandleOnReleaseFreeze;
				this.owner.status.onReleasePoison += this.HandleOnReleasePoison;
				this.owner.status.onReleaseStun += this.HandleOnReleaseStun;
				this.owner.status.onApplyWound += this.HandleOnApplyWound;
			}

			// Token: 0x06003987 RID: 14727 RVA: 0x000A9B05 File Offset: 0x000A7D05
			private void HandleOnApplyWound(Character attacker, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this._wound);
			}

			// Token: 0x06003988 RID: 14728 RVA: 0x000A9B2D File Offset: 0x000A7D2D
			private void HandleOnReleaseStun(Character attacker, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this._stun);
			}

			// Token: 0x06003989 RID: 14729 RVA: 0x000A9B55 File Offset: 0x000A7D55
			private void HandleOnReleasePoison(Character attacker, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this._poison);
			}

			// Token: 0x0600398A RID: 14730 RVA: 0x000A9B7D File Offset: 0x000A7D7D
			private void HandleOnReleaseFreeze(Character attacker, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this._freeze);
			}

			// Token: 0x0600398B RID: 14731 RVA: 0x000A9BA5 File Offset: 0x000A7DA5
			private void HandleOnReleaseBurn(Character attacker, Character target)
			{
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this._burn);
			}

			// Token: 0x0600398C RID: 14732 RVA: 0x000A9BD0 File Offset: 0x000A7DD0
			protected override void OnDetach()
			{
				this.owner.status.onReleaseBurn -= this.HandleOnReleaseBurn;
				this.owner.status.onReleaseFreeze -= this.HandleOnReleaseFreeze;
				this.owner.status.onReleasePoison -= this.HandleOnReleasePoison;
				this.owner.status.onReleaseStun -= this.HandleOnReleaseStun;
				this.owner.status.onApplyWound -= this.HandleOnApplyWound;
			}

			// Token: 0x04002D9D RID: 11677
			private CharacterStatus.ApplyInfo _burn;

			// Token: 0x04002D9E RID: 11678
			private CharacterStatus.ApplyInfo _freeze;

			// Token: 0x04002D9F RID: 11679
			private CharacterStatus.ApplyInfo _poison;

			// Token: 0x04002DA0 RID: 11680
			private CharacterStatus.ApplyInfo _stun;

			// Token: 0x04002DA1 RID: 11681
			private CharacterStatus.ApplyInfo _wound;
		}
	}
}
