using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009E3 RID: 2531
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004B8F RID: 19343 RVA: 0x001A8480 File Offset: 0x001A6680
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.loadCharmed && DlcManager.IsExpansion1Active())
		{
			base.gameObject.AddTag(GameTags.CharmedArtifact);
			this.SetEntombedDecor();
		}
		else
		{
			this.loadCharmed = false;
			this.SetAnalyzedDecor();
		}
		this.UpdateStatusItem();
		Components.SpaceArtifacts.Add(this);
		this.UpdateAnim();
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x001A84DE File Offset: 0x001A66DE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SpaceArtifacts.Remove(this);
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001A84F1 File Offset: 0x001A66F1
	public void RemoveCharm()
	{
		base.gameObject.RemoveTag(GameTags.CharmedArtifact);
		this.UpdateStatusItem();
		this.loadCharmed = false;
		this.UpdateAnim();
		this.SetAnalyzedDecor();
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x001A851C File Offset: 0x001A671C
	private void SetEntombedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(DECOR.BONUS.TIER0);
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x001A852E File Offset: 0x001A672E
	private void SetAnalyzedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x001A8548 File Offset: 0x001A6748
	public void UpdateStatusItem()
	{
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, null);
			return;
		}
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, false);
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x001A85AA File Offset: 0x001A67AA
	public void SetArtifactTier(ArtifactTier tier)
	{
		this.artifactTier = tier;
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x001A85B3 File Offset: 0x001A67B3
	public ArtifactTier GetArtifactTier()
	{
		return this.artifactTier;
	}

	// Token: 0x06004B97 RID: 19351 RVA: 0x001A85BB File Offset: 0x001A67BB
	public void SetUIAnim(string anim)
	{
		this.ui_anim = anim;
	}

	// Token: 0x06004B98 RID: 19352 RVA: 0x001A85C4 File Offset: 0x001A67C4
	public string GetUIAnim()
	{
		return this.ui_anim;
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x001A85CC File Offset: 0x001A67CC
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			Descriptor item = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		Descriptor item2 = new Descriptor(string.Format("This is an artifact from space", Array.Empty<object>()), string.Format("This is the tooltip string", Array.Empty<object>()), Descriptor.DescriptorType.Information, false);
		list.Add(item2);
		return list;
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x001A867C File Offset: 0x001A687C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x06004B9B RID: 19355 RVA: 0x001A8684 File Offset: 0x001A6884
	private void UpdateAnim()
	{
		string s;
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			s = "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
		}
		else
		{
			s = this.uniqueAnimNameFragment;
		}
		base.GetComponent<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x001A86E8 File Offset: 0x001A68E8
	[OnDeserialized]
	public void OnDeserialize()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			component.deleteOffGrid = false;
		}
	}

	// Token: 0x04003159 RID: 12633
	public const string ID = "SpaceArtifact";

	// Token: 0x0400315A RID: 12634
	private const string charmedPrefix = "entombed_";

	// Token: 0x0400315B RID: 12635
	private const string idlePrefix = "idle_";

	// Token: 0x0400315C RID: 12636
	[SerializeField]
	private string ui_anim;

	// Token: 0x0400315D RID: 12637
	[Serialize]
	private bool loadCharmed = true;

	// Token: 0x0400315E RID: 12638
	public ArtifactTier artifactTier;

	// Token: 0x0400315F RID: 12639
	public ArtifactType artifactType;

	// Token: 0x04003160 RID: 12640
	public string uniqueAnimNameFragment;
}
