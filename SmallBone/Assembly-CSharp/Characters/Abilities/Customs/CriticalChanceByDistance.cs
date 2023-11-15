using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D40 RID: 3392
	[Serializable]
	public class CriticalChanceByDistance : Ability
	{
		// Token: 0x0600445A RID: 17498 RVA: 0x000C6ADA File Offset: 0x000C4CDA
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CriticalChanceByDistance.Instance(owner, this);
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x000C6AE3 File Offset: 0x000C4CE3
		private float GetBonusCriticalChance(float distance)
		{
			return Mathf.Clamp01((distance - this._minBonusDistance) / this._maxBonusDistance) * (float)this._maxBonusCriticalChance * 0.01f;
		}

		// Token: 0x04003423 RID: 13347
		[SerializeField]
		[Range(0f, 100f)]
		private int _maxBonusCriticalChance;

		// Token: 0x04003424 RID: 13348
		[Header("Filter")]
		[SerializeField]
		private MotionTypeBoolArray _motionFilter;

		// Token: 0x04003425 RID: 13349
		[SerializeField]
		private AttackTypeBoolArray _attackFilter;

		// Token: 0x04003426 RID: 13350
		[SerializeField]
		[Header("Distance")]
		private float _minBonusDistance = 2f;

		// Token: 0x04003427 RID: 13351
		[SerializeField]
		private float _maxBonusDistance = 7f;

		// Token: 0x02000D41 RID: 3393
		public class Instance : AbilityInstance<CriticalChanceByDistance>
		{
			// Token: 0x0600445D RID: 17501 RVA: 0x000C6B25 File Offset: 0x000C4D25
			internal Instance(Character owner, CriticalChanceByDistance ability) : base(owner, ability)
			{
			}

			// Token: 0x0600445E RID: 17502 RVA: 0x000C6B2F File Offset: 0x000C4D2F
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x0600445F RID: 17503 RVA: 0x000C6B4E File Offset: 0x000C4D4E
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x06004460 RID: 17504 RVA: 0x000C6B70 File Offset: 0x000C4D70
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.ability._motionFilter[damage.motionType] || !this.ability._attackFilter[damage.attackType])
				{
					return false;
				}
				Vector2 a = MMMaths.Vector3ToVector2(target.transform.position);
				Vector2 b = MMMaths.Vector3ToVector2(this.owner.transform.position);
				float distance = Vector2.Distance(a, b);
				damage.criticalChance += (double)this.ability.GetBonusCriticalChance(distance);
				return false;
			}

			// Token: 0x04003428 RID: 13352
			private int _remainCount;
		}
	}
}
