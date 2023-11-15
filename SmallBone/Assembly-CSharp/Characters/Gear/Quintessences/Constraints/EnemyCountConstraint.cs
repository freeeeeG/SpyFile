using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints
{
	// Token: 0x020008F7 RID: 2295
	public sealed class EnemyCountConstraint : Constraint
	{
		// Token: 0x060030FF RID: 12543 RVA: 0x00092784 File Offset: 0x00090984
		public override bool Pass()
		{
			int num = 0;
			using (IEnumerator<Character> enumerator = Map.Instance.waveContainer.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.gameObject.activeInHierarchy)
					{
						num++;
					}
				}
			}
			switch (this._compare)
			{
			case EnemyCountConstraint.Compare.Greater:
				return num > this._count;
			case EnemyCountConstraint.Compare.Lower:
				return num < this._count;
			case EnemyCountConstraint.Compare.Equal:
				return num == this._count;
			default:
				return true;
			}
		}

		// Token: 0x0400284F RID: 10319
		[SerializeField]
		private EnemyCountConstraint.Compare _compare;

		// Token: 0x04002850 RID: 10320
		[SerializeField]
		private int _count;

		// Token: 0x020008F8 RID: 2296
		private enum Compare
		{
			// Token: 0x04002852 RID: 10322
			Greater,
			// Token: 0x04002853 RID: 10323
			Lower,
			// Token: 0x04002854 RID: 10324
			Equal
		}
	}
}
