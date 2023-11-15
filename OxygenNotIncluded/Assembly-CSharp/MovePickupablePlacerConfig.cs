using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class MovePickupablePlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06000E9C RID: 3740 RVA: 0x0005098C File Offset: 0x0004EB8C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MovePickupablePlacerConfig.ID, MISC.PLACERS.MOVEPICKUPABLEPLACER.NAME, Assets.instance.movePickupToPlacerAssets.material);
		gameObject.AddOrGet<CancellableMove>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.showUnreachableStatus = true;
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x000509F0 File Offset: 0x0004EBF0
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x000509F2 File Offset: 0x0004EBF2
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000865 RID: 2149
	public static string ID = "MovePickupablePlacer";

	// Token: 0x02000F81 RID: 3969
	[Serializable]
	public class MovePickupablePlacerAssets
	{
		// Token: 0x04005613 RID: 22035
		public Material material;
	}
}
