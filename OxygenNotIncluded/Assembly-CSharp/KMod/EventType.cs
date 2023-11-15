using System;

namespace KMod
{
	// Token: 0x02000D82 RID: 3458
	public enum EventType
	{
		// Token: 0x04004EF4 RID: 20212
		LoadError,
		// Token: 0x04004EF5 RID: 20213
		NotFound,
		// Token: 0x04004EF6 RID: 20214
		InstallInfoInaccessible,
		// Token: 0x04004EF7 RID: 20215
		OutOfOrder,
		// Token: 0x04004EF8 RID: 20216
		ExpectedActive,
		// Token: 0x04004EF9 RID: 20217
		ExpectedInactive,
		// Token: 0x04004EFA RID: 20218
		ActiveDuringCrash,
		// Token: 0x04004EFB RID: 20219
		InstallFailed,
		// Token: 0x04004EFC RID: 20220
		Installed,
		// Token: 0x04004EFD RID: 20221
		Uninstalled,
		// Token: 0x04004EFE RID: 20222
		VersionUpdate,
		// Token: 0x04004EFF RID: 20223
		AvailableContentChanged,
		// Token: 0x04004F00 RID: 20224
		RestartRequested,
		// Token: 0x04004F01 RID: 20225
		BadWorldGen,
		// Token: 0x04004F02 RID: 20226
		Deactivated,
		// Token: 0x04004F03 RID: 20227
		DisabledEarlyAccess
	}
}
