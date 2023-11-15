using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class FishFeederBotConfig : IEntityConfig
{
	// Token: 0x060005F1 RID: 1521 RVA: 0x00027750 File Offset: 0x00025950
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00027758 File Offset: 0x00025958
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("FishFeederBot", "FishFeederBot", true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("fishfeeder_kanim")
		};
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		SymbolOverrideControllerUtil.AddToPrefab(kbatchedAnimController.gameObject);
		return gameObject;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x000277B0 File Offset: 0x000259B0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x000277B2 File Offset: 0x000259B2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400042A RID: 1066
	public const string ID = "FishFeederBot";
}
