using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D60 RID: 3424
	public class LandOnAllWorlds : ColonyAchievementRequirement
	{
		// Token: 0x06006B1A RID: 27418 RVA: 0x0029B030 File Offset: 0x00299230
		public override string GetProgress(bool complete)
		{
			int num = 0;
			int num2 = 0;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					num++;
					if (worldContainer.IsDupeVisited || worldContainer.IsRoverVisted)
					{
						num2++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAND_DUPES_ON_ALL_WORLDS, num2, num);
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x0029B0BC File Offset: 0x002992BC
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && !worldContainer.IsDupeVisited && !worldContainer.IsRoverVisted)
				{
					return false;
				}
			}
			return true;
		}
	}
}
