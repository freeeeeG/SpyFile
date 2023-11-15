using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D33 RID: 3379
	public class BeforeCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A5B RID: 27227 RVA: 0x002984CC File Offset: 0x002966CC
		public BeforeCycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x002984DB File Offset: 0x002966DB
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x002984F4 File Offset: 0x002966F4
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x002984FF File Offset: 0x002966FF
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x0029850D File Offset: 0x0029670D
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), this.cycleNumber);
		}

		// Token: 0x04004D96 RID: 19862
		private int cycleNumber;
	}
}
