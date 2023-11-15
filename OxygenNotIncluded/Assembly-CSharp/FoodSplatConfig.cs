using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class FoodSplatConfig : IEntityConfig
{
	// Token: 0x060007F3 RID: 2035 RVA: 0x0002F084 File Offset: 0x0002D284
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002F08C File Offset: 0x0002D28C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateBasicEntity("FoodSplat", ITEMS.FOOD.FOODSPLAT.NAME, ITEMS.FOOD.FOODSPLAT.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "idle_sticker_a", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002F0DD File Offset: 0x0002D2DD
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<KSelectable>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		inst.AddOrGetDef<Splat.Def>();
		inst.AddOrGet<SplatWorkable>();
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002F11C File Offset: 0x0002D31C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000531 RID: 1329
	public const string ID = "FoodSplat";
}
