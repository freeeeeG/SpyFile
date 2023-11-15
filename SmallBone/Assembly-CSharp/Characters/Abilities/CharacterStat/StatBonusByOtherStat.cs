using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C5C RID: 3164
	[Serializable]
	public class StatBonusByOtherStat : Ability
	{
		// Token: 0x060040C0 RID: 16576 RVA: 0x000BC27B File Offset: 0x000BA47B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByOtherStat.Instance(owner, this);
		}

		// Token: 0x040031BF RID: 12735
		[SerializeField]
		[Header("Source Stat, 오른쪽 두개는 무시됨")]
		private Stat.Value _sourceStat;

		// Token: 0x040031C0 RID: 12736
		[SerializeField]
		[Header("스탯 타입 설정, 스탯값은 최대치를 의미함")]
		private Stat.Values _targetStats;

		// Token: 0x040031C1 RID: 12737
		[SerializeField]
		private float _conversionRatio = 0.5f;

		// Token: 0x02000C5D RID: 3165
		public class Instance : AbilityInstance<StatBonusByOtherStat>
		{
			// Token: 0x17000DA6 RID: 3494
			// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000BC297 File Offset: 0x000BA497
			public override int iconStacks
			{
				get
				{
					return (int)(this._cachedBonus * 100.0);
				}
			}

			// Token: 0x17000DA7 RID: 3495
			// (get) Token: 0x060040C3 RID: 16579 RVA: 0x00071719 File Offset: 0x0006F919
			public override float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x060040C4 RID: 16580 RVA: 0x000BC2AA File Offset: 0x000BA4AA
			public Instance(Character owner, StatBonusByOtherStat ability) : base(owner, ability)
			{
				this._stat = ability._targetStats.Clone();
			}

			// Token: 0x060040C5 RID: 16581 RVA: 0x000BC2C5 File Offset: 0x000BA4C5
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStat(true);
			}

			// Token: 0x060040C6 RID: 16582 RVA: 0x000BC2E4 File Offset: 0x000BA4E4
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x060040C7 RID: 16583 RVA: 0x000BC2FC File Offset: 0x000BA4FC
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.2f;
					this.UpdateStat(false);
				}
			}

			// Token: 0x060040C8 RID: 16584 RVA: 0x000BC334 File Offset: 0x000BA534
			public void UpdateStat(bool force)
			{
				double num = (this.owner.stat.GetFinal(Stat.Kind.values[this.ability._sourceStat.kindIndex]) - 1.0) * (double)this.ability._conversionRatio;
				double value = this.ability._targetStats.values[0].value;
				if (num > this.ability._targetStats.values[0].value)
				{
					num = value;
				}
				if (!force && num == this._cachedBonus)
				{
					return;
				}
				this._cachedBonus = num;
				this.SetStat(num);
			}

			// Token: 0x060040C9 RID: 16585 RVA: 0x000BC3D4 File Offset: 0x000BA5D4
			private void SetStat(double bonus)
			{
				Stat.Value[] values = this._stat.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = bonus;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031C2 RID: 12738
			private const float _updateInterval = 0.2f;

			// Token: 0x040031C3 RID: 12739
			private float _remainUpdateTime;

			// Token: 0x040031C4 RID: 12740
			private Stat.Values _stat;

			// Token: 0x040031C5 RID: 12741
			private double _cachedBonus;
		}
	}
}
