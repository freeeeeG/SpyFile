﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D29 RID: 3369
	public class Techs : ResourceSet<Tech>
	{
		// Token: 0x06006A2D RID: 27181 RVA: 0x0029590C File Offset: 0x00293B0C
		public Techs(ResourceSet parent) : base("Techs", parent)
		{
			if (!DlcManager.IsExpansion1Active())
			{
				this.TECH_TIERS = new List<List<global::Tuple<string, float>>>
				{
					new List<global::Tuple<string, float>>(),
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 15f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 20f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 30f),
						new global::Tuple<string, float>("advanced", 20f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 35f),
						new global::Tuple<string, float>("advanced", 30f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 40f),
						new global::Tuple<string, float>("advanced", 50f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 50f),
						new global::Tuple<string, float>("advanced", 70f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 70f),
						new global::Tuple<string, float>("advanced", 100f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 70f),
						new global::Tuple<string, float>("advanced", 100f),
						new global::Tuple<string, float>("space", 200f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 70f),
						new global::Tuple<string, float>("advanced", 100f),
						new global::Tuple<string, float>("space", 400f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 70f),
						new global::Tuple<string, float>("advanced", 100f),
						new global::Tuple<string, float>("space", 800f)
					},
					new List<global::Tuple<string, float>>
					{
						new global::Tuple<string, float>("basic", 70f),
						new global::Tuple<string, float>("advanced", 100f),
						new global::Tuple<string, float>("space", 1600f)
					}
				};
				return;
			}
			this.TECH_TIERS = new List<List<global::Tuple<string, float>>>
			{
				new List<global::Tuple<string, float>>(),
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 15f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 20f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 30f),
					new global::Tuple<string, float>("advanced", 20f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 35f),
					new global::Tuple<string, float>("advanced", 30f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 40f),
					new global::Tuple<string, float>("advanced", 50f),
					new global::Tuple<string, float>("orbital", 0f),
					new global::Tuple<string, float>("nuclear", 20f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 50f),
					new global::Tuple<string, float>("advanced", 70f),
					new global::Tuple<string, float>("orbital", 30f),
					new global::Tuple<string, float>("nuclear", 40f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 70f),
					new global::Tuple<string, float>("advanced", 100f),
					new global::Tuple<string, float>("orbital", 250f),
					new global::Tuple<string, float>("nuclear", 370f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 100f),
					new global::Tuple<string, float>("advanced", 130f),
					new global::Tuple<string, float>("orbital", 400f),
					new global::Tuple<string, float>("nuclear", 435f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 100f),
					new global::Tuple<string, float>("advanced", 130f),
					new global::Tuple<string, float>("orbital", 600f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 100f),
					new global::Tuple<string, float>("advanced", 130f),
					new global::Tuple<string, float>("orbital", 800f)
				},
				new List<global::Tuple<string, float>>
				{
					new global::Tuple<string, float>("basic", 100f),
					new global::Tuple<string, float>("advanced", 130f),
					new global::Tuple<string, float>("orbital", 1600f)
				}
			};
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x00295ED4 File Offset: 0x002940D4
		public void Init()
		{
			new Tech("FarmingTech", new List<string>
			{
				"AlgaeHabitat",
				"PlanterBox",
				"RationBox",
				"Compost"
			}, this, null);
			new Tech("FineDining", new List<string>
			{
				"CookingStation",
				"EggCracker",
				"DiningTable",
				"FarmTile"
			}, this, null);
			new Tech("FoodRepurposing", new List<string>
			{
				"Juicer",
				"SpiceGrinder",
				"MilkPress"
			}, this, null);
			new Tech("FinerDining", new List<string>
			{
				"GourmetCookingStation"
			}, this, null);
			new Tech("Agriculture", new List<string>
			{
				"FarmStation",
				"FertilizerMaker",
				"Refrigerator",
				"HydroponicFarm",
				"ParkSign",
				"RadiationLight"
			}, this, null);
			new Tech("Ranching", new List<string>
			{
				"RanchStation",
				"CreatureDeliveryPoint",
				"ShearingStation",
				"CreatureFeeder",
				"FishDeliveryPoint",
				"FishFeeder"
			}, this, null);
			new Tech("AnimalControl", new List<string>
			{
				"CreatureAirTrap",
				"CreatureGroundTrap",
				"WaterTrap",
				"EggIncubator",
				LogicCritterCountSensorConfig.ID
			}, this, null);
			new Tech("DairyOperation", new List<string>
			{
				"MilkFeeder",
				"MilkFatSeparator",
				"MilkingStation"
			}, this, null);
			new Tech("ImprovedOxygen", new List<string>
			{
				"Electrolyzer",
				"RustDeoxidizer"
			}, this, null);
			new Tech("GasPiping", new List<string>
			{
				"GasConduit",
				"GasConduitBridge",
				"GasPump",
				"GasVent"
			}, this, null);
			new Tech("ImprovedGasPiping", new List<string>
			{
				"InsulatedGasConduit",
				LogicPressureSensorGasConfig.ID,
				"GasLogicValve",
				"GasVentHighPressure"
			}, this, null);
			new Tech("SpaceGas", new List<string>
			{
				"CO2Engine",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				"GasCargoBaySmall"
			}, this, null);
			new Tech("PressureManagement", new List<string>
			{
				"LiquidValve",
				"GasValve",
				"GasPermeableMembrane",
				"ManualPressureDoor"
			}, this, null);
			new Tech("DirectedAirStreams", new List<string>
			{
				"AirFilter",
				"CO2Scrubber",
				"PressureDoor"
			}, this, null);
			new Tech("LiquidFiltering", new List<string>
			{
				"OreScrubber",
				"Desalinator"
			}, this, null);
			new Tech("MedicineI", new List<string>
			{
				"Apothecary"
			}, this, null);
			new Tech("MedicineII", new List<string>
			{
				"DoctorStation",
				"HandSanitizer"
			}, this, null);
			new Tech("MedicineIII", new List<string>
			{
				GasConduitDiseaseSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LogicDiseaseSensorConfig.ID
			}, this, null);
			new Tech("MedicineIV", new List<string>
			{
				"AdvancedDoctorStation",
				"AdvancedApothecary",
				"HotTub",
				LogicRadiationSensorConfig.ID
			}, this, null);
			new Tech("LiquidPiping", new List<string>
			{
				"LiquidConduit",
				"LiquidConduitBridge",
				"LiquidPump",
				"LiquidVent"
			}, this, null);
			new Tech("ImprovedLiquidPiping", new List<string>
			{
				"InsulatedLiquidConduit",
				LogicPressureSensorLiquidConfig.ID,
				"LiquidLogicValve",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidReservoir"
			}, this, null);
			new Tech("PrecisionPlumbing", new List<string>
			{
				"EspressoMachine",
				"LiquidFuelTankCluster"
			}, this, null);
			new Tech("SanitationSciences", new List<string>
			{
				"FlushToilet",
				"WashSink",
				ShowerConfig.ID,
				"MeshTile"
			}, this, null);
			new Tech("FlowRedirection", new List<string>
			{
				"MechanicalSurfboard",
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"LiquidCargoBaySmall"
			}, this, null);
			new Tech("LiquidDistribution", new List<string>
			{
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"WallToilet"
			}, this, null);
			new Tech("AdvancedSanitation", new List<string>
			{
				"DecontaminationShower"
			}, this, null);
			new Tech("AdvancedFiltration", new List<string>
			{
				"GasFilter",
				"LiquidFilter",
				"SludgePress"
			}, this, null);
			new Tech("Distillation", new List<string>
			{
				"AlgaeDistillery",
				"EthanolDistillery",
				"WaterPurifier"
			}, this, null);
			new Tech("Catalytics", new List<string>
			{
				"OxyliteRefinery",
				"Chlorinator",
				"SupermaterialRefinery",
				"SodaFountain",
				"GasCargoBayCluster"
			}, this, null);
			new Tech("AdvancedResourceExtraction", new List<string>
			{
				"NoseconeHarvest"
			}, this, null);
			new Tech("PowerRegulation", new List<string>
			{
				"BatteryMedium",
				SwitchConfig.ID,
				"WireBridge"
			}, this, null);
			new Tech("AdvancedPowerRegulation", new List<string>
			{
				"HighWattageWire",
				"WireBridgeHighWattage",
				"HydrogenGenerator",
				LogicPowerRelayConfig.ID,
				"PowerTransformerSmall",
				LogicWattageSensorConfig.ID
			}, this, null);
			new Tech("PrettyGoodConductors", new List<string>
			{
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"PowerTransformer"
			}, this, null);
			new Tech("RenewableEnergy", new List<string>
			{
				"SteamTurbine2",
				"SolarPanel",
				"Sauna",
				"SteamEngineCluster"
			}, this, null);
			new Tech("Combustion", new List<string>
			{
				"Generator",
				"WoodGasGenerator"
			}, this, null);
			new Tech("ImprovedCombustion", new List<string>
			{
				"MethaneGenerator",
				"OilRefinery",
				"PetroleumGenerator"
			}, this, null);
			new Tech("InteriorDecor", new List<string>
			{
				"FlowerVase",
				"FloorLamp",
				"CeilingLight"
			}, this, null);
			new Tech("Artistry", new List<string>
			{
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"CornerMoulding",
				"CrownMoulding",
				"ItemPedestal",
				"SmallSculpture",
				"IceSculpture"
			}, this, null);
			new Tech("Clothing", new List<string>
			{
				"ClothingFabricator",
				"CarpetTile",
				"ExteriorWall"
			}, this, null);
			new Tech("Acoustics", new List<string>
			{
				"BatterySmart",
				"Phonobox",
				"PowerControlStation"
			}, this, null);
			new Tech("SpacePower", new List<string>
			{
				"BatteryModule",
				"SolarPanelModule",
				"RocketInteriorPowerPlug"
			}, this, null);
			new Tech("NuclearRefinement", new List<string>
			{
				"NuclearReactor",
				"UraniumCentrifuge",
				"HEPBridgeTile"
			}, this, null);
			new Tech("FineArt", new List<string>
			{
				"Canvas",
				"Sculpture"
			}, this, null);
			new Tech("EnvironmentalAppreciation", new List<string>
			{
				"BeachChair"
			}, this, null);
			new Tech("Luxury", new List<string>
			{
				"LuxuryBed",
				"LadderFast",
				"PlasticTile",
				"ClothingAlterationStation"
			}, this, null);
			new Tech("RefractiveDecor", new List<string>
			{
				"CanvasWide",
				"MetalSculpture"
			}, this, null);
			new Tech("GlassFurnishings", new List<string>
			{
				"GlassTile",
				"FlowerVaseHangingFancy",
				"SunLamp"
			}, this, null);
			new Tech("Screens", new List<string>
			{
				PixelPackConfig.ID
			}, this, null);
			new Tech("RenaissanceArt", new List<string>
			{
				"CanvasTall",
				"MarbleSculpture"
			}, this, null);
			new Tech("Plastics", new List<string>
			{
				"Polymerizer",
				"OilWellCap"
			}, this, null);
			new Tech("ValveMiniaturization", new List<string>
			{
				"LiquidMiniPump",
				"GasMiniPump"
			}, this, null);
			new Tech("HydrocarbonPropulsion", new List<string>
			{
				"KeroseneEngineClusterSmall",
				"MissionControlCluster"
			}, this, null);
			new Tech("BetterHydroCarbonPropulsion", new List<string>
			{
				"KeroseneEngineCluster"
			}, this, null);
			new Tech("CryoFuelPropulsion", new List<string>
			{
				"HydrogenEngineCluster",
				"OxidizerTankLiquidCluster"
			}, this, null);
			new Tech("Suits", new List<string>
			{
				"SuitsOverlay",
				"AtmoSuit",
				"SuitFabricator",
				"SuitMarker",
				"SuitLocker"
			}, this, null);
			new Tech("Jobs", new List<string>
			{
				"WaterCooler",
				"CraftingTable"
			}, this, null);
			new Tech("AdvancedResearch", new List<string>
			{
				"BetaResearchPoint",
				"AdvancedResearchCenter",
				"ResetSkillsStation",
				"ClusterTelescope",
				"ExobaseHeadquarters"
			}, this, null);
			new Tech("SpaceProgram", new List<string>
			{
				"LaunchPad",
				"HabitatModuleSmall",
				"OrbitalCargoModule",
				RocketControlStationConfig.ID
			}, this, null);
			new Tech("CrashPlan", new List<string>
			{
				"OrbitalResearchPoint",
				"PioneerModule",
				"OrbitalResearchCenter",
				"DLC1CosmicResearchCenter"
			}, this, null);
			new Tech("DurableLifeSupport", new List<string>
			{
				"NoseconeBasic",
				"HabitatModuleMedium",
				"ArtifactAnalysisStation",
				"ArtifactCargoBay",
				"SpecialCargoBayCluster"
			}, this, null);
			new Tech("NuclearResearch", new List<string>
			{
				"DeltaResearchPoint",
				"NuclearResearchCenter",
				"ManualHighEnergyParticleSpawner"
			}, this, null);
			new Tech("AdvancedNuclearResearch", new List<string>
			{
				"HighEnergyParticleSpawner",
				"HighEnergyParticleRedirector"
			}, this, null);
			new Tech("NuclearStorage", new List<string>
			{
				"HEPBattery"
			}, this, null);
			new Tech("NuclearPropulsion", new List<string>
			{
				"HEPEngine"
			}, this, null);
			new Tech("NotificationSystems", new List<string>
			{
				LogicHammerConfig.ID,
				LogicAlarmConfig.ID,
				"Telephone"
			}, this, null);
			new Tech("ArtificialFriends", new List<string>
			{
				"SweepBotStation",
				"ScoutModule"
			}, this, null);
			new Tech("BasicRefinement", new List<string>
			{
				"RockCrusher",
				"Kiln"
			}, this, null);
			new Tech("RefinedObjects", new List<string>
			{
				"FirePole",
				"ThermalBlock",
				LadderBedConfig.ID
			}, this, null);
			new Tech("Smelting", new List<string>
			{
				"MetalRefinery",
				"MetalTile"
			}, this, null);
			new Tech("HighTempForging", new List<string>
			{
				"GlassForge",
				"BunkerTile",
				"BunkerDoor",
				"GeoTuner"
			}, this, null);
			new Tech("HighPressureForging", new List<string>
			{
				"DiamondPress"
			}, this, null);
			new Tech("RadiationProtection", new List<string>
			{
				"LeadSuit",
				"LeadSuitMarker",
				"LeadSuitLocker",
				LogicHEPSensorConfig.ID
			}, this, null);
			new Tech("TemperatureModulation", new List<string>
			{
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"InsulationTile",
				"SpaceHeater"
			}, this, null);
			new Tech("HVAC", new List<string>
			{
				"AirConditioner",
				LogicTemperatureSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID,
				GasConduitElementSensorConfig.ID,
				"GasConduitRadiant",
				"GasReservoir",
				"GasLimitValve"
			}, this, null);
			new Tech("LiquidTemperature", new List<string>
			{
				"LiquidConduitRadiant",
				"LiquidConditioner",
				LiquidConduitTemperatureSensorConfig.ID,
				LiquidConduitElementSensorConfig.ID,
				"LiquidHeater",
				"LiquidLimitValve",
				"ContactConductivePipeBridge"
			}, this, null);
			new Tech("LogicControl", new List<string>
			{
				"AutomationOverlay",
				LogicSwitchConfig.ID,
				"LogicWire",
				"LogicWireBridge",
				"LogicDuplicantSensor"
			}, this, null);
			new Tech("GenericSensors", new List<string>
			{
				"FloorSwitch",
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				"LogicGateNOT",
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicClusterLocationSensorConfig.ID
			}, this, null);
			new Tech("LogicCircuits", new List<string>
			{
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER"
			}, this, null);
			new Tech("ParallelAutomation", new List<string>
			{
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicRibbonWriterConfig.ID,
				LogicRibbonReaderConfig.ID
			}, this, null);
			new Tech("DupeTrafficControl", new List<string>
			{
				LogicCounterConfig.ID,
				LogicMemoryConfig.ID,
				"LogicGateXOR",
				"ArcadeMachine",
				"Checkpoint",
				"CosmicResearchCenter"
			}, this, null);
			new Tech("Multiplexing", new List<string>
			{
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, this, null);
			new Tech("SkyDetectors", new List<string>
			{
				CometDetectorConfig.ID,
				"Telescope",
				"ClusterTelescopeEnclosed",
				"AstronautTrainingCenter"
			}, this, null);
			new Tech("TravelTubes", new List<string>
			{
				"TravelTubeEntrance",
				"TravelTube",
				"TravelTubeWallBridge",
				"VerticalWindTunnel"
			}, this, null);
			new Tech("SmartStorage", new List<string>
			{
				"ConveyorOverlay",
				"SolidTransferArm",
				"StorageLockerSmart",
				"ObjectDispenser"
			}, this, null);
			new Tech("SolidManagement", new List<string>
			{
				"SolidFilter",
				SolidConduitTemperatureSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitDiseaseSensorConfig.ID,
				"CargoBayCluster"
			}, this, null);
			new Tech("HighVelocityTransport", new List<string>
			{
				"RailGun",
				"LandingBeacon"
			}, this, null);
			new Tech("BasicRocketry", new List<string>
			{
				"CommandModule",
				"SteamEngine",
				"ResearchModule",
				"Gantry"
			}, this, null);
			new Tech("CargoI", new List<string>
			{
				"CargoBay"
			}, this, null);
			new Tech("CargoII", new List<string>
			{
				"LiquidCargoBay",
				"GasCargoBay"
			}, this, null);
			new Tech("CargoIII", new List<string>
			{
				"TouristModule",
				"SpecialCargoBay"
			}, this, null);
			new Tech("EnginesI", new List<string>
			{
				"SolidBooster",
				"MissionControl"
			}, this, null);
			new Tech("EnginesII", new List<string>
			{
				"KeroseneEngine",
				"LiquidFuelTank",
				"OxidizerTank"
			}, this, null);
			new Tech("EnginesIII", new List<string>
			{
				"OxidizerTankLiquid",
				"OxidizerTankCluster",
				"HydrogenEngine"
			}, this, null);
			new Tech("Jetpacks", new List<string>
			{
				"JetSuit",
				"JetSuitMarker",
				"JetSuitLocker",
				"LiquidCargoBayCluster",
				"MissileFabricator",
				"MissileLauncher"
			}, this, null);
			new Tech("SolidTransport", new List<string>
			{
				"SolidConduitInbox",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidVent"
			}, this, null);
			new Tech("Monuments", new List<string>
			{
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop"
			}, this, null);
			new Tech("SolidSpace", new List<string>
			{
				"SolidLogicValve",
				"SolidConduitOutbox",
				"SolidLimitValve",
				"SolidCargoBaySmall",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, this, null);
			new Tech("RoboticTools", new List<string>
			{
				"AutoMiner",
				"RailGunPayloadOpener"
			}, this, null);
			new Tech("PortableGasses", new List<string>
			{
				"GasBottler",
				"BottleEmptierGas",
				"OxygenMask",
				"OxygenMaskLocker",
				"OxygenMaskMarker"
			}, this, null);
			this.InitExpansion1();
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x00297444 File Offset: 0x00295644
		private void InitExpansion1()
		{
			if (!DlcManager.IsExpansion1Active())
			{
				return;
			}
			base.Get("HighTempForging").AddUnlockedItemIDs(new string[]
			{
				"Gantry"
			});
			new Tech("Bioengineering", new List<string>
			{
				"GeneticAnalysisStation"
			}, this, null);
			new Tech("SpaceCombustion", new List<string>
			{
				"SugarEngine",
				"SmallOxidizerTank"
			}, this, null);
			new Tech("HighVelocityDestruction", new List<string>
			{
				"NoseconeHarvest"
			}, this, null);
			new Tech("GasDistribution", new List<string>
			{
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"OxidizerTankCluster"
			}, this, null);
			new Tech("AdvancedScanners", new List<string>
			{
				"ScannerModule",
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver"
			}, this, null);
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x00297540 File Offset: 0x00295740
		public void PostProcess()
		{
			foreach (Tech tech in this.resources)
			{
				List<TechItem> list = new List<TechItem>();
				foreach (string id in tech.unlockedItemIDs)
				{
					TechItem techItem = Db.Get().TechItems.TryGet(id);
					if (techItem != null)
					{
						list.Add(techItem);
					}
				}
				tech.unlockedItems = list;
			}
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x002975F4 File Offset: 0x002957F4
		public void Load(TextAsset tree_file)
		{
			ResourceTreeLoader<ResourceTreeNode> resourceTreeLoader = new ResourceTreeLoader<ResourceTreeNode>(tree_file);
			List<TechTreeTitle> list = new List<TechTreeTitle>();
			for (int i = 0; i < Db.Get().TechTreeTitles.Count; i++)
			{
				list.Add(Db.Get().TechTreeTitles[i]);
			}
			list.Sort((TechTreeTitle a, TechTreeTitle b) => a.center.y.CompareTo(b.center.y));
			foreach (ResourceTreeNode resourceTreeNode in resourceTreeLoader)
			{
				if (!string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					Tech tech = base.TryGet(resourceTreeNode.Id);
					global::Debug.Assert(tech != null, "Tech node found in yEd that is not found in DbTechs constructor: " + resourceTreeNode.Id);
					string categoryID = "";
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].center.y >= resourceTreeNode.center.y)
						{
							categoryID = list[j].Id;
							break;
						}
					}
					tech.SetNode(resourceTreeNode, categoryID);
					foreach (ResourceTreeNode resourceTreeNode2 in resourceTreeNode.references)
					{
						Tech tech2 = base.TryGet(resourceTreeNode2.Id);
						global::Debug.Assert(tech2 != null, "Tech node found in yEd that is not found in DbTechs constructor: " + resourceTreeNode2.Id);
						categoryID = "";
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].center.y >= resourceTreeNode.center.y)
							{
								categoryID = list[k].Id;
								break;
							}
						}
						tech2.SetNode(resourceTreeNode2, categoryID);
						tech2.requiredTech.Add(tech);
						tech.unlockedTech.Add(tech2);
					}
				}
			}
			foreach (Tech tech3 in this.resources)
			{
				tech3.tier = Techs.GetTier(tech3);
				foreach (global::Tuple<string, float> tuple in this.TECH_TIERS[tech3.tier])
				{
					if (!tech3.costsByResearchTypeID.ContainsKey(tuple.first))
					{
						tech3.costsByResearchTypeID.Add(tuple.first, tuple.second);
					}
				}
			}
			for (int l = this.Count - 1; l >= 0; l--)
			{
				if (!((Tech)this.GetResource(l)).FoundNode)
				{
					this.Remove((Tech)this.GetResource(l));
				}
			}
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x00297958 File Offset: 0x00295B58
		public static int GetTier(Tech tech)
		{
			if (tech == null)
			{
				return 0;
			}
			int num = 0;
			foreach (Tech tech2 in tech.requiredTech)
			{
				num = Math.Max(num, Techs.GetTier(tech2));
			}
			return num + 1;
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x002979BC File Offset: 0x00295BBC
		private void AddPrerequisite(Tech tech, string prerequisite_name)
		{
			Tech tech2 = base.TryGet(prerequisite_name);
			if (tech2 != null)
			{
				tech.requiredTech.Add(tech2);
				tech2.unlockedTech.Add(tech);
			}
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x002979EC File Offset: 0x00295BEC
		public Tech TryGetTechForTechItem(string itemId)
		{
			Predicate<string> <>9__0;
			for (int i = 0; i < this.Count; i++)
			{
				Tech tech = (Tech)this.GetResource(i);
				List<string> unlockedItemIDs = tech.unlockedItemIDs;
				Predicate<string> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((string match) => match == itemId));
				}
				if (unlockedItemIDs.Find(match2) != null)
				{
					return tech;
				}
			}
			return null;
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x00297A54 File Offset: 0x00295C54
		public bool IsTechItemComplete(string id)
		{
			foreach (Tech tech in this.resources)
			{
				using (List<TechItem>.Enumerator enumerator2 = tech.unlockedItems.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Id == id)
						{
							return tech.IsComplete();
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04004D59 RID: 19801
		private readonly List<List<global::Tuple<string, float>>> TECH_TIERS;
	}
}
