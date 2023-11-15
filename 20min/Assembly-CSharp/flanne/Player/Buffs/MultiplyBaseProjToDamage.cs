using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016D RID: 365
	public class MultiplyBaseProjToDamage : IDamageModifier
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x00025FB8 File Offset: 0x000241B8
		public ValueModifier GetMod()
		{
			PlayerController instance = PlayerController.Instance;
			float toMultiply = (float)(instance.stats[StatType.Projectiles].FlatBonus + instance.gun.gunData.numOfProjectiles) * this.multiplier;
			return new MultValueModifier(1, toMultiply);
		}

		// Token: 0x040006B3 RID: 1715
		[SerializeField]
		private float multiplier = 1f;
	}
}
