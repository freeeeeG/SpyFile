using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class PropFacilityGlobeDroorsConfig : IEntityConfig
{
	// Token: 0x06000F00 RID: 3840 RVA: 0x000522DC File Offset: 0x000504DC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x000522E4 File Offset: 0x000504E4
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityGlobeDroors";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_globe_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_newspaper", UI.USERMENUACTIONS.READLORE.SEARCH_CABINET));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x000523AA File Offset: 0x000505AA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000523AC File Offset: 0x000505AC
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
