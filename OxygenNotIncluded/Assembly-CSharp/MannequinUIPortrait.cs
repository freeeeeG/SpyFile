using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000289 RID: 649
public class MannequinUIPortrait : IEntityConfig
{
	// Token: 0x06000D2A RID: 3370 RVA: 0x0004911F File Offset: 0x0004731F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00049128 File Offset: 0x00047328
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MannequinUIPortrait.ID, MannequinUIPortrait.ID, true);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = 0.5f;
		kbatchedAnimController.setScaleFromAnim = false;
		kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mannequin_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		MinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00049257 File Offset: 0x00047457
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x00049259 File Offset: 0x00047459
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007B2 RID: 1970
	public static string ID = "MannequinUIPortrait";
}
