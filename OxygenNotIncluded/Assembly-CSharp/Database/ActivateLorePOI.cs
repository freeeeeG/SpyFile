using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D50 RID: 3408
	public class ActivateLorePOI : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AE0 RID: 27360 RVA: 0x0029A56A File Offset: 0x0029876A
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006AE1 RID: 27361 RVA: 0x0029A56C File Offset: 0x0029876C
		public override bool Success()
		{
			foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
			{
				if (!(buildingComplete == null))
				{
					Unsealable component = buildingComplete.GetComponent<Unsealable>();
					if (component != null && component.unsealed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x0029A5E4 File Offset: 0x002987E4
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
		}
	}
}
