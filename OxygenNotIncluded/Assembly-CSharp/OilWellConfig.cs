using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class OilWellConfig : IEntityConfig
{
	// Token: 0x06000693 RID: 1683 RVA: 0x0002B0DA File Offset: 0x000292DA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002B0E4 File Offset: 0x000292E4
	public GameObject CreatePrefab()
	{
		string id = "OilWell";
		string name = STRINGS.CREATURES.SPECIES.OIL_WELL.NAME;
		string desc = STRINGS.CREATURES.SPECIES.OIL_WELL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_oil_kanim"), "off", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.SedimentaryRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.OilWell, null)
		};
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002B1D4 File Offset: 0x000293D4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002B1D6 File Offset: 0x000293D6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000499 RID: 1177
	public const string ID = "OilWell";
}
