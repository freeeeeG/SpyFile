using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D4C RID: 3404
	public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006ACF RID: 27343 RVA: 0x00299FE4 File Offset: 0x002981E4
		public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
		{
			this.validBuildingTypes = validBuildingTypes;
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x0029A000 File Offset: 0x00298200
		public override bool Success()
		{
			if (Components.LiveMinionIdentities.Items.Count <= 0)
			{
				return false;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
				if (this.validBuildingTypes.Contains(prefabTag))
				{
					num++;
					if (prefabTag == "FlushToilet" || prefabTag == "Outhouse")
					{
						return true;
					}
				}
			}
			return num >= Components.LiveMinionIdentities.Items.Count;
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x0029A0C8 File Offset: 0x002982C8
		public override bool Fail()
		{
			return false;
		}

		// Token: 0x06006AD2 RID: 27346 RVA: 0x0029A0CC File Offset: 0x002982CC
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.validBuildingTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.validBuildingTypes.Add(new Tag(name));
			}
		}

		// Token: 0x06006AD3 RID: 27347 RVA: 0x0029A110 File Offset: 0x00298310
		public override string GetProgress(bool complete)
		{
			if (this.validBuildingTypes.Contains("FlushToilet"))
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
			}
			if (complete)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (this.validBuildingTypes.Contains(component.PrefabTag))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, complete ? Components.LiveMinionIdentities.Items.Count : num, Components.LiveMinionIdentities.Items.Count);
		}

		// Token: 0x04004DB6 RID: 19894
		private List<Tag> validBuildingTypes = new List<Tag>();
	}
}
