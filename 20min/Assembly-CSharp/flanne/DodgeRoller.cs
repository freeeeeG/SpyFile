using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F6 RID: 246
	public class DodgeRoller
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x0001F2DB File Offset: 0x0001D4DB
		public DodgeRoller(PlayerController p)
		{
			this.player = p;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
		public bool Roll()
		{
			float num = this.player.stats[StatType.Dodge].Modify(1f) - 1f;
			num = num.NotifyModifiers(DodgeRoller.TweakDodgeNotification, this.player);
			float max = this.player.stats[StatType.DodgeCapMod].Modify(60f) / 100f;
			num = Mathf.Clamp(num, 0f, max);
			bool result;
			if (num != 0f && (float)this._consecutiveHitCtr >= 1f / num)
			{
				result = true;
				this._consecutiveHitCtr = 1;
			}
			else
			{
				result = (Random.Range(0f, 1f) < num);
				this._consecutiveHitCtr++;
			}
			return result;
		}

		// Token: 0x040004DE RID: 1246
		public static string TweakDodgeNotification = "DodgeRoller.TweakDodgeNotification";

		// Token: 0x040004DF RID: 1247
		private PlayerController player;

		// Token: 0x040004E0 RID: 1248
		private int _consecutiveHitCtr = 1;
	}
}
