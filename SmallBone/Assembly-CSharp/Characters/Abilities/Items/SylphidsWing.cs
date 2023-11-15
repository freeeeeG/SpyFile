using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000D01 RID: 3329
	[Serializable]
	public sealed class SylphidsWing : Ability
	{
		// Token: 0x0600432F RID: 17199 RVA: 0x000C3E8F File Offset: 0x000C208F
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x000C3EA2 File Offset: 0x000C20A2
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SylphidsWing.Instance(owner, this);
		}

		// Token: 0x04003362 RID: 13154
		[SerializeField]
		private int _cycle;

		// Token: 0x04003363 RID: 13155
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x02000D02 RID: 3330
		public sealed class Instance : AbilityInstance<SylphidsWing>
		{
			// Token: 0x17000DF2 RID: 3570
			// (get) Token: 0x06004332 RID: 17202 RVA: 0x000C3EAB File Offset: 0x000C20AB
			public override Sprite icon
			{
				get
				{
					if (this._stack >= 1)
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x17000DF3 RID: 3571
			// (get) Token: 0x06004333 RID: 17203 RVA: 0x000C3EBE File Offset: 0x000C20BE
			public override float iconFillAmount
			{
				get
				{
					return (float)(this.owner.movement.isGrounded ? 1 : 0);
				}
			}

			// Token: 0x17000DF4 RID: 3572
			// (get) Token: 0x06004334 RID: 17204 RVA: 0x000C3ED7 File Offset: 0x000C20D7
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x06004335 RID: 17205 RVA: 0x000C3EDF File Offset: 0x000C20DF
			public Instance(Character owner, SylphidsWing ability) : base(owner, ability)
			{
			}

			// Token: 0x06004336 RID: 17206 RVA: 0x000C3EE9 File Offset: 0x000C20E9
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x06004337 RID: 17207 RVA: 0x000C3F04 File Offset: 0x000C2104
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (this.owner.movement.isGrounded)
				{
					return;
				}
				if (action.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this._stack++;
				if (this._stack >= this.ability._cycle)
				{
					this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
					this._stack = 0;
				}
			}

			// Token: 0x06004338 RID: 17208 RVA: 0x000C3F77 File Offset: 0x000C2177
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x04003364 RID: 13156
			private int _stack;
		}
	}
}
