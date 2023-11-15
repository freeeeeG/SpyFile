using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06000E97 RID: 3735 RVA: 0x00050918 File Offset: 0x0004EB18
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MopPlacerConfig.ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
		gameObject.AddTag(GameTags.NotConversationTopic);
		Moppable moppable = gameObject.AddOrGet<Moppable>();
		moppable.synchronizeAnims = false;
		moppable.amountMoppedPerTick = 20f;
		gameObject.AddOrGet<Cancellable>();
		return gameObject;
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00050972 File Offset: 0x0004EB72
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x00050974 File Offset: 0x0004EB74
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000864 RID: 2148
	public static string ID = "MopPlacer";

	// Token: 0x02000F80 RID: 3968
	[Serializable]
	public class MopPlacerAssets
	{
		// Token: 0x04005612 RID: 22034
		public Material material;
	}
}
