using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D4E RID: 3406
	[Serializable]
	public class ForbiddenSword : Ability
	{
		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x060044AE RID: 17582 RVA: 0x000C787B File Offset: 0x000C5A7B
		// (set) Token: 0x060044AF RID: 17583 RVA: 0x000C7883 File Offset: 0x000C5A83
		public ForbiddenSwordComponent component { get; set; }

		// Token: 0x060044B0 RID: 17584 RVA: 0x000C788C File Offset: 0x000C5A8C
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x000C789F File Offset: 0x000C5A9F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ForbiddenSword.Instance(owner, this);
		}

		// Token: 0x04003460 RID: 13408
		[SerializeField]
		private float _killCount = 666f;

		// Token: 0x04003461 RID: 13409
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

		// Token: 0x04003462 RID: 13410
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D4F RID: 3407
		public class Instance : AbilityInstance<ForbiddenSword>
		{
			// Token: 0x17000E45 RID: 3653
			// (get) Token: 0x060044B3 RID: 17587 RVA: 0x000C78D7 File Offset: 0x000C5AD7
			public override int iconStacks
			{
				get
				{
					return this.ability.component.currentKillCount;
				}
			}

			// Token: 0x17000E46 RID: 3654
			// (get) Token: 0x060044B4 RID: 17588 RVA: 0x00071719 File Offset: 0x0006F919
			public override float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x060044B5 RID: 17589 RVA: 0x000C78E9 File Offset: 0x000C5AE9
			public Instance(Character owner, ForbiddenSword ability) : base(owner, ability)
			{
			}

			// Token: 0x060044B6 RID: 17590 RVA: 0x000C78F3 File Offset: 0x000C5AF3
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
			}

			// Token: 0x060044B7 RID: 17591 RVA: 0x000C791C File Offset: 0x000C5B1C
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
			}

			// Token: 0x060044B8 RID: 17592 RVA: 0x000C7948 File Offset: 0x000C5B48
			private void OnOwnerKilled(ITarget target, ref Damage damage)
			{
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._characterTypeFilter[target.character.type])
				{
					return;
				}
				ForbiddenSwordComponent component = this.ability.component;
				int currentKillCount = component.currentKillCount;
				component.currentKillCount = currentKillCount + 1;
				if ((float)this.ability.component.currentKillCount < this.ability._killCount)
				{
					return;
				}
				if (this._changed)
				{
					return;
				}
				this._changed = true;
				this.ability._operations.Run(this.owner);
			}

			// Token: 0x04003463 RID: 13411
			private bool _changed;
		}
	}
}
