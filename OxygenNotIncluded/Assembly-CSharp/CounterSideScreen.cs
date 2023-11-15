using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C11 RID: 3089
public class CounterSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x060061CB RID: 25035 RVA: 0x00241E69 File Offset: 0x00240069
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060061CC RID: 25036 RVA: 0x00241E74 File Offset: 0x00240074
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.incrementMaxButton.onClick += this.IncrementMaxCount;
		this.decrementMaxButton.onClick += this.DecrementMaxCount;
		this.incrementModeButton.onClick += this.ToggleMode;
		this.advancedModeToggle.onClick += this.ToggleAdvanced;
		this.maxCountInput.onEndEdit += delegate()
		{
			this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue);
		};
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x060061CD RID: 25037 RVA: 0x00241F22 File Offset: 0x00240122
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCounter>() != null;
	}

	// Token: 0x060061CE RID: 25038 RVA: 0x00241F30 File Offset: 0x00240130
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.maxCountInput.minValue = 1f;
		this.maxCountInput.maxValue = 10f;
		this.targetLogicCounter = target.GetComponent<LogicCounter>();
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
	}

	// Token: 0x060061CF RID: 25039 RVA: 0x00241FA8 File Offset: 0x002401A8
	public void Render200ms(float dt)
	{
		if (this.targetLogicCounter == null)
		{
			return;
		}
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x060061D0 RID: 25040 RVA: 0x00241FCC File Offset: 0x002401CC
	private void UpdateCurrentCountLabel(int value)
	{
		string text = value.ToString();
		if (value == this.targetLogicCounter.maxCount)
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Active);
		}
		else
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Standby);
		}
		this.currentCount.text = (this.targetLogicCounter.advancedMode ? string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, text) : string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, text));
	}

	// Token: 0x060061D1 RID: 25041 RVA: 0x0024203B File Offset: 0x0024023B
	private void UpdateMaxCountLabel(int value)
	{
		this.maxCountInput.SetAmount((float)value);
	}

	// Token: 0x060061D2 RID: 25042 RVA: 0x0024204A File Offset: 0x0024024A
	private void UpdateMaxCountFromTextInput(float newValue)
	{
		this.SetMaxCount((int)newValue);
	}

	// Token: 0x060061D3 RID: 25043 RVA: 0x00242054 File Offset: 0x00240254
	private void IncrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount + 1);
	}

	// Token: 0x060061D4 RID: 25044 RVA: 0x00242069 File Offset: 0x00240269
	private void DecrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount - 1);
	}

	// Token: 0x060061D5 RID: 25045 RVA: 0x00242080 File Offset: 0x00240280
	private void SetMaxCount(int newValue)
	{
		if (newValue > 10)
		{
			newValue = 1;
		}
		if (newValue < 1)
		{
			newValue = 10;
		}
		if (newValue < this.targetLogicCounter.currentCount)
		{
			this.targetLogicCounter.currentCount = newValue;
		}
		this.targetLogicCounter.maxCount = newValue;
		this.UpdateCounterStates();
		this.UpdateMaxCountLabel(newValue);
	}

	// Token: 0x060061D6 RID: 25046 RVA: 0x002420D0 File Offset: 0x002402D0
	private void ResetCounter()
	{
		this.targetLogicCounter.ResetCounter();
	}

	// Token: 0x060061D7 RID: 25047 RVA: 0x002420DD File Offset: 0x002402DD
	private void UpdateCounterStates()
	{
		this.targetLogicCounter.SetCounterState();
		this.targetLogicCounter.UpdateLogicCircuit();
		this.targetLogicCounter.UpdateVisualState(true);
		this.targetLogicCounter.UpdateMeter();
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x0024210C File Offset: 0x0024030C
	private void ToggleMode()
	{
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x00242110 File Offset: 0x00240310
	private void ToggleAdvanced()
	{
		this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateCounterStates();
	}

	// Token: 0x040042A4 RID: 17060
	public LogicCounter targetLogicCounter;

	// Token: 0x040042A5 RID: 17061
	public KButton resetButton;

	// Token: 0x040042A6 RID: 17062
	public KButton incrementMaxButton;

	// Token: 0x040042A7 RID: 17063
	public KButton decrementMaxButton;

	// Token: 0x040042A8 RID: 17064
	public KButton incrementModeButton;

	// Token: 0x040042A9 RID: 17065
	public KToggle advancedModeToggle;

	// Token: 0x040042AA RID: 17066
	public KImage advancedModeCheckmark;

	// Token: 0x040042AB RID: 17067
	public LocText currentCount;

	// Token: 0x040042AC RID: 17068
	[SerializeField]
	private KNumberInputField maxCountInput;
}
