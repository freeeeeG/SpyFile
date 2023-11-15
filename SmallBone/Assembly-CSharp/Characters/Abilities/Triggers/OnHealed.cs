using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B44 RID: 2884
	[Serializable]
	public sealed class OnHealed : Trigger
	{
		// Token: 0x06003A15 RID: 14869 RVA: 0x000AB82F File Offset: 0x000A9A2F
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.health.onHealed += this.HandleOnHealed;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x000AB854 File Offset: 0x000A9A54
		private void HandleOnHealed(double healed, double overHealed)
		{
			if (!this.CheckHealthCondition(healed))
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x000AB866 File Offset: 0x000A9A66
		public override void Detach()
		{
			this._character.health.onHealed -= this.HandleOnHealed;
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000AB884 File Offset: 0x000A9A84
		private bool CheckHealthCondition(double healed)
		{
			switch (this._comparer)
			{
			case OnHealed.Comparer.GreaterThanOrEqual:
				if (this._type == OnHealed.Type.Constant && healed >= (double)this._amount)
				{
					return true;
				}
				if (this._type == OnHealed.Type.Percent && healed / this._character.health.maximumHealth >= (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealed.Comparer.LessThan:
				if (this._type == OnHealed.Type.Constant && healed <= (double)this._amount)
				{
					return true;
				}
				if (this._type == OnHealed.Type.Percent && healed / this._character.health.maximumHealth <= (double)this._amount * 0.01)
				{
					return true;
				}
				break;
			case OnHealed.Comparer.Equal:
				if (this._type == OnHealed.Type.Constant && healed >= (double)this._amount && healed < (double)(this._amount + 1))
				{
					return true;
				}
				if (this._type == OnHealed.Type.Percent && healed / this._character.health.maximumHealth > (double)(this._amount - 1) * 0.01 && healed / this._character.health.maximumHealth < (double)(this._amount + 1) * 0.01)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x04002E15 RID: 11797
		[SerializeField]
		private OnHealed.Type _type;

		// Token: 0x04002E16 RID: 11798
		[SerializeField]
		private OnHealed.Comparer _comparer;

		// Token: 0x04002E17 RID: 11799
		[SerializeField]
		private int _amount;

		// Token: 0x04002E18 RID: 11800
		private Character _character;

		// Token: 0x02000B45 RID: 2885
		private enum Type
		{
			// Token: 0x04002E1A RID: 11802
			Constant,
			// Token: 0x04002E1B RID: 11803
			Percent
		}

		// Token: 0x02000B46 RID: 2886
		private enum Comparer
		{
			// Token: 0x04002E1D RID: 11805
			GreaterThanOrEqual,
			// Token: 0x04002E1E RID: 11806
			LessThan,
			// Token: 0x04002E1F RID: 11807
			Equal
		}
	}
}
