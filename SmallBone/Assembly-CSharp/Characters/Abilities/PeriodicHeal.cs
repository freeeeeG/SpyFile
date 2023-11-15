using System;
using Characters.Abilities.Constraints;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities
{
	// Token: 0x02000A9D RID: 2717
	[Serializable]
	public class PeriodicHeal : Ability
	{
		// Token: 0x0600382A RID: 14378 RVA: 0x000A5B02 File Offset: 0x000A3D02
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new PeriodicHeal.Instance(owner, this);
		}

		// Token: 0x04002CC3 RID: 11459
		[SerializeField]
		private float _period;

		// Token: 0x04002CC4 RID: 11460
		[FormerlySerializedAs("_amount")]
		[SerializeField]
		private int _healPercent;

		// Token: 0x04002CC5 RID: 11461
		[SerializeField]
		private CustomFloat _healConstant;

		// Token: 0x04002CC6 RID: 11462
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		[Space]
		private Constraint.Subcomponents _constraints;

		// Token: 0x02000A9E RID: 2718
		public class Instance : AbilityInstance<PeriodicHeal>
		{
			// Token: 0x17000BC3 RID: 3011
			// (get) Token: 0x0600382B RID: 14379 RVA: 0x000A5B0B File Offset: 0x000A3D0B
			public override float iconFillAmount
			{
				get
				{
					return 1f - this._remainPeriod / this.ability._period;
				}
			}

			// Token: 0x0600382C RID: 14380 RVA: 0x000A5B25 File Offset: 0x000A3D25
			public Instance(Character owner, PeriodicHeal ability) : base(owner, ability)
			{
			}

			// Token: 0x0600382D RID: 14381 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x0600382E RID: 14382 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x0600382F RID: 14383 RVA: 0x000A5B30 File Offset: 0x000A3D30
			public override void UpdateTime(float deltaTime)
			{
				if (!this.ability._constraints.Pass())
				{
					return;
				}
				base.UpdateTime(deltaTime);
				this._remainPeriod -= deltaTime;
				if (this._remainPeriod <= 0f)
				{
					this._remainPeriod += this.ability._period;
					float num = (float)this.ability._healPercent * 0.01f;
					if (num > 0f)
					{
						this.owner.health.PercentHeal(num);
					}
					num = this.ability._healConstant.value;
					if (num > 0f)
					{
						this.owner.health.Heal((double)num, true);
					}
				}
			}

			// Token: 0x04002CC7 RID: 11463
			private float _remainPeriod;
		}
	}
}
