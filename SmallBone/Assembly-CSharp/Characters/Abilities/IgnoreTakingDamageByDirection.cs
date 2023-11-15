using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A53 RID: 2643
	[Serializable]
	public class IgnoreTakingDamageByDirection : Ability
	{
		// Token: 0x0600375A RID: 14170 RVA: 0x000A3301 File Offset: 0x000A1501
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new IgnoreTakingDamageByDirection.Instance(owner, this);
		}

		// Token: 0x04002C0E RID: 11278
		[SerializeField]
		private IgnoreTakingDamageByDirection.Direction _from;

		// Token: 0x02000A54 RID: 2644
		public class Instance : AbilityInstance<IgnoreTakingDamageByDirection>
		{
			// Token: 0x0600375C RID: 14172 RVA: 0x000A330A File Offset: 0x000A150A
			public Instance(Character owner, IgnoreTakingDamageByDirection ability) : base(owner, ability)
			{
			}

			// Token: 0x0600375D RID: 14173 RVA: 0x000A3314 File Offset: 0x000A1514
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.CancelDamage));
			}

			// Token: 0x0600375E RID: 14174 RVA: 0x000A333C File Offset: 0x000A153C
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.CancelDamage));
			}

			// Token: 0x0600375F RID: 14175 RVA: 0x000A3360 File Offset: 0x000A1560
			private bool CancelDamage(ref Damage damage)
			{
				if (damage.attackType == Damage.AttackType.Additional)
				{
					return false;
				}
				ref Vector3 position = this.owner.transform.position;
				Vector3 vector = damage.attacker.transform.position;
				if (damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile)
				{
					vector = damage.hitPoint;
				}
				bool flag = this.ability._from == IgnoreTakingDamageByDirection.Direction.Front && this.owner.lookingDirection == Character.LookingDirection.Right;
				flag |= (this.ability._from == IgnoreTakingDamageByDirection.Direction.Back && this.owner.lookingDirection == Character.LookingDirection.Left);
				bool flag2 = position.x < vector.x;
				if ((flag && !flag2) || (!flag && flag2))
				{
					return false;
				}
				damage.@null = true;
				return true;
			}
		}

		// Token: 0x02000A55 RID: 2645
		private enum Direction
		{
			// Token: 0x04002C10 RID: 11280
			Front,
			// Token: 0x04002C11 RID: 11281
			Back
		}
	}
}
