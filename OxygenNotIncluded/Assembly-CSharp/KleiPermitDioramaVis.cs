using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
public class KleiPermitDioramaVis : KMonoBehaviour
{
	// Token: 0x060058DA RID: 22746 RVA: 0x00208F42 File Offset: 0x00207142
	protected override void OnPrefabInit()
	{
		this.Init();
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x00208F4C File Offset: 0x0020714C
	private void Init()
	{
		if (this.initComplete)
		{
			return;
		}
		this.allVisList = ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.ConfigureSetup();
		}
		this.initComplete = true;
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x00208FBC File Offset: 0x002071BC
	public void ConfigureWith(PermitResource permit)
	{
		if (!this.initComplete)
		{
			this.Init();
		}
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.GetGameObject().SetActive(false);
		}
		IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(true);
		permitVisTarget.ConfigureWith(permit);
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x00209034 File Offset: 0x00207234
	public IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		KleiPermitDioramaVis.lastRenderedPermit = permit;
		if (permit == null)
		{
			return this.fallbackVis.WithError(string.Format("Given invalid permit: {0}", permit));
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes)
		{
			return this.equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			bool flag;
			BuildLocationRule buildLocationRule;
			KleiPermitVisUtil.GetBuildLocationRule(permit).Deconstruct(out flag, out buildLocationRule);
			bool flag2 = flag;
			BuildLocationRule buildLocationRule2 = buildLocationRule;
			if (!flag2)
			{
				return this.fallbackVis.WithError("Couldn't get BuildLocationRule on permit with id \"" + permit.Id + "\"");
			}
			switch (buildLocationRule2)
			{
			case BuildLocationRule.OnFloor:
				return this.buildingOnFloorVis;
			case BuildLocationRule.OnCeiling:
			{
				string prefabID = KleiPermitVisUtil.GetBuildingDef(permit).Value.PrefabID;
				if (prefabID == "FlowerVaseHanging" || prefabID == "FlowerVaseHangingFancy")
				{
					return this.buildingHangingHookVis;
				}
				return this.buildingPresentationStandVis.WithAlignment(Alignment.Top());
			}
			case BuildLocationRule.OnWall:
				return this.buildingPresentationStandVis.WithAlignment(Alignment.Left());
			case BuildLocationRule.InCorner:
				return this.buildingPresentationStandVis.WithAlignment(Alignment.TopLeft());
			case BuildLocationRule.NotInTiles:
				return this.pedestalAndItemVis;
			}
			return this.fallbackVis.WithError(string.Format("No visualization available for building with BuildLocationRule of {0}", buildLocationRule2));
		}
		else if (permit.Category == PermitCategory.Artwork)
		{
			bool flag;
			BuildingDef buildingDef;
			KleiPermitVisUtil.GetBuildingDef(permit).Deconstruct(out flag, out buildingDef);
			bool flag3 = flag;
			BuildingDef buildingDef2 = buildingDef;
			if (!flag3)
			{
				return this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|16_0<Sculpture>(buildingDef2))
			{
				return this.artableSculptureVis;
			}
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|16_0<Painting>(buildingDef2))
			{
				return this.artablePaintingVis;
			}
			return this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
		}
		else
		{
			if (permit.Category != PermitCategory.JoyResponse)
			{
				return this.fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
			}
			if (permit is BalloonArtistFacadeResource)
			{
				return this.joyResponseBalloonVis;
			}
			return this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x002092B4 File Offset: 0x002074B4
	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		}
		return null;
	}

	// Token: 0x060058DF RID: 22751 RVA: 0x00209360 File Offset: 0x00207560
	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return Assets.GetSprite("screen_bg_clothing");
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return Assets.GetSprite("screen_bg_atmosuit");
		default:
			return null;
		}
	}

	// Token: 0x060058E1 RID: 22753 RVA: 0x002093BA File Offset: 0x002075BA
	[CompilerGenerated]
	internal static bool <GetPermitVisTarget>g__Has|16_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04003C13 RID: 15379
	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	// Token: 0x04003C14 RID: 15380
	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	// Token: 0x04003C15 RID: 15381
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	// Token: 0x04003C16 RID: 15382
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingPresentationStandVis;

	// Token: 0x04003C17 RID: 15383
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStandHanging buildingPresentationStandHangingVis;

	// Token: 0x04003C18 RID: 15384
	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookVis;

	// Token: 0x04003C19 RID: 15385
	[SerializeField]
	private KleiPermitDioramaVis_PedestalAndItem pedestalAndItemVis;

	// Token: 0x04003C1A RID: 15386
	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	// Token: 0x04003C1B RID: 15387
	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	// Token: 0x04003C1C RID: 15388
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	// Token: 0x04003C1D RID: 15389
	private bool initComplete;

	// Token: 0x04003C1E RID: 15390
	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	// Token: 0x04003C1F RID: 15391
	public static PermitResource lastRenderedPermit;
}
