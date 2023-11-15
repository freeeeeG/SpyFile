using System;

namespace Database
{
	// Token: 0x02000CE0 RID: 3296
	public class ArtableStatuses : ResourceSet<ArtableStatusItem>
	{
		// Token: 0x0600692A RID: 26922 RVA: 0x0027AC58 File Offset: 0x00278E58
		public ArtableStatuses(ResourceSet parent) : base("ArtableStatuses", parent)
		{
			this.AwaitingArting = this.Add("AwaitingArting", ArtableStatuses.ArtableStatusType.AwaitingArting);
			this.LookingUgly = this.Add("LookingUgly", ArtableStatuses.ArtableStatusType.LookingUgly);
			this.LookingOkay = this.Add("LookingOkay", ArtableStatuses.ArtableStatusType.LookingOkay);
			this.LookingGreat = this.Add("LookingGreat", ArtableStatuses.ArtableStatusType.LookingGreat);
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x0027ACBC File Offset: 0x00278EBC
		public ArtableStatusItem Add(string id, ArtableStatuses.ArtableStatusType statusType)
		{
			ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
			this.resources.Add(artableStatusItem);
			return artableStatusItem;
		}

		// Token: 0x04004891 RID: 18577
		public ArtableStatusItem AwaitingArting;

		// Token: 0x04004892 RID: 18578
		public ArtableStatusItem LookingUgly;

		// Token: 0x04004893 RID: 18579
		public ArtableStatusItem LookingOkay;

		// Token: 0x04004894 RID: 18580
		public ArtableStatusItem LookingGreat;

		// Token: 0x02001C17 RID: 7191
		public enum ArtableStatusType
		{
			// Token: 0x04007ECC RID: 32460
			AwaitingArting,
			// Token: 0x04007ECD RID: 32461
			LookingUgly,
			// Token: 0x04007ECE RID: 32462
			LookingOkay,
			// Token: 0x04007ECF RID: 32463
			LookingGreat
		}
	}
}
