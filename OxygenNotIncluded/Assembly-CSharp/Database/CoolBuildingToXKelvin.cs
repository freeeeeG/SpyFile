using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D43 RID: 3395
	public class CoolBuildingToXKelvin : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AAA RID: 27306 RVA: 0x00299658 File Offset: 0x00297858
		public CoolBuildingToXKelvin(int kelvinToCoolTo)
		{
			this.kelvinToCoolTo = kelvinToCoolTo;
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x00299667 File Offset: 0x00297867
		public override bool Success()
		{
			return BuildingComplete.MinKelvinSeen <= (float)this.kelvinToCoolTo;
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x0029967A File Offset: 0x0029787A
		public void Deserialize(IReader reader)
		{
			this.kelvinToCoolTo = reader.ReadInt32();
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x00299688 File Offset: 0x00297888
		public override string GetProgress(bool complete)
		{
			float minKelvinSeen = BuildingComplete.MinKelvinSeen;
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, minKelvinSeen);
		}

		// Token: 0x04004DAC RID: 19884
		private int kelvinToCoolTo;
	}
}
