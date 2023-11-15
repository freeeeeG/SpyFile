using System;
using System.Collections;
using System.Collections.Generic;

namespace Characters.AI.Hero
{
	// Token: 0x02001272 RID: 4722
	public class ComboSystem
	{
		// Token: 0x06005DA2 RID: 23970 RVA: 0x0011376E File Offset: 0x0011196E
		public ComboSystem()
		{
			this._combos = new List<IComboable>();
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x0011379F File Offset: 0x0011199F
		public ComboSystem AddComboPattern(IComboable comboable)
		{
			this._combos.Add(comboable);
			return this;
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x001137AE File Offset: 0x001119AE
		public bool TryComboAttack()
		{
			return this._currentCount < this._maxCount;
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x001137C1 File Offset: 0x001119C1
		public IEnumerator CNext(AIController controller)
		{
			List<IComboable> combos = this._combos;
			int currentCount = this._currentCount;
			this._currentCount = currentCount + 1;
			yield return combos[currentCount].CTryContinuedCombo(controller, this);
			yield break;
		}

		// Token: 0x06005DA6 RID: 23974 RVA: 0x001137D7 File Offset: 0x001119D7
		public void Start()
		{
			this._combos.Shuffle<IComboable>();
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x001137E4 File Offset: 0x001119E4
		public void Clear()
		{
			this._currentCount = 0;
		}

		// Token: 0x04004B2A RID: 19242
		private int _maxCount = 2;

		// Token: 0x04004B2B RID: 19243
		private int _currentCount;

		// Token: 0x04004B2C RID: 19244
		private List<IComboable> _combos;

		// Token: 0x04004B2D RID: 19245
		private int[] _comboChances = new int[]
		{
			60,
			30,
			10,
			1,
			0
		};
	}
}
