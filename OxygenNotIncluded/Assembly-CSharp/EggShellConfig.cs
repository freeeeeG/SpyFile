using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class EggShellConfig : IEntityConfig
{
	// Token: 0x06000984 RID: 2436 RVA: 0x00037D39 File Offset: 0x00035F39
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00037D40 File Offset: 0x00035F40
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EggShell", ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim("eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00037DC0 File Offset: 0x00035FC0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00037DC2 File Offset: 0x00035FC2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FE RID: 1534
	public const string ID = "EggShell";

	// Token: 0x040005FF RID: 1535
	public static readonly Tag TAG = TagManager.Create("EggShell");

	// Token: 0x04000600 RID: 1536
	public const float EGG_TO_SHELL_RATIO = 0.5f;
}
