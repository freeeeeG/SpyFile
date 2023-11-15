using System;

namespace Characters.Cooldowns.Streaks
{
	// Token: 0x0200090F RID: 2319
	public class NoStreak : IStreak
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x00018EC5 File Offset: 0x000170C5
		// (set) Token: 0x060031AC RID: 12716 RVA: 0x00002191 File Offset: 0x00000391
		public int count
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x060031AE RID: 12718 RVA: 0x00002191 File Offset: 0x00000391
		public float timeout
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int remains
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060031B0 RID: 12720 RVA: 0x00071719 File Offset: 0x0006F919
		public float remainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool onStreak
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool Consume()
		{
			return false;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00002191 File Offset: 0x00000391
		public void Start()
		{
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x00002191 File Offset: 0x00000391
		public void Expire()
		{
		}
	}
}
