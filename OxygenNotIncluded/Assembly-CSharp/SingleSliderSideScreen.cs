using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C4F RID: 3151
public class SingleSliderSideScreen : SideScreenContent
{
	// Token: 0x060063DD RID: 25565 RVA: 0x0024EA4C File Offset: 0x0024CC4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x0024EA88 File Offset: 0x0024CC88
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		return (target.GetComponent<ISingleSliderControl>() != null || target.GetSMI<ISingleSliderControl>() != null) && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor);
	}

	// Token: 0x060063DF RID: 25567 RVA: 0x0024EB04 File Offset: 0x0024CD04
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ISingleSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<ISingleSliderControl>();
			if (this.target == null)
			{
				global::Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x04004424 RID: 17444
	private ISingleSliderControl target;

	// Token: 0x04004425 RID: 17445
	public List<SliderSet> sliderSets;
}
