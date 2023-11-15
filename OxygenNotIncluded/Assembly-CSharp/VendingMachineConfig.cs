using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class VendingMachineConfig : IEntityConfig
{
	// Token: 0x06001082 RID: 4226 RVA: 0x0005964F File Offset: 0x0005784F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x00059658 File Offset: 0x00057858
	public GameObject CreatePrefab()
	{
		string id = "VendingMachine";
		string name = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vendingmachine_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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
		setLocker.machineSound = "VendingMachine_LP";
		setLocker.overrideAnim = "anim_break_kanim";
		setLocker.dropOffset = new Vector2I(1, 1);
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0005974C File Offset: 0x0005794C
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"FieldRation"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x00059783 File Offset: 0x00057983
	public void OnSpawn(GameObject inst)
	{
	}
}
