using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000C83 RID: 3203
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06006619 RID: 26137 RVA: 0x00261B58 File Offset: 0x0025FD58
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x0600661A RID: 26138 RVA: 0x00261B74 File Offset: 0x0025FD74
	public Option<Personality> Personality
	{
		get
		{
			return default(Option<Personality>);
		}
	}

	// Token: 0x0600661B RID: 26139 RVA: 0x00261B8A File Offset: 0x0025FD8A
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600661C RID: 26140 RVA: 0x00261B94 File Offset: 0x0025FD94
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.LoadAnims();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle", KAnim.PlayMode.Paused, 1f, 0f);
			this.spawn = this.animController.gameObject;
			MinionConfig.ConfigureSymbols(this.spawn, false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	// Token: 0x0600661D RID: 26141 RVA: 0x00261C5C File Offset: 0x0025FE5C
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count<ClothingItemResource>() == 0;
		if (this.shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		MinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
		SymbolOverrideController component = this.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		Accessorizer component2 = this.SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component3 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.RemoveAllSymbolOverrides(0);
		if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			component2.ApplyMinionPersonality(this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"), null));
			component3.UpdateVisibleSymbols(outfitType);
			foreach (string str in UIMannequin.DEFAULT_CLOTHING_SYMBOLS_TO_SHOW_AND_HIDE)
			{
				this.animController.SetSymbolVisiblity(str, true);
			}
			if (!flag)
			{
				this.animController.SetSymbolVisiblity("belt", false);
			}
			ClothingItemResource itemForCategory = UIMannequin.GetItemForCategory(PermitCategory.DupeGloves, outfit);
			ClothingItemResource itemForCategory2 = UIMannequin.GetItemForCategory(PermitCategory.DupeTops, outfit);
			if (itemForCategory != null && itemForCategory2 != null && itemForCategory.AnimFile.GetData().build.GetSymbol("arm_lower_sleeve") == null && itemForCategory2.AnimFile.GetData().build.GetSymbol("arm_lower_sleeve") == null)
			{
				this.animController.SetSymbolVisiblity("arm_lower_sleeve", false);
			}
		}
		else
		{
			foreach (string str2 in UIMannequin.DEFAULT_CLOTHING_SYMBOLS_TO_SHOW_AND_HIDE)
			{
				this.animController.SetSymbolVisiblity(str2, false);
			}
		}
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			KAnim.Build build = clothingItemResource.AnimFile.GetData().build;
			if (build != null)
			{
				for (int j = 0; j < build.symbols.Length; j++)
				{
					string text = HashCache.Get().Get(build.symbols[j].hash);
					component.AddSymbolOverride(text, build.symbols[j], 0);
					this.animController.SetSymbolVisiblity(text, true);
				}
			}
		}
	}

	// Token: 0x0600661E RID: 26142 RVA: 0x00261EA8 File Offset: 0x002600A8
	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			if (clothingItemResource.Category == category)
			{
				return clothingItemResource;
			}
		}
		return null;
	}

	// Token: 0x0600661F RID: 26143 RVA: 0x00261EFC File Offset: 0x002600FC
	public void React(UIMinionOrMannequinReactSource source)
	{
		this.animController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04004656 RID: 18006
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04004657 RID: 18007
	private KBatchedAnimController animController;

	// Token: 0x04004658 RID: 18008
	private GameObject spawn;

	// Token: 0x04004659 RID: 18009
	public bool shouldShowOutfitWithDefaultItems = true;

	// Token: 0x0400465A RID: 18010
	public Option<Personality> personalityToUseForDefaultClothing;

	// Token: 0x0400465B RID: 18011
	private static readonly string[] DEFAULT_CLOTHING_SYMBOLS_TO_SHOW_AND_HIDE = new string[]
	{
		"arm_lower",
		"arm_lower_sleeve",
		"arm_sleeve",
		"belt",
		"cuff",
		"foot",
		"hand_paint",
		"leg",
		"neck",
		"pelvis",
		"torso"
	};
}
