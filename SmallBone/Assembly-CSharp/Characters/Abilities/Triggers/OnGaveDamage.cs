using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B3D RID: 2877
	[Serializable]
	public class OnGaveDamage : Trigger
	{
		// Token: 0x06003A02 RID: 14850 RVA: 0x000AB233 File Offset: 0x000A9433
		public override void Attach(Character character)
		{
			this._character = character;
			Character character2 = this._character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(this.OnCharacterGaveDamage));
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x000AB263 File Offset: 0x000A9463
		public override void Detach()
		{
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(this.OnCharacterGaveDamage));
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x000AB28C File Offset: 0x000A948C
		private void OnCharacterGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (target.character == null)
			{
				return;
			}
			Damage damage = tookDamage;
			if (damage.amount < this._minDamage)
			{
				return;
			}
			damage = tookDamage;
			if (damage.amount < target.character.health.maximumHealth * this._minDamagePercent)
			{
				return;
			}
			if (this._needCritical && !tookDamage.critical)
			{
				return;
			}
			if (!this._attackTypes[tookDamage.motionType])
			{
				return;
			}
			if (!this._damageTypes[tookDamage.attackType])
			{
				return;
			}
			if (!this._targetType[target.character.type])
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(this._attackKey) && !tookDamage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			if (this._backOnly)
			{
				if (this._character.movement.config.type == Movement.Config.Type.Static)
				{
					return;
				}
				Vector3 position = target.transform.position;
				Vector3 position2 = this._character.transform.position;
				if ((target.character.lookingDirection == Character.LookingDirection.Right && position.x < position2.x) || (target.character.lookingDirection == Character.LookingDirection.Left && position.x > position2.x))
				{
					return;
				}
			}
			if (this._moveToHitPosition != null)
			{
				this._moveToHitPosition.position = tookDamage.hitPoint;
			}
			base.Invoke();
		}

		// Token: 0x04002DFB RID: 11771
		[SerializeField]
		private double _minDamage = 1.0;

		// Token: 0x04002DFC RID: 11772
		[Range(0f, 1f)]
		[SerializeField]
		private double _minDamagePercent;

		// Token: 0x04002DFD RID: 11773
		[SerializeField]
		private Transform _moveToHitPosition;

		// Token: 0x04002DFE RID: 11774
		[Header("Filter")]
		[SerializeField]
		private bool _needCritical;

		// Token: 0x04002DFF RID: 11775
		[SerializeField]
		private bool _backOnly;

		// Token: 0x04002E00 RID: 11776
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002E01 RID: 11777
		[SerializeField]
		private CharacterTypeBoolArray _targetType = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x04002E02 RID: 11778
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002E03 RID: 11779
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002E04 RID: 11780
		private Character _character;
	}
}
