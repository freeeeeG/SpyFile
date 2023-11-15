using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class BasicFabricConfig : IEntityConfig
{
	// Token: 0x06000606 RID: 1542 RVA: 0x00027C0A File Offset: 0x00025E0A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00027C14 File Offset: 0x00025E14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(BasicFabricConfig.ID, ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.DESC, 1f, true, Assets.GetAnim("swampreedwool_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.BuildingFiber
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
		return gameObject;
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00027CAA File Offset: 0x00025EAA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00027CAC File Offset: 0x00025EAC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000430 RID: 1072
	public static string ID = "BasicFabric";

	// Token: 0x04000431 RID: 1073
	private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true, false, true);
}
