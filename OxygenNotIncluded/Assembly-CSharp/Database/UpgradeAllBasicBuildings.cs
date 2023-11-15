using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D4D RID: 3405
	public class UpgradeAllBasicBuildings : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AD4 RID: 27348 RVA: 0x0029A1F0 File Offset: 0x002983F0
		public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
		{
			this.basicBuilding = basicBuilding;
			this.upgradeBuilding = upgradeBuilding;
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x0029A208 File Offset: 0x00298408
		public override bool Success()
		{
			bool result = false;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (component.HasTag(this.basicBuilding))
				{
					return false;
				}
				if (component.HasTag(this.upgradeBuilding))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x0029A28C File Offset: 0x0029848C
		public void Deserialize(IReader reader)
		{
			string name = reader.ReadKleiString();
			this.basicBuilding = new Tag(name);
			string name2 = reader.ReadKleiString();
			this.upgradeBuilding = new Tag(name2);
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x0029A2C0 File Offset: 0x002984C0
		public override string GetProgress(bool complete)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(this.basicBuilding.Name);
			BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, buildingDef.Name, buildingDef2.Name);
		}

		// Token: 0x04004DB7 RID: 19895
		private Tag basicBuilding;

		// Token: 0x04004DB8 RID: 19896
		private Tag upgradeBuilding;
	}
}
