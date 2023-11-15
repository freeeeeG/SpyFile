using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class PropFacilityDeskConfig : IEntityConfig
{
	// Token: 0x06000EEC RID: 3820 RVA: 0x00051E6C File Offset: 0x0005006C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x00051E74 File Offset: 0x00050074
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_magazine", UI.USERMENUACTIONS.READLORE.SEARCH_STERNSDESK));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x00051F3A File Offset: 0x0005013A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x00051F3C File Offset: 0x0005013C
	public void OnSpawn(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
		}
	}
}
