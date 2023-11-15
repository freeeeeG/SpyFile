using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B41 RID: 2881
	[Serializable]
	public class OnGaveEmberDamage : Trigger
	{
		// Token: 0x06003A0D RID: 14861 RVA: 0x000AB639 File Offset: 0x000A9839
		public OnGaveEmberDamage()
		{
			this._remainKillCount = this._killCount;
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000AB670 File Offset: 0x000A9870
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.status.onGaveEmberDamage += this.HandleOnGaveEmberDamage;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x000AB698 File Offset: 0x000A9898
		private void HandleOnGaveEmberDamage(Character attacker, Character target)
		{
			if (!this._characterTypes[target.type])
			{
				return;
			}
			if (this._characterTypes[Character.Type.Boss] && (target.key == Key.FirstHero1 || target.key == Key.FirstHero2 || target.key == Key.Unspecified))
			{
				return;
			}
			bool flag = false;
			if ((this._characterStatusKinds[CharacterStatus.Kind.Wound] && target.status.wounded) || (this._characterStatusKinds[CharacterStatus.Kind.Burn] && target.status.burning) || (this._characterStatusKinds[CharacterStatus.Kind.Freeze] && target.status.freezed) || (this._characterStatusKinds[CharacterStatus.Kind.Poison] && target.status.poisoned) || (this._characterStatusKinds[CharacterStatus.Kind.Stun] && target.status.stuned))
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (this._moveToHitPosition != null)
			{
				this._moveToHitPosition.position = target.transform.position;
			}
			this._remainKillCount--;
			if (this._remainKillCount <= 0)
			{
				this._remainKillCount = this._killCount;
				base.Invoke();
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000AB7C6 File Offset: 0x000A99C6
		public override void Detach()
		{
			this._character.status.onGaveEmberDamage -= this.HandleOnGaveEmberDamage;
		}

		// Token: 0x04002E0E RID: 11790
		[SerializeField]
		private Transform _moveToHitPosition;

		// Token: 0x04002E0F RID: 11791
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(new bool[]
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

		// Token: 0x04002E10 RID: 11792
		[SerializeField]
		private CharacterStatusKindBoolArray _characterStatusKinds;

		// Token: 0x04002E11 RID: 11793
		[SerializeField]
		private int _killCount = 1;

		// Token: 0x04002E12 RID: 11794
		private int _remainKillCount;

		// Token: 0x04002E13 RID: 11795
		private Character _character;
	}
}
