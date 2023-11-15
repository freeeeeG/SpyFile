using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000C84 RID: 3204
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06006622 RID: 26146 RVA: 0x00261FA3 File Offset: 0x002601A3
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

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06006623 RID: 26147 RVA: 0x00261FBF File Offset: 0x002601BF
	// (set) Token: 0x06006624 RID: 26148 RVA: 0x00261FC7 File Offset: 0x002601C7
	public Option<Personality> Personality { get; private set; }

	// Token: 0x06006625 RID: 26149 RVA: 0x00261FD0 File Offset: 0x002601D0
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x06006626 RID: 26150 RVA: 0x00261FD8 File Offset: 0x002601D8
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			MinionConfig.ConfigureSymbols(this.animController.gameObject, true);
			this.spawn = this.animController.gameObject;
		}
	}

	// Token: 0x06006627 RID: 26151 RVA: 0x0026207F File Offset: 0x0026027F
	public void SetMinion(Personality personality)
	{
		this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		this.Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	// Token: 0x06006628 RID: 26152 RVA: 0x002620BC File Offset: 0x002602BC
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearAllOutfitItems(null);
		component.ApplyClothingItems(outfitType, outfit);
	}

	// Token: 0x06006629 RID: 26153 RVA: 0x002620F4 File Offset: 0x002602F4
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x0600662A RID: 26154 RVA: 0x0026211C File Offset: 0x0026031C
	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
		{
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			this.animController.Play("react", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			this.animController.Play("react_bottoms", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnHatChanged:
			this.animController.Play("react_glasses", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			this.animController.Play("react_tops", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			this.animController.Play("react_gloves", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			this.animController.Play("react_shoes", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		}
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		IL_195:
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.lastReactSource = source;
	}

	// Token: 0x0400465C RID: 18012
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x0400465D RID: 18013
	private KBatchedAnimController animController;

	// Token: 0x0400465E RID: 18014
	private GameObject spawn;

	// Token: 0x04004660 RID: 18016
	private UIMinionOrMannequinReactSource lastReactSource;
}
