using System;
using Characters.Monsters;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011D3 RID: 4563
	public class MonsterCount : Condition
	{
		// Token: 0x0600599D RID: 22941 RVA: 0x0010A938 File Offset: 0x00108B38
		protected override bool Check(AIController controller)
		{
			int num = this._minionContainer.Count();
			MonsterCount.Comparer compare = this._compare;
			if (compare != MonsterCount.Comparer.GreaterThan)
			{
				return compare == MonsterCount.Comparer.LessThan && num <= this._count;
			}
			return num >= this._count;
		}

		// Token: 0x04004861 RID: 18529
		[SerializeField]
		private MonsterContainer _minionContainer;

		// Token: 0x04004862 RID: 18530
		[SerializeField]
		private MonsterCount.Comparer _compare;

		// Token: 0x04004863 RID: 18531
		[SerializeField]
		[Range(0f, 100f)]
		private int _count;

		// Token: 0x020011D4 RID: 4564
		private enum Comparer
		{
			// Token: 0x04004865 RID: 18533
			GreaterThan,
			// Token: 0x04004866 RID: 18534
			LessThan
		}
	}
}
