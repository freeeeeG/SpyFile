using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A5F RID: 2655
public class DisinfectThresholdDiagram : MonoBehaviour
{
	// Token: 0x06005048 RID: 20552 RVA: 0x001C75F0 File Offset: 0x001C57F0
	private void Start()
	{
		this.inputField.minValue = 0f;
		this.inputField.maxValue = (float)DisinfectThresholdDiagram.MAX_VALUE;
		this.inputField.currentValue = (float)SaveGame.Instance.minGermCountForDisinfect;
		this.inputField.SetDisplayValue(SaveGame.Instance.minGermCountForDisinfect.ToString());
		this.inputField.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.inputField.currentValue);
		};
		this.inputField.decimalPlaces = 1;
		this.inputField.Activate();
		this.slider.minValue = 0f;
		this.slider.maxValue = (float)(DisinfectThresholdDiagram.MAX_VALUE / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.wholeNumbers = true;
		this.slider.value = (float)(SaveGame.Instance.minGermCountForDisinfect / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.onReleaseHandle += this.OnReleaseHandle;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
			this.OnReleaseHandle();
		};
		this.unitsLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.UNITS);
		this.minLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MIN_LABEL);
		this.maxLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MAX_LABEL);
		this.thresholdPrefix.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.THRESHOLD_PREFIX);
		this.toolTip.OnToolTip = delegate()
		{
			this.toolTip.ClearMultiStringTooltip();
			if (SaveGame.Instance.enableAutoDisinfect)
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP.ToString().Replace("{NumberOfGerms}", SaveGame.Instance.minGermCountForDisinfect.ToString()), null);
			}
			else
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP_DISABLED.ToString(), null);
			}
			return "";
		};
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
		this.toggle.isOn = SaveGame.Instance.enableAutoDisinfect;
		this.toggle.onValueChanged += this.OnClickToggle;
	}

	// Token: 0x06005049 RID: 20553 RVA: 0x001C77DC File Offset: 0x001C59DC
	private void OnReleaseHandle()
	{
		float num = (float)((int)this.slider.value * DisinfectThresholdDiagram.SLIDER_CONVERSION);
		SaveGame.Instance.minGermCountForDisinfect = (int)num;
		this.inputField.SetDisplayValue(num.ToString());
	}

	// Token: 0x0600504A RID: 20554 RVA: 0x001C781C File Offset: 0x001C5A1C
	private void ReceiveValueFromSlider(float new_value)
	{
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value * DisinfectThresholdDiagram.SLIDER_CONVERSION;
		this.inputField.SetDisplayValue((new_value * (float)DisinfectThresholdDiagram.SLIDER_CONVERSION).ToString());
	}

	// Token: 0x0600504B RID: 20555 RVA: 0x001C7856 File Offset: 0x001C5A56
	private void ReceiveValueFromInput(float new_value)
	{
		this.slider.value = new_value / (float)DisinfectThresholdDiagram.SLIDER_CONVERSION;
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value;
	}

	// Token: 0x0600504C RID: 20556 RVA: 0x001C7877 File Offset: 0x001C5A77
	private void OnClickToggle(bool new_value)
	{
		SaveGame.Instance.enableAutoDisinfect = new_value;
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
	}

	// Token: 0x0400348F RID: 13455
	[SerializeField]
	private KNumberInputField inputField;

	// Token: 0x04003490 RID: 13456
	[SerializeField]
	private KSlider slider;

	// Token: 0x04003491 RID: 13457
	[SerializeField]
	private LocText minLabel;

	// Token: 0x04003492 RID: 13458
	[SerializeField]
	private LocText maxLabel;

	// Token: 0x04003493 RID: 13459
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04003494 RID: 13460
	[SerializeField]
	private LocText thresholdPrefix;

	// Token: 0x04003495 RID: 13461
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04003496 RID: 13462
	[SerializeField]
	private KToggle toggle;

	// Token: 0x04003497 RID: 13463
	[SerializeField]
	private Image disabledImage;

	// Token: 0x04003498 RID: 13464
	private static int MAX_VALUE = 1000000;

	// Token: 0x04003499 RID: 13465
	private static int SLIDER_CONVERSION = 1000;
}
