using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D56 RID: 3414
	public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AF9 RID: 27385 RVA: 0x0029AA9F File Offset: 0x00298C9F
		public override bool Success()
		{
			return Game.Instance.savedInfo.blockedCometWithBunkerDoor;
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x0029AAB0 File Offset: 0x00298CB0
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x0029AAB2 File Offset: 0x00298CB2
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
		}
	}
}
