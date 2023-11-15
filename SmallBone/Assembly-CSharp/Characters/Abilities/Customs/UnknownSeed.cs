using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D9C RID: 3484
	[Serializable]
	public class UnknownSeed : Ability
	{
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x000CAE0B File Offset: 0x000C900B
		// (set) Token: 0x06004614 RID: 17940 RVA: 0x000CAE13 File Offset: 0x000C9013
		public UnknownSeedComponent component { get; set; }

		// Token: 0x06004615 RID: 17941 RVA: 0x000CAE1C File Offset: 0x000C901C
		public override void Initialize()
		{
			base.Initialize();
			this._changingOperations.Initialize();
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x000CAE30 File Offset: 0x000C9030
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new UnknownSeed.Instance(owner, this);
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x000CAE4D File Offset: 0x000C904D
		public void UpdateStat()
		{
			if (this._instance == null)
			{
				return;
			}
			this._instance.UpdateStat(true);
		}

		// Token: 0x04003529 RID: 13609
		[Space]
		[SerializeField]
		private Stat.Values _statBonus;

		// Token: 0x0400352A RID: 13610
		[SerializeField]
		private float _healToMax;

		// Token: 0x0400352B RID: 13611
		[SerializeField]
		private float _healAmountPerStack;

		// Token: 0x0400352C RID: 13612
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _changingOperations;

		// Token: 0x0400352D RID: 13613
		private UnknownSeed.Instance _instance;

		// Token: 0x02000D9D RID: 3485
		public class Instance : AbilityInstance<UnknownSeed>
		{
			// Token: 0x17000E93 RID: 3731
			// (get) Token: 0x06004619 RID: 17945 RVA: 0x000CAE64 File Offset: 0x000C9064
			public override Sprite icon
			{
				get
				{
					if (this.ability.component.healed <= 0f)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000E94 RID: 3732
			// (get) Token: 0x0600461A RID: 17946 RVA: 0x000CAE8A File Offset: 0x000C908A
			public override int iconStacks
			{
				get
				{
					return (int)(this._stat.values[0].value * 100.0);
				}
			}

			// Token: 0x0600461B RID: 17947 RVA: 0x000CAEA9 File Offset: 0x000C90A9
			public Instance(Character owner, UnknownSeed ability) : base(owner, ability)
			{
				this._stat = ability._statBonus.Clone();
			}

			// Token: 0x0600461C RID: 17948 RVA: 0x000CAEC4 File Offset: 0x000C90C4
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStat(true);
				this.owner.health.onHealed += this.OnHealed;
			}

			// Token: 0x0600461D RID: 17949 RVA: 0x000CAEFF File Offset: 0x000C90FF
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.health.onHealed -= this.OnHealed;
			}

			// Token: 0x0600461E RID: 17950 RVA: 0x000CAF34 File Offset: 0x000C9134
			private void OnHealed(double healed, double overHealed)
			{
				this.ability.component.healed += (float)healed;
				if (this.ability.component.healed > this.ability._healToMax)
				{
					this.ability._changingOperations.Run(this.owner);
				}
			}

			// Token: 0x0600461F RID: 17951 RVA: 0x000CAF8D File Offset: 0x000C918D
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

			// Token: 0x06004620 RID: 17952 RVA: 0x000CAFC4 File Offset: 0x000C91C4
			public void UpdateStat(bool force = false)
			{
				if (!force && this.ability.component.healed == this.ability.component.healedBefore)
				{
					return;
				}
				Stat.Value[] values = this._stat.values;
				int num = Mathf.FloorToInt(this.ability.component.healed / this.ability._healAmountPerStack);
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._statBonus.values[i].value * (double)num;
				}
				this.owner.stat.SetNeedUpdate();
				this.ability.component.healedBefore = this.ability.component.healed;
			}

			// Token: 0x0400352E RID: 13614
			private const float _updateInterval = 0.2f;

			// Token: 0x0400352F RID: 13615
			private float _remainUpdateTime;

			// Token: 0x04003530 RID: 13616
			private Stat.Values _stat;
		}
	}
}
