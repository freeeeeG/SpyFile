using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200098C RID: 2444
public static class AutoRocketUtility
{
	// Token: 0x0600481C RID: 18460 RVA: 0x00196C4E File Offset: 0x00194E4E
	public static void StartAutoRocket(LaunchPad selectedPad)
	{
		selectedPad.StartCoroutine(AutoRocketUtility.AutoRocketRoutine(selectedPad));
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x00196C5D File Offset: 0x00194E5D
	private static IEnumerator AutoRocketRoutine(LaunchPad selectedPad)
	{
		GameObject baseModule = AutoRocketUtility.AddEngine(selectedPad);
		GameObject oxidizerTank = AutoRocketUtility.AddOxidizerTank(baseModule);
		yield return SequenceUtil.WaitForEndOfFrame;
		AutoRocketUtility.AddOxidizer(oxidizerTank);
		GameObject gameObject = AutoRocketUtility.AddPassengerModule(oxidizerTank);
		AutoRocketUtility.AddDrillCone(AutoRocketUtility.AddSolidStorageModule(gameObject));
		PassengerRocketModule passengerModule = gameObject.GetComponent<PassengerRocketModule>();
		ClustercraftExteriorDoor exteriorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>();
		int max = 100;
		while (exteriorDoor.GetInteriorDoor() == null && max > 0)
		{
			int num = max;
			max = num - 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		WorldContainer interiorWorld = passengerModule.GetComponent<RocketModuleCluster>().CraftInterface.GetInteriorWorld();
		RocketControlStation station = Components.RocketControlStations.GetWorldItems(interiorWorld.id, false)[0];
		GameObject minion = AutoRocketUtility.AddPilot(station);
		AutoRocketUtility.AddOxygen(station);
		yield return SequenceUtil.WaitForEndOfFrame;
		AutoRocketUtility.AssignCrew(minion, passengerModule);
		yield break;
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x00196C6C File Offset: 0x00194E6C
	private static GameObject AddEngine(LaunchPad selectedPad)
	{
		BuildingDef buildingDef = Assets.GetBuildingDef("KeroseneEngineClusterSmall");
		List<Tag> elements = new List<Tag>
		{
			SimHashes.Steel.CreateTag()
		};
		GameObject gameObject = selectedPad.AddBaseModule(buildingDef, elements);
		Element element = ElementLoader.GetElement(gameObject.GetComponent<RocketEngineCluster>().fuelTag);
		Storage component = gameObject.GetComponent<Storage>();
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
			return gameObject;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
			return gameObject;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
		}
		return gameObject;
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x00196D48 File Offset: 0x00194F48
	private static GameObject AddPassengerModule(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("HabitatModuleMedium");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x00196D84 File Offset: 0x00194F84
	private static GameObject AddSolidStorageModule(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("SolidCargoBaySmall");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Steel.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x00196DC0 File Offset: 0x00194FC0
	private static GameObject AddDrillCone(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("NoseconeHarvest");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Steel.CreateTag(),
			SimHashes.Polypropylene.CreateTag()
		};
		GameObject gameObject = component.AddModule(buildingDef, buildMaterials);
		gameObject.GetComponent<Storage>().AddOre(SimHashes.Diamond, 1000f, 273f, byte.MaxValue, 0, false, true);
		return gameObject;
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x00196E30 File Offset: 0x00195030
	private static GameObject AddOxidizerTank(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("SmallOxidizerTank");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x00196E6C File Offset: 0x0019506C
	private static void AddOxidizer(GameObject oxidizerTank)
	{
		SimHashes simHashes = SimHashes.OxyRock;
		Element element = ElementLoader.FindElementByHash(simHashes);
		DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
		oxidizerTank.GetComponent<OxidizerTank>().DEBUG_FillTank(simHashes);
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x00196EA8 File Offset: 0x001950A8
	private static GameObject AddPilot(RocketControlStation station)
	{
		Vector3 position = station.transform.position;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position2);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, null, true).Apply(gameObject);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
		{
			component.ForceAddSkillPoint();
		}
		string id = Db.Get().Skills.RocketPiloting1.Id;
		MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(id);
		bool flag = component.CanMasterSkill(skillMasteryConditions);
		if (component != null && !component.HasMasteredSkill(id) && flag)
		{
			component.MasterSkill(id);
		}
		return gameObject;
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x00196F94 File Offset: 0x00195194
	private static void AddOxygen(RocketControlStation station)
	{
		SimMessages.ReplaceElement(Grid.PosToCell(station.transform.position + Vector3.up * 2f), SimHashes.OxyRock, CellEventLogger.Instance.DebugTool, 1000f, 273f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x00196FEC File Offset: 0x001951EC
	private static void AssignCrew(GameObject minion, PassengerRocketModule passengerModule)
	{
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			if (Components.MinionAssignablesProxy[i].GetTargetGameObject() == minion)
			{
				passengerModule.GetComponent<AssignmentGroupController>().SetMember(Components.MinionAssignablesProxy[i], true);
				break;
			}
		}
		passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x00197046 File Offset: 0x00195246
	private static void SetDestination(CraftModuleInterface craftModuleInterface, PassengerRocketModule passengerModule)
	{
		craftModuleInterface.GetComponent<ClusterDestinationSelector>().SetDestination(passengerModule.GetMyWorldLocation() + AxialI.NORTHEAST);
	}
}
