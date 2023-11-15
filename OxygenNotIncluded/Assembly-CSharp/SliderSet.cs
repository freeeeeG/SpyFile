using System;
using UnityEngine;

// Token: 0x02000C4D RID: 3149
[Serializable]
public class SliderSet
{
	// Token: 0x060063D1 RID: 25553 RVA: 0x0024E610 File Offset: 0x0024C810
	public void SetupSlider(int index)
	{
		this.index = index;
		this.valueSlider.onReleaseHandle += delegate()
		{
			this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput();
		};
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x0024E698 File Offset: 0x0024C898
	public void SetTarget(ISliderControl target, int index)
	{
		this.index = index;
		this.target = target;
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(target.GetSliderTooltip(index));
		}
		if (this.targetLabel != null)
		{
			this.targetLabel.text = ((target.SliderTitleKey != null) ? Strings.Get(target.SliderTitleKey) : "");
		}
		this.unitsLabel.text = target.SliderUnits;
		this.minLabel.text = target.GetSliderMin(index).ToString() + target.SliderUnits;
		this.maxLabel.text = target.GetSliderMax(index).ToString() + target.SliderUnits;
		this.numberInput.minValue = target.GetSliderMin(index);
		this.numberInput.maxValue = target.GetSliderMax(index);
		this.numberInput.decimalPlaces = target.SliderDecimalPlaces(index);
		this.numberInput.field.characterLimit = Mathf.FloorToInt(1f + Mathf.Log10(this.numberInput.maxValue + (float)this.numberInput.decimalPlaces));
		Vector2 sizeDelta = this.numberInput.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)((this.numberInput.field.characterLimit + 1) * 10);
		this.numberInput.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.valueSlider.minValue = target.GetSliderMin(index);
		this.valueSlider.maxValue = target.GetSliderMax(index);
		this.valueSlider.value = target.GetSliderValue(index);
		this.SetValue(target.GetSliderValue(index));
		if (index == 0)
		{
			this.numberInput.Activate();
		}
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x0024E86C File Offset: 0x0024CA6C
	private void ReceiveValueFromSlider()
	{
		float num = this.valueSlider.value;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.SetValue(num);
	}

	// Token: 0x060063D4 RID: 25556 RVA: 0x0024E8BC File Offset: 0x0024CABC
	private void ReceiveValueFromInput()
	{
		float num = this.numberInput.currentValue;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.valueSlider.value = num;
		this.SetValue(num);
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x0024E918 File Offset: 0x0024CB18
	private void SetValue(float value)
	{
		float num = value;
		if (num > this.target.GetSliderMax(this.index))
		{
			num = this.target.GetSliderMax(this.index);
		}
		else if (num < this.target.GetSliderMin(this.index))
		{
			num = this.target.GetSliderMin(this.index);
		}
		this.UpdateLabel(num);
		this.target.SetSliderValue(num, this.index);
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(this.target.GetSliderTooltip(this.index));
		}
	}

	// Token: 0x060063D6 RID: 25558 RVA: 0x0024E9C0 File Offset: 0x0024CBC0
	private void UpdateLabel(float value)
	{
		float num = Mathf.Round(value * 10f) / 10f;
		this.numberInput.SetDisplayValue(num.ToString());
	}

	// Token: 0x0400441C RID: 17436
	public KSlider valueSlider;

	// Token: 0x0400441D RID: 17437
	public KNumberInputField numberInput;

	// Token: 0x0400441E RID: 17438
	public LocText targetLabel;

	// Token: 0x0400441F RID: 17439
	public LocText unitsLabel;

	// Token: 0x04004420 RID: 17440
	public LocText minLabel;

	// Token: 0x04004421 RID: 17441
	public LocText maxLabel;

	// Token: 0x04004422 RID: 17442
	[NonSerialized]
	public int index;

	// Token: 0x04004423 RID: 17443
	private ISliderControl target;
}
