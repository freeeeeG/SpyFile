using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000D86 RID: 3462
	public class BUILDINGS
	{
		// Token: 0x04004F24 RID: 20260
		public const float DEFAULT_STORAGE_CAPACITY = 2000f;

		// Token: 0x04004F25 RID: 20261
		public const float STANDARD_MANUAL_REFILL_LEVEL = 0.2f;

		// Token: 0x04004F26 RID: 20262
		public const float MASS_TEMPERATURE_SCALE = 0.2f;

		// Token: 0x04004F27 RID: 20263
		public const float AIRCONDITIONER_TEMPDELTA = -14f;

		// Token: 0x04004F28 RID: 20264
		public const float MAX_ENVIRONMENT_DELTA = -50f;

		// Token: 0x04004F29 RID: 20265
		public const float COMPOST_FLIP_TIME = 20f;

		// Token: 0x04004F2A RID: 20266
		public const int TUBE_LAUNCHER_MAX_CHARGES = 3;

		// Token: 0x04004F2B RID: 20267
		public const float TUBE_LAUNCHER_RECHARGE_TIME = 10f;

		// Token: 0x04004F2C RID: 20268
		public const float TUBE_LAUNCHER_WORK_TIME = 1f;

		// Token: 0x04004F2D RID: 20269
		public const float SMELTER_INGOT_INPUTKG = 500f;

		// Token: 0x04004F2E RID: 20270
		public const float SMELTER_INGOT_OUTPUTKG = 100f;

		// Token: 0x04004F2F RID: 20271
		public const float SMELTER_FABRICATIONTIME = 120f;

		// Token: 0x04004F30 RID: 20272
		public const float GEOREFINERY_SLAB_INPUTKG = 1000f;

		// Token: 0x04004F31 RID: 20273
		public const float GEOREFINERY_SLAB_OUTPUTKG = 200f;

		// Token: 0x04004F32 RID: 20274
		public const float GEOREFINERY_FABRICATIONTIME = 120f;

		// Token: 0x04004F33 RID: 20275
		public const float MASS_BURN_RATE_HYDROGENGENERATOR = 0.1f;

		// Token: 0x04004F34 RID: 20276
		public const float COOKER_FOOD_TEMPERATURE = 368.15f;

		// Token: 0x04004F35 RID: 20277
		public const float OVERHEAT_DAMAGE_INTERVAL = 7.5f;

		// Token: 0x04004F36 RID: 20278
		public const float MIN_BUILD_TEMPERATURE = 288.15f;

		// Token: 0x04004F37 RID: 20279
		public const float MAX_BUILD_TEMPERATURE = 318.15f;

		// Token: 0x04004F38 RID: 20280
		public const float MELTDOWN_TEMPERATURE = 533.15f;

		// Token: 0x04004F39 RID: 20281
		public const float REPAIR_FORCE_TEMPERATURE = 293.15f;

		// Token: 0x04004F3A RID: 20282
		public const int REPAIR_EFFECTIVENESS_BASE = 10;

		// Token: 0x04004F3B RID: 20283
		public static Dictionary<string, string> PLANSUBCATEGORYSORTING = new Dictionary<string, string>
		{
			{
				"Ladder",
				"ladders"
			},
			{
				"FirePole",
				"ladders"
			},
			{
				"LadderFast",
				"ladders"
			},
			{
				"Tile",
				"tiles"
			},
			{
				"GasPermeableMembrane",
				"tiles"
			},
			{
				"MeshTile",
				"tiles"
			},
			{
				"InsulationTile",
				"tiles"
			},
			{
				"PlasticTile",
				"tiles"
			},
			{
				"MetalTile",
				"tiles"
			},
			{
				"GlassTile",
				"tiles"
			},
			{
				"BunkerTile",
				"tiles"
			},
			{
				"ExteriorWall",
				"tiles"
			},
			{
				"CarpetTile",
				"tiles"
			},
			{
				"ExobaseHeadquarters",
				"printingpods"
			},
			{
				"Door",
				"doors"
			},
			{
				"ManualPressureDoor",
				"doors"
			},
			{
				"PressureDoor",
				"doors"
			},
			{
				"BunkerDoor",
				"doors"
			},
			{
				"StorageLocker",
				"storage"
			},
			{
				"StorageLockerSmart",
				"storage"
			},
			{
				"LiquidReservoir",
				"storage"
			},
			{
				"GasReservoir",
				"storage"
			},
			{
				"ObjectDispenser",
				"storage"
			},
			{
				"TravelTube",
				"transport"
			},
			{
				"TravelTubeEntrance",
				"transport"
			},
			{
				"TravelTubeWallBridge",
				"transport"
			},
			{
				"MineralDeoxidizer",
				"producers"
			},
			{
				"SublimationStation",
				"producers"
			},
			{
				"Electrolyzer",
				"producers"
			},
			{
				"RustDeoxidizer",
				"producers"
			},
			{
				"AirFilter",
				"scrubbers"
			},
			{
				"CO2Scrubber",
				"scrubbers"
			},
			{
				"AlgaeHabitat",
				"scrubbers"
			},
			{
				"DevGenerator",
				"generators"
			},
			{
				"ManualGenerator",
				"generators"
			},
			{
				"Generator",
				"generators"
			},
			{
				"WoodGasGenerator",
				"generators"
			},
			{
				"HydrogenGenerator",
				"generators"
			},
			{
				"MethaneGenerator",
				"generators"
			},
			{
				"PetroleumGenerator",
				"generators"
			},
			{
				"SteamTurbine",
				"generators"
			},
			{
				"SteamTurbine2",
				"generators"
			},
			{
				"SolarPanel",
				"generators"
			},
			{
				"Wire",
				"wires"
			},
			{
				"WireBridge",
				"wires"
			},
			{
				"HighWattageWire",
				"wires"
			},
			{
				"WireBridgeHighWattage",
				"wires"
			},
			{
				"WireRefined",
				"wires"
			},
			{
				"WireRefinedBridge",
				"wires"
			},
			{
				"WireRefinedHighWattage",
				"wires"
			},
			{
				"WireRefinedBridgeHighWattage",
				"wires"
			},
			{
				"Battery",
				"batteries"
			},
			{
				"BatteryMedium",
				"batteries"
			},
			{
				"BatterySmart",
				"batteries"
			},
			{
				"PowerTransformerSmall",
				"powercontrol"
			},
			{
				"PowerTransformer",
				"powercontrol"
			},
			{
				SwitchConfig.ID,
				"switches"
			},
			{
				LogicPowerRelayConfig.ID,
				"switches"
			},
			{
				TemperatureControlledSwitchConfig.ID,
				"switches"
			},
			{
				PressureSwitchLiquidConfig.ID,
				"switches"
			},
			{
				PressureSwitchGasConfig.ID,
				"switches"
			},
			{
				"MicrobeMusher",
				"cooking"
			},
			{
				"CookingStation",
				"cooking"
			},
			{
				"GourmetCookingStation",
				"cooking"
			},
			{
				"SpiceGrinder",
				"cooking"
			},
			{
				"PlanterBox",
				"farming"
			},
			{
				"FarmTile",
				"farming"
			},
			{
				"HydroponicFarm",
				"farming"
			},
			{
				"RationBox",
				"storage"
			},
			{
				"Refrigerator",
				"storage"
			},
			{
				"CreatureDeliveryPoint",
				"ranching"
			},
			{
				"FishDeliveryPoint",
				"ranching"
			},
			{
				"CreatureFeeder",
				"ranching"
			},
			{
				"FishFeeder",
				"ranching"
			},
			{
				"MilkFeeder",
				"ranching"
			},
			{
				"EggIncubator",
				"ranching"
			},
			{
				"EggCracker",
				"ranching"
			},
			{
				"CreatureGroundTrap",
				"ranching"
			},
			{
				"CreatureAirTrap",
				"ranching"
			},
			{
				"WaterTrap",
				"ranching"
			},
			{
				"Outhouse",
				"washroom"
			},
			{
				"FlushToilet",
				"washroom"
			},
			{
				"WallToilet",
				"washroom"
			},
			{
				ShowerConfig.ID,
				"washroom"
			},
			{
				"LiquidConduit",
				"pipes"
			},
			{
				"InsulatedLiquidConduit",
				"pipes"
			},
			{
				"LiquidConduitRadiant",
				"pipes"
			},
			{
				"LiquidConduitBridge",
				"pipes"
			},
			{
				"ContactConductivePipeBridge",
				"pipes"
			},
			{
				"LiquidVent",
				"pipes"
			},
			{
				"LiquidPump",
				"pumps"
			},
			{
				"LiquidMiniPump",
				"pumps"
			},
			{
				"LiquidPumpingStation",
				"pumps"
			},
			{
				"DevPumpLiquid",
				"pumps"
			},
			{
				"BottleEmptier",
				"valves"
			},
			{
				"LiquidFilter",
				"valves"
			},
			{
				"LiquidConduitPreferentialFlow",
				"valves"
			},
			{
				"LiquidConduitOverflow",
				"valves"
			},
			{
				"LiquidValve",
				"valves"
			},
			{
				"LiquidLogicValve",
				"valves"
			},
			{
				"LiquidLimitValve",
				"valves"
			},
			{
				LiquidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortLiquid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortLiquidUnloader",
				"buildmenuports"
			},
			{
				"GasConduit",
				"pipes"
			},
			{
				"InsulatedGasConduit",
				"pipes"
			},
			{
				"GasConduitRadiant",
				"pipes"
			},
			{
				"GasConduitBridge",
				"pipes"
			},
			{
				"GasVent",
				"pipes"
			},
			{
				"GasVentHighPressure",
				"pipes"
			},
			{
				"GasPump",
				"pumps"
			},
			{
				"GasMiniPump",
				"pumps"
			},
			{
				"DevPumpGas",
				"pumps"
			},
			{
				"GasBottler",
				"valves"
			},
			{
				"BottleEmptierGas",
				"valves"
			},
			{
				"GasFilter",
				"valves"
			},
			{
				"GasConduitPreferentialFlow",
				"valves"
			},
			{
				"GasConduitOverflow",
				"valves"
			},
			{
				"GasValve",
				"valves"
			},
			{
				"GasLogicValve",
				"valves"
			},
			{
				"GasLimitValve",
				"valves"
			},
			{
				GasConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortGas",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortGasUnloader",
				"buildmenuports"
			},
			{
				"Compost",
				"organic"
			},
			{
				"FertilizerMaker",
				"organic"
			},
			{
				"AlgaeDistillery",
				"organic"
			},
			{
				"EthanolDistillery",
				"organic"
			},
			{
				"SludgePress",
				"organic"
			},
			{
				"MilkFatSeparator",
				"organic"
			},
			{
				"MilkPress",
				"organic"
			},
			{
				"WaterPurifier",
				"materials"
			},
			{
				"Desalinator",
				"materials"
			},
			{
				"RockCrusher",
				"materials"
			},
			{
				"Kiln",
				"materials"
			},
			{
				"MetalRefinery",
				"materials"
			},
			{
				"GlassForge",
				"materials"
			},
			{
				"OilRefinery",
				"oil"
			},
			{
				"Polymerizer",
				"oil"
			},
			{
				"OxyliteRefinery",
				"advanced"
			},
			{
				"SupermaterialRefinery",
				"advanced"
			},
			{
				"DiamondPress",
				"advanced"
			},
			{
				"Chlorinator",
				"advanced"
			},
			{
				"WashBasin",
				"hygiene"
			},
			{
				"WashSink",
				"hygiene"
			},
			{
				"HandSanitizer",
				"hygiene"
			},
			{
				"DecontaminationShower",
				"hygiene"
			},
			{
				"Apothecary",
				"medical"
			},
			{
				"DoctorStation",
				"medical"
			},
			{
				"AdvancedDoctorStation",
				"medical"
			},
			{
				"MedicalCot",
				"medical"
			},
			{
				"DevLifeSupport",
				"medical"
			},
			{
				"MassageTable",
				"wellness"
			},
			{
				"Grave",
				"wellness"
			},
			{
				"Bed",
				"beds"
			},
			{
				"LuxuryBed",
				"beds"
			},
			{
				LadderBedConfig.ID,
				"beds"
			},
			{
				"FloorLamp",
				"lights"
			},
			{
				"CeilingLight",
				"lights"
			},
			{
				"SunLamp",
				"lights"
			},
			{
				"DevLightGenerator",
				"lights"
			},
			{
				"DiningTable",
				"dining"
			},
			{
				"WaterCooler",
				"recreation"
			},
			{
				"Phonobox",
				"recreation"
			},
			{
				"ArcadeMachine",
				"recreation"
			},
			{
				"EspressoMachine",
				"recreation"
			},
			{
				"HotTub",
				"recreation"
			},
			{
				"MechanicalSurfboard",
				"recreation"
			},
			{
				"Sauna",
				"recreation"
			},
			{
				"Juicer",
				"recreation"
			},
			{
				"SodaFountain",
				"recreation"
			},
			{
				"BeachChair",
				"recreation"
			},
			{
				"VerticalWindTunnel",
				"recreation"
			},
			{
				"Telephone",
				"recreation"
			},
			{
				"FlowerVase",
				"decor"
			},
			{
				"FlowerVaseWall",
				"decor"
			},
			{
				"FlowerVaseHanging",
				"decor"
			},
			{
				"FlowerVaseHangingFancy",
				"decor"
			},
			{
				PixelPackConfig.ID,
				"decor"
			},
			{
				"SmallSculpture",
				"decor"
			},
			{
				"Sculpture",
				"decor"
			},
			{
				"IceSculpture",
				"decor"
			},
			{
				"MarbleSculpture",
				"decor"
			},
			{
				"MetalSculpture",
				"decor"
			},
			{
				"CrownMoulding",
				"decor"
			},
			{
				"CornerMoulding",
				"decor"
			},
			{
				"Canvas",
				"decor"
			},
			{
				"CanvasWide",
				"decor"
			},
			{
				"CanvasTall",
				"decor"
			},
			{
				"ItemPedestal",
				"decor"
			},
			{
				"ParkSign",
				"decor"
			},
			{
				"MonumentBottom",
				"decor"
			},
			{
				"MonumentMiddle",
				"decor"
			},
			{
				"MonumentTop",
				"decor"
			},
			{
				"ResearchCenter",
				"research"
			},
			{
				"AdvancedResearchCenter",
				"research"
			},
			{
				"GeoTuner",
				"research"
			},
			{
				"NuclearResearchCenter",
				"research"
			},
			{
				"OrbitalResearchCenter",
				"research"
			},
			{
				"CosmicResearchCenter",
				"research"
			},
			{
				"DLC1CosmicResearchCenter",
				"research"
			},
			{
				"ArtifactAnalysisStation",
				"archaeology"
			},
			{
				"MissileFabricator",
				"meteordefense"
			},
			{
				"AstronautTrainingCenter",
				"exploration"
			},
			{
				"PowerControlStation",
				"industrialstation"
			},
			{
				"ResetSkillsStation",
				"industrialstation"
			},
			{
				"RoleStation",
				"workstations"
			},
			{
				"RanchStation",
				"ranching"
			},
			{
				"ShearingStation",
				"ranching"
			},
			{
				"MilkingStation",
				"ranching"
			},
			{
				"FarmStation",
				"farming"
			},
			{
				"GeneticAnalysisStation",
				"farming"
			},
			{
				"CraftingTable",
				"manufacturing"
			},
			{
				"ClothingFabricator",
				"manufacturing"
			},
			{
				"ClothingAlterationStation",
				"manufacturing"
			},
			{
				"SuitFabricator",
				"manufacturing"
			},
			{
				"OxygenMaskMarker",
				"equipment"
			},
			{
				"OxygenMaskLocker",
				"equipment"
			},
			{
				"SuitMarker",
				"equipment"
			},
			{
				"SuitLocker",
				"equipment"
			},
			{
				"JetSuitMarker",
				"equipment"
			},
			{
				"JetSuitLocker",
				"equipment"
			},
			{
				"MissileLauncher",
				"missiles"
			},
			{
				"LeadSuitMarker",
				"equipment"
			},
			{
				"LeadSuitLocker",
				"equipment"
			},
			{
				"SpaceHeater",
				"temperature"
			},
			{
				"LiquidHeater",
				"temperature"
			},
			{
				"LiquidConditioner",
				"temperature"
			},
			{
				"LiquidCooledFan",
				"temperature"
			},
			{
				"IceCooledFan",
				"temperature"
			},
			{
				"IceMachine",
				"temperature"
			},
			{
				"AirConditioner",
				"temperature"
			},
			{
				"ThermalBlock",
				"temperature"
			},
			{
				"OreScrubber",
				"sanitation"
			},
			{
				"OilWellCap",
				"oil"
			},
			{
				"SweepBotStation",
				"sanitation"
			},
			{
				"LogicWire",
				"wires"
			},
			{
				"LogicWireBridge",
				"wires"
			},
			{
				"LogicRibbon",
				"wires"
			},
			{
				"LogicRibbonBridge",
				"wires"
			},
			{
				LogicRibbonReaderConfig.ID,
				"wires"
			},
			{
				LogicRibbonWriterConfig.ID,
				"wires"
			},
			{
				"LogicDuplicantSensor",
				"sensors"
			},
			{
				LogicPressureSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicPressureSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				LogicLightSensorConfig.ID,
				"sensors"
			},
			{
				LogicWattageSensorConfig.ID,
				"sensors"
			},
			{
				LogicTimeOfDaySensorConfig.ID,
				"sensors"
			},
			{
				LogicTimerSensorConfig.ID,
				"sensors"
			},
			{
				LogicDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicCritterCountSensorConfig.ID,
				"sensors"
			},
			{
				LogicRadiationSensorConfig.ID,
				"sensors"
			},
			{
				LogicHEPSensorConfig.ID,
				"sensors"
			},
			{
				CometDetectorConfig.ID,
				"sensors"
			},
			{
				LogicCounterConfig.ID,
				"logicmanager"
			},
			{
				"Checkpoint",
				"logicmanager"
			},
			{
				LogicAlarmConfig.ID,
				"logicmanager"
			},
			{
				LogicHammerConfig.ID,
				"logicaudio"
			},
			{
				LogicSwitchConfig.ID,
				"switches"
			},
			{
				"FloorSwitch",
				"switches"
			},
			{
				"LogicGateNOT",
				"logicgates"
			},
			{
				"LogicGateAND",
				"logicgates"
			},
			{
				"LogicGateOR",
				"logicgates"
			},
			{
				"LogicGateBUFFER",
				"logicgates"
			},
			{
				"LogicGateFILTER",
				"logicgates"
			},
			{
				"LogicGateXOR",
				"logicgates"
			},
			{
				LogicMemoryConfig.ID,
				"logicgates"
			},
			{
				"LogicGateMultiplexer",
				"logicgates"
			},
			{
				"LogicGateDemultiplexer",
				"logicgates"
			},
			{
				"LogicInterasteroidSender",
				"transmissions"
			},
			{
				"LogicInterasteroidReceiver",
				"transmissions"
			},
			{
				"SolidConduit",
				"conveyancestructures"
			},
			{
				"SolidConduitBridge",
				"conveyancestructures"
			},
			{
				"SolidConduitInbox",
				"conveyancestructures"
			},
			{
				"SolidConduitOutbox",
				"conveyancestructures"
			},
			{
				"SolidFilter",
				"conveyancestructures"
			},
			{
				"SolidVent",
				"conveyancestructures"
			},
			{
				"DevPumpSolid",
				"pumps"
			},
			{
				"SolidLogicValve",
				"valves"
			},
			{
				"SolidLimitValve",
				"valves"
			},
			{
				SolidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"AutoMiner",
				"automated"
			},
			{
				"SolidTransferArm",
				"automated"
			},
			{
				"ModularLaunchpadPortSolid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortSolidUnloader",
				"buildmenuports"
			},
			{
				"Telescope",
				"telescopes"
			},
			{
				"ClusterTelescope",
				"telescopes"
			},
			{
				"ClusterTelescopeEnclosed",
				"telescopes"
			},
			{
				"LaunchPad",
				"rocketstructures"
			},
			{
				"Gantry",
				"rocketstructures"
			},
			{
				"RailGun",
				"fittings"
			},
			{
				"RailGunPayloadOpener",
				"fittings"
			},
			{
				"LandingBeacon",
				"rocketnav"
			},
			{
				"SteamEngine",
				"engines"
			},
			{
				"KeroseneEngine",
				"engines"
			},
			{
				"HydrogenEngine",
				"engines"
			},
			{
				"SolidBooster",
				"engines"
			},
			{
				"LiquidFuelTank",
				"tanks"
			},
			{
				"OxidizerTank",
				"tanks"
			},
			{
				"OxidizerTankLiquid",
				"tanks"
			},
			{
				"CargoBay",
				"cargo"
			},
			{
				"GasCargoBay",
				"cargo"
			},
			{
				"LiquidCargoBay",
				"cargo"
			},
			{
				"SpecialCargoBay",
				"cargo"
			},
			{
				"CommandModule",
				"rocketnav"
			},
			{
				RocketControlStationConfig.ID,
				"rocketnav"
			},
			{
				LogicClusterLocationSensorConfig.ID,
				"rocketnav"
			},
			{
				"MissionControl",
				"rocketnav"
			},
			{
				"MissionControlCluster",
				"rocketnav"
			},
			{
				"TouristModule",
				"module"
			},
			{
				"ResearchModule",
				"module"
			},
			{
				"RocketInteriorPowerPlug",
				"fittings"
			},
			{
				"RocketInteriorLiquidInput",
				"fittings"
			},
			{
				"RocketInteriorLiquidOutput",
				"fittings"
			},
			{
				"RocketInteriorGasInput",
				"fittings"
			},
			{
				"RocketInteriorGasOutput",
				"fittings"
			},
			{
				"RocketInteriorSolidInput",
				"fittings"
			},
			{
				"RocketInteriorSolidOutput",
				"fittings"
			},
			{
				"ManualHighEnergyParticleSpawner",
				"producers"
			},
			{
				"HighEnergyParticleSpawner",
				"producers"
			},
			{
				"DevHEPSpawner",
				"producers"
			},
			{
				"HighEnergyParticleRedirector",
				"transmissions"
			},
			{
				"HEPBattery",
				"batteries"
			},
			{
				"HEPBridgeTile",
				"transmissions"
			},
			{
				"NuclearReactor",
				"producers"
			},
			{
				"UraniumCentrifuge",
				"producers"
			},
			{
				"RadiationLight",
				"producers"
			},
			{
				"DevRadiationGenerator",
				"producers"
			}
		};

		// Token: 0x04004F3C RID: 20284
		public static List<PlanScreen.PlanInfo> PLANORDER = new List<PlanScreen.PlanInfo>
		{
			new PlanScreen.PlanInfo(new HashedString("Base"), false, new List<string>
			{
				"Ladder",
				"FirePole",
				"LadderFast",
				"Tile",
				"GasPermeableMembrane",
				"MeshTile",
				"InsulationTile",
				"PlasticTile",
				"MetalTile",
				"GlassTile",
				"BunkerTile",
				"CarpetTile",
				"ExteriorWall",
				"ExobaseHeadquarters",
				"Door",
				"ManualPressureDoor",
				"PressureDoor",
				"BunkerDoor",
				"StorageLocker",
				"StorageLockerSmart",
				"LiquidReservoir",
				"GasReservoir",
				"ObjectDispenser",
				"TravelTube",
				"TravelTubeEntrance",
				"TravelTubeWallBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Oxygen"), false, new List<string>
			{
				"MineralDeoxidizer",
				"SublimationStation",
				"AlgaeHabitat",
				"AirFilter",
				"CO2Scrubber",
				"Electrolyzer",
				"RustDeoxidizer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Power"), false, new List<string>
			{
				"DevGenerator",
				"ManualGenerator",
				"Generator",
				"WoodGasGenerator",
				"HydrogenGenerator",
				"MethaneGenerator",
				"PetroleumGenerator",
				"SteamTurbine",
				"SteamTurbine2",
				"SolarPanel",
				"Wire",
				"WireBridge",
				"HighWattageWire",
				"WireBridgeHighWattage",
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"Battery",
				"BatteryMedium",
				"BatterySmart",
				"PowerTransformerSmall",
				"PowerTransformer",
				SwitchConfig.ID,
				LogicPowerRelayConfig.ID,
				TemperatureControlledSwitchConfig.ID,
				PressureSwitchLiquidConfig.ID,
				PressureSwitchGasConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Food"), false, new List<string>
			{
				"MicrobeMusher",
				"CookingStation",
				"GourmetCookingStation",
				"SpiceGrinder",
				"PlanterBox",
				"FarmTile",
				"HydroponicFarm",
				"RationBox",
				"Refrigerator",
				"CreatureDeliveryPoint",
				"FishDeliveryPoint",
				"CreatureFeeder",
				"FishFeeder",
				"MilkFeeder",
				"EggIncubator",
				"EggCracker",
				"CreatureGroundTrap",
				"WaterTrap",
				"CreatureAirTrap"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Plumbing"), false, new List<string>
			{
				"DevPumpLiquid",
				"Outhouse",
				"FlushToilet",
				"WallToilet",
				ShowerConfig.ID,
				"LiquidPumpingStation",
				"BottleEmptier",
				"LiquidConduit",
				"InsulatedLiquidConduit",
				"LiquidConduitRadiant",
				"LiquidConduitBridge",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidPump",
				"LiquidMiniPump",
				"LiquidVent",
				"LiquidFilter",
				"LiquidValve",
				"LiquidLogicValve",
				"LiquidLimitValve",
				LiquidConduitElementSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LiquidConduitTemperatureSensorConfig.ID,
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"ContactConductivePipeBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HVAC"), false, new List<string>
			{
				"DevPumpGas",
				"GasConduit",
				"InsulatedGasConduit",
				"GasConduitRadiant",
				"GasConduitBridge",
				"GasConduitPreferentialFlow",
				"GasConduitOverflow",
				"GasPump",
				"GasMiniPump",
				"GasVent",
				"GasVentHighPressure",
				"GasFilter",
				"GasValve",
				"GasLogicValve",
				"GasLimitValve",
				"GasBottler",
				"BottleEmptierGas",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				GasConduitElementSensorConfig.ID,
				GasConduitDiseaseSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Refining"), false, new List<string>
			{
				"Compost",
				"WaterPurifier",
				"Desalinator",
				"FertilizerMaker",
				"AlgaeDistillery",
				"EthanolDistillery",
				"RockCrusher",
				"Kiln",
				"SludgePress",
				"MetalRefinery",
				"GlassForge",
				"OilRefinery",
				"Polymerizer",
				"OxyliteRefinery",
				"Chlorinator",
				"SupermaterialRefinery",
				"DiamondPress",
				"MilkFatSeparator",
				"MilkPress"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Medical"), false, new List<string>
			{
				"DevLifeSupport",
				"WashBasin",
				"WashSink",
				"HandSanitizer",
				"DecontaminationShower",
				"Apothecary",
				"DoctorStation",
				"AdvancedDoctorStation",
				"MedicalCot",
				"MassageTable",
				"Grave"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Furniture"), false, new List<string>
			{
				"Bed",
				"LuxuryBed",
				LadderBedConfig.ID,
				"FloorLamp",
				"CeilingLight",
				"SunLamp",
				"DevLightGenerator",
				"DiningTable",
				"WaterCooler",
				"Phonobox",
				"ArcadeMachine",
				"EspressoMachine",
				"HotTub",
				"MechanicalSurfboard",
				"Sauna",
				"Juicer",
				"SodaFountain",
				"BeachChair",
				"VerticalWindTunnel",
				PixelPackConfig.ID,
				"Telephone",
				"FlowerVase",
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"FlowerVaseHangingFancy",
				"SmallSculpture",
				"Sculpture",
				"IceSculpture",
				"MarbleSculpture",
				"MetalSculpture",
				"CrownMoulding",
				"CornerMoulding",
				"Canvas",
				"CanvasWide",
				"CanvasTall",
				"ItemPedestal",
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop",
				"ParkSign"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Equipment"), false, new List<string>
			{
				"ResearchCenter",
				"AdvancedResearchCenter",
				"NuclearResearchCenter",
				"OrbitalResearchCenter",
				"CosmicResearchCenter",
				"DLC1CosmicResearchCenter",
				"Telescope",
				"GeoTuner",
				"PowerControlStation",
				"FarmStation",
				"GeneticAnalysisStation",
				"RanchStation",
				"ShearingStation",
				"MilkingStation",
				"RoleStation",
				"ResetSkillsStation",
				"ArtifactAnalysisStation",
				"MissileFabricator",
				"CraftingTable",
				"ClothingFabricator",
				"ClothingAlterationStation",
				"SuitFabricator",
				"OxygenMaskMarker",
				"OxygenMaskLocker",
				"SuitMarker",
				"SuitLocker",
				"JetSuitMarker",
				"JetSuitLocker",
				"LeadSuitMarker",
				"LeadSuitLocker",
				"AstronautTrainingCenter"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Utilities"), true, new List<string>
			{
				"SpaceHeater",
				"LiquidHeater",
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"AirConditioner",
				"LiquidConditioner",
				"OreScrubber",
				"OilWellCap",
				"ThermalBlock",
				"SweepBotStation"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Automation"), true, new List<string>
			{
				"LogicWire",
				"LogicWireBridge",
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicSwitchConfig.ID,
				"LogicDuplicantSensor",
				LogicPressureSensorGasConfig.ID,
				LogicPressureSensorLiquidConfig.ID,
				LogicTemperatureSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicWattageSensorConfig.ID,
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicDiseaseSensorConfig.ID,
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				LogicCritterCountSensorConfig.ID,
				LogicRadiationSensorConfig.ID,
				LogicHEPSensorConfig.ID,
				LogicCounterConfig.ID,
				LogicAlarmConfig.ID,
				LogicHammerConfig.ID,
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver",
				LogicRibbonReaderConfig.ID,
				LogicRibbonWriterConfig.ID,
				"FloorSwitch",
				"Checkpoint",
				CometDetectorConfig.ID,
				"LogicGateNOT",
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER",
				"LogicGateXOR",
				LogicMemoryConfig.ID,
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Conveyance"), true, new List<string>
			{
				"DevPumpSolid",
				"SolidTransferArm",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidConduitInbox",
				"SolidConduitOutbox",
				"SolidFilter",
				"SolidVent",
				"SolidLogicValve",
				"SolidLimitValve",
				SolidConduitDiseaseSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitTemperatureSensorConfig.ID,
				"AutoMiner",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Rocketry"), true, new List<string>
			{
				"ClusterTelescope",
				"ClusterTelescopeEnclosed",
				"MissionControl",
				"MissionControlCluster",
				"LaunchPad",
				"Gantry",
				"SteamEngine",
				"KeroseneEngine",
				"SolidBooster",
				"LiquidFuelTank",
				"OxidizerTank",
				"OxidizerTankLiquid",
				"CargoBay",
				"GasCargoBay",
				"LiquidCargoBay",
				"CommandModule",
				"TouristModule",
				"ResearchModule",
				"SpecialCargoBay",
				"HydrogenEngine",
				RocketControlStationConfig.ID,
				"RocketInteriorPowerPlug",
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				LogicClusterLocationSensorConfig.ID,
				"RailGun",
				"RailGunPayloadOpener",
				"LandingBeacon",
				"MissileLauncher"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HEP"), true, new List<string>
			{
				"RadiationLight",
				"ManualHighEnergyParticleSpawner",
				"NuclearReactor",
				"UraniumCentrifuge",
				"HighEnergyParticleSpawner",
				"DevHEPSpawner",
				"HighEnergyParticleRedirector",
				"HEPBattery",
				"HEPBridgeTile",
				"DevRadiationGenerator"
			}, "EXPANSION1_ID")
		};

		// Token: 0x04004F3D RID: 20285
		public static List<Type> COMPONENT_DESCRIPTION_ORDER = new List<Type>
		{
			typeof(BottleEmptier),
			typeof(CookingStation),
			typeof(GourmetCookingStation),
			typeof(RoleStation),
			typeof(ResearchCenter),
			typeof(NuclearResearchCenter),
			typeof(LiquidCooledFan),
			typeof(HandSanitizer),
			typeof(HandSanitizer.Work),
			typeof(PlantAirConditioner),
			typeof(Clinic),
			typeof(BuildingElementEmitter),
			typeof(ElementConverter),
			typeof(ElementConsumer),
			typeof(PassiveElementConsumer),
			typeof(TinkerStation),
			typeof(EnergyConsumer),
			typeof(AirConditioner),
			typeof(Storage),
			typeof(Battery),
			typeof(AirFilter),
			typeof(FlushToilet),
			typeof(Toilet),
			typeof(EnergyGenerator),
			typeof(MassageTable),
			typeof(Shower),
			typeof(Ownable),
			typeof(PlantablePlot),
			typeof(RelaxationPoint),
			typeof(BuildingComplete),
			typeof(Building),
			typeof(BuildingPreview),
			typeof(BuildingUnderConstruction),
			typeof(Crop),
			typeof(Growing),
			typeof(Equippable),
			typeof(ColdBreather),
			typeof(ResearchPointObject),
			typeof(SuitTank),
			typeof(IlluminationVulnerable),
			typeof(TemperatureVulnerable),
			typeof(PressureVulnerable),
			typeof(SubmersionMonitor),
			typeof(BatterySmart),
			typeof(Compost),
			typeof(Refrigerator),
			typeof(Bed),
			typeof(OreScrubber),
			typeof(OreScrubber.Work),
			typeof(MinimumOperatingTemperature),
			typeof(RoomTracker),
			typeof(EnergyConsumerSelfSustaining),
			typeof(ArcadeMachine),
			typeof(Telescope),
			typeof(EspressoMachine),
			typeof(JetSuitTank),
			typeof(Phonobox),
			typeof(ArcadeMachine),
			typeof(BeachChair),
			typeof(Sauna),
			typeof(VerticalWindTunnel),
			typeof(HotTub),
			typeof(Juicer),
			typeof(SodaFountain),
			typeof(MechanicalSurfboard),
			typeof(BottleEmptier),
			typeof(AccessControl),
			typeof(GammaRayOven),
			typeof(Reactor),
			typeof(HighEnergyParticlePort),
			typeof(LeadSuitTank),
			typeof(ActiveParticleConsumer.Def),
			typeof(WaterCooler),
			typeof(Edible),
			typeof(PlantableSeed),
			typeof(SicknessTrigger),
			typeof(MedicinalPill),
			typeof(SeedProducer),
			typeof(Geyser),
			typeof(SpaceHeater),
			typeof(Overheatable),
			typeof(CreatureCalorieMonitor.Def),
			typeof(LureableMonitor.Def),
			typeof(CropSleepingMonitor.Def),
			typeof(FertilizationMonitor.Def),
			typeof(IrrigationMonitor.Def),
			typeof(ScaleGrowthMonitor.Def),
			typeof(TravelTubeEntrance.Work),
			typeof(ToiletWorkableUse),
			typeof(ReceptacleMonitor),
			typeof(Light2D),
			typeof(Ladder),
			typeof(SimCellOccupier),
			typeof(Vent),
			typeof(LogicPorts),
			typeof(Capturable),
			typeof(Trappable),
			typeof(SpaceArtifact),
			typeof(MessStation),
			typeof(PlantElementEmitter),
			typeof(Radiator),
			typeof(DecorProvider)
		};

		// Token: 0x02001C5E RID: 7262
		public class PHARMACY
		{
			// Token: 0x0200223F RID: 8767
			public class FABRICATIONTIME
			{
				// Token: 0x04009906 RID: 39174
				public const float TIER0 = 50f;

				// Token: 0x04009907 RID: 39175
				public const float TIER1 = 100f;

				// Token: 0x04009908 RID: 39176
				public const float TIER2 = 200f;
			}
		}

		// Token: 0x02001C5F RID: 7263
		public class NUCLEAR_REACTOR
		{
			// Token: 0x02002240 RID: 8768
			public class REACTOR_MASSES
			{
				// Token: 0x04009909 RID: 39177
				public const float MIN = 1f;

				// Token: 0x0400990A RID: 39178
				public const float MAX = 10f;
			}
		}

		// Token: 0x02001C60 RID: 7264
		public class OVERPRESSURE
		{
			// Token: 0x04008093 RID: 32915
			public const float TIER0 = 1.8f;
		}

		// Token: 0x02001C61 RID: 7265
		public class OVERHEAT_TEMPERATURES
		{
			// Token: 0x04008094 RID: 32916
			public const float LOW_3 = 10f;

			// Token: 0x04008095 RID: 32917
			public const float LOW_2 = 328.15f;

			// Token: 0x04008096 RID: 32918
			public const float LOW_1 = 338.15f;

			// Token: 0x04008097 RID: 32919
			public const float NORMAL = 348.15f;

			// Token: 0x04008098 RID: 32920
			public const float HIGH_1 = 363.15f;

			// Token: 0x04008099 RID: 32921
			public const float HIGH_2 = 398.15f;

			// Token: 0x0400809A RID: 32922
			public const float HIGH_3 = 1273.15f;

			// Token: 0x0400809B RID: 32923
			public const float HIGH_4 = 2273.15f;
		}

		// Token: 0x02001C62 RID: 7266
		public class OVERHEAT_MATERIAL_MOD
		{
			// Token: 0x0400809C RID: 32924
			public const float LOW_3 = -200f;

			// Token: 0x0400809D RID: 32925
			public const float LOW_2 = -20f;

			// Token: 0x0400809E RID: 32926
			public const float LOW_1 = -10f;

			// Token: 0x0400809F RID: 32927
			public const float NORMAL = 0f;

			// Token: 0x040080A0 RID: 32928
			public const float HIGH_1 = 15f;

			// Token: 0x040080A1 RID: 32929
			public const float HIGH_2 = 50f;

			// Token: 0x040080A2 RID: 32930
			public const float HIGH_3 = 200f;

			// Token: 0x040080A3 RID: 32931
			public const float HIGH_4 = 500f;

			// Token: 0x040080A4 RID: 32932
			public const float HIGH_5 = 900f;
		}

		// Token: 0x02001C63 RID: 7267
		public class DECOR_MATERIAL_MOD
		{
			// Token: 0x040080A5 RID: 32933
			public const float NORMAL = 0f;

			// Token: 0x040080A6 RID: 32934
			public const float HIGH_1 = 0.1f;

			// Token: 0x040080A7 RID: 32935
			public const float HIGH_2 = 0.2f;

			// Token: 0x040080A8 RID: 32936
			public const float HIGH_3 = 0.5f;

			// Token: 0x040080A9 RID: 32937
			public const float HIGH_4 = 1f;
		}

		// Token: 0x02001C64 RID: 7268
		public class CONSTRUCTION_MASS_KG
		{
			// Token: 0x040080AA RID: 32938
			public static readonly float[] TIER_TINY = new float[]
			{
				5f
			};

			// Token: 0x040080AB RID: 32939
			public static readonly float[] TIER0 = new float[]
			{
				25f
			};

			// Token: 0x040080AC RID: 32940
			public static readonly float[] TIER1 = new float[]
			{
				50f
			};

			// Token: 0x040080AD RID: 32941
			public static readonly float[] TIER2 = new float[]
			{
				100f
			};

			// Token: 0x040080AE RID: 32942
			public static readonly float[] TIER3 = new float[]
			{
				200f
			};

			// Token: 0x040080AF RID: 32943
			public static readonly float[] TIER4 = new float[]
			{
				400f
			};

			// Token: 0x040080B0 RID: 32944
			public static readonly float[] TIER5 = new float[]
			{
				800f
			};

			// Token: 0x040080B1 RID: 32945
			public static readonly float[] TIER6 = new float[]
			{
				1200f
			};

			// Token: 0x040080B2 RID: 32946
			public static readonly float[] TIER7 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02001C65 RID: 7269
		public class ROCKETRY_MASS_KG
		{
			// Token: 0x040080B3 RID: 32947
			public static float[] COMMAND_MODULE_MASS = new float[]
			{
				200f
			};

			// Token: 0x040080B4 RID: 32948
			public static float[] CARGO_MASS = new float[]
			{
				1000f
			};

			// Token: 0x040080B5 RID: 32949
			public static float[] CARGO_MASS_SMALL = new float[]
			{
				400f
			};

			// Token: 0x040080B6 RID: 32950
			public static float[] FUEL_TANK_DRY_MASS = new float[]
			{
				100f
			};

			// Token: 0x040080B7 RID: 32951
			public static float[] FUEL_TANK_WET_MASS = new float[]
			{
				900f
			};

			// Token: 0x040080B8 RID: 32952
			public static float[] FUEL_TANK_WET_MASS_SMALL = new float[]
			{
				300f
			};

			// Token: 0x040080B9 RID: 32953
			public static float[] FUEL_TANK_WET_MASS_GAS = new float[]
			{
				100f
			};

			// Token: 0x040080BA RID: 32954
			public static float[] FUEL_TANK_WET_MASS_GAS_LARGE = new float[]
			{
				150f
			};

			// Token: 0x040080BB RID: 32955
			public static float[] OXIDIZER_TANK_OXIDIZER_MASS = new float[]
			{
				900f
			};

			// Token: 0x040080BC RID: 32956
			public static float[] ENGINE_MASS_SMALL = new float[]
			{
				200f
			};

			// Token: 0x040080BD RID: 32957
			public static float[] ENGINE_MASS_LARGE = new float[]
			{
				500f
			};

			// Token: 0x040080BE RID: 32958
			public static float[] NOSE_CONE_TIER1 = new float[]
			{
				200f,
				100f
			};

			// Token: 0x040080BF RID: 32959
			public static float[] NOSE_CONE_TIER2 = new float[]
			{
				400f,
				200f
			};

			// Token: 0x040080C0 RID: 32960
			public static float[] HOLLOW_TIER1 = new float[]
			{
				200f
			};

			// Token: 0x040080C1 RID: 32961
			public static float[] HOLLOW_TIER2 = new float[]
			{
				400f
			};

			// Token: 0x040080C2 RID: 32962
			public static float[] HOLLOW_TIER3 = new float[]
			{
				800f
			};

			// Token: 0x040080C3 RID: 32963
			public static float[] DENSE_TIER0 = new float[]
			{
				200f
			};

			// Token: 0x040080C4 RID: 32964
			public static float[] DENSE_TIER1 = new float[]
			{
				500f
			};

			// Token: 0x040080C5 RID: 32965
			public static float[] DENSE_TIER2 = new float[]
			{
				1000f
			};

			// Token: 0x040080C6 RID: 32966
			public static float[] DENSE_TIER3 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02001C66 RID: 7270
		public class ENERGY_CONSUMPTION_WHEN_ACTIVE
		{
			// Token: 0x040080C7 RID: 32967
			public const float TIER0 = 0f;

			// Token: 0x040080C8 RID: 32968
			public const float TIER1 = 5f;

			// Token: 0x040080C9 RID: 32969
			public const float TIER2 = 60f;

			// Token: 0x040080CA RID: 32970
			public const float TIER3 = 120f;

			// Token: 0x040080CB RID: 32971
			public const float TIER4 = 240f;

			// Token: 0x040080CC RID: 32972
			public const float TIER5 = 480f;

			// Token: 0x040080CD RID: 32973
			public const float TIER6 = 960f;

			// Token: 0x040080CE RID: 32974
			public const float TIER7 = 1200f;

			// Token: 0x040080CF RID: 32975
			public const float TIER8 = 1600f;
		}

		// Token: 0x02001C67 RID: 7271
		public class EXHAUST_ENERGY_ACTIVE
		{
			// Token: 0x040080D0 RID: 32976
			public const float TIER0 = 0f;

			// Token: 0x040080D1 RID: 32977
			public const float TIER1 = 0.125f;

			// Token: 0x040080D2 RID: 32978
			public const float TIER2 = 0.25f;

			// Token: 0x040080D3 RID: 32979
			public const float TIER3 = 0.5f;

			// Token: 0x040080D4 RID: 32980
			public const float TIER4 = 1f;

			// Token: 0x040080D5 RID: 32981
			public const float TIER5 = 2f;

			// Token: 0x040080D6 RID: 32982
			public const float TIER6 = 4f;

			// Token: 0x040080D7 RID: 32983
			public const float TIER7 = 8f;

			// Token: 0x040080D8 RID: 32984
			public const float TIER8 = 16f;
		}

		// Token: 0x02001C68 RID: 7272
		public class JOULES_LEAK_PER_CYCLE
		{
			// Token: 0x040080D9 RID: 32985
			public const float TIER0 = 400f;

			// Token: 0x040080DA RID: 32986
			public const float TIER1 = 1000f;

			// Token: 0x040080DB RID: 32987
			public const float TIER2 = 2000f;
		}

		// Token: 0x02001C69 RID: 7273
		public class SELF_HEAT_KILOWATTS
		{
			// Token: 0x040080DC RID: 32988
			public const float TIER0 = 0f;

			// Token: 0x040080DD RID: 32989
			public const float TIER1 = 0.5f;

			// Token: 0x040080DE RID: 32990
			public const float TIER2 = 1f;

			// Token: 0x040080DF RID: 32991
			public const float TIER3 = 2f;

			// Token: 0x040080E0 RID: 32992
			public const float TIER4 = 4f;

			// Token: 0x040080E1 RID: 32993
			public const float TIER5 = 8f;

			// Token: 0x040080E2 RID: 32994
			public const float TIER6 = 16f;

			// Token: 0x040080E3 RID: 32995
			public const float TIER7 = 32f;

			// Token: 0x040080E4 RID: 32996
			public const float TIER8 = 64f;

			// Token: 0x040080E5 RID: 32997
			public const float TIER_NUCLEAR = 16384f;
		}

		// Token: 0x02001C6A RID: 7274
		public class MELTING_POINT_KELVIN
		{
			// Token: 0x040080E6 RID: 32998
			public const float TIER0 = 800f;

			// Token: 0x040080E7 RID: 32999
			public const float TIER1 = 1600f;

			// Token: 0x040080E8 RID: 33000
			public const float TIER2 = 2400f;

			// Token: 0x040080E9 RID: 33001
			public const float TIER3 = 3200f;

			// Token: 0x040080EA RID: 33002
			public const float TIER4 = 9999f;
		}

		// Token: 0x02001C6B RID: 7275
		public class CONSTRUCTION_TIME_SECONDS
		{
			// Token: 0x040080EB RID: 33003
			public const float TIER0 = 3f;

			// Token: 0x040080EC RID: 33004
			public const float TIER1 = 10f;

			// Token: 0x040080ED RID: 33005
			public const float TIER2 = 30f;

			// Token: 0x040080EE RID: 33006
			public const float TIER3 = 60f;

			// Token: 0x040080EF RID: 33007
			public const float TIER4 = 120f;

			// Token: 0x040080F0 RID: 33008
			public const float TIER5 = 240f;

			// Token: 0x040080F1 RID: 33009
			public const float TIER6 = 480f;
		}

		// Token: 0x02001C6C RID: 7276
		public class HITPOINTS
		{
			// Token: 0x040080F2 RID: 33010
			public const int TIER0 = 10;

			// Token: 0x040080F3 RID: 33011
			public const int TIER1 = 30;

			// Token: 0x040080F4 RID: 33012
			public const int TIER2 = 100;

			// Token: 0x040080F5 RID: 33013
			public const int TIER3 = 250;

			// Token: 0x040080F6 RID: 33014
			public const int TIER4 = 1000;
		}

		// Token: 0x02001C6D RID: 7277
		public class DAMAGE_SOURCES
		{
			// Token: 0x040080F7 RID: 33015
			public const int CONDUIT_CONTENTS_BOILED = 1;

			// Token: 0x040080F8 RID: 33016
			public const int CONDUIT_CONTENTS_FROZE = 1;

			// Token: 0x040080F9 RID: 33017
			public const int BAD_INPUT_ELEMENT = 1;

			// Token: 0x040080FA RID: 33018
			public const int BUILDING_OVERHEATED = 1;

			// Token: 0x040080FB RID: 33019
			public const int HIGH_LIQUID_PRESSURE = 10;

			// Token: 0x040080FC RID: 33020
			public const int MICROMETEORITE = 1;

			// Token: 0x040080FD RID: 33021
			public const int CORROSIVE_ELEMENT = 1;
		}

		// Token: 0x02001C6E RID: 7278
		public class RELOCATION_TIME_SECONDS
		{
			// Token: 0x040080FE RID: 33022
			public const float DECONSTRUCT = 4f;

			// Token: 0x040080FF RID: 33023
			public const float CONSTRUCT = 4f;
		}

		// Token: 0x02001C6F RID: 7279
		public class WORK_TIME_SECONDS
		{
			// Token: 0x04008100 RID: 33024
			public const float VERYSHORT_WORK_TIME = 5f;

			// Token: 0x04008101 RID: 33025
			public const float SHORT_WORK_TIME = 15f;

			// Token: 0x04008102 RID: 33026
			public const float MEDIUM_WORK_TIME = 30f;

			// Token: 0x04008103 RID: 33027
			public const float LONG_WORK_TIME = 90f;

			// Token: 0x04008104 RID: 33028
			public const float VERY_LONG_WORK_TIME = 150f;

			// Token: 0x04008105 RID: 33029
			public const float EXTENSIVE_WORK_TIME = 180f;
		}

		// Token: 0x02001C70 RID: 7280
		public class FABRICATION_TIME_SECONDS
		{
			// Token: 0x04008106 RID: 33030
			public const float VERY_SHORT = 20f;

			// Token: 0x04008107 RID: 33031
			public const float SHORT = 40f;

			// Token: 0x04008108 RID: 33032
			public const float MODERATE = 80f;

			// Token: 0x04008109 RID: 33033
			public const float LONG = 250f;
		}

		// Token: 0x02001C71 RID: 7281
		public class DECOR
		{
			// Token: 0x0400810A RID: 33034
			public static readonly EffectorValues NONE = new EffectorValues
			{
				amount = 0,
				radius = 1
			};

			// Token: 0x02002241 RID: 8769
			public class BONUS
			{
				// Token: 0x0400990B RID: 39179
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = 5,
					radius = 1
				};

				// Token: 0x0400990C RID: 39180
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = 10,
					radius = 2
				};

				// Token: 0x0400990D RID: 39181
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = 15,
					radius = 3
				};

				// Token: 0x0400990E RID: 39182
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = 20,
					radius = 4
				};

				// Token: 0x0400990F RID: 39183
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = 25,
					radius = 5
				};

				// Token: 0x04009910 RID: 39184
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = 30,
					radius = 6
				};

				// Token: 0x02002F6B RID: 12139
				public class MONUMENT
				{
					// Token: 0x0400C1C2 RID: 49602
					public static readonly EffectorValues COMPLETE = new EffectorValues
					{
						amount = 40,
						radius = 10
					};

					// Token: 0x0400C1C3 RID: 49603
					public static readonly EffectorValues INCOMPLETE = new EffectorValues
					{
						amount = 10,
						radius = 5
					};
				}
			}

			// Token: 0x02002242 RID: 8770
			public class PENALTY
			{
				// Token: 0x04009911 RID: 39185
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = -5,
					radius = 1
				};

				// Token: 0x04009912 RID: 39186
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = -10,
					radius = 2
				};

				// Token: 0x04009913 RID: 39187
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = -15,
					radius = 3
				};

				// Token: 0x04009914 RID: 39188
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = -20,
					radius = 4
				};

				// Token: 0x04009915 RID: 39189
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = -20,
					radius = 5
				};

				// Token: 0x04009916 RID: 39190
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = -25,
					radius = 6
				};
			}
		}

		// Token: 0x02001C72 RID: 7282
		public class MASS_KG
		{
			// Token: 0x0400810B RID: 33035
			public const float TIER0 = 25f;

			// Token: 0x0400810C RID: 33036
			public const float TIER1 = 50f;

			// Token: 0x0400810D RID: 33037
			public const float TIER2 = 100f;

			// Token: 0x0400810E RID: 33038
			public const float TIER3 = 200f;

			// Token: 0x0400810F RID: 33039
			public const float TIER4 = 400f;

			// Token: 0x04008110 RID: 33040
			public const float TIER5 = 800f;

			// Token: 0x04008111 RID: 33041
			public const float TIER6 = 1200f;

			// Token: 0x04008112 RID: 33042
			public const float TIER7 = 2000f;
		}

		// Token: 0x02001C73 RID: 7283
		public class UPGRADES
		{
			// Token: 0x04008113 RID: 33043
			public const float BUILDTIME_TIER0 = 120f;

			// Token: 0x02002243 RID: 8771
			public class MATERIALTAGS
			{
				// Token: 0x04009917 RID: 39191
				public const string METAL = "Metal";

				// Token: 0x04009918 RID: 39192
				public const string REFINEDMETAL = "RefinedMetal";

				// Token: 0x04009919 RID: 39193
				public const string CARBON = "Carbon";
			}

			// Token: 0x02002244 RID: 8772
			public class MATERIALMASS
			{
				// Token: 0x0400991A RID: 39194
				public const int TIER0 = 100;

				// Token: 0x0400991B RID: 39195
				public const int TIER1 = 200;

				// Token: 0x0400991C RID: 39196
				public const int TIER2 = 400;

				// Token: 0x0400991D RID: 39197
				public const int TIER3 = 500;
			}

			// Token: 0x02002245 RID: 8773
			public class MODIFIERAMOUNTS
			{
				// Token: 0x0400991E RID: 39198
				public const float MANUALGENERATOR_ENERGYGENERATION = 1.2f;

				// Token: 0x0400991F RID: 39199
				public const float MANUALGENERATOR_CAPACITY = 2f;

				// Token: 0x04009920 RID: 39200
				public const float PROPANEGENERATOR_ENERGYGENERATION = 1.6f;

				// Token: 0x04009921 RID: 39201
				public const float PROPANEGENERATOR_HEATGENERATION = 1.6f;

				// Token: 0x04009922 RID: 39202
				public const float GENERATOR_HEATGENERATION = 0.8f;

				// Token: 0x04009923 RID: 39203
				public const float GENERATOR_ENERGYGENERATION = 1.3f;

				// Token: 0x04009924 RID: 39204
				public const float TURBINE_ENERGYGENERATION = 1.2f;

				// Token: 0x04009925 RID: 39205
				public const float TURBINE_CAPACITY = 1.2f;

				// Token: 0x04009926 RID: 39206
				public const float SUITRECHARGER_EXECUTIONTIME = 1.2f;

				// Token: 0x04009927 RID: 39207
				public const float SUITRECHARGER_HEATGENERATION = 1.2f;

				// Token: 0x04009928 RID: 39208
				public const float STORAGELOCKER_CAPACITY = 2f;

				// Token: 0x04009929 RID: 39209
				public const float SOLARPANEL_ENERGYGENERATION = 1.2f;

				// Token: 0x0400992A RID: 39210
				public const float SMELTER_HEATGENERATION = 0.7f;
			}
		}
	}
}
