using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BF5 RID: 3061
public class ActiveRangeSideScreen : SideScreenContent
{
	// Token: 0x060060C8 RID: 24776 RVA: 0x0023BF2F File Offset: 0x0023A12F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x0023BF38 File Offset: 0x0023A138
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateValueLabel.maxValue = this.target.MaxValue;
		this.activateValueLabel.minValue = this.target.MinValue;
		this.deactivateValueLabel.maxValue = this.target.MaxValue;
		this.deactivateValueLabel.minValue = this.target.MinValue;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x0023BFDC File Offset: 0x0023A1DC
	private void OnActivateValueChanged(float new_value)
	{
		this.target.ActivateValue = new_value;
		if (this.target.ActivateValue < this.target.DeactivateValue)
		{
			this.target.ActivateValue = this.target.DeactivateValue;
			this.activateValueSlider.value = this.target.ActivateValue;
		}
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x0023C060 File Offset: 0x0023A260
	private void OnDeactivateValueChanged(float new_value)
	{
		this.target.DeactivateValue = new_value;
		if (this.target.DeactivateValue > this.target.ActivateValue)
		{
			this.target.DeactivateValue = this.activateValueSlider.value;
			this.deactivateValueSlider.value = this.target.DeactivateValue;
		}
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x0023C0E4 File Offset: 0x0023A2E4
	private void RefreshTooltips()
	{
		this.activateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.ActivateTooltip, this.activateValueSlider.value, this.deactivateValueSlider.value));
		this.deactivateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.DeactivateTooltip, this.deactivateValueSlider.value, this.activateValueSlider.value));
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x0023C171 File Offset: 0x0023A371
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IActivationRangeTarget>() != null;
	}

	// Token: 0x060060CE RID: 24782 RVA: 0x0023C17C File Offset: 0x0023A37C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IActivationRangeTarget>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IActivationRangeTarget component");
			return;
		}
		this.activateLabel.text = this.target.ActivateSliderLabelText;
		this.deactivateLabel.text = this.target.DeactivateSliderLabelText;
		this.activateValueLabel.Activate();
		this.deactivateValueLabel.Activate();
		this.activateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueSlider.minValue = this.target.MinValue;
		this.activateValueSlider.maxValue = this.target.MaxValue;
		this.activateValueSlider.value = this.target.ActivateValue;
		this.activateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.activateValueLabel.onEndEdit += delegate()
		{
			float activateValue = this.target.ActivateValue;
			float.TryParse(this.activateValueLabel.field.text, out activateValue);
			this.OnActivateValueChanged(activateValue);
			this.activateValueSlider.value = activateValue;
		};
		this.deactivateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueSlider.minValue = this.target.MinValue;
		this.deactivateValueSlider.maxValue = this.target.MaxValue;
		this.deactivateValueSlider.value = this.target.DeactivateValue;
		this.deactivateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.deactivateValueLabel.onEndEdit += delegate()
		{
			float deactivateValue = this.target.DeactivateValue;
			float.TryParse(this.deactivateValueLabel.field.text, out deactivateValue);
			this.OnDeactivateValueChanged(deactivateValue);
			this.deactivateValueSlider.value = deactivateValue;
		};
		this.RefreshTooltips();
	}

	// Token: 0x060060CF RID: 24783 RVA: 0x0023C38E File Offset: 0x0023A58E
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.target.ActivationRangeTitleText;
		}
		return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
	}

	// Token: 0x040041ED RID: 16877
	private IActivationRangeTarget target;

	// Token: 0x040041EE RID: 16878
	[SerializeField]
	private KSlider activateValueSlider;

	// Token: 0x040041EF RID: 16879
	[SerializeField]
	private KSlider deactivateValueSlider;

	// Token: 0x040041F0 RID: 16880
	[SerializeField]
	private LocText activateLabel;

	// Token: 0x040041F1 RID: 16881
	[SerializeField]
	private LocText deactivateLabel;

	// Token: 0x040041F2 RID: 16882
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField activateValueLabel;

	// Token: 0x040041F3 RID: 16883
	[SerializeField]
	private KNumberInputField deactivateValueLabel;
}
