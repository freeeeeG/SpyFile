using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class SwampLilyFlowerConfig : IEntityConfig
{
	// Token: 0x060007CC RID: 1996 RVA: 0x0002EC9C File Offset: 0x0002CE9C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0002ECA4 File Offset: 0x0002CEA4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SwampLilyFlowerConfig.ID, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.DESC, 1f, false, Assets.GetAnim("swamplilyflower_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0002ED1B File Offset: 0x0002CF1B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002ED1D File Offset: 0x0002CF1D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000522 RID: 1314
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x04000523 RID: 1315
	public static string ID = "SwampLilyFlower";
}
