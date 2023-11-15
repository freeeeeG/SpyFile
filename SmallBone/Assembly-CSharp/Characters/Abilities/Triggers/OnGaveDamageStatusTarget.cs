using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B3F RID: 2879
	[Serializable]
	public sealed class OnGaveDamageStatusTarget : Trigger
	{
		// Token: 0x06003A07 RID: 14855 RVA: 0x000AB438 File Offset: 0x000A9638
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
			Character character2 = this._character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x000AB494 File Offset: 0x000A9694
		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			this._passPrecondition = false;
			if (target.character.status == null || !target.character.status.IsApplying(this._characterStatusKinds))
			{
				return false;
			}
			this._passPrecondition = true;
			return false;
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x000AB4D4 File Offset: 0x000A96D4
		public override void Detach()
		{
			this._character.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000AB528 File Offset: 0x000A9728
		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!this._passPrecondition)
			{
				return;
			}
			if (!this._characterTypes[target.character.type])
			{
				return;
			}
			if (!this._attackType[gaveDamage.attackType])
			{
				return;
			}
			if (this._onCritical && !gaveDamage.critical)
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(this._attackKey) && !gaveDamage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			if (this._moveToHitPosition != null)
			{
				this._moveToHitPosition.position = gaveDamage.hitPoint;
			}
			if (this._moveToTargetPosition != null)
			{
				this._moveToTargetPosition.position = target.collider.transform.position;
			}
			base.Invoke();
		}

		// Token: 0x04002E05 RID: 11781
		[SerializeField]
		private Transform _moveToHitPosition;

		// Token: 0x04002E06 RID: 11782
		[SerializeField]
		private Transform _moveToTargetPosition;

		// Token: 0x04002E07 RID: 11783
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002E08 RID: 11784
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002E09 RID: 11785
		[SerializeField]
		private AttackTypeBoolArray _attackType = new AttackTypeBoolArray(new bool[]
		{
			false,
			true,
			true,
			true,
			false
		});

		// Token: 0x04002E0A RID: 11786
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

		// Token: 0x04002E0B RID: 11787
		[SerializeField]
		private CharacterStatusKindBoolArray _characterStatusKinds;

		// Token: 0x04002E0C RID: 11788
		private Character _character;

		// Token: 0x04002E0D RID: 11789
		private bool _passPrecondition;
	}
}
