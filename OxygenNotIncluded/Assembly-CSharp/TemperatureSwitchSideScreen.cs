using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C54 RID: 3156
public class TemperatureSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006404 RID: 25604 RVA: 0x0024F73C File Offset: 0x0024D93C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.coolerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		this.warmerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		LocText component = this.coolerToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.warmerToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.COLDER_BUTTON);
		component2.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.WARMER_BUTTON);
		Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
		sliderEvent.AddListener(new UnityAction<float>(this.OnTargetTemperatureChanged));
		this.targetTemperatureSlider.onValueChanged = sliderEvent;
	}

	// Token: 0x06006405 RID: 25605 RVA: 0x0024F7ED File Offset: 0x0024D9ED
	public void Render200ms(float dt)
	{
		if (this.targetTemperatureSwitch == null)
		{
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x06006406 RID: 25606 RVA: 0x0024F804 File Offset: 0x0024DA04
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TemperatureControlledSwitch>() != null;
	}

	// Token: 0x06006407 RID: 25607 RVA: 0x0024F814 File Offset: 0x0024DA14
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targetTemperatureSwitch = target.GetComponent<TemperatureControlledSwitch>();
		if (this.targetTemperatureSwitch == null)
		{
			global::Debug.LogError("The gameObject received does not contain a TimedSwitch component");
			return;
		}
		this.UpdateLabels();
		this.UpdateTargetTemperatureLabel();
		this.OnConditionButtonClicked(this.targetTemperatureSwitch.activateOnWarmerThan);
	}

	// Token: 0x06006408 RID: 25608 RVA: 0x0024F877 File Offset: 0x0024DA77
	private void OnTargetTemperatureChanged(float new_value)
	{
		this.targetTemperatureSwitch.thresholdTemperature = new_value;
		this.UpdateTargetTemperatureLabel();
	}

	// Token: 0x06006409 RID: 25609 RVA: 0x0024F88C File Offset: 0x0024DA8C
	private void OnConditionButtonClicked(bool isWarmer)
	{
		this.targetTemperatureSwitch.activateOnWarmerThan = isWarmer;
		if (isWarmer)
		{
			this.coolerToggle.isOn = false;
			this.warmerToggle.isOn = true;
			this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			return;
		}
		this.coolerToggle.isOn = true;
		this.warmerToggle.isOn = false;
		this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
	}

	// Token: 0x0600640A RID: 25610 RVA: 0x0024F91D File Offset: 0x0024DB1D
	private void UpdateTargetTemperatureLabel()
	{
		this.targetTemperature.text = GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.thresholdTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
	}

	// Token: 0x0600640B RID: 25611 RVA: 0x0024F93E File Offset: 0x0024DB3E
	private void UpdateLabels()
	{
		this.currentTemperature.text = string.Format(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.CURRENT_TEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x04004443 RID: 17475
	private TemperatureControlledSwitch targetTemperatureSwitch;

	// Token: 0x04004444 RID: 17476
	[SerializeField]
	private LocText currentTemperature;

	// Token: 0x04004445 RID: 17477
	[SerializeField]
	private LocText targetTemperature;

	// Token: 0x04004446 RID: 17478
	[SerializeField]
	private KToggle coolerToggle;

	// Token: 0x04004447 RID: 17479
	[SerializeField]
	private KToggle warmerToggle;

	// Token: 0x04004448 RID: 17480
	[SerializeField]
	private KSlider targetTemperatureSlider;
}
