using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A5A RID: 2650
	[Serializable]
	public class ModifyActionSpeed : Ability
	{
		// Token: 0x06003768 RID: 14184 RVA: 0x000A34B8 File Offset: 0x000A16B8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyActionSpeed.Instance(owner, this);
		}

		// Token: 0x04002C13 RID: 11283
		[SerializeField]
		private int _count;

		// Token: 0x04002C14 RID: 11284
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04002C15 RID: 11285
		[SerializeField]
		private float _extraSpeedMultiplier;

		// Token: 0x02000A5B RID: 2651
		public class Instance : AbilityInstance<ModifyActionSpeed>
		{
			// Token: 0x17000BB0 RID: 2992
			// (get) Token: 0x0600376A RID: 14186 RVA: 0x000A34C1 File Offset: 0x000A16C1
			public override int iconStacks
			{
				get
				{
					return this._remainCount;
				}
			}

			// Token: 0x0600376B RID: 14187 RVA: 0x000A34C9 File Offset: 0x000A16C9
			internal Instance(Character owner, ModifyActionSpeed ability) : base(owner, ability)
			{
			}

			// Token: 0x0600376C RID: 14188 RVA: 0x000A34D4 File Offset: 0x000A16D4
			protected override void OnAttach()
			{
				Characters.Actions.Action[] actions = this.ability._actions;
				for (int i = 0; i < actions.Length; i++)
				{
					actions[i].extraSpeedMultiplier += this.ability._extraSpeedMultiplier;
				}
				if (this.ability._count > 0)
				{
					this._remainCount = this.ability._count;
					this.owner.onStartAction -= this.HandleOnStartAction;
					this.owner.onStartAction += this.HandleOnStartAction;
				}
			}

			// Token: 0x0600376D RID: 14189 RVA: 0x000A3564 File Offset: 0x000A1764
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type == Characters.Actions.Action.Type.Swap)
				{
					this.owner.ability.Remove(this);
					return;
				}
				bool flag = false;
				Characters.Actions.Action[] actions = this.ability._actions;
				for (int i = 0; i < actions.Length; i++)
				{
					if (actions[i] == action)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
				this._remainCount--;
				if (this._remainCount <= 0)
				{
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x0600376E RID: 14190 RVA: 0x000A35E8 File Offset: 0x000A17E8
			protected override void OnDetach()
			{
				Characters.Actions.Action[] actions = this.ability._actions;
				for (int i = 0; i < actions.Length; i++)
				{
					actions[i].extraSpeedMultiplier -= this.ability._extraSpeedMultiplier;
				}
				if (this.ability._count > 0)
				{
					this.owner.onStartAction -= this.HandleOnStartAction;
				}
			}

			// Token: 0x04002C16 RID: 11286
			private int _remainCount;
		}
	}
}
