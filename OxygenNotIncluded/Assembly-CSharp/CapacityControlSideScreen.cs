using System;
using UnityEngine;

// Token: 0x02000C02 RID: 3074
public class CapacityControlSideScreen : SideScreenContent
{
	// Token: 0x06006144 RID: 24900 RVA: 0x0023E830 File Offset: 0x0023CA30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = this.target.CapacityUnits;
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
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06006145 RID: 24901 RVA: 0x0023E8C6 File Offset: 0x0023CAC6
	public override bool IsValidForTarget(GameObject target)
	{
		return !target.GetComponent<IUserControlledCapacity>().IsNullOrDestroyed();
	}

	// Token: 0x06006146 RID: 24902 RVA: 0x0023E8D8 File Offset: 0x0023CAD8
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IUserControlledCapacity>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.slider.minValue = this.target.MinCapacity;
		this.slider.maxValue = this.target.MaxCapacity;
		this.slider.value = this.target.UserMaxCapacity;
		this.slider.GetComponentInChildren<ToolTip>();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.numberInput.minValue = this.target.MinCapacity;
		this.numberInput.maxValue = this.target.MaxCapacity;
		this.numberInput.currentValue = Mathf.Max(this.target.MinCapacity, Mathf.Min(this.target.MaxCapacity, this.target.UserMaxCapacity));
		this.numberInput.Activate();
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x06006147 RID: 24903 RVA: 0x0023E9F4 File Offset: 0x0023CBF4
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006148 RID: 24904 RVA: 0x0023E9FD File Offset: 0x0023CBFD
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006149 RID: 24905 RVA: 0x0023EA06 File Offset: 0x0023CC06
	private void UpdateMaxCapacity(float newValue)
	{
		this.target.UserMaxCapacity = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x0600614A RID: 24906 RVA: 0x0023EA28 File Offset: 0x0023CC28
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.target.UserMaxCapacity.ToString());
	}

	// Token: 0x04004241 RID: 16961
	private IUserControlledCapacity target;

	// Token: 0x04004242 RID: 16962
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x04004243 RID: 16963
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004244 RID: 16964
	[SerializeField]
	private LocText unitsLabel;
}
