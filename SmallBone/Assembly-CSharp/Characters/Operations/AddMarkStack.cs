using System;
using Characters.Marks;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DA8 RID: 3496
	public class AddMarkStack : CharacterOperation
	{
		// Token: 0x06004667 RID: 18023 RVA: 0x000CB645 File Offset: 0x000C9845
		public override void Run(Character target)
		{
			if (!MMMaths.PercentChance(this._chance))
			{
				return;
			}
			target.mark.AddStack(this._mark, this._count);
		}

		// Token: 0x0400354E RID: 13646
		[SerializeField]
		private MarkInfo _mark;

		// Token: 0x0400354F RID: 13647
		[SerializeField]
		[Range(1f, 100f)]
		private int _chance = 100;

		// Token: 0x04003550 RID: 13648
		[SerializeField]
		private float _count = 1f;
	}
}
