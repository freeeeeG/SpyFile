using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016F RID: 367
	public class MultiplyStatToDamage : IDamageModifier
	{
		// Token: 0x0600092F RID: 2351 RVA: 0x00026058 File Offset: 0x00024258
		public ValueModifier GetMod()
		{
			float toMultiply = PlayerController.Instance.stats[this.statType].Modify(1f);
			return new MultValueModifier(1, toMultiply);
		}

		// Token: 0x040006B5 RID: 1717
		[SerializeField]
		private StatType statType;
	}
}
