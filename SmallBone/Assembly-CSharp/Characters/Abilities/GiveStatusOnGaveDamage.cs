using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A41 RID: 2625
	[Serializable]
	public class GiveStatusOnGaveDamage : Ability
	{
		// Token: 0x06003725 RID: 14117 RVA: 0x000A2D03 File Offset: 0x000A0F03
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GiveStatusOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002BF5 RID: 11253
		[SerializeField]
		private float _cooldown;

		// Token: 0x04002BF6 RID: 11254
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002BF7 RID: 11255
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x04002BF8 RID: 11256
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x02000A42 RID: 2626
		public class Instance : AbilityInstance<GiveStatusOnGaveDamage>
		{
			// Token: 0x17000BAA RID: 2986
			// (get) Token: 0x06003727 RID: 14119 RVA: 0x000A2D0C File Offset: 0x000A0F0C
			public override float iconFillAmount
			{
				get
				{
					return this._remainCooldown / this.ability._cooldown;
				}
			}

			// Token: 0x06003728 RID: 14120 RVA: 0x000A2D20 File Offset: 0x000A0F20
			public Instance(Character owner, GiveStatusOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003729 RID: 14121 RVA: 0x000A2D2A File Offset: 0x000A0F2A
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600372A RID: 14122 RVA: 0x000A2D53 File Offset: 0x000A0F53
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600372B RID: 14123 RVA: 0x000A2D7C File Offset: 0x000A0F7C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldown -= deltaTime;
			}

			// Token: 0x0600372C RID: 14124 RVA: 0x000A2D94 File Offset: 0x000A0F94
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
				if (!this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._damageTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				this._remainCooldown = this.ability._cooldown;
				this.owner.GiveStatus(target.character, this.ability._status);
			}

			// Token: 0x04002BF9 RID: 11257
			private float _remainCooldown;
		}
	}
}
