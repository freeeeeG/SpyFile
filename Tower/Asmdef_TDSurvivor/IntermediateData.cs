using System;

// Token: 0x0200005E RID: 94
[Serializable]
public class IntermediateData
{
	// Token: 0x0400019D RID: 413
	public eWorldType worldType = eWorldType.NONE;

	// Token: 0x0400019E RID: 414
	public eMissionType missionType;

	// Token: 0x0400019F RID: 415
	public int difficulty;

	// Token: 0x040001A0 RID: 416
	public bool isCorrupted;

	// Token: 0x040001A1 RID: 417
	public string stageName = "";

	// Token: 0x040001A2 RID: 418
	public StageSettingData stageData;

	// Token: 0x040001A3 RID: 419
	public MapNodeData mapNodeData;
}
