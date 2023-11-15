using System;

// Token: 0x02000970 RID: 2416
public class SimData
{
	// Token: 0x04002EFD RID: 12029
	public unsafe Sim.EmittedMassInfo* emittedMassEntries;

	// Token: 0x04002EFE RID: 12030
	public unsafe Sim.ElementChunkInfo* elementChunks;

	// Token: 0x04002EFF RID: 12031
	public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

	// Token: 0x04002F00 RID: 12032
	public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

	// Token: 0x04002F01 RID: 12033
	public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
}
