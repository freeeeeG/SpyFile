using System;

namespace flanne
{
	// Token: 0x02000108 RID: 264
	[Serializable]
	public class SaveData
	{
		// Token: 0x04000559 RID: 1369
		public int points;

		// Token: 0x0400055A RID: 1370
		public UnlockData characterUnlocks;

		// Token: 0x0400055B RID: 1371
		public UnlockData gunUnlocks;

		// Token: 0x0400055C RID: 1372
		public TieredUnlockData runeUnlocks;

		// Token: 0x0400055D RID: 1373
		public int[] swordRuneSelections;

		// Token: 0x0400055E RID: 1374
		public int[] shieldRuneSelections;

		// Token: 0x0400055F RID: 1375
		public int difficultyUnlocked;

		// Token: 0x04000560 RID: 1376
		public int[] characterHighestClear;

		// Token: 0x04000561 RID: 1377
		public int[] gunHighestClear;

		// Token: 0x04000562 RID: 1378
		public bool playedGame;

		// Token: 0x04000563 RID: 1379
		public bool checkedRunes;
	}
}
