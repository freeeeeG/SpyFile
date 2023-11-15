using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class MissileBasicConfig : IEntityConfig
{
	// Token: 0x06000D54 RID: 3412 RVA: 0x0004B45D File Offset: 0x0004965D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0004B464 File Offset: 0x00049664
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0004B4E4 File Offset: 0x000496E4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0004B4E6 File Offset: 0x000496E6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007C1 RID: 1985
	public const string ID = "MissileBasic";

	// Token: 0x040007C2 RID: 1986
	public static ComplexRecipe recipe;

	// Token: 0x040007C3 RID: 1987
	public const float MASS_PER_MISSILE = 10f;
}
