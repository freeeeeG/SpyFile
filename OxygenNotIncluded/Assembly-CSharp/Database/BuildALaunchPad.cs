using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5D RID: 3421
	public class BuildALaunchPad : ColonyAchievementRequirement
	{
		// Token: 0x06006B11 RID: 27409 RVA: 0x0029AEF1 File Offset: 0x002990F1
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x0029AF00 File Offset: 0x00299100
		public override bool Success()
		{
			foreach (LaunchPad component in Components.LaunchPads.Items)
			{
				WorldContainer myWorld = component.GetMyWorld();
				if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id, false).Count == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
