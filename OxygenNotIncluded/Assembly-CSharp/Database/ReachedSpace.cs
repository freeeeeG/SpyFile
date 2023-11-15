using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D36 RID: 3382
	public class ReachedSpace : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A6A RID: 27242 RVA: 0x00298712 File Offset: 0x00296912
		public ReachedSpace(SpaceDestinationType destinationType = null)
		{
			this.destinationType = destinationType;
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x00298721 File Offset: 0x00296921
		public override string Name()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x00298750 File Offset: 0x00296950
		public override string Description()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x00298780 File Offset: 0x00296980
		public override bool Success()
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					SpaceDestination destination = SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.savedSpacecraftDestinations[spacecraft.id]);
					if (this.destinationType == null || destination.GetDestinationType() == this.destinationType)
					{
						if (this.destinationType == Db.Get().SpaceDestinationTypes.Wormhole)
						{
							Game.Instance.unlocks.Unlock("temporaltear", true);
						}
						return true;
					}
				}
			}
			return SpacecraftManager.instance.hasVisitedWormHole;
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x0029885C File Offset: 0x00296A5C
		public void Deserialize(IReader reader)
		{
			if (reader.ReadByte() <= 0)
			{
				string id = reader.ReadKleiString();
				this.destinationType = Db.Get().SpaceDestinationTypes.Get(id);
			}
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x00298891 File Offset: 0x00296A91
		public override string GetProgress(bool completed)
		{
			if (this.destinationType == null)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x04004D99 RID: 19865
		private SpaceDestinationType destinationType;
	}
}
