using System;
using UnityEngine;

// Token: 0x02000BB5 RID: 2997
public readonly struct OutfitDesignerScreenConfig
{
	// Token: 0x06005D91 RID: 23953 RVA: 0x00223CB4 File Offset: 0x00221EB4
	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		this.outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : Option.None);
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		this.isValid = true;
		ClothingOutfitTarget.MinionInstance minionInstance;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
		{
			global::Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
		}
	}

	// Token: 0x06005D92 RID: 23954 RVA: 0x00223D2E File Offset: 0x00221F2E
	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
	}

	// Token: 0x06005D93 RID: 23955 RVA: 0x00223D48 File Offset: 0x00221F48
	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
	}

	// Token: 0x06005D94 RID: 23956 RVA: 0x00223D62 File Offset: 0x00221F62
	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None, null);
	}

	// Token: 0x06005D95 RID: 23957 RVA: 0x00223D7F File Offset: 0x00221F7F
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None, null);
	}

	// Token: 0x06005D96 RID: 23958 RVA: 0x00223D98 File Offset: 0x00221F98
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality value = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		ClothingOutfitTarget.MinionInstance minionInstance;
		global::Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
		global::Debug.Assert(minionInstance.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, value, targetMinionInstance, null);
	}

	// Token: 0x06005D97 RID: 23959 RVA: 0x00223DF4 File Offset: 0x00221FF4
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06005D98 RID: 23960 RVA: 0x00223E34 File Offset: 0x00222034
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen, null);
	}

	// Token: 0x04003F03 RID: 16131
	public readonly ClothingOutfitTarget sourceTarget;

	// Token: 0x04003F04 RID: 16132
	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	// Token: 0x04003F05 RID: 16133
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04003F06 RID: 16134
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04003F07 RID: 16135
	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	// Token: 0x04003F08 RID: 16136
	public readonly bool isValid;
}
