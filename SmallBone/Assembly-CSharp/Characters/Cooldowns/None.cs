using System;
using Characters.Cooldowns.Streaks;

namespace Characters.Cooldowns
{
	// Token: 0x0200090A RID: 2314
	public class None : ICooldown
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000076D4 File Offset: 0x000058D4
		public int maxStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int streakCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x00071719 File Offset: 0x0006F919
		public float streakTimeout
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int streakRemains
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x000076D4 File Offset: 0x000058D4
		// (set) Token: 0x0600317D RID: 12669 RVA: 0x00002191 File Offset: 0x00000391
		public int stacks
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x00071719 File Offset: 0x0006F919
		public float remainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x00071719 File Offset: 0x0006F919
		public float streakRemainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06003180 RID: 12672 RVA: 0x00093B8C File Offset: 0x00091D8C
		// (remove) Token: 0x06003181 RID: 12673 RVA: 0x00093BC4 File Offset: 0x00091DC4
		public event Action onReady;

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool canUse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x00093BF9 File Offset: 0x00091DF9
		public IStreak streak
		{
			get
			{
				return this._streak;
			}
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool Consume()
		{
			return true;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x00002191 File Offset: 0x00000391
		public void ExpireStreak()
		{
		}

		// Token: 0x040028A7 RID: 10407
		private readonly NoStreak _streak = new NoStreak();
	}
}
