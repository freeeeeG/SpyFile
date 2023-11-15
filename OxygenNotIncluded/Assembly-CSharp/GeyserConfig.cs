using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class GeyserConfig : IEntityConfig
{
	// Token: 0x0600066E RID: 1646 RVA: 0x00029C6F File Offset: 0x00027E6F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00029C78 File Offset: 0x00027E78
	public GameObject CreatePrefab()
	{
		string id = "Geyser";
		string name = STRINGS.CREATURES.SPECIES.GEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_steam_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<UserNameable>();
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "steam";
		geyserConfigurator.presetMin = 0.5f;
		geyserConfigurator.presetMax = 0.75f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00029D9E File Offset: 0x00027F9E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00029DA0 File Offset: 0x00027FA0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000473 RID: 1139
	public const int GEOTUNERS_REQUIRED_FOR_MAJOR_TRACKER_ANIMATION = 5;

	// Token: 0x02000F36 RID: 3894
	public enum TrackerMeterAnimNames
	{
		// Token: 0x04005542 RID: 21826
		tracker,
		// Token: 0x04005543 RID: 21827
		geotracker,
		// Token: 0x04005544 RID: 21828
		geotracker_minor,
		// Token: 0x04005545 RID: 21829
		geotracker_major
	}
}
