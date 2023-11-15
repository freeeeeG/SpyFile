using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A6B RID: 2667
	[Serializable]
	public class ModifyDamageByOwnerAndTargetHealth : Ability
	{
		// Token: 0x060037A2 RID: 14242 RVA: 0x000A4194 File Offset: 0x000A2394
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByOwnerAndTargetHealth.Instance(owner, this);
		}

		// Token: 0x04002C49 RID: 11337
		[SerializeField]
		private bool _ownerHealthGreaterThanTargerHealth;

		// Token: 0x04002C4A RID: 11338
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04002C4B RID: 11339
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C4C RID: 11340
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C4D RID: 11341
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C4E RID: 11342
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x02000A6C RID: 2668
		public class Instance : AbilityInstance<ModifyDamageByOwnerAndTargetHealth>
		{
			// Token: 0x060037A4 RID: 14244 RVA: 0x000A41B0 File Offset: 0x000A23B0
			internal Instance(Character owner, ModifyDamageByOwnerAndTargetHealth ability) : base(owner, ability)
			{
			}

			// Token: 0x060037A5 RID: 14245 RVA: 0x000A41BA File Offset: 0x000A23BA
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037A6 RID: 14246 RVA: 0x000A41D9 File Offset: 0x000A23D9
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037A7 RID: 14247 RVA: 0x000A41F8 File Offset: 0x000A23F8
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if (this.ability._ownerHealthGreaterThanTargerHealth)
				{
					if (this.owner.health.percent < character.health.percent)
					{
						return false;
					}
				}
				else if (this.owner.health.percent > character.health.percent)
				{
					return false;
				}
				if (this.ability._targetLayer.Evaluate(this.owner.gameObject).Contains(target.character.gameObject.layer))
				{
					damage.percentMultiplier *= (double)this.ability._damagePercent;
					damage.multiplier += (double)this.ability._damagePercentPoint;
					damage.criticalChance += (double)this.ability._extraCriticalChance;
					damage.criticalDamageMultiplier += (double)this.ability._extraCriticalDamageMultiplier;
				}
				return false;
			}
		}
	}
}
