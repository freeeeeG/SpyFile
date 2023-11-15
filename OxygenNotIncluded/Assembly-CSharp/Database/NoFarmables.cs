using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D44 RID: 3396
	public class NoFarmables : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AAE RID: 27310 RVA: 0x002996B0 File Offset: 0x002978B0
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(worldContainer.id))
				{
					if (plantablePlot.Occupant != null)
					{
						using (IEnumerator<Tag> enumerator3 = plantablePlot.possibleDepositObjectTags.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current != GameTags.DecorSeed)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x002997A4 File Offset: 0x002979A4
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x002997AF File Offset: 0x002979AF
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x002997B1 File Offset: 0x002979B1
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
		}
	}
}
