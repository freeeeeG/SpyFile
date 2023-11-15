using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B1C RID: 2844
	[Serializable]
	public class OnAction : Trigger
	{
		// Token: 0x060039BD RID: 14781 RVA: 0x000AA8C2 File Offset: 0x000A8AC2
		public OnAction()
		{
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x000AA8CA File Offset: 0x000A8ACA
		public OnAction(OnAction.Timing timing, ActionTypeBoolArray types)
		{
			this._timing = timing;
			this._types = types;
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x000AA8E0 File Offset: 0x000A8AE0
		public override void Attach(Character character)
		{
			this._character = character;
			this._count = 0;
			if (this._timing == OnAction.Timing.Start)
			{
				this._character.onStartAction += this.OnCharacterAction;
				return;
			}
			if (this._timing == OnAction.Timing.End)
			{
				this._character.onCancelAction += this.OnCharacterAction;
				this._character.onEndAction += this.OnCharacterAction;
			}
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x000AA954 File Offset: 0x000A8B54
		public override void Detach()
		{
			if (this._timing == OnAction.Timing.Start)
			{
				this._character.onStartAction -= this.OnCharacterAction;
				return;
			}
			if (this._timing == OnAction.Timing.End)
			{
				this._character.onCancelAction -= this.OnCharacterAction;
				this._character.onEndAction -= this.OnCharacterAction;
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x000AA9B8 File Offset: 0x000A8BB8
		private void OnCharacterAction(Characters.Actions.Action action)
		{
			if (!this._types.GetOrDefault(action.type))
			{
				return;
			}
			if (action.type == Characters.Actions.Action.Type.Skill && action.cooldown.usedByStreak)
			{
				return;
			}
			this._count++;
			if (this._count < this._cycle)
			{
				return;
			}
			this._count = 0;
			base.Invoke();
		}

		// Token: 0x04002DCE RID: 11726
		[SerializeField]
		private OnAction.Timing _timing;

		// Token: 0x04002DCF RID: 11727
		[SerializeField]
		private ActionTypeBoolArray _types;

		// Token: 0x04002DD0 RID: 11728
		[SerializeField]
		private int _cycle;

		// Token: 0x04002DD1 RID: 11729
		private int _count;

		// Token: 0x04002DD2 RID: 11730
		private Character _character;

		// Token: 0x02000B1D RID: 2845
		public enum Timing
		{
			// Token: 0x04002DD4 RID: 11732
			Start,
			// Token: 0x04002DD5 RID: 11733
			End
		}
	}
}
