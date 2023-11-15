using System;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D77 RID: 3447
	[Serializable]
	public class OffensiveWheel : Ability
	{
		// Token: 0x06004576 RID: 17782 RVA: 0x000C95B5 File Offset: 0x000C77B5
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x000C95C8 File Offset: 0x000C77C8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OffensiveWheel.Instance(owner, this);
		}

		// Token: 0x040034CB RID: 13515
		[SerializeField]
		private float _skillCount = 5f;

		// Token: 0x040034CC RID: 13516
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D78 RID: 3448
		public class Instance : AbilityInstance<OffensiveWheel>
		{
			// Token: 0x17000E73 RID: 3699
			// (get) Token: 0x06004579 RID: 17785 RVA: 0x000C95E4 File Offset: 0x000C77E4
			public override Sprite icon
			{
				get
				{
					if (this._currentSkillCount <= 0)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000E74 RID: 3700
			// (get) Token: 0x0600457A RID: 17786 RVA: 0x000C95F7 File Offset: 0x000C77F7
			public override int iconStacks
			{
				get
				{
					return this._currentSkillCount;
				}
			}

			// Token: 0x17000E75 RID: 3701
			// (get) Token: 0x0600457B RID: 17787 RVA: 0x000C95FF File Offset: 0x000C77FF
			public override float iconFillAmount
			{
				get
				{
					return (float)(((float)this._currentSkillCount == this.ability._skillCount) ? 0 : 1);
				}
			}

			// Token: 0x0600457C RID: 17788 RVA: 0x000C961A File Offset: 0x000C781A
			public Instance(Character owner, OffensiveWheel ability) : base(owner, ability)
			{
			}

			// Token: 0x0600457D RID: 17789 RVA: 0x000C9624 File Offset: 0x000C7824
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnOwnerStartAction;
				this.owner.playerComponents.inventory.weapon.onSwap += this.OnOwnerSwap;
			}

			// Token: 0x0600457E RID: 17790 RVA: 0x000C9663 File Offset: 0x000C7863
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnOwnerStartAction;
				this.owner.playerComponents.inventory.weapon.onSwap -= this.OnOwnerSwap;
			}

			// Token: 0x0600457F RID: 17791 RVA: 0x000B7067 File Offset: 0x000B5267
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
			}

			// Token: 0x06004580 RID: 17792 RVA: 0x000C96A2 File Offset: 0x000C78A2
			private void OnOwnerStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Skill)
				{
					return;
				}
				if (this.ability._skillCount == (float)this._currentSkillCount)
				{
					return;
				}
				this._currentSkillCount++;
			}

			// Token: 0x06004581 RID: 17793 RVA: 0x000C96D1 File Offset: 0x000C78D1
			private void OnOwnerSwap()
			{
				if ((float)this._currentSkillCount < this.ability._skillCount)
				{
					return;
				}
				this._currentSkillCount = 0;
				this.ability._operations.Run(this.owner);
			}

			// Token: 0x040034CD RID: 13517
			private int _currentSkillCount;
		}
	}
}
