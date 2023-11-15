using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000169 RID: 361
	public class AddHPToDamageMod : IDamageModifier
	{
		// Token: 0x06000923 RID: 2339 RVA: 0x00025EF9 File Offset: 0x000240F9
		public ValueModifier GetMod()
		{
			return new AddValueModifier(1, (float)PlayerController.Instance.playerHealth.hp * this.multiplier);
		}

		// Token: 0x040006AD RID: 1709
		[SerializeField]
		private float multiplier;
	}
}
