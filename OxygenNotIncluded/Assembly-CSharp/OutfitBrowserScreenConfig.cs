using System;
using UnityEngine;

// Token: 0x02000BB1 RID: 2993
public readonly struct OutfitBrowserScreenConfig
{
	// Token: 0x06005D63 RID: 23907 RVA: 0x00222594 File Offset: 0x00220794
	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		this.isPickingOutfitForDupe = (minionPersonality.HasValue || minionInstance.HasValue);
		this.targetMinionInstance = minionInstance;
		this.isValid = true;
		if (minionPersonality.IsSome() || this.targetMinionInstance.IsSome())
		{
			global::Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	// Token: 0x06005D64 RID: 23908 RVA: 0x00222605 File Offset: 0x00220805
	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x06005D65 RID: 23909 RVA: 0x0022261F File Offset: 0x0022081F
	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x06005D66 RID: 23910 RVA: 0x0022263C File Offset: 0x0022083C
	public string GetMinionName()
	{
		if (this.targetMinionInstance.HasValue)
		{
			return this.targetMinionInstance.Value.GetProperName();
		}
		if (this.minionPersonality.HasValue)
		{
			return this.minionPersonality.Value.Name;
		}
		return "-";
	}

	// Token: 0x06005D67 RID: 23911 RVA: 0x0022268A File Offset: 0x0022088A
	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	// Token: 0x06005D68 RID: 23912 RVA: 0x002226B9 File Offset: 0x002208B9
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	// Token: 0x06005D69 RID: 23913 RVA: 0x002226E0 File Offset: 0x002208E0
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality value = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), value, minionInstance);
	}

	// Token: 0x06005D6A RID: 23914 RVA: 0x0022272C File Offset: 0x0022092C
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x0022276C File Offset: 0x0022096C
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen, null);
	}

	// Token: 0x04003ED8 RID: 16088
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	// Token: 0x04003ED9 RID: 16089
	public readonly Option<ClothingOutfitTarget> selectedTarget;

	// Token: 0x04003EDA RID: 16090
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04003EDB RID: 16091
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04003EDC RID: 16092
	public readonly bool isValid;

	// Token: 0x04003EDD RID: 16093
	public readonly bool isPickingOutfitForDupe;
}
