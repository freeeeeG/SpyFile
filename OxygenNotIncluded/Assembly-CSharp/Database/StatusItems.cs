using System;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000D23 RID: 3363
	public class StatusItems : ResourceSet<StatusItem>
	{
		// Token: 0x06006A17 RID: 27159 RVA: 0x00294E85 File Offset: 0x00293085
		public StatusItems(string id, ResourceSet parent) : base(id, parent)
		{
		}

		// Token: 0x02001C3C RID: 7228
		[DebuggerDisplay("{Id}")]
		public class StatusItemInfo : Resource
		{
			// Token: 0x04008032 RID: 32818
			public string Type;

			// Token: 0x04008033 RID: 32819
			public string Tooltip;

			// Token: 0x04008034 RID: 32820
			public bool IsIconTinted;

			// Token: 0x04008035 RID: 32821
			public StatusItem.IconType IconType;

			// Token: 0x04008036 RID: 32822
			public string Icon;

			// Token: 0x04008037 RID: 32823
			public string SoundPath;

			// Token: 0x04008038 RID: 32824
			public bool ShouldNotify;

			// Token: 0x04008039 RID: 32825
			public float NotificationDelay;

			// Token: 0x0400803A RID: 32826
			public NotificationType NotificationType;

			// Token: 0x0400803B RID: 32827
			public bool AllowMultiples;

			// Token: 0x0400803C RID: 32828
			public string Effect;

			// Token: 0x0400803D RID: 32829
			public HashedString Overlay;

			// Token: 0x0400803E RID: 32830
			public HashedString SecondOverlay;
		}
	}
}
