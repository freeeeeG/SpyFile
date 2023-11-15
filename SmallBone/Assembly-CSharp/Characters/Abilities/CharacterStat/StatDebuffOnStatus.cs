using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C83 RID: 3203
	[Serializable]
	public sealed class StatDebuffOnStatus : Ability
	{
		// Token: 0x06004156 RID: 16726 RVA: 0x000BDE0F File Offset: 0x000BC00F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatDebuffOnStatus.Instance(owner, this);
		}

		// Token: 0x04003223 RID: 12835
		[SerializeField]
		private bool _onRelease;

		// Token: 0x04003224 RID: 12836
		[SerializeField]
		internal CharacterStatus.Kind _kind;

		// Token: 0x04003225 RID: 12837
		[SerializeField]
		private StatBonus _statBonus;

		// Token: 0x02000C84 RID: 3204
		public class Instance : AbilityInstance<StatDebuffOnStatus>
		{
			// Token: 0x06004158 RID: 16728 RVA: 0x000BDE18 File Offset: 0x000BC018
			public Instance(Character owner, StatDebuffOnStatus ability) : base(owner, ability)
			{
				ability._statBonus.Initialize();
			}

			// Token: 0x06004159 RID: 16729 RVA: 0x000BDE30 File Offset: 0x000BC030
			protected override void OnAttach()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun += this.AttachStatDebuff;
						this.owner.status.onReleaseStun += this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleaseStun += this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze += this.AttachStatDebuff;
						this.owner.status.onReleaseFreeze += this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleaseFreeze += this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn += this.AttachStatDebuff;
						this.owner.status.onReleaseBurn += this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleaseBurn += this.DetachStatDebuff;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound += this.AttachStatDebuff;
						this.owner.status.onApplyBleed += this.DetachStatDebuff;
						return;
					}
					this.owner.status.onApplyBleed += this.DetachStatDebuff;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison += this.AttachStatDebuff;
						this.owner.status.onReleasePoison += this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleasePoison += this.AttachStatDebuff;
					return;
				default:
					return;
				}
			}

			// Token: 0x0600415A RID: 16730 RVA: 0x000BE054 File Offset: 0x000BC254
			protected override void OnDetach()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun -= this.AttachStatDebuff;
						this.owner.status.onReleaseStun -= this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleasePoison -= this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze -= this.AttachStatDebuff;
						this.owner.status.onReleaseFreeze -= this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleaseFreeze -= this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn -= this.AttachStatDebuff;
						this.owner.status.onReleaseBurn -= this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleaseBurn -= this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound -= this.AttachStatDebuff;
						this.owner.status.onApplyBleed -= this.DetachStatDebuff;
						return;
					}
					this.owner.status.onApplyBleed -= this.AttachStatDebuff;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison -= this.AttachStatDebuff;
						this.owner.status.onReleasePoison -= this.DetachStatDebuff;
						return;
					}
					this.owner.status.onReleasePoison -= this.AttachStatDebuff;
					return;
				default:
					return;
				}
			}

			// Token: 0x0600415B RID: 16731 RVA: 0x000BE276 File Offset: 0x000BC476
			private void AttachStatDebuff(Character owner, Character target)
			{
				target.ability.Add(this.ability._statBonus);
			}

			// Token: 0x0600415C RID: 16732 RVA: 0x000BE28F File Offset: 0x000BC48F
			private void DetachStatDebuff(Character owner, Character target)
			{
				if (target.health.dead)
				{
					return;
				}
				target.ability.Remove(this.ability._statBonus);
			}
		}
	}
}
