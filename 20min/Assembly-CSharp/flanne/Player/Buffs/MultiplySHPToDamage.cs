using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016E RID: 366
	public class MultiplySHPToDamage : IDamageModifier
	{
		// Token: 0x0600092D RID: 2349 RVA: 0x00026010 File Offset: 0x00024210
		public ValueModifier GetMod()
		{
			float num = (float)PlayerController.Instance.playerHealth.shp * this.multiplierPerSHP;
			num += 1f;
			return new MultValueModifier(1, num);
		}

		// Token: 0x040006B4 RID: 1716
		[SerializeField]
		private float multiplierPerSHP = 0.1f;
	}
}
