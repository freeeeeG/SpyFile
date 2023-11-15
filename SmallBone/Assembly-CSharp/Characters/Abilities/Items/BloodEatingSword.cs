using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000C97 RID: 3223
	[Serializable]
	public class BloodEatingSword : Ability
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x000BEAA3 File Offset: 0x000BCCA3
		// (set) Token: 0x06004190 RID: 16784 RVA: 0x000BEAAB File Offset: 0x000BCCAB
		public BloodEatingSwordComponent component { get; set; }

		// Token: 0x06004191 RID: 16785 RVA: 0x000BEAB4 File Offset: 0x000BCCB4
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x000BEAC7 File Offset: 0x000BCCC7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BloodEatingSword.Instance(owner, this);
		}

		// Token: 0x04003241 RID: 12865
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x04003242 RID: 12866
		[SerializeField]
		private float _maxStack = 666f;

		// Token: 0x04003243 RID: 12867
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false
		});

		// Token: 0x04003244 RID: 12868
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000C98 RID: 3224
		public class Instance : AbilityInstance<BloodEatingSword>
		{
			// Token: 0x17000DC3 RID: 3523
			// (get) Token: 0x06004194 RID: 16788 RVA: 0x000BEAFF File Offset: 0x000BCCFF
			public override int iconStacks
			{
				get
				{
					return this.ability.component.currentStack;
				}
			}

			// Token: 0x17000DC4 RID: 3524
			// (get) Token: 0x06004195 RID: 16789 RVA: 0x00071719 File Offset: 0x0006F919
			public override float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x06004196 RID: 16790 RVA: 0x000BEB11 File Offset: 0x000BCD11
			public Instance(Character owner, BloodEatingSword ability) : base(owner, ability)
			{
			}

			// Token: 0x06004197 RID: 16791 RVA: 0x000BEB1C File Offset: 0x000BCD1C
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.owner.status.Register(CharacterStatus.Kind.Wound, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.OnApplyBleed));
			}

			// Token: 0x06004198 RID: 16792 RVA: 0x000BEB73 File Offset: 0x000BCD73
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.status.Unregister(CharacterStatus.Kind.Wound, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.OnApplyBleed));
			}

			// Token: 0x06004199 RID: 16793 RVA: 0x000BEBAC File Offset: 0x000BCDAC
			private void OnApplyBleed(Character attacker, Character target)
			{
				if (target == null)
				{
					return;
				}
				if (!this.ability._characterTypeFilter[target.type])
				{
					return;
				}
				BloodEatingSwordComponent component = this.ability.component;
				int currentStack = component.currentStack;
				component.currentStack = currentStack + 1;
				this.UpdateStack();
				if ((float)this.ability.component.currentStack < this.ability._maxStack)
				{
					return;
				}
				if (this.changed)
				{
					return;
				}
				this.changed = true;
				this.ability._operations.Run(this.owner);
			}

			// Token: 0x0600419A RID: 16794 RVA: 0x000BEC44 File Offset: 0x000BCE44
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability.component.currentStack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003245 RID: 12869
			private bool changed;

			// Token: 0x04003246 RID: 12870
			private Stat.Values _stat;
		}
	}
}
