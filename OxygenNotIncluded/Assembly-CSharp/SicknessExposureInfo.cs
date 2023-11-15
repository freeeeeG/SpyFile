using System;

// Token: 0x02000891 RID: 2193
[Serializable]
public struct SicknessExposureInfo
{
	// Token: 0x06003FC5 RID: 16325 RVA: 0x00164921 File Offset: 0x00162B21
	public SicknessExposureInfo(string id, string infection_source_info)
	{
		this.sicknessID = id;
		this.sourceInfo = infection_source_info;
	}

	// Token: 0x04002972 RID: 10610
	public string sicknessID;

	// Token: 0x04002973 RID: 10611
	public string sourceInfo;
}
