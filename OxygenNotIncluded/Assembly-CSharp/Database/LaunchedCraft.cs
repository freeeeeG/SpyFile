using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5A RID: 3418
	public class LaunchedCraft : ColonyAchievementRequirement
	{
		// Token: 0x06006B08 RID: 27400 RVA: 0x0029ADE1 File Offset: 0x00298FE1
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x0029ADF0 File Offset: 0x00298FF0
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Clustercraft)enumerator.Current).Status == Clustercraft.CraftStatus.InFlight)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
