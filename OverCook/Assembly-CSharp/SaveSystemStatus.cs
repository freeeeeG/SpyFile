using System;

// Token: 0x02000720 RID: 1824
public struct SaveSystemStatus
{
	// Token: 0x0600229F RID: 8863 RVA: 0x000A67C6 File Offset: 0x000A4BC6
	public SaveSystemStatus(SaveSystemStatus.SaveStatus _status, SaveLoadResult _result)
	{
		this.Status = _status;
		this.Result = _result;
	}

	// Token: 0x04001A9B RID: 6811
	public SaveSystemStatus.SaveStatus Status;

	// Token: 0x04001A9C RID: 6812
	public SaveLoadResult Result;

	// Token: 0x02000721 RID: 1825
	public enum SaveStatus
	{
		// Token: 0x04001A9E RID: 6814
		InProgress,
		// Token: 0x04001A9F RID: 6815
		Retry,
		// Token: 0x04001AA0 RID: 6816
		Complete,
		// Token: 0x04001AA1 RID: 6817
		COUNT
	}
}
