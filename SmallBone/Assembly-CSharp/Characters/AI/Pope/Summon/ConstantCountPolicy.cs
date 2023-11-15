using System;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x02001219 RID: 4633
	public sealed class ConstantCountPolicy : CountPolicy
	{
		// Token: 0x06005ADC RID: 23260 RVA: 0x0010D1CF File Offset: 0x0010B3CF
		public override int GetCount()
		{
			return this._count;
		}

		// Token: 0x0400494E RID: 18766
		[SerializeField]
		private int _count;
	}
}
