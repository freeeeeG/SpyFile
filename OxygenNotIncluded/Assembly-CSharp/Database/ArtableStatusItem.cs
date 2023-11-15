using System;

namespace Database
{
	// Token: 0x02000CE1 RID: 3297
	public class ArtableStatusItem : StatusItem
	{
		// Token: 0x0600692C RID: 26924 RVA: 0x0027ACE0 File Offset: 0x00278EE0
		public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType) : base(id, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null)
		{
			this.StatusType = statusType;
		}

		// Token: 0x04004895 RID: 18581
		public ArtableStatuses.ArtableStatusType StatusType;
	}
}
