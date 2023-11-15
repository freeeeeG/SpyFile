using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200013C RID: 316
public class UIGameSettingItem_Slider : AUIGameSettingItem
{
	// Token: 0x06000828 RID: 2088 RVA: 0x0001EFFA File Offset: 0x0001D1FA
	private void Start()
	{
		this.slider.value = (float)this.curValue;
		this.UpdateDisplay();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0001F014 File Offset: 0x0001D214
	protected override void OnEnable()
	{
		base.OnEnable();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderChanged));
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0001F038 File Offset: 0x0001D238
	protected override void OnDisable()
	{
		base.OnDisable();
		this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnSliderChanged));
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0001F05C File Offset: 0x0001D25C
	private void OnSliderChanged(float value)
	{
		this.curValue = Mathf.RoundToInt(value);
		this.UpdateDisplay();
		SoundManager.PlaySound("UI", "Settings_Slider_Change", 0.8f + 0.5f * ((float)this.curValue / (float)this.maxValue), -1f, -1f);
		this.ApplySetting();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0001F0B6 File Offset: 0x0001D2B6
	protected override void ApplySetting()
	{
		base.ApplySetting();
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0001F0BE File Offset: 0x0001D2BE
	protected override void ResetToDefault()
	{
		base.ResetToDefault();
		this.slider.value = (float)this.curValue;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0001F0D8 File Offset: 0x0001D2D8
	protected override void UpdateDisplay()
	{
		this.text_Value.text = string.Format("{0}%", this.curValue);
	}

	// Token: 0x040006A1 RID: 1697
	[SerializeField]
	protected Slider slider;

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	protected TMP_Text text_Value;

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	[Header("最小值")]
	protected int minValue;

	// Token: 0x040006A4 RID: 1700
	[SerializeField]
	[Header("最大值")]
	protected int maxValue = 100;
}
