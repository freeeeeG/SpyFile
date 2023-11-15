using System;
using Characters.Cooldowns.Streaks;

namespace Characters.Cooldowns
{
	// Token: 0x02000906 RID: 2310
	public class Custom : ICooldown
	{
		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x0009392B File Offset: 0x00091B2B
		public int maxStack
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int streakCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003154 RID: 12628 RVA: 0x00071719 File Offset: 0x0006F919
		public float streakTimeout
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int streakRemains
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x00093932 File Offset: 0x00091B32
		// (set) Token: 0x06003157 RID: 12631 RVA: 0x0009393A File Offset: 0x00091B3A
		public int stacks { get; set; }

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x00071719 File Offset: 0x0006F919
		public float remainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x00071719 File Offset: 0x0006F919
		public float streakRemainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600315A RID: 12634 RVA: 0x00093944 File Offset: 0x00091B44
		// (remove) Token: 0x0600315B RID: 12635 RVA: 0x0009397C File Offset: 0x00091B7C
		public event Action onReady;

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x000939B1 File Offset: 0x00091BB1
		public bool canUse
		{
			get
			{
				return this.stacks > 0;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x000939BC File Offset: 0x00091BBC
		public IStreak streak
		{
			get
			{
				return this._streak;
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x000939C4 File Offset: 0x00091BC4
		public bool Consume()
		{
			if (this.stacks == 0)
			{
				return false;
			}
			int stacks = this.stacks;
			this.stacks = stacks - 1;
			return true;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00002191 File Offset: 0x00000391
		public void ExpireStreak()
		{
		}

		// Token: 0x040028A0 RID: 10400
		private readonly NoStreak _streak = new NoStreak();
	}
}
