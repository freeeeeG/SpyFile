using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D31 RID: 3377
	public class NumberOfDupes : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A4F RID: 27215 RVA: 0x0029837A File Offset: 0x0029657A
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, this.numDupes);
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x00298396 File Offset: 0x00296596
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, this.numDupes);
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x002983B2 File Offset: 0x002965B2
		public NumberOfDupes(int num)
		{
			this.numDupes = num;
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x002983C1 File Offset: 0x002965C1
		public override bool Success()
		{
			return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x002983DD File Offset: 0x002965DD
		public void Deserialize(IReader reader)
		{
			this.numDupes = reader.ReadInt32();
		}

		// Token: 0x06006A54 RID: 27220 RVA: 0x002983EB File Offset: 0x002965EB
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count, this.numDupes);
		}

		// Token: 0x04004D94 RID: 19860
		private int numDupes;
	}
}
