using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B28 RID: 2856
	[Serializable]
	public class OnChargeAction : Trigger
	{
		// Token: 0x060039D5 RID: 14805 RVA: 0x000AA8C2 File Offset: 0x000A8AC2
		public OnChargeAction()
		{
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000AAC9B File Offset: 0x000A8E9B
		public OnChargeAction(ActionTypeBoolArray types)
		{
			this._types = types;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x000AACAC File Offset: 0x000A8EAC
		public override void Attach(Character character)
		{
			this._character = character;
			if (this._timing == OnChargeAction.Timing.Start)
			{
				Character character2 = this._character;
				character2.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character2.onStartCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
				return;
			}
			if (this._timing == OnChargeAction.Timing.End)
			{
				Character character3 = this._character;
				character3.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character3.onCancelCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
				Character character4 = this._character;
				character4.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Combine(character4.onStopCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
			}
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x000AAD48 File Offset: 0x000A8F48
		public override void Detach()
		{
			if (this._timing == OnChargeAction.Timing.Start)
			{
				Character character = this._character;
				character.onStartCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character.onStartCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
				return;
			}
			if (this._timing == OnChargeAction.Timing.End)
			{
				Character character2 = this._character;
				character2.onCancelCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character2.onCancelCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
				Character character3 = this._character;
				character3.onStopCharging = (Action<Characters.Actions.Action>)Delegate.Remove(character3.onStopCharging, new Action<Characters.Actions.Action>(this.OnCharacterCharging));
			}
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x000AADDC File Offset: 0x000A8FDC
		private void OnCharacterCharging(Characters.Actions.Action action)
		{
			if (!this._types.GetOrDefault(action.type))
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x04002DE1 RID: 11745
		[SerializeField]
		private OnChargeAction.Timing _timing;

		// Token: 0x04002DE2 RID: 11746
		[SerializeField]
		private ActionTypeBoolArray _types;

		// Token: 0x04002DE3 RID: 11747
		private Character _character;

		// Token: 0x02000B29 RID: 2857
		public enum Timing
		{
			// Token: 0x04002DE5 RID: 11749
			Start,
			// Token: 0x04002DE6 RID: 11750
			End
		}
	}
}
