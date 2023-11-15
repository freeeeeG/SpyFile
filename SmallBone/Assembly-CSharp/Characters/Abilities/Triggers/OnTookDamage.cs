using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B5E RID: 2910
	[Serializable]
	public class OnTookDamage : Trigger
	{
		// Token: 0x06003A4D RID: 14925 RVA: 0x000AC638 File Offset: 0x000AA838
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.health.onTakeDamage.Add(-2147483647, new TakeDamageDelegate(this.OnCharacterTakeDamage));
			this._character.health.onTookDamage += new TookDamageDelegate(this.OnCharacterTookDamage);
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x000AC68E File Offset: 0x000AA88E
		public override void Detach()
		{
			this._character.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnCharacterTakeDamage));
			this._character.health.onTookDamage -= new TookDamageDelegate(this.OnCharacterTookDamage);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x000AC6D0 File Offset: 0x000AA8D0
		private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if ((this._onCritical && !tookDamage.critical) || !this._attackTypes[tookDamage.motionType] || !this._damageTypes[tookDamage.attackType])
			{
				return;
			}
			if (this._character.invulnerable.value || tookDamage.@null)
			{
				if (!this._onInvulnerable)
				{
					return;
				}
			}
			else
			{
				Damage damage = tookDamage;
				if (damage.amount < this._minDamage || (this._hasShield && !this._character.health.shield.hasAny) || this._beforeHealth - this._character.health.currentHealth < this._minHealthChanged)
				{
					return;
				}
			}
			base.Invoke();
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x000AC792 File Offset: 0x000AA992
		private bool OnCharacterTakeDamage(ref Damage damage)
		{
			this._beforeHealth = this._character.health.currentHealth;
			return false;
		}

		// Token: 0x04002E5E RID: 11870
		[SerializeField]
		private bool _onInvulnerable;

		// Token: 0x04002E5F RID: 11871
		[SerializeField]
		private double _minDamage = 1.0;

		// Token: 0x04002E60 RID: 11872
		[SerializeField]
		private double _minHealthChanged;

		// Token: 0x04002E61 RID: 11873
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002E62 RID: 11874
		[SerializeField]
		private bool _hasShield;

		// Token: 0x04002E63 RID: 11875
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002E64 RID: 11876
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002E65 RID: 11877
		private Character _character;

		// Token: 0x04002E66 RID: 11878
		private double _beforeHealth;
	}
}
