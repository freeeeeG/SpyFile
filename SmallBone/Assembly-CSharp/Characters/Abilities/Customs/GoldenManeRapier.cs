using System;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D53 RID: 3411
	[Serializable]
	public class GoldenManeRapier : Ability
	{
		// Token: 0x060044D3 RID: 17619 RVA: 0x000C7D25 File Offset: 0x000C5F25
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x000C7D38 File Offset: 0x000C5F38
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GoldenManeRapier.Instance(owner, this);
		}

		// Token: 0x04003472 RID: 13426
		[SerializeField]
		private float _basicAttackCount = 3f;

		// Token: 0x04003473 RID: 13427
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x02000D54 RID: 3412
		public class Instance : AbilityInstance<GoldenManeRapier>
		{
			// Token: 0x17000E51 RID: 3665
			// (get) Token: 0x060044D6 RID: 17622 RVA: 0x000C7D54 File Offset: 0x000C5F54
			public override int iconStacks
			{
				get
				{
					return this._currentBasicAttackCount;
				}
			}

			// Token: 0x060044D7 RID: 17623 RVA: 0x000C7D5C File Offset: 0x000C5F5C
			public Instance(Character owner, GoldenManeRapier ability) : base(owner, ability)
			{
			}

			// Token: 0x060044D8 RID: 17624 RVA: 0x000C7D66 File Offset: 0x000C5F66
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnOwnerStartAction;
			}

			// Token: 0x060044D9 RID: 17625 RVA: 0x000C7D7F File Offset: 0x000C5F7F
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnOwnerStartAction;
			}

			// Token: 0x060044DA RID: 17626 RVA: 0x000B7067 File Offset: 0x000B5267
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
			}

			// Token: 0x060044DB RID: 17627 RVA: 0x000C7D98 File Offset: 0x000C5F98
			private void OnOwnerStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.BasicAttack && action.type != Characters.Actions.Action.Type.JumpAttack)
				{
					return;
				}
				this._currentBasicAttackCount++;
				if ((float)this._currentBasicAttackCount < this.ability._basicAttackCount)
				{
					return;
				}
				this._currentBasicAttackCount = 0;
				this.ability._operations.Run(this.owner);
			}

			// Token: 0x04003474 RID: 13428
			private int _currentBasicAttackCount;
		}
	}
}
