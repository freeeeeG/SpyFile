using System;

namespace Characters.Cooldowns
{
	// Token: 0x02000909 RID: 2313
	public static class ICooldownExtension
	{
		// Token: 0x06003177 RID: 12663 RVA: 0x00093B6F File Offset: 0x00091D6F
		public static bool OnStreak(this ICooldown cooldown)
		{
			return cooldown.streak.remains < cooldown.streak.count;
		}
	}
}
