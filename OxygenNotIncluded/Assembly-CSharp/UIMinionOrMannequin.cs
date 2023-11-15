using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C86 RID: 3206
public class UIMinionOrMannequin : KMonoBehaviour
{
	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x0600662C RID: 26156 RVA: 0x002622ED File Offset: 0x002604ED
	// (set) Token: 0x0600662D RID: 26157 RVA: 0x002622F5 File Offset: 0x002604F5
	public UIMinionOrMannequin.ITarget current { get; private set; }

	// Token: 0x0600662E RID: 26158 RVA: 0x002622FE File Offset: 0x002604FE
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600662F RID: 26159 RVA: 0x00262308 File Offset: 0x00260508
	public bool TrySpawn()
	{
		bool flag = false;
		if (this.mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			this.mannequin = gameObject.AddOrGet<UIMannequin>();
			this.mannequin.TrySpawn();
			gameObject.SetActive(false);
			flag = true;
		}
		if (this.minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			this.minion = gameObject2.AddOrGet<UIMinion>();
			this.minion.TrySpawn();
			gameObject2.SetActive(false);
			flag = true;
		}
		if (flag)
		{
			this.SetAsMannequin();
		}
		return flag;
	}

	// Token: 0x06006630 RID: 26160 RVA: 0x00262410 File Offset: 0x00260610
	public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return this.SetAsMinion(personality.Unwrap());
		}
		return this.SetAsMannequin();
	}

	// Token: 0x06006631 RID: 26161 RVA: 0x00262430 File Offset: 0x00260630
	public UIMinion SetAsMinion(Personality personality)
	{
		this.mannequin.gameObject.SetActive(false);
		this.minion.gameObject.SetActive(true);
		this.minion.SetMinion(personality);
		this.current = this.minion;
		return this.minion;
	}

	// Token: 0x06006632 RID: 26162 RVA: 0x0026247D File Offset: 0x0026067D
	public UIMannequin SetAsMannequin()
	{
		this.minion.gameObject.SetActive(false);
		this.mannequin.gameObject.SetActive(true);
		this.current = this.mannequin;
		return this.mannequin;
	}

	// Token: 0x06006633 RID: 26163 RVA: 0x002624B4 File Offset: 0x002606B4
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x0400466A RID: 18026
	public UIMinion minion;

	// Token: 0x0400466B RID: 18027
	public UIMannequin mannequin;

	// Token: 0x02001BB3 RID: 7091
	public interface ITarget
	{
		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06009ABA RID: 39610
		GameObject SpawnedAvatar { get; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06009ABB RID: 39611
		Option<Personality> Personality { get; }

		// Token: 0x06009ABC RID: 39612
		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		// Token: 0x06009ABD RID: 39613
		void React(UIMinionOrMannequinReactSource source);
	}
}
