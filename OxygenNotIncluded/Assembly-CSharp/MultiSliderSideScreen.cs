using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C32 RID: 3122
public class MultiSliderSideScreen : SideScreenContent
{
	// Token: 0x060062C9 RID: 25289 RVA: 0x0024794C File Offset: 0x00245B4C
	public override bool IsValidForTarget(GameObject target)
	{
		IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
		return component != null && component.SidescreenEnabled();
	}

	// Token: 0x060062CA RID: 25290 RVA: 0x0024796B File Offset: 0x00245B6B
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IMultiSliderControl>();
		this.titleKey = this.target.SidescreenTitleKey;
		this.Refresh();
	}

	// Token: 0x060062CB RID: 25291 RVA: 0x002479A4 File Offset: 0x00245BA4
	private void Refresh()
	{
		while (this.liveSliders.Count < this.target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			SliderSet sliderSet = new SliderSet();
			sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
			sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
			if (sliderSet.numberInput != null)
			{
				sliderSet.numberInput.Activate();
			}
			sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
			sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
			sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
			sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
			sliderSet.SetupSlider(this.liveSliders.Count);
			this.liveSliders.Add(gameObject);
			this.sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < this.liveSliders.Count; i++)
		{
			if (i >= this.target.sliderControls.Length)
			{
				this.liveSliders[i].SetActive(false);
			}
			else
			{
				if (!this.liveSliders[i].activeSelf)
				{
					this.liveSliders[i].SetActive(true);
					this.liveSliders[i].gameObject.SetActive(true);
				}
				this.sliderSets[i].SetTarget(this.target.sliderControls[i], i);
			}
		}
	}

	// Token: 0x04004356 RID: 17238
	public LayoutElement sliderPrefab;

	// Token: 0x04004357 RID: 17239
	public RectTransform sliderContainer;

	// Token: 0x04004358 RID: 17240
	private IMultiSliderControl target;

	// Token: 0x04004359 RID: 17241
	private List<GameObject> liveSliders = new List<GameObject>();

	// Token: 0x0400435A RID: 17242
	private List<SliderSet> sliderSets = new List<SliderSet>();
}
