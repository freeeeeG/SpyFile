using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B5A RID: 2906
	[Serializable]
	public class OnStatusTargetKilled : Trigger
	{
		// Token: 0x06003A43 RID: 14915 RVA: 0x000AC378 File Offset: 0x000AA578
		public OnStatusTargetKilled()
		{
			this._remainKillCount = this._killCount;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000AC3AF File Offset: 0x000AA5AF
		public override void Attach(Character character)
		{
			this._character = character;
			Character character2 = this._character;
			character2.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character2.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000AC3DF File Offset: 0x000AA5DF
		public override void Detach()
		{
			Character character = this._character;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000AC408 File Offset: 0x000AA608
		private void OnCharacterKilled(ITarget target, ref Damage damage)
		{
			if (!this._characterTypes[target.character.type])
			{
				return;
			}
			if (this._characterTypes[Character.Type.Boss] && (target.character.key == Key.FirstHero1 || target.character.key == Key.FirstHero2 || target.character.key == Key.Unspecified))
			{
				return;
			}
			if (this._onCritical && !damage.critical)
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(this._attackKey) && !damage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			bool flag = false;
			if ((this._characterStatusKinds[CharacterStatus.Kind.Wound] && target.character.status.wounded) || (this._characterStatusKinds[CharacterStatus.Kind.Burn] && target.character.status.burning) || (this._characterStatusKinds[CharacterStatus.Kind.Freeze] && target.character.status.freezed) || (this._characterStatusKinds[CharacterStatus.Kind.Poison] && target.character.status.poisoned) || (this._characterStatusKinds[CharacterStatus.Kind.Stun] && target.character.status.stuned))
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (this._moveToHitPosition != null)
			{
				this._moveToHitPosition.position = damage.hitPoint;
			}
			this._remainKillCount--;
			if (this._remainKillCount <= 0)
			{
				this._remainKillCount = this._killCount;
				base.Invoke();
			}
		}

		// Token: 0x04002E54 RID: 11860
		[SerializeField]
		private Transform _moveToHitPosition;

		// Token: 0x04002E55 RID: 11861
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002E56 RID: 11862
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002E57 RID: 11863
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

		// Token: 0x04002E58 RID: 11864
		[SerializeField]
		private CharacterStatusKindBoolArray _characterStatusKinds;

		// Token: 0x04002E59 RID: 11865
		[SerializeField]
		private int _killCount = 1;

		// Token: 0x04002E5A RID: 11866
		private int _remainKillCount;

		// Token: 0x04002E5B RID: 11867
		private Character _character;
	}
}
