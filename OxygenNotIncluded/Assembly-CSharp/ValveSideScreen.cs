using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02000C5C RID: 3164
public class ValveSideScreen : SideScreenContent
{
	// Token: 0x06006493 RID: 25747 RVA: 0x0025243C File Offset: 0x0025063C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.AddTimeSliceText(UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
		this.flowSlider.onReleaseHandle += this.OnReleaseHandle;
		this.flowSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x002524E9 File Offset: 0x002506E9
	public void OnReleaseHandle()
	{
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x002524FC File Offset: 0x002506FC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Valve>() != null;
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x0025250C File Offset: 0x0025070C
	public override void SetTarget(GameObject target)
	{
		this.targetValve = target.GetComponent<Valve>();
		if (this.targetValve == null)
		{
			global::Debug.LogError("The target object does not have a Valve component.");
			return;
		}
		this.flowSlider.minValue = 0f;
		this.flowSlider.maxValue = this.targetValve.MaxFlow;
		this.flowSlider.value = this.targetValve.DesiredFlow;
		this.minFlowLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
		this.numberInput.Activate();
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x0025261E File Offset: 0x0025081E
	private void ReceiveValueFromSlider(float newValue)
	{
		newValue = Mathf.Round(newValue * 1000f) / 1000f;
		this.UpdateFlowValue(newValue);
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x0025263C File Offset: 0x0025083C
	private void ReceiveValueFromInput(float input)
	{
		float newValue = input / 1000f;
		this.UpdateFlowValue(newValue);
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x06006499 RID: 25753 RVA: 0x00252669 File Offset: 0x00250869
	private void UpdateFlowValue(float newValue)
	{
		this.targetFlow = newValue;
		this.flowSlider.value = newValue;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
	}

	// Token: 0x0600649A RID: 25754 RVA: 0x00252697 File Offset: 0x00250897
	private IEnumerator SettingDelay(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OnReleaseHandle();
		yield break;
	}

	// Token: 0x040044A3 RID: 17571
	private Valve targetValve;

	// Token: 0x040044A4 RID: 17572
	[Header("Slider")]
	[SerializeField]
	private KSlider flowSlider;

	// Token: 0x040044A5 RID: 17573
	[SerializeField]
	private LocText minFlowLabel;

	// Token: 0x040044A6 RID: 17574
	[SerializeField]
	private LocText maxFlowLabel;

	// Token: 0x040044A7 RID: 17575
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040044A8 RID: 17576
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040044A9 RID: 17577
	private float targetFlow;
}
