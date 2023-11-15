using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D83 RID: 3459
	[Serializable]
	public class RiderPassive : Ability
	{
		// Token: 0x0600459D RID: 17821 RVA: 0x000C9CF6 File Offset: 0x000C7EF6
		public RiderPassive()
		{
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x000C9D2A File Offset: 0x000C7F2A
		public RiderPassive(Stat.Values stat)
		{
			this._stat = stat;
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x000C9D65 File Offset: 0x000C7F65
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RiderPassive.Instance(owner, this);
		}

		// Token: 0x040034E5 RID: 13541
		public const float _overlapInterval = 0.25f;

		// Token: 0x040034E6 RID: 13542
		private Stat.Values _stat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 1.0)
		});

		// Token: 0x040034E7 RID: 13543
		[SerializeField]
		private float _criticalChancePerSpeed;

		// Token: 0x040034E8 RID: 13544
		[SerializeField]
		private float _maxCriticalChance;

		// Token: 0x02000D84 RID: 3460
		public class Instance : AbilityInstance<RiderPassive>
		{
			// Token: 0x17000E78 RID: 3704
			// (get) Token: 0x060045A0 RID: 17824 RVA: 0x000C9D6E File Offset: 0x000C7F6E
			public override int iconStacks
			{
				get
				{
					return this._criticalChanceStack;
				}
			}

			// Token: 0x060045A1 RID: 17825 RVA: 0x000C9D76 File Offset: 0x000C7F76
			public Instance(Character owner, RiderPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x060045A2 RID: 17826 RVA: 0x000C9D80 File Offset: 0x000C7F80
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x060045A3 RID: 17827 RVA: 0x000C9D9D File Offset: 0x000C7F9D
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
			}

			// Token: 0x060045A4 RID: 17828 RVA: 0x000C9DBA File Offset: 0x000C7FBA
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCheckTime -= deltaTime;
				if (this._remainCheckTime < 0f)
				{
					this._remainCheckTime += 0.25f;
					this.UpdateStat();
				}
			}

			// Token: 0x060045A5 RID: 17829 RVA: 0x000C9DF8 File Offset: 0x000C7FF8
			private void UpdateStat()
			{
				double num = Math.Min(this.owner.stat.GetPercentPoint(Stat.Kind.MovementSpeed) * (double)this.ability._criticalChancePerSpeed, (double)this.ability._maxCriticalChance * 0.01);
				this._criticalChanceStack = (int)(num * 100.0);
				this.ability._stat.values[0].value = num;
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040034E9 RID: 13545
			private float _remainCheckTime;

			// Token: 0x040034EA RID: 13546
			private int _criticalChanceStack;
		}
	}
}
