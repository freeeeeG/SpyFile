using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A6E RID: 2670
	[Serializable]
	public sealed class ModifyDamageByStackResolver : Ability
	{
		// Token: 0x060037A9 RID: 14249 RVA: 0x000A42F5 File Offset: 0x000A24F5
		public override void Initialize()
		{
			base.Initialize();
			this._stackResolver.Initialize();
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000A4308 File Offset: 0x000A2508
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByStackResolver.Instance(owner, this);
		}

		// Token: 0x04002C4F RID: 11343
		[SerializeField]
		private int _priority;

		// Token: 0x04002C50 RID: 11344
		[SubclassSelector]
		[SerializeReference]
		private IStackResolver _stackResolver;

		// Token: 0x04002C51 RID: 11345
		[Header("공격 대상 필터")]
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x04002C52 RID: 11346
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C53 RID: 11347
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C54 RID: 11348
		[Header("설정")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C55 RID: 11349
		[SerializeField]
		private string[] _attackKeys;

		// Token: 0x04002C56 RID: 11350
		[SerializeField]
		private float _damagePercentPerStack;

		// Token: 0x04002C57 RID: 11351
		[SerializeField]
		private float _damagePercentPointPerStack;

		// Token: 0x04002C58 RID: 11352
		[SerializeField]
		private float _extraCriticalChancePerStack;

		// Token: 0x04002C59 RID: 11353
		[SerializeField]
		private float _extraCriticalDamageMultiplierPerStack;

		// Token: 0x02000A6F RID: 2671
		public sealed class Instance : AbilityInstance<ModifyDamageByStackResolver>
		{
			// Token: 0x060037AC RID: 14252 RVA: 0x000A4311 File Offset: 0x000A2511
			public Instance(Character owner, ModifyDamageByStackResolver ability) : base(owner, ability)
			{
			}

			// Token: 0x060037AD RID: 14253 RVA: 0x000A431B File Offset: 0x000A251B
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(this.ability._priority, new GiveDamageDelegate(this.HandleOnGiveDamage));
				this.ability._stackResolver.Attach(this.owner);
			}

			// Token: 0x060037AE RID: 14254 RVA: 0x000A435A File Offset: 0x000A255A
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this.ability._stackResolver.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060037AF RID: 14255 RVA: 0x000A4384 File Offset: 0x000A2584
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.Filter(target, damage))
				{
					return false;
				}
				int stack = this.ability._stackResolver.GetStack(ref damage);
				damage.percentMultiplier *= (double)(1f + this.ability._damagePercentPerStack * (float)stack);
				damage.multiplier += (double)(this.ability._damagePercentPointPerStack * (float)stack);
				damage.criticalChance += (double)(this.ability._extraCriticalChancePerStack * (float)stack);
				damage.criticalDamageMultiplier += (double)(this.ability._extraCriticalDamageMultiplierPerStack * (float)stack);
				this._remainCooldownTime = this.ability._cooldownTime;
				return false;
			}

			// Token: 0x060037B0 RID: 14256 RVA: 0x000A4430 File Offset: 0x000A2630
			private bool Filter(ITarget target, Damage damage)
			{
				if (this._remainCooldownTime > 0f)
				{
					return false;
				}
				if (target.character != null && !this.ability._characterTypes[target.character.type])
				{
					return false;
				}
				if (!this.ability._attackTypes[damage.motionType])
				{
					return false;
				}
				if (!this.ability._damageTypes[damage.attackType])
				{
					return false;
				}
				if (this.ability._attackKeys.Length != 0)
				{
					bool flag = false;
					foreach (string value in this.ability._attackKeys)
					{
						if (damage.key.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060037B1 RID: 14257 RVA: 0x000A44F4 File Offset: 0x000A26F4
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				this.ability._stackResolver.Detach(this.owner);
			}

			// Token: 0x04002C5A RID: 11354
			private float _remainCooldownTime;
		}
	}
}
