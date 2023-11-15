using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B48 RID: 2888
	[Serializable]
	public class OnHealthChanged : Trigger
	{
		// Token: 0x06003A1B RID: 14875 RVA: 0x000AB9C2 File Offset: 0x000A9BC2
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.health.onTookDamage += new TookDamageDelegate(this.OnCharacterTookDamage);
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000AB9E7 File Offset: 0x000A9BE7
		public override void Detach()
		{
			this._character.health.onTookDamage -= new TookDamageDelegate(this.OnCharacterTookDamage);
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000ABA08 File Offset: 0x000A9C08
		private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Damage damage = tookDamage;
			if (damage.amount < this._minDamage || (this._onCritical && !tookDamage.critical) || !this._attackTypes[tookDamage.motionType] || !this._damageTypes[tookDamage.attackType])
			{
				return;
			}
			if (!this.CheckHealthCondition())
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x000ABA74 File Offset: 0x000A9C74
		private bool CheckHealthCondition()
		{
			switch (this._compareType)
			{
			case OnHealthChanged.CompareType.GreaterThanOrEqual:
				if (this._healthType == OnHealthChanged.HealthType.Constant && this._character.health.currentHealth >= (double)this._amount)
				{
					return true;
				}
				if (this._healthType == OnHealthChanged.HealthType.Percent && this._character.health.percent >= (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealthChanged.CompareType.LessThan:
				if (this._healthType == OnHealthChanged.HealthType.Constant && this._character.health.currentHealth < (double)this._amount)
				{
					return true;
				}
				if (this._healthType == OnHealthChanged.HealthType.Percent && this._character.health.percent < (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealthChanged.CompareType.Equal:
				if (this._healthType == OnHealthChanged.HealthType.Constant && this._character.health.currentHealth >= (double)this._amount && this._character.health.currentHealth < (double)(this._amount + 1))
				{
					return true;
				}
				if (this._healthType == OnHealthChanged.HealthType.Percent && this._character.health.percent > (double)(this._amount - 1) * 0.01 && this._character.health.percent < (double)(this._amount + 1) * 0.01)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x04002E20 RID: 11808
		[SerializeField]
		[Header("Health")]
		private OnHealthChanged.CompareType _compareType;

		// Token: 0x04002E21 RID: 11809
		[SerializeField]
		private OnHealthChanged.HealthType _healthType;

		// Token: 0x04002E22 RID: 11810
		[SerializeField]
		private int _amount;

		// Token: 0x04002E23 RID: 11811
		[SerializeField]
		[Header("Damage")]
		private double _minDamage = 1.0;

		// Token: 0x04002E24 RID: 11812
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002E25 RID: 11813
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002E26 RID: 11814
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002E27 RID: 11815
		private Character _character;

		// Token: 0x02000B49 RID: 2889
		private enum CompareType
		{
			// Token: 0x04002E29 RID: 11817
			GreaterThanOrEqual,
			// Token: 0x04002E2A RID: 11818
			LessThan,
			// Token: 0x04002E2B RID: 11819
			Equal
		}

		// Token: 0x02000B4A RID: 2890
		private enum HealthType
		{
			// Token: 0x04002E2D RID: 11821
			Constant,
			// Token: 0x04002E2E RID: 11822
			Percent
		}
	}
}
