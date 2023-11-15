using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class ClustercraftInteriorDoorConfig : IEntityConfig
{
	// Token: 0x06000125 RID: 293 RVA: 0x00008A65 File Offset: 0x00006C65
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00008A6C File Offset: 0x00006C6C
	public GameObject CreatePrefab()
	{
		string id = ClustercraftInteriorDoorConfig.ID;
		string name = STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_hatch_door_kanim"), "closed", Grid.SceneLayer.TileMain, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.InteriorWall;
		gameObject.AddOrGet<ClustercraftInteriorDoor>();
		gameObject.AddOrGet<AssignmentGroupController>().generateGroupOnStart = false;
		gameObject.AddOrGet<NavTeleporter>().offset = new CellOffset(1, 0);
		gameObject.AddOrGet<AccessControl>();
		return gameObject;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00008B51 File Offset: 0x00006D51
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00008B68 File Offset: 0x00006D68
	public void OnSpawn(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		OccupyArea component2 = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		CellOffset[] occupiedCellsOffsets = component2.OccupiedCellsOffsets;
		int[] array = new int[occupiedCellsOffsets.Length];
		for (int i = 0; i < occupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = occupiedCellsOffsets[i];
			int num = Grid.OffsetCell(cell, offset);
			array[i] = num;
		}
		foreach (int num2 in array)
		{
			Grid.HasDoor[num2] = true;
			SimMessages.SetCellProperties(num2, 8);
			Grid.RenderedByWorld[num2] = false;
			World.Instance.groundRenderer.MarkDirty(num2);
			SimMessages.ReplaceAndDisplaceElement(num2, component.ElementID, CellEventLogger.Instance.DoorClose, component.Mass / 2f, component.Temperature, byte.MaxValue, 0, -1);
			SimMessages.SetCellProperties(num2, 4);
		}
	}

	// Token: 0x040000AA RID: 170
	public static string ID = "ClustercraftInteriorDoor";
}
