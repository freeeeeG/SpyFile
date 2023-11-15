using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B54 RID: 2900
	[Serializable]
	public class OnKilled : Trigger
	{
		// Token: 0x06003A34 RID: 14900 RVA: 0x000AC031 File Offset: 0x000AA231
		public OnKilled()
		{
			this._remainKillCount = this._killCount;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x000AC068 File Offset: 0x000AA268
		public override void Attach(Character character)
		{
			this._character = character;
			Character character2 = this._character;
			character2.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character2.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x000AC098 File Offset: 0x000AA298
		public override void Detach()
		{
			Character character = this._character;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x000AC0C4 File Offset: 0x000AA2C4
		private void OnCharacterKilled(ITarget target, ref Damage damage)
		{
			if (!this._characterTypes[target.character.type])
			{
				return;
			}
			if (this._characterTypes[Character.Type.Boss] && target.character.type == Character.Type.Boss && (target.character.key == Key.FirstHero1 || target.character.key == Key.FirstHero2 || target.character.key == Key.Unspecified))
			{
				return;
			}
			if (!this._attackTypes[damage.motionType])
			{
				return;
			}
			if (!this._damageTypes[damage.attackType])
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(this._attackKey) && !damage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
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

		// Token: 0x04002E42 RID: 11842
		[SerializeField]
		private Transform _moveToHitPosition;

		// Token: 0x04002E43 RID: 11843
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002E44 RID: 11844
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002E45 RID: 11845
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002E46 RID: 11846
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002E47 RID: 11847
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

		// Token: 0x04002E48 RID: 11848
		[SerializeField]
		private int _killCount = 1;

		// Token: 0x04002E49 RID: 11849
		private int _remainKillCount;

		// Token: 0x04002E4A RID: 11850
		private Character _character;
	}
}
