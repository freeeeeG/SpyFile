using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D3A RID: 3386
	public class EstablishColonies : VictoryColonyAchievementRequirement
	{
		// Token: 0x06006A81 RID: 27265 RVA: 0x00298A38 File Offset: 0x00296C38
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES.Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x00298A8F File Offset: 0x00296C8F
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x00298A9D File Offset: 0x00296C9D
		public override bool Success()
		{
			return this.GetColonyCount() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x00298AAF File Offset: 0x00296CAF
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x00298ABC File Offset: 0x00296CBC
		private int GetColonyCount()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04004D9C RID: 19868
		public static int BASE_COUNT = 5;
	}
}
