using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AA6 RID: 2726
	[Serializable]
	public class ReduceTakingDamageByDirection : Ability
	{
		// Token: 0x06003845 RID: 14405 RVA: 0x000A5E91 File Offset: 0x000A4091
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ReduceTakingDamageByDirection.Instance(owner, this);
		}

		// Token: 0x04002CD2 RID: 11474
		[SerializeField]
		private ReduceTakingDamageByDirection.Direction _from;

		// Token: 0x04002CD3 RID: 11475
		[Range(0f, 1f)]
		[SerializeField]
		private float _takingDamageReducePercent;

		// Token: 0x02000AA7 RID: 2727
		public class Instance : AbilityInstance<ReduceTakingDamageByDirection>
		{
			// Token: 0x06003847 RID: 14407 RVA: 0x000A5E9A File Offset: 0x000A409A
			public Instance(Character owner, ReduceTakingDamageByDirection ability) : base(owner, ability)
			{
			}

			// Token: 0x06003848 RID: 14408 RVA: 0x000A5EA4 File Offset: 0x000A40A4
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.CancelDamage));
			}

			// Token: 0x06003849 RID: 14409 RVA: 0x000A5ECC File Offset: 0x000A40CC
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.CancelDamage));
			}

			// Token: 0x0600384A RID: 14410 RVA: 0x000A5EF0 File Offset: 0x000A40F0
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
				bool flag = this.ability._from == ReduceTakingDamageByDirection.Direction.Front && this.owner.lookingDirection == Character.LookingDirection.Right;
				flag |= (this.ability._from == ReduceTakingDamageByDirection.Direction.Back && this.owner.lookingDirection == Character.LookingDirection.Left);
				bool flag2 = position.x < vector.x;
				if ((flag && !flag2) || (!flag && flag2))
				{
					return false;
				}
				damage.multiplier -= (double)this.ability._takingDamageReducePercent;
				return false;
			}
		}

		// Token: 0x02000AA8 RID: 2728
		private enum Direction
		{
			// Token: 0x04002CD5 RID: 11477
			Front,
			// Token: 0x04002CD6 RID: 11478
			Back
		}
	}
}
