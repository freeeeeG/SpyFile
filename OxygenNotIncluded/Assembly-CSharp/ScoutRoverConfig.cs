using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class ScoutRoverConfig : IEntityConfig
{
	// Token: 0x06000596 RID: 1430 RVA: 0x000251FC File Offset: 0x000233FC
	public static GameObject CreateScout(string id, string name, string desc, string anim_file)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(id, name, desc, 100f, true, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, SimHashes.Creature, new List<Tag>
		{
			GameTags.Experimental
		}, 293f);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.isMovable = true;
		gameObject.AddOrGet<Modifiers>();
		gameObject.AddOrGet<LoopingSounds>();
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.size = new Vector2(1f, 2f);
		kboxCollider2D.offset = new Vector2f(0f, 1f);
		Modifiers component2 = gameObject.GetComponent<Modifiers>();
		component2.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
		component2.initialAmounts.Add(Db.Get().Amounts.InternalChemicalBattery.Id);
		component2.initialAttributes.Add(Db.Get().Attributes.Construction.Id);
		component2.initialAttributes.Add(Db.Get().Attributes.Digging.Id);
		component2.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);
		component2.initialAttributes.Add(Db.Get().Attributes.Machinery.Id);
		component2.initialAttributes.Add(Db.Get().Attributes.Athletics.Id);
		ChoreGroup[] disabled_chore_groups = new ChoreGroup[]
		{
			Db.Get().ChoreGroups.Basekeeping,
			Db.Get().ChoreGroups.Cook,
			Db.Get().ChoreGroups.Art,
			Db.Get().ChoreGroups.Research,
			Db.Get().ChoreGroups.Farming,
			Db.Get().ChoreGroups.Ranching,
			Db.Get().ChoreGroups.MachineOperating,
			Db.Get().ChoreGroups.MedicalAid,
			Db.Get().ChoreGroups.Combat,
			Db.Get().ChoreGroups.LifeSupport,
			Db.Get().ChoreGroups.Recreation,
			Db.Get().ChoreGroups.Toggle
		};
		gameObject.AddOrGet<Traits>();
		Trait trait = Db.Get().CreateTrait(ScoutRoverConfig.ROVER_BASE_TRAIT_ID, STRINGS.ROBOTS.MODELS.SCOUT.NAME, STRINGS.ROBOTS.MODELS.SCOUT.NAME, null, false, disabled_chore_groups, true, true);
		trait.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, 200f, STRINGS.ROBOTS.MODELS.SCOUT.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, TUNING.ROBOTS.SCOUTBOT.DIGGING, STRINGS.ROBOTS.MODELS.SCOUT.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Attributes.Construction.Id, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, STRINGS.ROBOTS.MODELS.SCOUT.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, STRINGS.ROBOTS.MODELS.SCOUT.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, STRINGS.ROBOTS.MODELS.SCOUT.NAME, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalChemicalBattery.maxAttribute.Id, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalChemicalBattery.deltaAttribute.Id, -TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, name, false, false, true));
		component2.initialTraits.Add(ScoutRoverConfig.ROVER_BASE_TRAIT_ID);
		gameObject.AddOrGet<AttributeConverters>();
		GridVisibility gridVisibility = gameObject.AddOrGet<GridVisibility>();
		gridVisibility.radius = 30;
		gridVisibility.innerRadius = 20f;
		gameObject.AddOrGet<Worker>();
		gameObject.AddOrGet<Effects>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<AnimEventHandler>();
		gameObject.AddOrGet<Health>();
		MoverLayerOccupier moverLayerOccupier = gameObject.AddOrGet<MoverLayerOccupier>();
		moverLayerOccupier.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Rover,
			ObjectLayer.Mover
		};
		moverLayerOccupier.cellOffsets = new CellOffset[]
		{
			CellOffset.none,
			new CellOffset(0, 1)
		};
		RobotBatteryMonitor.Def def = gameObject.AddOrGetDef<RobotBatteryMonitor.Def>();
		def.batteryAmountId = Db.Get().Amounts.InternalChemicalBattery.Id;
		def.canCharge = false;
		def.lowBatteryWarningPercent = 0.2f;
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.fxPrefix = Storage.FXPrefix.PickedUp;
		storage.dropOnLoad = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Preserve,
			Storage.StoredItemModifier.Seal
		});
		gameObject.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
		Deconstructable deconstructable = gameObject.AddOrGet<Deconstructable>();
		deconstructable.enabled = false;
		deconstructable.audioSize = "medium";
		deconstructable.looseEntityDeconstructable = true;
		gameObject.AddOrGetDef<RobotAi.Def>();
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new RobotDeathStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new IdleStates.Def(), true, Db.Get().ChoreTypes.Idle.priority);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Robots.Models.ScoutRover, null);
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.RemoveTag(GameTags.CreatureBrain);
		kprefabID.AddTag(GameTags.DupeBrain, false);
		kprefabID.AddTag(GameTags.Robot, false);
		Navigator navigator = gameObject.AddOrGet<Navigator>();
		string navGridName = "RobotNavGrid";
		navigator.NavGridName = navGridName;
		navigator.CurrentNavType = NavType.Floor;
		navigator.defaultSpeed = 2f;
		navigator.updateProber = true;
		navigator.sceneLayer = Grid.SceneLayer.Creatures;
		gameObject.AddOrGet<Sensors>();
		gameObject.AddOrGet<Pickupable>().SetWorkTime(5f);
		gameObject.AddOrGet<SnapOn>();
		component.SetSymbolVisiblity("snapto_pivot", false);
		component.SetSymbolVisiblity("snapto_radar", false);
		return gameObject;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x000257F1 File Offset: 0x000239F1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x000257F8 File Offset: 0x000239F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = ScoutRoverConfig.CreateScout("ScoutRover", STRINGS.ROBOTS.MODELS.SCOUT.NAME, STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim");
		this.SetupLaserEffects(gameObject);
		return gameObject;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00025834 File Offset: 0x00023A34
	private void SetupLaserEffects(GameObject prefab)
	{
		GameObject gameObject = new GameObject("LaserEffect");
		gameObject.transform.parent = prefab.transform;
		KBatchedAnimEventToggler kbatchedAnimEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
		kbatchedAnimEventToggler.eventSource = prefab;
		kbatchedAnimEventToggler.enableEvent = "LaserOn";
		kbatchedAnimEventToggler.disableEvent = "LaserOff";
		kbatchedAnimEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
		ScoutRoverConfig.LaserEffect[] array = new ScoutRoverConfig.LaserEffect[]
		{
			new ScoutRoverConfig.LaserEffect
			{
				id = "DigEffect",
				animFile = "laser_kanim",
				anim = "idle",
				context = "dig"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "BuildEffect",
				animFile = "construct_beam_kanim",
				anim = "loop",
				context = "build"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "FetchLiquidEffect",
				animFile = "hose_fx_kanim",
				anim = "loop",
				context = "fetchliquid"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "PaintEffect",
				animFile = "paint_beam_kanim",
				anim = "loop",
				context = "paint"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "HarvestEffect",
				animFile = "plant_harvest_beam_kanim",
				anim = "loop",
				context = "harvest"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "CaptureEffect",
				animFile = "net_gun_fx_kanim",
				anim = "loop",
				context = "capture"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "AttackEffect",
				animFile = "attack_beam_fx_kanim",
				anim = "loop",
				context = "attack"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "PickupEffect",
				animFile = "vacuum_fx_kanim",
				anim = "loop",
				context = "pickup"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "StoreEffect",
				animFile = "vacuum_reverse_fx_kanim",
				anim = "loop",
				context = "store"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "DisinfectEffect",
				animFile = "plant_spray_beam_kanim",
				anim = "loop",
				context = "disinfect"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "TendEffect",
				animFile = "plant_tending_beam_fx_kanim",
				anim = "loop",
				context = "tend"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "PowerTinkerEffect",
				animFile = "electrician_beam_fx_kanim",
				anim = "idle",
				context = "powertinker"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "SpecialistDigEffect",
				animFile = "senior_miner_beam_fx_kanim",
				anim = "idle",
				context = "specialistdig"
			},
			new ScoutRoverConfig.LaserEffect
			{
				id = "DemolishEffect",
				animFile = "poi_demolish_fx_kanim",
				anim = "idle",
				context = "demolish"
			}
		};
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		foreach (ScoutRoverConfig.LaserEffect laserEffect in array)
		{
			GameObject gameObject2 = new GameObject(laserEffect.id);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
			KBatchedAnimTracker kbatchedAnimTracker = gameObject2.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.controller = component;
			kbatchedAnimTracker.symbol = new HashedString("snapto_radar");
			kbatchedAnimTracker.offset = new Vector3(40f, 0f, 0f);
			kbatchedAnimTracker.useTargetPoint = true;
			KBatchedAnimController kbatchedAnimController = gameObject2.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(laserEffect.animFile)
			};
			KBatchedAnimEventToggler.Entry item = new KBatchedAnimEventToggler.Entry
			{
				anim = laserEffect.anim,
				context = laserEffect.context,
				controller = kbatchedAnimController
			};
			kbatchedAnimEventToggler.entries.Add(item);
			gameObject2.AddOrGet<LoopingSounds>();
		}
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00025D74 File Offset: 0x00023F74
	public void OnPrefabInit(GameObject inst)
	{
		ChoreConsumer component = inst.GetComponent<ChoreConsumer>();
		if (component != null)
		{
			component.AddProvider(GlobalChoreProvider.Instance);
		}
		AmountInstance amountInstance = Db.Get().Amounts.InternalChemicalBattery.Lookup(inst);
		amountInstance.value = amountInstance.GetMax();
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00025DBC File Offset: 0x00023FBC
	public void OnSpawn(GameObject inst)
	{
		Sensors component = inst.GetComponent<Sensors>();
		component.Add(new PathProberSensor(component));
		component.Add(new PickupableSensor(component));
		Navigator component2 = inst.GetComponent<Navigator>();
		component2.transitionDriver.overrideLayers.Add(new BipedTransitionLayer(component2, 3.325f, 2.5f));
		component2.transitionDriver.overrideLayers.Add(new DoorTransitionLayer(component2));
		component2.transitionDriver.overrideLayers.Add(new LadderDiseaseTransitionLayer(component2));
		component2.transitionDriver.overrideLayers.Add(new SplashTransitionLayer(component2));
		component2.SetFlags(PathFinder.PotentialPath.Flags.None);
		component2.CurrentNavType = NavType.Floor;
		PathProber component3 = inst.GetComponent<PathProber>();
		if (component3 != null)
		{
			component3.SetGroupProber(MinionGroupProber.Get());
		}
		Effects effects = inst.GetComponent<Effects>();
		if (inst.transform.parent == null)
		{
			if (effects.HasEffect("ScoutBotCharging"))
			{
				effects.Remove("ScoutBotCharging");
			}
		}
		else if (!effects.HasEffect("ScoutBotCharging"))
		{
			effects.Add("ScoutBotCharging", false);
		}
		inst.Subscribe(856640610, delegate(object data)
		{
			if (inst.transform.parent == null)
			{
				if (effects.HasEffect("ScoutBotCharging"))
				{
					effects.Remove("ScoutBotCharging");
					return;
				}
			}
			else if (!effects.HasEffect("ScoutBotCharging"))
			{
				effects.Add("ScoutBotCharging", false);
			}
		});
	}

	// Token: 0x040003D9 RID: 985
	public const string ID = "ScoutRover";

	// Token: 0x040003DA RID: 986
	public static string ROVER_BASE_TRAIT_ID = "ScoutRoverBaseTrait";

	// Token: 0x040003DB RID: 987
	public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;

	// Token: 0x040003DC RID: 988
	public const float MASS = 100f;

	// Token: 0x040003DD RID: 989
	private const float WIDTH = 1f;

	// Token: 0x040003DE RID: 990
	private const float HEIGHT = 2f;

	// Token: 0x02000F34 RID: 3892
	public struct LaserEffect
	{
		// Token: 0x0400553B RID: 21819
		public string id;

		// Token: 0x0400553C RID: 21820
		public string animFile;

		// Token: 0x0400553D RID: 21821
		public string anim;

		// Token: 0x0400553E RID: 21822
		public HashedString context;
	}
}
