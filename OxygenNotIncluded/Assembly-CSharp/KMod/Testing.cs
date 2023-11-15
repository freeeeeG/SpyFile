using System;

namespace KMod
{
	// Token: 0x02000D6F RID: 3439
	public static class Testing
	{
		// Token: 0x04004E9D RID: 20125
		public static Testing.DLLLoading dll_loading;

		// Token: 0x04004E9E RID: 20126
		public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;

		// Token: 0x04004E9F RID: 20127
		public const Testing.Install INSTALL = Testing.Install.NoTesting;

		// Token: 0x04004EA0 RID: 20128
		public const Testing.Boot BOOT = Testing.Boot.NoTesting;

		// Token: 0x02001C45 RID: 7237
		public enum DLLLoading
		{
			// Token: 0x0400804F RID: 32847
			NoTesting,
			// Token: 0x04008050 RID: 32848
			Fail,
			// Token: 0x04008051 RID: 32849
			UseModLoaderDLLExclusively
		}

		// Token: 0x02001C46 RID: 7238
		public enum SaveLoad
		{
			// Token: 0x04008053 RID: 32851
			NoTesting,
			// Token: 0x04008054 RID: 32852
			FailSave,
			// Token: 0x04008055 RID: 32853
			FailLoad
		}

		// Token: 0x02001C47 RID: 7239
		public enum Install
		{
			// Token: 0x04008057 RID: 32855
			NoTesting,
			// Token: 0x04008058 RID: 32856
			ForceUninstall,
			// Token: 0x04008059 RID: 32857
			ForceReinstall,
			// Token: 0x0400805A RID: 32858
			ForceUpdate
		}

		// Token: 0x02001C48 RID: 7240
		public enum Boot
		{
			// Token: 0x0400805C RID: 32860
			NoTesting,
			// Token: 0x0400805D RID: 32861
			Crash
		}
	}
}
