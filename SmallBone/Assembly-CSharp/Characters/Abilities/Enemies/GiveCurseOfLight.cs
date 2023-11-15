using System;
using UnityEngine;

namespace Characters.Abilities.Enemies
{
	// Token: 0x02000B98 RID: 2968
	[Serializable]
	public class GiveCurseOfLight : Ability
	{
		// Token: 0x06003D77 RID: 15735 RVA: 0x000B2C42 File Offset: 0x000B0E42
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GiveCurseOfLight.Instance(owner, this);
		}

		// Token: 0x04002F8D RID: 12173
		[SerializeField]
		[Information("Add만 할 뿐 Remove를 하지 않으니 신중히 사용해야함", InformationAttribute.InformationType.Warning, true)]
		private float _cooldown;

		// Token: 0x04002F8E RID: 12174
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002F8F RID: 12175
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x04002F90 RID: 12176
		[SerializeField]
		private string _attackKey;

		// Token: 0x02000B99 RID: 2969
		public class Instance : AbilityInstance<GiveCurseOfLight>
		{
			// Token: 0x06003D79 RID: 15737 RVA: 0x000B2C4B File Offset: 0x000B0E4B
			public Instance(Character owner, GiveCurseOfLight ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D7A RID: 15738 RVA: 0x000B2C55 File Offset: 0x000B0E55
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003D7B RID: 15739 RVA: 0x000B2C7E File Offset: 0x000B0E7E
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003D7C RID: 15740 RVA: 0x000B2CA7 File Offset: 0x000B0EA7
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldown -= deltaTime;
			}

			// Token: 0x06003D7D RID: 15741 RVA: 0x000B2CC0 File Offset: 0x000B0EC0
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
				if (target.character.playerComponents == null)
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
				if (target.character.invulnerable.value)
				{
					return;
				}
				this._remainCooldown = this.ability._cooldown;
				target.character.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.Curse);
			}

			// Token: 0x04002F91 RID: 12177
			private float _remainCooldown;
		}
	}
}
