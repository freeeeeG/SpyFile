using System;

namespace Characters.Cooldowns.Streaks
{
	// Token: 0x0200090E RID: 2318
	public interface IStreak
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060031A2 RID: 12706
		// (set) Token: 0x060031A3 RID: 12707
		int count { get; set; }

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060031A4 RID: 12708
		// (set) Token: 0x060031A5 RID: 12709
		float timeout { get; set; }

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060031A6 RID: 12710
		int remains { get; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060031A7 RID: 12711
		float remainPercent { get; }

		// Token: 0x060031A8 RID: 12712
		bool Consume();

		// Token: 0x060031A9 RID: 12713
		void Start();

		// Token: 0x060031AA RID: 12714
		void Expire();
	}
}
