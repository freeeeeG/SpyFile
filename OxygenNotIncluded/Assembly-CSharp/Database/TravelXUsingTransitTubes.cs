using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D49 RID: 3401
	public class TravelXUsingTransitTubes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AC3 RID: 27331 RVA: 0x00299C46 File Offset: 0x00297E46
		public TravelXUsingTransitTubes(NavType navType, int distanceToTravel)
		{
			this.navType = navType;
			this.distanceToTravel = distanceToTravel;
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x00299C5C File Offset: 0x00297E5C
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return num >= this.distanceToTravel;
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x00299CF0 File Offset: 0x00297EF0
		public void Deserialize(IReader reader)
		{
			byte b = reader.ReadByte();
			this.navType = (NavType)b;
			this.distanceToTravel = reader.ReadInt32();
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x00299D18 File Offset: 0x00297F18
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TRAVELED_IN_TUBES, complete ? this.distanceToTravel : num, this.distanceToTravel);
		}

		// Token: 0x04004DB2 RID: 19890
		private int distanceToTravel;

		// Token: 0x04004DB3 RID: 19891
		private NavType navType;
	}
}
