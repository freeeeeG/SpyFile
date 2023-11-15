using System;
using Database;
using UnityEngine;

// Token: 0x02000B4C RID: 2892
public static class KleiPermitVisUtil
{
	// Token: 0x0600592A RID: 22826 RVA: 0x00209E9C File Offset: 0x0020809C
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			anim
		});
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x0600592B RID: 22827 RVA: 0x00209F04 File Offset: 0x00208104
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x0600592C RID: 22828 RVA: 0x00209F5C File Offset: 0x0020815C
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artablePermit.animFile)
		});
		buildingKAnim.Play(artablePermit.anim, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x0600592D RID: 22829 RVA: 0x00209FC4 File Offset: 0x002081C4
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			artablePermit.animFile
		});
		bool flag;
		HashedString hashedString;
		KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile).Deconstruct(out flag, out hashedString);
		bool flag2 = flag;
		HashedString anim_name = hashedString;
		if (flag2)
		{
			buildingKAnim.Play(anim_name, KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			global::Debug.Assert(false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile), KAnim.PlayMode.Once, 1f, 0f);
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x0600592E RID: 22830 RVA: 0x0020A068 File Offset: 0x00208268
	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x0020A0C4 File Offset: 0x002082C4
	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x0020A112 File Offset: 0x00208312
	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x0020A128 File Offset: 0x00208328
	public static Option<HashedString> GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
	{
		KAnimFileData data = stickerAnimFile.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.name.StartsWith("idle_sticker"))
			{
				return anim.hash;
			}
		}
		return Option.None;
	}

	// Token: 0x06005932 RID: 22834 RVA: 0x0020A180 File Offset: 0x00208380
	public static Option<BuildLocationRule> GetBuildLocationRule(PermitResource permit)
	{
		Option<BuildingDef> buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		if (!buildingDef.HasValue)
		{
			return Option.None;
		}
		return buildingDef.Value.BuildLocationRule;
	}

	// Token: 0x06005933 RID: 22835 RVA: 0x0020A1BC File Offset: 0x002083BC
	public static Option<BuildingDef> GetBuildingDef(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
		if (buildingFacadeResource != null)
		{
			GameObject prefab = Assets.GetPrefab(buildingFacadeResource.PrefabID);
			if (prefab == null)
			{
				return Option.None;
			}
			BuildingComplete component = prefab.GetComponent<BuildingComplete>();
			if (component == null || !component)
			{
				return Option.None;
			}
			return component.Def;
		}
		else
		{
			ArtableStage artableStage = permit as ArtableStage;
			if (artableStage == null)
			{
				return Option.None;
			}
			BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
			if (component2 == null || !component2)
			{
				return Option.None;
			}
			return component2.Def;
		}
	}

	// Token: 0x04003C4D RID: 15437
	public const float TILE_SIZE_UI = 176f;
}
