using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C38 RID: 3128
	[Serializable]
	public class StackableStatBonusByTime : Ability
	{
		// Token: 0x06004031 RID: 16433 RVA: 0x000BA50C File Offset: 0x000B870C
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x000BA527 File Offset: 0x000B8727
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StackableStatBonusByTime.Instance(owner, this);
		}

		// Token: 0x0400315B RID: 12635
		[SerializeField]
		private bool _visibleIconStack = true;

		// Token: 0x0400315C RID: 12636
		[SerializeField]
		private bool _positive = true;

		// Token: 0x0400315D RID: 12637
		[SerializeField]
		private float _updateInterval;

		// Token: 0x0400315E RID: 12638
		[SerializeField]
		private int _startStack;

		// Token: 0x0400315F RID: 12639
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003160 RID: 12640
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x04003161 RID: 12641
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000C39 RID: 3129
		public class Instance : AbilityInstance<StackableStatBonusByTime>
		{
			// Token: 0x17000D90 RID: 3472
			// (get) Token: 0x06004034 RID: 16436 RVA: 0x000BA554 File Offset: 0x000B8754
			public override float iconFillAmount
			{
				get
				{
					if (this._stacks >= this.ability._maxStack)
					{
						return base.iconFillAmount;
					}
					if (this._remainUpdateTime <= 0f)
					{
						return base.iconFillAmount;
					}
					if (!this.ability._visibleIconStack)
					{
						return base.remainTime / this.ability.duration;
					}
					return this._remainUpdateTime / this.ability._updateInterval;
				}
			}

			// Token: 0x17000D91 RID: 3473
			// (get) Token: 0x06004035 RID: 16437 RVA: 0x000BA5C1 File Offset: 0x000B87C1
			public override int iconStacks
			{
				get
				{
					if (!this.ability._visibleIconStack)
					{
						return 0;
					}
					return (int)((float)this._stacks * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x06004036 RID: 16438 RVA: 0x000BA5E6 File Offset: 0x000B87E6
			public Instance(Character owner, StackableStatBonusByTime ability) : base(owner, ability)
			{
			}

			// Token: 0x06004037 RID: 16439 RVA: 0x000BA5F0 File Offset: 0x000B87F0
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this._stacks = this.ability._startStack;
				this._remainUpdateTime = this.ability._updateInterval;
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStack();
			}

			// Token: 0x06004038 RID: 16440 RVA: 0x000BA651 File Offset: 0x000B8851
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004039 RID: 16441 RVA: 0x000BA669 File Offset: 0x000B8869
			public override void Refresh()
			{
				base.Refresh();
				this._stacks = this.ability._startStack;
				this.UpdateStack();
				this._remainUpdateTime = this.ability._updateInterval;
			}

			// Token: 0x0600403A RID: 16442 RVA: 0x000BA69C File Offset: 0x000B889C
			private void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this._stacks);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600403B RID: 16443 RVA: 0x000BA704 File Offset: 0x000B8904
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this.ability._positive && this._stacks >= this.ability._maxStack)
				{
					return;
				}
				if (!this.ability._positive && this._stacks <= 0)
				{
					return;
				}
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime += this.ability._updateInterval;
					this._stacks += (this.ability._positive ? 1 : -1);
					this.UpdateStack();
				}
			}

			// Token: 0x04003162 RID: 12642
			private float _remainUpdateTime;

			// Token: 0x04003163 RID: 12643
			private int _stacks;

			// Token: 0x04003164 RID: 12644
			private Stat.Values _stat;
		}
	}
}
