using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016B RID: 363
	public class AdditiveDamageMod : IDamageModifier
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x00025F86 File Offset: 0x00024186
		public ValueModifier GetMod()
		{
			return new AddValueModifier(1, (float)this.addDamage);
		}

		// Token: 0x040006B1 RID: 1713
		[SerializeField]
		private int addDamage;
	}
}
