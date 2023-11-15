using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class PropSurfaceSatellite3Config : IEntityConfig
{
	// Token: 0x06000F8F RID: 3983 RVA: 0x00053E38 File Offset: 0x00052038
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00053E40 File Offset: 0x00052040
	public GameObject CreatePrefab()
	{
		string id = PropSurfaceSatellite3Config.ID;
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE3.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE3.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite3_kanim"), "off", Grid.SceneLayer.Building, 6, 6, tier, tier2, SimHashes.Creature, new List<Tag>
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
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00053F20 File Offset: 0x00052120
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

	// Token: 0x06000F92 RID: 3986 RVA: 0x00053F98 File Offset: 0x00052198
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-372600542, delegate(object locker)
		{
			this.OnLockerLooted(inst);
		});
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x00053FF2 File Offset: 0x000521F2
	private void OnLockerLooted(GameObject inst)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Any)), inst.transform.position);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		gameObject.SetActive(true);
	}

	// Token: 0x04000886 RID: 2182
	public static string ID = "PropSurfaceSatellite3";
}
