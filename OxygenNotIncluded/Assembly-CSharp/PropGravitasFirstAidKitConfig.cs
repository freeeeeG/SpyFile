using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class PropGravitasFirstAidKitConfig : IEntityConfig
{
	// Token: 0x06000F37 RID: 3895 RVA: 0x00052CFA File Offset: 0x00050EFA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00052D04 File Offset: 0x00050F04
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFirstAidKit";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_first_aid_kit_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00052DC8 File Offset: 0x00050FC8
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure";
		return new string[][]
		{
			new string[]
			{
				"BasicCure",
				"BasicCure",
				"BasicCure"
			},
			new string[]
			{
				text,
				text
			}
		};
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00052E21 File Offset: 0x00051021
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropGravitasFirstAidKitConfig.GetLockerBaseContents();
		component.ChooseContents();
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00052E4E File Offset: 0x0005104E
	public void OnSpawn(GameObject inst)
	{
	}
}
