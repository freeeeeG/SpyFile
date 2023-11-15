using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C25 RID: 3109
public class IntSliderSideScreen : SideScreenContent
{
	// Token: 0x06006254 RID: 25172 RVA: 0x00244C30 File Offset: 0x00242E30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
			this.sliderSets[i].valueSlider.wholeNumbers = true;
		}
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x00244C82 File Offset: 0x00242E82
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;
	}

	// Token: 0x06006256 RID: 25174 RVA: 0x00244C98 File Offset: 0x00242E98
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IIntSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IIntSliderControl>();
		}
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

	// Token: 0x04004303 RID: 17155
	private IIntSliderControl target;

	// Token: 0x04004304 RID: 17156
	public List<SliderSet> sliderSets;
}
