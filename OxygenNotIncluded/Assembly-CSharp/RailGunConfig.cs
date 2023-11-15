using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class RailGunConfig : IBuildingConfig
{
	// Token: 0x06000FA6 RID: 4006 RVA: 0x000543E9 File Offset: 0x000525E9
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x000543F0 File Offset: 0x000525F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RailGun";
		int width = 5;
		int height = 6;
		string anim = "rail_gun_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(-2, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RailGun.PORT_ID, new CellOffset(-2, 2), STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(2, 0), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x00054548 File Offset: 0x00052748
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00054580 File Offset: 0x00052780
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		RailGun railGun = go.AddOrGet<RailGun>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		ClusterDestinationSelector clusterDestinationSelector = go.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		railGun.liquidPortInfo = this.liquidInputPort;
		railGun.gasPortInfo = this.gasInputPort;
		railGun.solidPortInfo = this.solidInputPort;
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.capacity = 200f;
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.showInUI = false;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00054614 File Offset: 0x00052814
	public override void DoPostConfigureComplete(GameObject go)
	{
		List<Tag> list = new List<Tag>();
		list.AddRange(STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
		list.AddRange(STORAGEFILTERS.GASES);
		list.AddRange(STORAGEFILTERS.FOOD);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.storageFilters = list;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.capacityKg = 1200f;
		go.GetComponent<HighEnergyParticlePort>().requireOperational = false;
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x00054692 File Offset: 0x00052892
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x000546A1 File Offset: 0x000528A1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x000546B0 File Offset: 0x000528B0
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = -2;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.AllOrNothingVisibility = true;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(RailGunConfig.RailGunSkyVisibility);
		};
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x00054704 File Offset: 0x00052904
	private static bool RailGunSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "RailGun assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000891 RID: 2193
	public const string ID = "RailGun";

	// Token: 0x04000892 RID: 2194
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04000893 RID: 2195
	public const int RANGE = 20;

	// Token: 0x04000894 RID: 2196
	public const float BASE_PARTICLE_COST = 0f;

	// Token: 0x04000895 RID: 2197
	public const float HEX_PARTICLE_COST = 10f;

	// Token: 0x04000896 RID: 2198
	public const float HEP_CAPACITY = 200f;

	// Token: 0x04000897 RID: 2199
	public const float TAKEOFF_VELOCITY = 35f;

	// Token: 0x04000898 RID: 2200
	public const int MAINTENANCE_AFTER_NUM_PAYLOADS = 6;

	// Token: 0x04000899 RID: 2201
	public const int MAINTENANCE_COOLDOWN = 30;

	// Token: 0x0400089A RID: 2202
	public const float CAPACITY = 1200f;

	// Token: 0x0400089B RID: 2203
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));

	// Token: 0x0400089C RID: 2204
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

	// Token: 0x0400089D RID: 2205
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));
}
