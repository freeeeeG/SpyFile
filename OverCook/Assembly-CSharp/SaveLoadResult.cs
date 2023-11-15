using System;

// Token: 0x0200071F RID: 1823
public enum SaveLoadResult
{
	// Token: 0x04001A93 RID: 6803
	Exists,
	// Token: 0x04001A94 RID: 6804
	NotExist,
	// Token: 0x04001A95 RID: 6805
	Corrupted,
	// Token: 0x04001A96 RID: 6806
	NoSpace,
	// Token: 0x04001A97 RID: 6807
	NotSaveable,
	// Token: 0x04001A98 RID: 6808
	UnhandledResult,
	// Token: 0x04001A99 RID: 6809
	Cancel,
	// Token: 0x04001A9A RID: 6810
	InvalidPath
}
