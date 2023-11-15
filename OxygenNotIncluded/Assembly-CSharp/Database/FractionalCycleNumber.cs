using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D34 RID: 3380
	public class FractionalCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A60 RID: 27232 RVA: 0x00298545 File Offset: 0x00296745
		public FractionalCycleNumber(float fractionalCycleNumber)
		{
			this.fractionalCycleNumber = fractionalCycleNumber;
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x00298554 File Offset: 0x00296754
		public override bool Success()
		{
			int num = (int)this.fractionalCycleNumber;
			float num2 = this.fractionalCycleNumber - (float)num;
			return (float)(GameClock.Instance.GetCycle() + 1) > this.fractionalCycleNumber || (GameClock.Instance.GetCycle() + 1 == num && GameClock.Instance.GetCurrentCycleAsPercentage() >= num2);
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x002985AB File Offset: 0x002967AB
		public void Deserialize(IReader reader)
		{
			this.fractionalCycleNumber = reader.ReadSingle();
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x002985BC File Offset: 0x002967BC
		public override string GetProgress(bool complete)
		{
			float num = (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, complete ? this.fractionalCycleNumber : num, this.fractionalCycleNumber);
		}

		// Token: 0x04004D97 RID: 19863
		private float fractionalCycleNumber;
	}
}
