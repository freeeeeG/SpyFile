using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B72 RID: 2930
public class T17InfoObject : MonoBehaviour
{
	// Token: 0x06003B95 RID: 15253 RVA: 0x0011B899 File Offset: 0x00119C99
	public void SetImage(Sprite image)
	{
		if (this.m_InfoIcon != null)
		{
			this.m_InfoIcon.sprite = image;
		}
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0011B8B8 File Offset: 0x00119CB8
	public void SetTitle(string title)
	{
		if (this.m_Title != null)
		{
			this.m_Title.SetNewPlaceHolder(title);
			this.m_Title.m_bNeedsLocalization = false;
			this.m_Title.text = title;
		}
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0011B8EF File Offset: 0x00119CEF
	public void SetValue(string value)
	{
		if (this.m_Value != null)
		{
			this.m_Value.SetNewPlaceHolder(value);
			this.m_Value.m_bNeedsLocalization = false;
			this.m_Value.text = value;
		}
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0011B926 File Offset: 0x00119D26
	public Sprite GetImage()
	{
		if (this.m_InfoIcon != null)
		{
			return this.m_InfoIcon.sprite;
		}
		return null;
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0011B946 File Offset: 0x00119D46
	public string GetTitle()
	{
		if (this.m_Title != null)
		{
			return this.m_Title.text;
		}
		return string.Empty;
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0011B96A File Offset: 0x00119D6A
	public string GetValue()
	{
		if (this.m_Value != null)
		{
			return this.m_Value.text;
		}
		return string.Empty;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0011B990 File Offset: 0x00119D90
	public void SetSliderProgress(float ratio)
	{
		if (this.m_Slider != null)
		{
			this.m_Slider.value = (this.m_Slider.maxValue - this.m_Slider.minValue) * ratio + this.m_Slider.minValue;
		}
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x0011B9DE File Offset: 0x00119DDE
	public void SetValues(int rawValue, int maxValue)
	{
		this.SetValues(rawValue, (float)maxValue);
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x0011B9EC File Offset: 0x00119DEC
	public void SetValues(int rawValue, float maxValue)
	{
		if (rawValue != this.m_PreviousRawValue || maxValue != this.m_PreviousMaxValue)
		{
			this.m_PreviousRawValue = rawValue;
			this.m_PreviousMaxValue = maxValue;
			this.SetValue(rawValue.ToString());
			this.SetSliderProgress((float)rawValue / maxValue);
		}
	}

	// Token: 0x0400306E RID: 12398
	public Image m_InfoIcon;

	// Token: 0x0400306F RID: 12399
	public T17Text m_Title;

	// Token: 0x04003070 RID: 12400
	public T17Text m_Value;

	// Token: 0x04003071 RID: 12401
	public T17Slider m_Slider;

	// Token: 0x04003072 RID: 12402
	private int m_PreviousRawValue = -1;

	// Token: 0x04003073 RID: 12403
	private float m_PreviousMaxValue = -1f;
}
