using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D32 RID: 3378
	public class CycleNumber : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A55 RID: 27221 RVA: 0x00298426 File Offset: 0x00296626
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, this.cycleNumber);
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x00298442 File Offset: 0x00296642
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, this.cycleNumber);
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x0029845E File Offset: 0x0029665E
		public CycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x0029846D File Offset: 0x0029666D
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x00298486 File Offset: 0x00296686
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x00298494 File Offset: 0x00296694
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, complete ? this.cycleNumber : (GameClock.Instance.GetCycle() + 1), this.cycleNumber);
		}

		// Token: 0x04004D95 RID: 19861
		private int cycleNumber;
	}
}
