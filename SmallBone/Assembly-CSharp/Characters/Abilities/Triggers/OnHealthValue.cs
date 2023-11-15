using System;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B4C RID: 2892
	[Serializable]
	public class OnHealthValue : Trigger
	{
		// Token: 0x06003A21 RID: 14881 RVA: 0x000ABC00 File Offset: 0x000A9E00
		public override void Attach(Character character)
		{
			if (this._times == 0)
			{
				this._times = int.MaxValue;
			}
			this._remainTimes = this._times;
			this._character = character;
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000ABC28 File Offset: 0x000A9E28
		public override void Refresh()
		{
			base.Refresh();
			this._remainCooldownTime = 0f;
			this._remainTimes = this._times;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00002191 File Offset: 0x00000391
		public override void Detach()
		{
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x000ABC48 File Offset: 0x000A9E48
		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			this._elapsed += deltaTime;
			if (this._remainTimes <= 0 || this._remainCooldownTime > 0f)
			{
				return;
			}
			if (!this._constraint.components.Pass())
			{
				return;
			}
			if (this._elapsed < this._checkInterval)
			{
				return;
			}
			this._elapsed = 0f;
			if (!this.CheckHealthCondition())
			{
				return;
			}
			if (MMMaths.PercentChance(this._possibility))
			{
				this._remainTimes--;
				Action onTriggered = this._onTriggered;
				if (onTriggered != null)
				{
					onTriggered();
				}
			}
			this._remainCooldownTime = base.cooldownTime;
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x000ABCF0 File Offset: 0x000A9EF0
		private bool CheckHealthCondition()
		{
			switch (this._compareType)
			{
			case OnHealthValue.CompareType.GreaterThanOrEqual:
				if (this._healthType == OnHealthValue.HealthType.Constant && this._character.health.currentHealth >= (double)this._amount)
				{
					return true;
				}
				if (this._healthType == OnHealthValue.HealthType.Percent && this._character.health.percent >= (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealthValue.CompareType.LessThan:
				if (this._healthType == OnHealthValue.HealthType.Constant && this._character.health.currentHealth <= (double)this._amount)
				{
					return true;
				}
				if (this._healthType == OnHealthValue.HealthType.Percent && this._character.health.percent <= (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealthValue.CompareType.Equal:
				if (this._healthType == OnHealthValue.HealthType.Constant && this._character.health.currentHealth >= (double)this._amount && this._character.health.currentHealth < (double)(this._amount + 1))
				{
					return true;
				}
				if (this._healthType == OnHealthValue.HealthType.Percent && this._character.health.percent > (double)(this._amount - 1) * 0.01 && this._character.health.percent < (double)(this._amount + 1) * 0.01)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x04002E2F RID: 11823
		[SerializeField]
		private OnHealthValue.CompareType _compareType;

		// Token: 0x04002E30 RID: 11824
		[SerializeField]
		private OnHealthValue.HealthType _healthType;

		// Token: 0x04002E31 RID: 11825
		[SerializeField]
		private int _amount;

		// Token: 0x04002E32 RID: 11826
		[FrameTime]
		[SerializeField]
		private float _checkInterval;

		// Token: 0x04002E33 RID: 11827
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _constraint;

		// Token: 0x04002E34 RID: 11828
		[SerializeField]
		private int _times;

		// Token: 0x04002E35 RID: 11829
		private int _remainTimes;

		// Token: 0x04002E36 RID: 11830
		private Character _character;

		// Token: 0x04002E37 RID: 11831
		private float _elapsed;

		// Token: 0x02000B4D RID: 2893
		private enum CompareType
		{
			// Token: 0x04002E39 RID: 11833
			GreaterThanOrEqual,
			// Token: 0x04002E3A RID: 11834
			LessThan,
			// Token: 0x04002E3B RID: 11835
			Equal
		}

		// Token: 0x02000B4E RID: 2894
		private enum HealthType
		{
			// Token: 0x04002E3D RID: 11837
			Constant,
			// Token: 0x04002E3E RID: 11838
			Percent
		}
	}
}
