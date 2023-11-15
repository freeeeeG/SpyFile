using System;

// Token: 0x02000A88 RID: 2696
public enum SimMessageHashes
{
	// Token: 0x040036BB RID: 14011
	Elements_CreateTable = 1108437482,
	// Token: 0x040036BC RID: 14012
	Elements_CreateInteractions = -930289787,
	// Token: 0x040036BD RID: 14013
	SetWorldZones = -457308393,
	// Token: 0x040036BE RID: 14014
	ModifyCellWorldZone = -449718014,
	// Token: 0x040036BF RID: 14015
	Disease_CreateTable = 825301935,
	// Token: 0x040036C0 RID: 14016
	Load = -672538170,
	// Token: 0x040036C1 RID: 14017
	Start = -931446686,
	// Token: 0x040036C2 RID: 14018
	AllocateCells = 1092408308,
	// Token: 0x040036C3 RID: 14019
	ClearUnoccupiedCells = -1836204275,
	// Token: 0x040036C4 RID: 14020
	DefineWorldOffsets = -895846551,
	// Token: 0x040036C5 RID: 14021
	PrepareGameData = 1078620451,
	// Token: 0x040036C6 RID: 14022
	SimData_InitializeFromCells = 2062421945,
	// Token: 0x040036C7 RID: 14023
	SimData_ResizeAndInitializeVacuumCells = -752676153,
	// Token: 0x040036C8 RID: 14024
	SimData_FreeCells = -1167792921,
	// Token: 0x040036C9 RID: 14025
	SimFrameManager_NewGameFrame = -775326397,
	// Token: 0x040036CA RID: 14026
	Dig = 833038498,
	// Token: 0x040036CB RID: 14027
	ModifyCell = -1252920804,
	// Token: 0x040036CC RID: 14028
	ModifyCellEnergy = 818320644,
	// Token: 0x040036CD RID: 14029
	SetInsulationValue = -898773121,
	// Token: 0x040036CE RID: 14030
	SetStrengthValue = 1593243982,
	// Token: 0x040036CF RID: 14031
	SetVisibleCells = -563057023,
	// Token: 0x040036D0 RID: 14032
	ChangeCellProperties = -469311643,
	// Token: 0x040036D1 RID: 14033
	AddBuildingHeatExchange = 1739021608,
	// Token: 0x040036D2 RID: 14034
	ModifyBuildingHeatExchange = 1818001569,
	// Token: 0x040036D3 RID: 14035
	ModifyBuildingEnergy = -1348791658,
	// Token: 0x040036D4 RID: 14036
	RemoveBuildingHeatExchange = -456116629,
	// Token: 0x040036D5 RID: 14037
	AddBuildingToBuildingHeatExchange = -1338718217,
	// Token: 0x040036D6 RID: 14038
	AddInContactBuildingToBuildingToBuildingHeatExchange = -1586724321,
	// Token: 0x040036D7 RID: 14039
	RemoveBuildingInContactFromBuildingToBuildingHeatExchange = -1993857213,
	// Token: 0x040036D8 RID: 14040
	RemoveBuildingToBuildingHeatExchange = 697100730,
	// Token: 0x040036D9 RID: 14041
	SetDebugProperties = -1683118492,
	// Token: 0x040036DA RID: 14042
	MassConsumption = 1727657959,
	// Token: 0x040036DB RID: 14043
	MassEmission = 797274363,
	// Token: 0x040036DC RID: 14044
	AddElementConsumer = 2024405073,
	// Token: 0x040036DD RID: 14045
	RemoveElementConsumer = 894417742,
	// Token: 0x040036DE RID: 14046
	SetElementConsumerData = 1575539738,
	// Token: 0x040036DF RID: 14047
	AddElementEmitter = -505471181,
	// Token: 0x040036E0 RID: 14048
	ModifyElementEmitter = 403589164,
	// Token: 0x040036E1 RID: 14049
	RemoveElementEmitter = -1524118282,
	// Token: 0x040036E2 RID: 14050
	AddElementChunk = 1445724082,
	// Token: 0x040036E3 RID: 14051
	RemoveElementChunk = -912908555,
	// Token: 0x040036E4 RID: 14052
	SetElementChunkData = -435115907,
	// Token: 0x040036E5 RID: 14053
	MoveElementChunk = -374911358,
	// Token: 0x040036E6 RID: 14054
	ModifyElementChunkEnergy = 1020555667,
	// Token: 0x040036E7 RID: 14055
	ModifyChunkTemperatureAdjuster = -1387601379,
	// Token: 0x040036E8 RID: 14056
	AddDiseaseEmitter = 1486783027,
	// Token: 0x040036E9 RID: 14057
	ModifyDiseaseEmitter = -1899123924,
	// Token: 0x040036EA RID: 14058
	RemoveDiseaseEmitter = 468135926,
	// Token: 0x040036EB RID: 14059
	AddDiseaseConsumer = 348345681,
	// Token: 0x040036EC RID: 14060
	ModifyDiseaseConsumer = -1822987624,
	// Token: 0x040036ED RID: 14061
	RemoveDiseaseConsumer = -781641650,
	// Token: 0x040036EE RID: 14062
	ConsumeDisease = -1019841536,
	// Token: 0x040036EF RID: 14063
	CellDiseaseModification = -1853671274,
	// Token: 0x040036F0 RID: 14064
	ToggleProfiler = -409964931,
	// Token: 0x040036F1 RID: 14065
	SetSavedOptions = 1154135737,
	// Token: 0x040036F2 RID: 14066
	CellRadiationModification = -1914877797,
	// Token: 0x040036F3 RID: 14067
	RadiationSickness = -727746602,
	// Token: 0x040036F4 RID: 14068
	AddRadiationEmitter = -1505895314,
	// Token: 0x040036F5 RID: 14069
	ModifyRadiationEmitter = -503965465,
	// Token: 0x040036F6 RID: 14070
	RemoveRadiationEmitter = -704259919,
	// Token: 0x040036F7 RID: 14071
	RadiationParamsModification = 377112707
}
