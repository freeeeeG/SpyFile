using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016C RID: 364
	public class MultiplierDamageMod : IDamageModifier
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x00025F95 File Offset: 0x00024195
		public ValueModifier GetMod()
		{
			return new MultValueModifier(0, this.multiplier);
		}

		// Token: 0x040006B2 RID: 1714
		[SerializeField]
		private float multiplier = 1f;
	}
}
