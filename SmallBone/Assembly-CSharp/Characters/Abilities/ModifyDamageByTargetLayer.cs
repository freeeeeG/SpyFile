using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A74 RID: 2676
	[Serializable]
	public class ModifyDamageByTargetLayer : Ability
	{
		// Token: 0x060037BC RID: 14268 RVA: 0x000A473B File Offset: 0x000A293B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByTargetLayer.Instance(owner, this);
		}

		// Token: 0x04002C66 RID: 11366
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04002C67 RID: 11367
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C68 RID: 11368
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C69 RID: 11369
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C6A RID: 11370
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x02000A75 RID: 2677
		public class Instance : AbilityInstance<ModifyDamageByTargetLayer>
		{
			// Token: 0x060037BE RID: 14270 RVA: 0x000A4757 File Offset: 0x000A2957
			internal Instance(Character owner, ModifyDamageByTargetLayer ability) : base(owner, ability)
			{
			}

			// Token: 0x060037BF RID: 14271 RVA: 0x000A4761 File Offset: 0x000A2961
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037C0 RID: 14272 RVA: 0x000A4780 File Offset: 0x000A2980
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037C1 RID: 14273 RVA: 0x000A47A0 File Offset: 0x000A29A0
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
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
