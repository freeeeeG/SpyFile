using System;
using Characters.Cooldowns.Streaks;

namespace Characters.Cooldowns
{
	// Token: 0x02000908 RID: 2312
	public interface ICooldown
	{
		// Token: 0x14000088 RID: 136
		// (add) Token: 0x0600316E RID: 12654
		// (remove) Token: 0x0600316F RID: 12655
		event Action onReady;

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003170 RID: 12656
		int maxStack { get; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003171 RID: 12657
		// (set) Token: 0x06003172 RID: 12658
		int stacks { get; set; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003173 RID: 12659
		bool canUse { get; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003174 RID: 12660
		float remainPercent { get; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003175 RID: 12661
		IStreak streak { get; }

		// Token: 0x06003176 RID: 12662
		bool Consume();
	}
}
