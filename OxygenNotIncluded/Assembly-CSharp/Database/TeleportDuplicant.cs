using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5B RID: 3419
	public class TeleportDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06006B0B RID: 27403 RVA: 0x0029AE58 File Offset: 0x00299058
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x0029AE64 File Offset: 0x00299064
		public override bool Success()
		{
			using (List<WarpReceiver>.Enumerator enumerator = Components.WarpReceivers.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Used)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
