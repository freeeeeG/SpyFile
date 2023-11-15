using System;

// Token: 0x020007CE RID: 1998
public enum GameHashes
{
	// Token: 0x040022A7 RID: 8871
	Saved = 1589904519,
	// Token: 0x040022A8 RID: 8872
	Loaded = -1594984443,
	// Token: 0x040022A9 RID: 8873
	SetModel = 364584999,
	// Token: 0x040022AA RID: 8874
	SelectObject = -1503271301,
	// Token: 0x040022AB RID: 8875
	HighlightObject = -1201923725,
	// Token: 0x040022AC RID: 8876
	HighlightStatusItem = 2095258329,
	// Token: 0x040022AD RID: 8877
	ClickTile = -2043434986,
	// Token: 0x040022AE RID: 8878
	HoverObject = -1454059909,
	// Token: 0x040022AF RID: 8879
	UnhoverObject = -983359998,
	// Token: 0x040022B0 RID: 8880
	ObjectReplaced = 1606648047,
	// Token: 0x040022B1 RID: 8881
	DebugInsantBuildModeChanged = 1557339983,
	// Token: 0x040022B2 RID: 8882
	ConnectWiring = -1950680436,
	// Token: 0x040022B3 RID: 8883
	DisconnectWiring = 1903019870,
	// Token: 0x040022B4 RID: 8884
	SetActivator = 315865555,
	// Token: 0x040022B5 RID: 8885
	FocusLost = 553322396,
	// Token: 0x040022B6 RID: 8886
	UIClear = 288942073,
	// Token: 0x040022B7 RID: 8887
	UIRefresh = 1980521255,
	// Token: 0x040022B8 RID: 8888
	UIRefreshData = -1514841199,
	// Token: 0x040022B9 RID: 8889
	BuildToolDeactivated = -1190690038,
	// Token: 0x040022BA RID: 8890
	ActiveToolChanged = 1174281782,
	// Token: 0x040022BB RID: 8891
	OnStorageInteracted = -778359855,
	// Token: 0x040022BC RID: 8892
	OnStorageChange = -1697596308,
	// Token: 0x040022BD RID: 8893
	UpdateStorageInfo = -1197125120,
	// Token: 0x040022BE RID: 8894
	OnStore = 856640610,
	// Token: 0x040022BF RID: 8895
	OnStorageLockerSetupComplete = -1683615038,
	// Token: 0x040022C0 RID: 8896
	Died = 1623392196,
	// Token: 0x040022C1 RID: 8897
	DeathAnimComplete = -66249442,
	// Token: 0x040022C2 RID: 8898
	Revived = -1117766961,
	// Token: 0x040022C3 RID: 8899
	Defrosted = -1804024542,
	// Token: 0x040022C4 RID: 8900
	VisualizerChanged = -2100764682,
	// Token: 0x040022C5 RID: 8901
	BuildingStateChanged = -809948329,
	// Token: 0x040022C6 RID: 8902
	OperationalChanged = -592767678,
	// Token: 0x040022C7 RID: 8903
	FunctionalChanged = -1852328367,
	// Token: 0x040022C8 RID: 8904
	OperationalFlagChanged = 187661686,
	// Token: 0x040022C9 RID: 8905
	ActiveChanged = 824508782,
	// Token: 0x040022CA RID: 8906
	Open = 1677747338,
	// Token: 0x040022CB RID: 8907
	Close = 476357528,
	// Token: 0x040022CC RID: 8908
	DoorStateChanged = 1734268753,
	// Token: 0x040022CD RID: 8909
	DoorControlStateChanged = 279163026,
	// Token: 0x040022CE RID: 8910
	AccessControlChanged = -1525636549,
	// Token: 0x040022CF RID: 8911
	NewBuilding = -1661515756,
	// Token: 0x040022D0 RID: 8912
	RefreshUserMenu = 493375141,
	// Token: 0x040022D1 RID: 8913
	NewConstruction = 2121280625,
	// Token: 0x040022D2 RID: 8914
	DroppedAsLoot = -375153990,
	// Token: 0x040022D3 RID: 8915
	DroppedAll = -1957399615,
	// Token: 0x040022D4 RID: 8916
	ResearchComplete = -107300940,
	// Token: 0x040022D5 RID: 8917
	ActiveResearchChanged = -1914338957,
	// Token: 0x040022D6 RID: 8918
	StudyComplete = -1436775550,
	// Token: 0x040022D7 RID: 8919
	StudyCancel = 1488501379,
	// Token: 0x040022D8 RID: 8920
	StatusChange = -111137758,
	// Token: 0x040022D9 RID: 8921
	EquippedItemEquipper = -448952673,
	// Token: 0x040022DA RID: 8922
	UnequippedItemEquipper = -1285462312,
	// Token: 0x040022DB RID: 8923
	EquippedItemEquippable = -1617557748,
	// Token: 0x040022DC RID: 8924
	UnequippedItemEquippable = -170173755,
	// Token: 0x040022DD RID: 8925
	EnableOverlay = 1248612973,
	// Token: 0x040022DE RID: 8926
	DisableOverlay = 2015652040,
	// Token: 0x040022DF RID: 8927
	OverlayChanged = 1798162660,
	// Token: 0x040022E0 RID: 8928
	SleepStarted = -1283701846,
	// Token: 0x040022E1 RID: 8929
	SleepFail = 1338475637,
	// Token: 0x040022E2 RID: 8930
	SleepDisturbedByNoise = 1621815900,
	// Token: 0x040022E3 RID: 8931
	SleepDisturbedByLight = -1063113160,
	// Token: 0x040022E4 RID: 8932
	SleepDisturbedByFearOfDark = -1307593733,
	// Token: 0x040022E5 RID: 8933
	SleepDisturbedByMovement = -717201811,
	// Token: 0x040022E6 RID: 8934
	SleepFinished = -2090444759,
	// Token: 0x040022E7 RID: 8935
	EatStart = 1356255274,
	// Token: 0x040022E8 RID: 8936
	EatFail = 1723868278,
	// Token: 0x040022E9 RID: 8937
	EatCompleteEdible = -10536414,
	// Token: 0x040022EA RID: 8938
	EatStartEater = 1406130139,
	// Token: 0x040022EB RID: 8939
	EatCompleteEater = 1121894420,
	// Token: 0x040022EC RID: 8940
	EatSolidComplete = 1386391852,
	// Token: 0x040022ED RID: 8941
	CaloriesConsumed = -2038961714,
	// Token: 0x040022EE RID: 8942
	PrepareForExplosion = -979425869,
	// Token: 0x040022EF RID: 8943
	Cancel = 2127324410,
	// Token: 0x040022F0 RID: 8944
	WorkChoreDisabled = 2108245096,
	// Token: 0x040022F1 RID: 8945
	MarkForDeconstruct = -790448070,
	// Token: 0x040022F2 RID: 8946
	MarkForRelocate = 923661281,
	// Token: 0x040022F3 RID: 8947
	Prioritize = 1531330463,
	// Token: 0x040022F4 RID: 8948
	Deprioritize = -195779040,
	// Token: 0x040022F5 RID: 8949
	DeconstructStart = 1830962028,
	// Token: 0x040022F6 RID: 8950
	DeconstructComplete = -702296337,
	// Token: 0x040022F7 RID: 8951
	ItemReachable = 1261805274,
	// Token: 0x040022F8 RID: 8952
	ItemUnreachable = -600999071,
	// Token: 0x040022F9 RID: 8953
	CameraMove = 564128022,
	// Token: 0x040022FA RID: 8954
	PauseChanged = -1788536802,
	// Token: 0x040022FB RID: 8955
	BaseAlreadyCreated = -1992507039,
	// Token: 0x040022FC RID: 8956
	RegionChanged = -1601261024,
	// Token: 0x040022FD RID: 8957
	Rotated = -1643076535,
	// Token: 0x040022FE RID: 8958
	AnimQueueComplete = -1061186183,
	// Token: 0x040022FF RID: 8959
	HungerStatusChanged = -12937937,
	// Token: 0x04002300 RID: 8960
	TutorialOpened = 1634669191,
	// Token: 0x04002301 RID: 8961
	CreatureStatusChanged = -151109373,
	// Token: 0x04002302 RID: 8962
	LevelUp = -110704193,
	// Token: 0x04002303 RID: 8963
	NewDay = 631075836,
	// Token: 0x04002304 RID: 8964
	NewBlock = -1215042067,
	// Token: 0x04002305 RID: 8965
	ScheduleChanged = 467134493,
	// Token: 0x04002306 RID: 8966
	ScheduleBlocksTick = 1714332666,
	// Token: 0x04002307 RID: 8967
	ScheduleBlocksChanged = -894023145,
	// Token: 0x04002308 RID: 8968
	Craft = 748399584,
	// Token: 0x04002309 RID: 8969
	Harvest = 1272413801,
	// Token: 0x0400230A RID: 8970
	HarvestComplete = 113170146,
	// Token: 0x0400230B RID: 8971
	Absorb = -2064133523,
	// Token: 0x0400230C RID: 8972
	AbsorbedBy = -1940207677,
	// Token: 0x0400230D RID: 8973
	SplitFromChunk = 1335436905,
	// Token: 0x0400230E RID: 8974
	Butcher = 395373363,
	// Token: 0x0400230F RID: 8975
	WalkBy = -517744704,
	// Token: 0x04002310 RID: 8976
	DoDamage = -184635526,
	// Token: 0x04002311 RID: 8977
	NearMelting = -2009062694,
	// Token: 0x04002312 RID: 8978
	MeltDown = 1930836866,
	// Token: 0x04002313 RID: 8979
	Landed = 1188683690,
	// Token: 0x04002314 RID: 8980
	GeotunerChange = 1763323737,
	// Token: 0x04002315 RID: 8981
	GeyserEruption = -593169791,
	// Token: 0x04002316 RID: 8982
	Pacified = -1427155335,
	// Token: 0x04002317 RID: 8983
	Rescued = -638309935,
	// Token: 0x04002318 RID: 8984
	Released = 501672573,
	// Token: 0x04002319 RID: 8985
	FeedingStart = -1114035522,
	// Token: 0x0400231A RID: 8986
	FeedingEnd = 581341623,
	// Token: 0x0400231B RID: 8987
	FeedingComplete = 1270416669,
	// Token: 0x0400231C RID: 8988
	PlanterStorage = 1309017699,
	// Token: 0x0400231D RID: 8989
	GrowthStateMature = -2116516046,
	// Token: 0x0400231E RID: 8990
	Fertilized = -1396791468,
	// Token: 0x0400231F RID: 8991
	Unfertilized = -1073674739,
	// Token: 0x04002320 RID: 8992
	CropDepleted = 591871899,
	// Token: 0x04002321 RID: 8993
	BlightChanged = -1425542080,
	// Token: 0x04002322 RID: 8994
	CropTended = 90606262,
	// Token: 0x04002323 RID: 8995
	SeedProduced = 472291861,
	// Token: 0x04002324 RID: 8996
	SeedDropped = -1736624145,
	// Token: 0x04002325 RID: 8997
	DestinationReached = 387220196,
	// Token: 0x04002326 RID: 8998
	NavigationFailed = -766531887,
	// Token: 0x04002327 RID: 8999
	NavigationCellChanged = 915392638,
	// Token: 0x04002328 RID: 9000
	AssignablesChanged = -1585839766,
	// Token: 0x04002329 RID: 9001
	AssigneeChanged = 684616645,
	// Token: 0x0400232A RID: 9002
	ObjectDestroyed = 1969584890,
	// Token: 0x0400232B RID: 9003
	QueueDestroyObject = 1502190696,
	// Token: 0x0400232C RID: 9004
	TargetLost = 2144432245,
	// Token: 0x0400232D RID: 9005
	NewGameSpawn = 1119167081,
	// Token: 0x0400232E RID: 9006
	OccupiableChanged = 2004582811,
	// Token: 0x0400232F RID: 9007
	OccupantChanged = -731304873,
	// Token: 0x04002330 RID: 9008
	OccupantValidChanged = -1820564715,
	// Token: 0x04002331 RID: 9009
	EffectAdded = -1901442097,
	// Token: 0x04002332 RID: 9010
	EffectRemoved = -1157678353,
	// Token: 0x04002333 RID: 9011
	GameOver = 1719568262,
	// Token: 0x04002334 RID: 9012
	ConduitConnectionChanged = -2094018600,
	// Token: 0x04002335 RID: 9013
	PowerStatusChanged = 1088293757,
	// Token: 0x04002336 RID: 9014
	ConnectionsChanged = -1041684577,
	// Token: 0x04002337 RID: 9015
	ReachableChanged = -1432940121,
	// Token: 0x04002338 RID: 9016
	AddedFetchable = -1588644844,
	// Token: 0x04002339 RID: 9017
	RemovedFetchable = -1491270284,
	// Token: 0x0400233A RID: 9018
	EnteredRedAlert = 1585324898,
	// Token: 0x0400233B RID: 9019
	ExitedRedAlert = -1393151672,
	// Token: 0x0400233C RID: 9020
	SafeCellDetected = 982561777,
	// Token: 0x0400233D RID: 9021
	SafeCellLost = 506919987,
	// Token: 0x0400233E RID: 9022
	CreatureMoveComplete = -1436222551,
	// Token: 0x0400233F RID: 9023
	FishableDepleted = -1851044355,
	// Token: 0x04002340 RID: 9024
	TooHotWarning = -1234705021,
	// Token: 0x04002341 RID: 9025
	TooHotFatal = -55477301,
	// Token: 0x04002342 RID: 9026
	TooColdWarning = -107174716,
	// Token: 0x04002343 RID: 9027
	TooColdFatal = -1758196852,
	// Token: 0x04002344 RID: 9028
	TooColdSickness = 54654253,
	// Token: 0x04002345 RID: 9029
	TooHotSickness = -1174019026,
	// Token: 0x04002346 RID: 9030
	OptimalTemperatureAchieved = 115888613,
	// Token: 0x04002347 RID: 9031
	CreatureReproduce = 230069070,
	// Token: 0x04002348 RID: 9032
	ReadyToHatch = 657149762,
	// Token: 0x04002349 RID: 9033
	Hatch = 1922945024,
	// Token: 0x0400234A RID: 9034
	IlluminationComfort = 1113102781,
	// Token: 0x0400234B RID: 9035
	IlluminationDiscomfort = 1387626797,
	// Token: 0x0400234C RID: 9036
	RadiationComfort = 874353739,
	// Token: 0x0400234D RID: 9037
	RadiationDiscomfort = 1788072223,
	// Token: 0x0400234E RID: 9038
	RadiationRecovery = 1556680150,
	// Token: 0x0400234F RID: 9039
	UseSuccess = 58624316,
	// Token: 0x04002350 RID: 9040
	UseFail = 1572098533,
	// Token: 0x04002351 RID: 9041
	BodyOfWaterChanged = -263784810,
	// Token: 0x04002352 RID: 9042
	BeginChore = -1988963660,
	// Token: 0x04002353 RID: 9043
	EndChore = 1745615042,
	// Token: 0x04002354 RID: 9044
	AddUrge = -736698276,
	// Token: 0x04002355 RID: 9045
	RemoveUrge = 231622047,
	// Token: 0x04002356 RID: 9046
	ClosestEdibleChanged = 86328522,
	// Token: 0x04002357 RID: 9047
	CellChanged = 1088554450,
	// Token: 0x04002358 RID: 9048
	ObjectMovementStateChanged = 1027377649,
	// Token: 0x04002359 RID: 9049
	ObjectMovementWakeUp = -97592435,
	// Token: 0x0400235A RID: 9050
	ObjectMovementSleep = -696472375,
	// Token: 0x0400235B RID: 9051
	SicknessAdded = 1592732331,
	// Token: 0x0400235C RID: 9052
	SicknessCured = 77635178,
	// Token: 0x0400235D RID: 9053
	GermPresenceChanged = -1689370368,
	// Token: 0x0400235E RID: 9054
	EnteredToxicArea = -1198182067,
	// Token: 0x0400235F RID: 9055
	ExitedToxicArea = 369532135,
	// Token: 0x04002360 RID: 9056
	EnteredBreathableArea = 99949694,
	// Token: 0x04002361 RID: 9057
	ExitedBreathableArea = -1189351068,
	// Token: 0x04002362 RID: 9058
	AreaElementSafeChanged = -2023773544,
	// Token: 0x04002363 RID: 9059
	ColonyHasRationsChanged = -171255324,
	// Token: 0x04002364 RID: 9060
	AssignableReachabilityChanged = 334784980,
	// Token: 0x04002365 RID: 9061
	TimeOfDayChanged = 1791086652,
	// Token: 0x04002366 RID: 9062
	MessagesChanged = -599791736,
	// Token: 0x04002367 RID: 9063
	MessageAdded = 1558809273,
	// Token: 0x04002368 RID: 9064
	SaveGameReady = -1917495436,
	// Token: 0x04002369 RID: 9065
	StartGameUser = -838649377,
	// Token: 0x0400236A RID: 9066
	ToiletSensorChanged = -752545459,
	// Token: 0x0400236B RID: 9067
	ExposeToDisease = -283306403,
	// Token: 0x0400236C RID: 9068
	EntombedChanged = -1089732772,
	// Token: 0x0400236D RID: 9069
	HiddenChanged = -298960054,
	// Token: 0x0400236E RID: 9070
	EnteredFogOfWar = 40674218,
	// Token: 0x0400236F RID: 9071
	ExitedFogOfWar = 1357164944,
	// Token: 0x04002370 RID: 9072
	Nighttime = -722330267,
	// Token: 0x04002371 RID: 9073
	NewBaseCreated = -122303817,
	// Token: 0x04002372 RID: 9074
	ExposedToPutridOdour = -513254434,
	// Token: 0x04002373 RID: 9075
	FinishedActingOut = -1734580852,
	// Token: 0x04002374 RID: 9076
	Cringe = 508119890,
	// Token: 0x04002375 RID: 9077
	Uprooted = -216549700,
	// Token: 0x04002376 RID: 9078
	UprootTypeChanged = 755331037,
	// Token: 0x04002377 RID: 9079
	Drowning = 1949704522,
	// Token: 0x04002378 RID: 9080
	Drowned = -750750377,
	// Token: 0x04002379 RID: 9081
	DryingOut = -2057657673,
	// Token: 0x0400237A RID: 9082
	EnteredWetArea = 1555379996,
	// Token: 0x0400237B RID: 9083
	DriedOut = 1514431252,
	// Token: 0x0400237C RID: 9084
	CropReady = -1686619725,
	// Token: 0x0400237D RID: 9085
	CropPicked = -1072826864,
	// Token: 0x0400237E RID: 9086
	CropSpawned = 35625290,
	// Token: 0x0400237F RID: 9087
	LowPressureWarning = -1175525437,
	// Token: 0x04002380 RID: 9088
	LowPressureFatal = -593125877,
	// Token: 0x04002381 RID: 9089
	OptimalPressureAchieved = -907106982,
	// Token: 0x04002382 RID: 9090
	HighPressureWarning = 103243573,
	// Token: 0x04002383 RID: 9091
	HighPressureFatal = 646131325,
	// Token: 0x04002384 RID: 9092
	Wilt = -724860998,
	// Token: 0x04002385 RID: 9093
	WiltRecover = 712767498,
	// Token: 0x04002386 RID: 9094
	CropSleep = 655199271,
	// Token: 0x04002387 RID: 9095
	CropWakeUp = -1389112401,
	// Token: 0x04002388 RID: 9096
	Grow = -254803949,
	// Token: 0x04002389 RID: 9097
	BurstEmitDisease = 944091753,
	// Token: 0x0400238A RID: 9098
	LaserOn = -673283254,
	// Token: 0x0400238B RID: 9099
	LaserOff = -1559999068,
	// Token: 0x0400238C RID: 9100
	VentAnimatingChanged = -793429877,
	// Token: 0x0400238D RID: 9101
	VentClosed = -997182271,
	// Token: 0x0400238E RID: 9102
	VentOpen = 1531265279,
	// Token: 0x0400238F RID: 9103
	StartTransition = 1189352983,
	// Token: 0x04002390 RID: 9104
	Attacking = 1039067354,
	// Token: 0x04002391 RID: 9105
	StoppedAttacking = 379728621,
	// Token: 0x04002392 RID: 9106
	Attacked = -787691065,
	// Token: 0x04002393 RID: 9107
	HealthChanged = -1664904872,
	// Token: 0x04002394 RID: 9108
	BecameIncapacitated = -1506500077,
	// Token: 0x04002395 RID: 9109
	IncapacitationRecovery = -1256572400,
	// Token: 0x04002396 RID: 9110
	FullyHealed = -1491582671,
	// Token: 0x04002397 RID: 9111
	LayEgg = 1193600993,
	// Token: 0x04002398 RID: 9112
	DebugGoTo = 775300118,
	// Token: 0x04002399 RID: 9113
	Threatened = -96307134,
	// Token: 0x0400239A RID: 9114
	SafeFromThreats = -21431934,
	// Token: 0x0400239B RID: 9115
	BeginWalk = 1773898642,
	// Token: 0x0400239C RID: 9116
	EndWalk = 1597112836,
	// Token: 0x0400239D RID: 9117
	IsMovableChanged = -962627472,
	// Token: 0x0400239E RID: 9118
	CollectPriorityCommands = 809822742,
	// Token: 0x0400239F RID: 9119
	StartMining = -1762453998,
	// Token: 0x040023A0 RID: 9120
	StopMining = 939543986,
	// Token: 0x040023A1 RID: 9121
	OreSizeChanged = 1807976145,
	// Token: 0x040023A2 RID: 9122
	UIScaleChange = -810220474,
	// Token: 0x040023A3 RID: 9123
	NotStressed = -1175311898,
	// Token: 0x040023A4 RID: 9124
	Stressed = 402754227,
	// Token: 0x040023A5 RID: 9125
	StressedHadEnough = -115777784,
	// Token: 0x040023A6 RID: 9126
	ResearchPointsChanged = -125623018,
	// Token: 0x040023A7 RID: 9127
	ConduitContentsFrozen = -700727624,
	// Token: 0x040023A8 RID: 9128
	ConduitContentsBoiling = -1152799878,
	// Token: 0x040023A9 RID: 9129
	BuildingReceivedDamage = -1964935036,
	// Token: 0x040023AA RID: 9130
	BuildingBroken = 774203113,
	// Token: 0x040023AB RID: 9131
	BuildingPartiallyRepaired = -1699355994,
	// Token: 0x040023AC RID: 9132
	BuildingFullyRepaired = -1735440190,
	// Token: 0x040023AD RID: 9133
	DoBuildingDamage = -794517298,
	// Token: 0x040023AE RID: 9134
	UseBuilding = 1175726587,
	// Token: 0x040023AF RID: 9135
	Preserved = 751746776,
	// Token: 0x040023B0 RID: 9136
	AcousticDisturbance = -527751701,
	// Token: 0x040023B1 RID: 9137
	CopySettings = -905833192,
	// Token: 0x040023B2 RID: 9138
	AttachFollowCam = -1506069671,
	// Token: 0x040023B3 RID: 9139
	DetachFollowCam = -485480405,
	// Token: 0x040023B4 RID: 9140
	EmitterBlocked = 1615168894,
	// Token: 0x040023B5 RID: 9141
	EmitterUnblocked = -657992955,
	// Token: 0x040023B6 RID: 9142
	EnterPeaceful = -649988633,
	// Token: 0x040023B7 RID: 9143
	ExitPeaceful = -1215818035,
	// Token: 0x040023B8 RID: 9144
	BuildingOverheated = 1832602615,
	// Token: 0x040023B9 RID: 9145
	BuildingNoLongerOverheated = 171119937,
	// Token: 0x040023BA RID: 9146
	LiquidResourceEmpty = -370379773,
	// Token: 0x040023BB RID: 9147
	LiquidResourceRecieved = 207387507,
	// Token: 0x040023BC RID: 9148
	WrongAtmosphere = 221594799,
	// Token: 0x040023BD RID: 9149
	CorrectAtmosphere = 777259436,
	// Token: 0x040023BE RID: 9150
	ReceptacleOperational = 1628751838,
	// Token: 0x040023BF RID: 9151
	ReceptacleInoperational = 960378201,
	// Token: 0x040023C0 RID: 9152
	PathAdvanced = 1347184327,
	// Token: 0x040023C1 RID: 9153
	UpdateRoom = 144050788,
	// Token: 0x040023C2 RID: 9154
	ChangeRoom = -832141045,
	// Token: 0x040023C3 RID: 9155
	EquipmentChanged = -2146166042,
	// Token: 0x040023C4 RID: 9156
	LogicEvent = -801688580,
	// Token: 0x040023C5 RID: 9157
	BeginBreathRecovery = 961737054,
	// Token: 0x040023C6 RID: 9158
	EndBreathRecovery = -2037519664,
	// Token: 0x040023C7 RID: 9159
	FabricatorOrdersUpdated = 1721324763,
	// Token: 0x040023C8 RID: 9160
	TagsChanged = -1582839653,
	// Token: 0x040023C9 RID: 9161
	RolesUpdated = -1523247426,
	// Token: 0x040023CA RID: 9162
	AssignedRoleChanged = 540773776,
	// Token: 0x040023CB RID: 9163
	OnlyFetchMarkedItemsSettingChanged = 644822890,
	// Token: 0x040023CC RID: 9164
	OnlyFetchSpicedItemsSettingChanged = 1163645216,
	// Token: 0x040023CD RID: 9165
	BuildingCompleteDestroyed = -21016276,
	// Token: 0x040023CE RID: 9166
	BehaviourTagComplete = -739654666,
	// Token: 0x040023CF RID: 9167
	CreatureArrivedAtRanchStation = -1357116271,
	// Token: 0x040023D0 RID: 9168
	RancherReadyAtRanchStation = 1084749845,
	// Token: 0x040023D1 RID: 9169
	RanchingComplete = 1827504087,
	// Token: 0x040023D2 RID: 9170
	CreatureAbandonedRanchStation = -364750427,
	// Token: 0x040023D3 RID: 9171
	RanchStationNoLongerAvailable = 1689625967,
	// Token: 0x040023D4 RID: 9172
	CreatureUpdateRanch = 1445644124,
	// Token: 0x040023D5 RID: 9173
	CreatureArrivedAtCapturePoint = -1992722293,
	// Token: 0x040023D6 RID: 9174
	RancherReadyAtCapturePoint = 449143823,
	// Token: 0x040023D7 RID: 9175
	FixedCaptureComplete = 643180843,
	// Token: 0x040023D8 RID: 9176
	CreatureAbandonedCapturePoint = -1000356449,
	// Token: 0x040023D9 RID: 9177
	CapturePointNoLongerAvailable = 1034952693,
	// Token: 0x040023DA RID: 9178
	ElementNoLongerAvailable = 801383139,
	// Token: 0x040023DB RID: 9179
	BreedingChancesChanged = 1059811075,
	// Token: 0x040023DC RID: 9180
	ToggleSandbox = -1948169901,
	// Token: 0x040023DD RID: 9181
	ConsumePlant = -1793167409,
	// Token: 0x040023DE RID: 9182
	AteFromStorage = -1452790913,
	// Token: 0x040023DF RID: 9183
	GrowIntoAdult = 2143203559,
	// Token: 0x040023E0 RID: 9184
	SpawnedFrom = -2027483228,
	// Token: 0x040023E1 RID: 9185
	UserSettingsChanged = -543130682,
	// Token: 0x040023E2 RID: 9186
	StructureTemperatureRegistered = -1555603773,
	// Token: 0x040023E3 RID: 9187
	TopicDiscovered = 937885943,
	// Token: 0x040023E4 RID: 9188
	TopicDiscussed = 1102989392,
	// Token: 0x040023E5 RID: 9189
	StartedTalking = -594200555,
	// Token: 0x040023E6 RID: 9190
	StoppedTalking = 25860745,
	// Token: 0x040023E7 RID: 9191
	MetaUnlockUnlocked = 1594320620,
	// Token: 0x040023E8 RID: 9192
	UnstableGroundImpact = -975551167,
	// Token: 0x040023E9 RID: 9193
	ReturnRocket = 1366341636,
	// Token: 0x040023EA RID: 9194
	DoLaunchRocket = 705820818,
	// Token: 0x040023EB RID: 9195
	DoReturnRocket = -1165815793,
	// Token: 0x040023EC RID: 9196
	RocketLanded = -887025858,
	// Token: 0x040023ED RID: 9197
	RocketLaunched = -1277991738,
	// Token: 0x040023EE RID: 9198
	RocketModuleChanged = 1512695988,
	// Token: 0x040023EF RID: 9199
	RequestRegisterLaunchCondition = 1711162550,
	// Token: 0x040023F0 RID: 9200
	IgniteEngine = -1358394196,
	// Token: 0x040023F1 RID: 9201
	RocketInteriorComplete = -71801987,
	// Token: 0x040023F2 RID: 9202
	RocketRequestLaunch = 191901966,
	// Token: 0x040023F3 RID: 9203
	RocketReadyToLaunch = -849456099,
	// Token: 0x040023F4 RID: 9204
	UpdateRocketStatus = -688990705,
	// Token: 0x040023F5 RID: 9205
	StartRocketLaunch = 546421097,
	// Token: 0x040023F6 RID: 9206
	RocketTouchDown = -735346771,
	// Token: 0x040023F7 RID: 9207
	RocketCreated = 374403796,
	// Token: 0x040023F8 RID: 9208
	ReorderableBuildingChanged = -1447108533,
	// Token: 0x040023F9 RID: 9209
	RocketRestrictionChanged = 1861523068,
	// Token: 0x040023FA RID: 9210
	StoragePriorityChanged = -1626373771,
	// Token: 0x040023FB RID: 9211
	StorageCapacityChanged = -945020481,
	// Token: 0x040023FC RID: 9212
	AttachmentNetworkChanged = 486707561,
	// Token: 0x040023FD RID: 9213
	LaunchConditionChanged = 1655598572,
	// Token: 0x040023FE RID: 9214
	BabyToAdult = 663420073,
	// Token: 0x040023FF RID: 9215
	ChoreInterrupt = 1485595942,
	// Token: 0x04002400 RID: 9216
	Flush = -350347868,
	// Token: 0x04002401 RID: 9217
	WorkerPlayPostAnim = -1142962013,
	// Token: 0x04002402 RID: 9218
	DeactivateResearchScreen = -1974454597,
	// Token: 0x04002403 RID: 9219
	StarmapDestinationChanged = 929158128,
	// Token: 0x04002404 RID: 9220
	DuplicantDied = 282337316,
	// Token: 0x04002405 RID: 9221
	DiscoveredSpace = -818188514,
	// Token: 0x04002406 RID: 9222
	StarmapAnalysisTargetChanged = 532901469,
	// Token: 0x04002407 RID: 9223
	MeteorShowerBombardStateBegins = 78366336,
	// Token: 0x04002408 RID: 9224
	MeteorShowerBombardStateEnds = 1749562766,
	// Token: 0x04002409 RID: 9225
	BeginMeteorBombardment = -84771526,
	// Token: 0x0400240A RID: 9226
	ClusterMapMeteorShowerIdentified = 1427028915,
	// Token: 0x0400240B RID: 9227
	ClusterMapMeteorShowerMoved = -1975776133,
	// Token: 0x0400240C RID: 9228
	PrefabInstanceIDRedirected = 17633999,
	// Token: 0x0400240D RID: 9229
	RootHealthChanged = 912965142,
	// Token: 0x0400240E RID: 9230
	HarvestDesignationChanged = -266953818,
	// Token: 0x0400240F RID: 9231
	TreeClimbComplete = -230295600,
	// Token: 0x04002410 RID: 9232
	EnteredYellowAlert = -741654735,
	// Token: 0x04002411 RID: 9233
	ExitedYellowAlert = -2062778933,
	// Token: 0x04002412 RID: 9234
	SkillPointAquired = 1505456302,
	// Token: 0x04002413 RID: 9235
	MedCotMinimumThresholdUpdated = 875045922,
	// Token: 0x04002414 RID: 9236
	GameOptionsUpdated = 75424175,
	// Token: 0x04002415 RID: 9237
	CheckColonyAchievements = 395452326,
	// Token: 0x04002416 RID: 9238
	StartWork = 1568504979,
	// Token: 0x04002417 RID: 9239
	WorkableStartWork = 853695848,
	// Token: 0x04002418 RID: 9240
	WorkableCompleteWork = -2011693419,
	// Token: 0x04002419 RID: 9241
	WorkableStopWork = 679550494,
	// Token: 0x0400241A RID: 9242
	WorkableEntombOffset = 580035959,
	// Token: 0x0400241B RID: 9243
	StartReactable = -909573545,
	// Token: 0x0400241C RID: 9244
	EndReactable = 824899998,
	// Token: 0x0400241D RID: 9245
	TeleporterIDsChanged = -1266722732,
	// Token: 0x0400241E RID: 9246
	ClusterDestinationChanged = 543433792,
	// Token: 0x0400241F RID: 9247
	ClusterDestinationReached = 1796608350,
	// Token: 0x04002420 RID: 9248
	ClusterLocationChanged = -1298331547,
	// Token: 0x04002421 RID: 9249
	TemperatureUnitChanged = 999382396,
	// Token: 0x04002422 RID: 9250
	GameplayEventStart = 1491341646,
	// Token: 0x04002423 RID: 9251
	GameplayEventMonitorStart = -1660384580,
	// Token: 0x04002424 RID: 9252
	GameplayEventMonitorChanged = -1122598290,
	// Token: 0x04002425 RID: 9253
	GameplayEventMonitorEnd = -828272459,
	// Token: 0x04002426 RID: 9254
	GameplayEventCommence = -2043101269,
	// Token: 0x04002427 RID: 9255
	GameplayEventEnd = 1287635015,
	// Token: 0x04002428 RID: 9256
	ClusterTelescopeTargetAdded = -1554423969,
	// Token: 0x04002429 RID: 9257
	ClusterFogOfWarRevealed = -1991583975,
	// Token: 0x0400242A RID: 9258
	ActiveWorldChanged = 1983128072,
	// Token: 0x0400242B RID: 9259
	DiscoveredWorldsChanged = -521212405,
	// Token: 0x0400242C RID: 9260
	WorldAdded = -1280433810,
	// Token: 0x0400242D RID: 9261
	WorldRemoved = -1078710002,
	// Token: 0x0400242E RID: 9262
	WorldParentChanged = 880851192,
	// Token: 0x0400242F RID: 9263
	WorldRenamed = 1943181844,
	// Token: 0x04002430 RID: 9264
	EntityMigration = 1142724171,
	// Token: 0x04002431 RID: 9265
	MinionMigration = 586301400,
	// Token: 0x04002432 RID: 9266
	MinionStorageChanged = -392340561,
	// Token: 0x04002433 RID: 9267
	NewWorldVisited = -434755240,
	// Token: 0x04002434 RID: 9268
	MinionDelta = 2144209314,
	// Token: 0x04002435 RID: 9269
	OnParticleStorageChanged = -1837862626,
	// Token: 0x04002436 RID: 9270
	OnParticleStorageEmpty = 155636535,
	// Token: 0x04002437 RID: 9271
	ParticleStorageCapacityChanged = -795826715,
	// Token: 0x04002438 RID: 9272
	PoorAirQuality = -935848905,
	// Token: 0x04002439 RID: 9273
	NameChanged = 1102426921,
	// Token: 0x0400243A RID: 9274
	TrapTriggered = -358342870,
	// Token: 0x0400243B RID: 9275
	TrapCaptureCompleted = -2138047022,
	// Token: 0x0400243C RID: 9276
	TrapArmWorkPST = -2025798095,
	// Token: 0x0400243D RID: 9277
	BuildingActivated = -1909216579,
	// Token: 0x0400243E RID: 9278
	EnterHome = -2099923209,
	// Token: 0x0400243F RID: 9279
	ExitHome = -1220248099,
	// Token: 0x04002440 RID: 9280
	JettisonedLander = 1792516731,
	// Token: 0x04002441 RID: 9281
	JettisonCargo = 218291192,
	// Token: 0x04002442 RID: 9282
	AssignmentGroupChanged = -1123234494,
	// Token: 0x04002443 RID: 9283
	SuitTankDelta = 608245985,
	// Token: 0x04002444 RID: 9284
	ChainedNetworkChanged = -1009905786,
	// Token: 0x04002445 RID: 9285
	FoundationChanged = -1960061727,
	// Token: 0x04002446 RID: 9286
	PlantSubspeciesProgress = -1531232972,
	// Token: 0x04002447 RID: 9287
	PlantSubspeciesComplete = -98362560,
	// Token: 0x04002448 RID: 9288
	LimitValveAmountChanged = -1722241721,
	// Token: 0x04002449 RID: 9289
	LockerDroppedContents = -372600542,
	// Token: 0x0400244A RID: 9290
	Happy = 1890751808,
	// Token: 0x0400244B RID: 9291
	Unhappy = -647798969,
	// Token: 0x0400244C RID: 9292
	DoorsLinked = -1118736034,
	// Token: 0x0400244D RID: 9293
	RocketSelfDestructRequested = -1061799784,
	// Token: 0x0400244E RID: 9294
	RocketExploded = -1311384361,
	// Token: 0x0400244F RID: 9295
	ModuleLanderLanded = 1591811118,
	// Token: 0x04002450 RID: 9296
	PartyLineJoined = 564760259,
	// Token: 0x04002451 RID: 9297
	ScreenResolutionChanged = 445618876,
	// Token: 0x04002452 RID: 9298
	GamepadUIModeChanged = -442024484,
	// Token: 0x04002453 RID: 9299
	CatchyTune = -1278274506,
	// Token: 0x04002454 RID: 9300
	MegaBrainTankCandidateDupesChanged = 374655100,
	// Token: 0x04002455 RID: 9301
	DreamsOn = -1768884913,
	// Token: 0x04002456 RID: 9302
	DreamsOff = 49503455,
	// Token: 0x04002457 RID: 9303
	MarkForMove = 1122777325,
	// Token: 0x04002458 RID: 9304
	RailGunLaunchMassChanged = 161772031
}
