using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A8C RID: 2700
	[Serializable]
	public class ModifyTrapDamage : Ability
	{
		// Token: 0x060037F7 RID: 14327 RVA: 0x000A5314 File Offset: 0x000A3514
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyTrapDamage.Instance(owner, this);
		}

		// Token: 0x04002CAA RID: 11434
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002CAB RID: 11435
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x02000A8D RID: 2701
		public class Instance : AbilityInstance<ModifyTrapDamage>
		{
			// Token: 0x060037F9 RID: 14329 RVA: 0x000A5330 File Offset: 0x000A3530
			public Instance(Character owner, ModifyTrapDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060037FA RID: 14330 RVA: 0x000A533A File Offset: 0x000A353A
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x060037FB RID: 14331 RVA: 0x000A5362 File Offset: 0x000A3562
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x060037FC RID: 14332 RVA: 0x000A5388 File Offset: 0x000A3588
			private bool OnOwnerTakeDamage(ref Damage damage)
			{
				if (damage.attacker.character != null && damage.attacker.character.type == Character.Type.Trap)
				{
					damage.percentMultiplier *= (double)this.ability._damagePercent;
					damage.multiplier += (double)this.ability._damagePercentPoint;
				}
				return false;
			}

			// Token: 0x04002CAC RID: 11436
			private float _remainCooldownTime;
		}
	}
}
