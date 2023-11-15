using System;
using Level;

namespace Runnables.Triggers
{
	// Token: 0x0200035D RID: 861
	public class MapRewardActivated : Trigger
	{
		// Token: 0x0600100C RID: 4108 RVA: 0x0002FECF File Offset: 0x0002E0CF
		protected override bool Check()
		{
			return Map.Instance.mapReward.activated;
		}
	}
}
