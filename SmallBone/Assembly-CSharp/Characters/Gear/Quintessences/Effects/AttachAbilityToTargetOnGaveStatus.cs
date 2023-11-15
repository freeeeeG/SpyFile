using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008EB RID: 2283
	public sealed class AttachAbilityToTargetOnGaveStatus : QuintessenceEffect
	{
		// Token: 0x060030CE RID: 12494 RVA: 0x000920D4 File Offset: 0x000902D4
		private void Awake()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000920E4 File Offset: 0x000902E4
		protected override void OnInvoke(Quintessence quintessence)
		{
			Character owner = quintessence.owner;
			this._quintessence = quintessence;
			Character character = owner;
			character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			if (owner != this._owner)
			{
				quintessence.onDiscard += this.Detach;
			}
			if (this._character != null)
			{
				Character character2 = this._character;
				character2.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character2.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}
			this._owner = owner;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x00092180 File Offset: 0x00090380
		private void Detach(Gear gear)
		{
			if (this._character != null)
			{
				Character character = this._character;
				character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}
			Character owner = this._owner;
			owner.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(owner.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			this._quintessence.onDiscard -= this.Detach;
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x00092200 File Offset: 0x00090400
		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			if (!this._statusKind[applyInfo.kind])
			{
				return;
			}
			if (!result)
			{
				return;
			}
			target.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0400283F RID: 10303
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002840 RID: 10304
		[SerializeField]
		private CharacterStatusKindBoolArray _statusKind;

		// Token: 0x04002841 RID: 10305
		[SerializeField]
		private Character _character;

		// Token: 0x04002842 RID: 10306
		private Quintessence _quintessence;

		// Token: 0x04002843 RID: 10307
		private Character _owner;
	}
}
