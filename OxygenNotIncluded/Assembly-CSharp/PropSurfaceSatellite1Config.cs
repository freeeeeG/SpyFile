using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class PropSurfaceSatellite1Config : IEntityConfig
{
	// Token: 0x06000F83 RID: 3971 RVA: 0x00053A8C File Offset: 0x00051C8C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x00053A94 File Offset: 0x00051C94
	public GameObject CreatePrefab()
	{
		string id = "PropSurfaceSatellite1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite1_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[]
		{
			4,
			9
		};
		gameObject.AddOrGet<Demolishable>();
		LoreBearerUtil.AddLoreTo(gameObject);
		return gameObject;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x00053B74 File Offset: 0x00051D74
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureClusterSpaceEnabled() ? "OrbitalResearchDatabank" : "ResearchDatabank";
		return new string[][]
		{
			new string[]
			{
				text,
				text,
				text
			},
			new string[]
			{
				"ColdBreatherSeed",
				"ColdBreatherSeed",
				"ColdBreatherSeed"
			},
			new string[]
			{
				"Atmo_Suit",
				"Glom",
				"Glom",
				"Glom"
			}
		};
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x00053BFC File Offset: 0x00051DFC
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
		component.ChooseContents();
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		RadiationEmitter radiationEmitter = inst.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x00053C74 File Offset: 0x00051E74
	public void OnSpawn(GameObject inst)
	{
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x04000884 RID: 2180
	public const string ID = "PropSurfaceSatellite1";
}
