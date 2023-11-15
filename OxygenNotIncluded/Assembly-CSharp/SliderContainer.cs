using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C66 RID: 3174
[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
	// Token: 0x06006513 RID: 25875 RVA: 0x00258B5A File Offset: 0x00256D5A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
	}

	// Token: 0x06006514 RID: 25876 RVA: 0x00258B80 File Offset: 0x00256D80
	public void UpdateSliderLabel(float newValue)
	{
		if (this.isPercentValue)
		{
			this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
			return;
		}
		this.valueLabel.text = newValue.ToString();
	}

	// Token: 0x0400454C RID: 17740
	public bool isPercentValue = true;

	// Token: 0x0400454D RID: 17741
	public KSlider slider;

	// Token: 0x0400454E RID: 17742
	public LocText nameLabel;

	// Token: 0x0400454F RID: 17743
	public LocText valueLabel;
}
