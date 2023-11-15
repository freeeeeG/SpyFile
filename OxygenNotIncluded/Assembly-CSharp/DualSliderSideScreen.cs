using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C17 RID: 3095
public class DualSliderSideScreen : SideScreenContent
{
	// Token: 0x060061FA RID: 25082 RVA: 0x00242A98 File Offset: 0x00240C98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x00242AD3 File Offset: 0x00240CD3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDualSliderControl>() != null;
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x00242AE0 File Offset: 0x00240CE0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IDualSliderControl>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a Manual Generator component");
			return;
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040042BE RID: 17086
	private IDualSliderControl target;

	// Token: 0x040042BF RID: 17087
	public List<SliderSet> sliderSets;
}
