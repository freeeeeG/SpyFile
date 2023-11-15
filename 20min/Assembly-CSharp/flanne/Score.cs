using System;

namespace flanne
{
	// Token: 0x0200010A RID: 266
	public class Score
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00020C32 File Offset: 0x0001EE32
		public int totalScore
		{
			get
			{
				return this.timeSurvivedScore + this.enemiesKilledScore + this.levelsEarnedScore;
			}
		}

		// Token: 0x04000565 RID: 1381
		public string timeSurvivedString;

		// Token: 0x04000566 RID: 1382
		public int timeSurvivedScore;

		// Token: 0x04000567 RID: 1383
		public int enemiesKilled;

		// Token: 0x04000568 RID: 1384
		public int enemiesKilledScore;

		// Token: 0x04000569 RID: 1385
		public int levelsEarned;

		// Token: 0x0400056A RID: 1386
		public int levelsEarnedScore;
	}
}
