using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028D RID: 653
public class MinionSelectPreviewConfig : IEntityConfig
{
	// Token: 0x06000D48 RID: 3400 RVA: 0x0004B0E3 File Offset: 0x000492E3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0004B0EC File Offset: 0x000492EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionSelectPreviewConfig.ID, MinionSelectPreviewConfig.ID, true);
		RectTransform rectTransform = gameObject.AddOrGet<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		LayoutElement layoutElement = gameObject.AddOrGet<LayoutElement>();
		layoutElement.preferredHeight = 100f;
		layoutElement.preferredWidth = 100f;
		gameObject.AddOrGet<Effects>();
		gameObject.AddOrGet<Traits>();
		MinionModifiers minionModifiers = gameObject.AddOrGet<MinionModifiers>();
		minionModifiers.initialTraits.Add(MinionConfig.MINION_BASE_TRAIT_ID);
		MinionConfig.AddMinionAmounts(minionModifiers);
		gameObject.AddOrGet<AttributeLevels>();
		gameObject.AddOrGet<AttributeConverters>();
		gameObject.AddOrGet<MinionIdentity>().addToIdentityList = false;
		gameObject.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
		gameObject.AddOrGet<FaceGraph>();
		gameObject.AddOrGet<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
		kbatchedAnimController.animScale = 0.5f;
		kbatchedAnimController.setScaleFromAnim = false;
		kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_construction_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_cheer_kanim")
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		MinionConfig.ConfigureSymbols(gameObject, false);
		return gameObject;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0004B29B File Offset: 0x0004949B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004B29D File Offset: 0x0004949D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007BF RID: 1983
	public static string ID = "MinionSelectPreview";
}
