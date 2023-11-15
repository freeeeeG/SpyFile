using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D53 RID: 3411
	public class TuneUpGenerator : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AEC RID: 27372 RVA: 0x0029A6F7 File Offset: 0x002988F7
		public TuneUpGenerator(float numChoreseToComplete)
		{
			this.numChoreseToComplete = numChoreseToComplete;
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x0029A708 File Offset: 0x00298908
		public override bool Success()
		{
			float num = 0f;
			ReportManager.ReportEntry entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
			for (int i = 0; i < entry.contextEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = entry.contextEntries[i];
				if (reportEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
				{
					num += reportEntry.Negative;
				}
			}
			string name = Db.Get().ChoreTypes.PowerTinker.Name;
			int count = ReportManager.Instance.reports.Count;
			for (int j = 0; j < count; j++)
			{
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[j].GetEntry(ReportManager.ReportType.ChoreStatus);
				int count2 = entry2.contextEntries.Count;
				for (int k = 0; k < count2; k++)
				{
					ReportManager.ReportEntry reportEntry2 = entry2.contextEntries[k];
					if (reportEntry2.context == name)
					{
						num += reportEntry2.Negative;
					}
				}
			}
			this.choresCompleted = Math.Abs(num);
			return Math.Abs(num) >= this.numChoreseToComplete;
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x0029A834 File Offset: 0x00298A34
		public void Deserialize(IReader reader)
		{
			this.numChoreseToComplete = reader.ReadSingle();
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x0029A844 File Offset: 0x00298A44
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, complete ? this.numChoreseToComplete : this.choresCompleted, this.numChoreseToComplete, Db.Get().ChoreTypes.PowerTinker.Name);
		}

		// Token: 0x04004DBA RID: 19898
		private float numChoreseToComplete;

		// Token: 0x04004DBB RID: 19899
		private float choresCompleted;
	}
}
