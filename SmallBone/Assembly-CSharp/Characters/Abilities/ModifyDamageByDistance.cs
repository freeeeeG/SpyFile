using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A65 RID: 2661
	[Serializable]
	public sealed class ModifyDamageByDistance : Ability
	{
		// Token: 0x06003793 RID: 14227 RVA: 0x000A3E9E File Offset: 0x000A209E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByDistance.Instance(owner, this);
		}

		// Token: 0x04002C3C RID: 11324
		[SerializeField]
		private Transform _center;

		// Token: 0x04002C3D RID: 11325
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002C3E RID: 11326
		[SerializeField]
		private float _minBonusDistance = 2f;

		// Token: 0x04002C3F RID: 11327
		[SerializeField]
		private float _maxBonusDistance = 7f;

		// Token: 0x04002C40 RID: 11328
		[SerializeField]
		private float additionalDamageMultiplier = 1f;

		// Token: 0x04002C41 RID: 11329
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C42 RID: 11330
		[SerializeField]
		private float additionalCriticalChance;

		// Token: 0x04002C43 RID: 11331
		[SerializeField]
		private bool _skipMaxOver;

		// Token: 0x04002C44 RID: 11332
		[SerializeField]
		private bool _skipMinUnder = true;

		// Token: 0x04002C45 RID: 11333
		[SerializeField]
		private bool _discrete = true;

		// Token: 0x02000A66 RID: 2662
		public class Instance : AbilityInstance<ModifyDamageByDistance>
		{
			// Token: 0x06003795 RID: 14229 RVA: 0x000A3EF5 File Offset: 0x000A20F5
			internal Instance(Character owner, ModifyDamageByDistance ability) : base(owner, ability)
			{
			}

			// Token: 0x06003796 RID: 14230 RVA: 0x000A3EFF File Offset: 0x000A20FF
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003797 RID: 14231 RVA: 0x000A3F1E File Offset: 0x000A211E
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003798 RID: 14232 RVA: 0x000A3F40 File Offset: 0x000A2140
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.ability._motionType[damage.motionType])
				{
					return false;
				}
				ref Vector2 ptr = MMMaths.Vector3ToVector2(target.transform.position);
				Vector2 vector = (this.ability._center == null) ? MMMaths.Vector3ToVector2(this.owner.transform.position) : MMMaths.Vector3ToVector2(this.ability._center.transform.position);
				float num = Mathf.Abs(ptr.x - vector.x);
				if (this.ability._skipMinUnder && num < this.ability._minBonusDistance)
				{
					return false;
				}
				if (this.ability._skipMaxOver && num > this.ability._maxBonusDistance)
				{
					return false;
				}
				float num2 = this.ability._damagePercent;
				float num3 = this.ability.additionalDamageMultiplier;
				float num4 = this.ability.additionalCriticalChance;
				if (!this.ability._discrete)
				{
					num2 = Mathf.Lerp(1f, num2, num / this.ability._maxBonusDistance);
					num3 = Mathf.Lerp(0f, num3, num / this.ability._maxBonusDistance);
					num4 = Mathf.Lerp(0f, num4, num / this.ability._maxBonusDistance);
				}
				damage.percentMultiplier *= (double)num2;
				damage.multiplier += (double)num3;
				damage.criticalChance += (double)num4;
				return false;
			}
		}
	}
}
