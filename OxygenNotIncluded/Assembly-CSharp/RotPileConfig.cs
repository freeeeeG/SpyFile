using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class RotPileConfig : IEntityConfig
{
	// Token: 0x0600079C RID: 1948 RVA: 0x0002E7A9 File Offset: 0x0002C9A9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, ITEMS.FOOD.ROTPILE.NAME, ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim("rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Organics, false);
		component.AddTag(GameTags.Compostable, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<OccupyArea>();
		gameObject.AddOrGet<Modifiers>();
		gameObject.AddOrGet<RotPile>();
		gameObject.AddComponent<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		return gameObject;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002E853 File Offset: 0x0002CA53
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<DecorProvider>().overrideName = ITEMS.FOOD.ROTPILE.NAME;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002E86A File Offset: 0x0002CA6A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000514 RID: 1300
	public static string ID = "RotPile";
}
