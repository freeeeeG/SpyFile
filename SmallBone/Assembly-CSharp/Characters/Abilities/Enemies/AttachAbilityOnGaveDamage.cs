using System;
using UnityEngine;

namespace Characters.Abilities.Enemies
{
	// Token: 0x02000B92 RID: 2962
	[Serializable]
	public class AttachAbilityOnGaveDamage : Ability
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x000B278A File Offset: 0x000B098A
		public override void Initialize()
		{
			base.Initialize();
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x000B279D File Offset: 0x000B099D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AttachAbilityOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002F7E RID: 12158
		[SerializeField]
		[Information("Add만 할 뿐 Remove를 하지 않으니 신중히 사용해야함", InformationAttribute.InformationType.Warning, true)]
		private float _cooldown;

		// Token: 0x04002F7F RID: 12159
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002F80 RID: 12160
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x04002F81 RID: 12161
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002F82 RID: 12162
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002F83 RID: 12163
		[SerializeField]
		private bool _refreshIfHas = true;

		// Token: 0x02000B93 RID: 2963
		public class Instance : AbilityInstance<AttachAbilityOnGaveDamage>
		{
			// Token: 0x06003D61 RID: 15713 RVA: 0x000B27B5 File Offset: 0x000B09B5
			public Instance(Character owner, AttachAbilityOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D62 RID: 15714 RVA: 0x000B27BF File Offset: 0x000B09BF
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003D63 RID: 15715 RVA: 0x000B27E8 File Offset: 0x000B09E8
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003D64 RID: 15716 RVA: 0x000B2811 File Offset: 0x000B0A11
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldown -= deltaTime;
			}

			// Token: 0x06003D65 RID: 15717 RVA: 0x000B2828 File Offset: 0x000B0A28
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (this._remainCooldown > 0f)
				{
					return;
				}
				if (target.character == null || target.character.health.dead)
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !gaveDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (!this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._damageTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (this.ability._refreshIfHas)
				{
					if (this._cache != null && this._cache.attached)
					{
						this.RefreshCache();
						return;
					}
					this._cache = target.character.ability.GetInstanceType(this.ability._abilityComponent.ability);
					if (this._cache != null)
					{
						this.RefreshCache();
						return;
					}
				}
				this._remainCooldown = this.ability._cooldown;
				target.character.ability.Add(this.ability._abilityComponent.ability);
			}

			// Token: 0x06003D66 RID: 15718 RVA: 0x000B2951 File Offset: 0x000B0B51
			private void RefreshCache()
			{
				this._remainCooldown = this.ability._cooldown;
				this._cache.Refresh();
			}

			// Token: 0x04002F84 RID: 12164
			private float _remainCooldown;

			// Token: 0x04002F85 RID: 12165
			private IAbilityInstance _cache;
		}
	}
}
