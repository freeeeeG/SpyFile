using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class MushBarConfig : IEntityConfig
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0002DFDE File Offset: 0x0002C1DE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0002DFE8 File Offset: 0x0002C1E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MushBar", ITEMS.FOOD.MUSHBAR.NAME, ITEMS.FOOD.MUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MUSHBAR);
		ComplexRecipeManager.Get().GetRecipe(MushBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0002E06F File Offset: 0x0002C26F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0002E071 File Offset: 0x0002C271
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0002E074 File Offset: 0x0002C274
	public static GameObject CreateFabricationVisualizer(GameObject result)
	{
		KBatchedAnimController component = result.GetComponent<KBatchedAnimController>();
		GameObject gameObject = new GameObject();
		gameObject.name = result.name + "Visualizer";
		gameObject.SetActive(false);
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = component.AnimFiles;
		kbatchedAnimController.initialAnim = "fabricating";
		kbatchedAnimController.isMovable = true;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = new HashedString("meter_ration");
		kbatchedAnimTracker.offset = Vector3.zero;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject;
	}

	// Token: 0x04000501 RID: 1281
	public const string ID = "MushBar";

	// Token: 0x04000502 RID: 1282
	public static ComplexRecipe recipe;
}
