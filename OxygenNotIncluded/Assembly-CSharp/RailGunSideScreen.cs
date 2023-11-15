using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C3B RID: 3131
public class RailGunSideScreen : SideScreenContent
{
	// Token: 0x06006311 RID: 25361 RVA: 0x00249BF0 File Offset: 0x00247DF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
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

	// Token: 0x06006312 RID: 25362 RVA: 0x00249C81 File Offset: 0x00247E81
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x06006313 RID: 25363 RVA: 0x00249C9D File Offset: 0x00247E9D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x06006314 RID: 25364 RVA: 0x00249CB9 File Offset: 0x00247EB9
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RailGun>() != null;
	}

	// Token: 0x06006315 RID: 25365 RVA: 0x00249CC8 File Offset: 0x00247EC8
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.selectedGun = new_target.GetComponent<RailGun>();
		if (this.selectedGun == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RailGun component");
			return;
		}
		this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
		this.slider.minValue = this.selectedGun.MinLaunchMass;
		this.slider.maxValue = this.selectedGun.MaxLaunchMass;
		this.slider.value = this.selectedGun.launchMass;
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.numberInput.minValue = this.selectedGun.MinLaunchMass;
		this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
		this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
		this.UpdateMaxCapacityLabel();
		this.numberInput.Activate();
		this.UpdateHEPLabels(null);
	}

	// Token: 0x06006316 RID: 25366 RVA: 0x00249E02 File Offset: 0x00248002
	public override void ClearTarget()
	{
		if (this.targetRailgunHEPStorageSubHandle != -1 && this.selectedGun != null)
		{
			this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
			this.targetRailgunHEPStorageSubHandle = -1;
		}
		this.selectedGun = null;
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x00249E3C File Offset: 0x0024803C
	public void UpdateHEPLabels(object data = null)
	{
		if (this.selectedGun == null)
		{
			return;
		}
		string text = BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
		text = text.Replace("{current}", this.selectedGun.CurrentEnergy.ToString());
		text = text.Replace("{required}", this.selectedGun.EnergyCost.ToString());
		this.hepStorageInfo.text = text;
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x00249EAD File Offset: 0x002480AD
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006319 RID: 25369 RVA: 0x00249EB6 File Offset: 0x002480B6
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x00249EBF File Offset: 0x002480BF
	private void UpdateMaxCapacity(float newValue)
	{
		this.selectedGun.launchMass = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
		this.selectedGun.Trigger(161772031, null);
	}

	// Token: 0x0600631B RID: 25371 RVA: 0x00249EF0 File Offset: 0x002480F0
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
	}

	// Token: 0x0400438D RID: 17293
	public GameObject content;

	// Token: 0x0400438E RID: 17294
	private RailGun selectedGun;

	// Token: 0x0400438F RID: 17295
	public LocText DescriptionText;

	// Token: 0x04004390 RID: 17296
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x04004391 RID: 17297
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004392 RID: 17298
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004393 RID: 17299
	[SerializeField]
	private LocText hepStorageInfo;

	// Token: 0x04004394 RID: 17300
	private int targetRailgunHEPStorageSubHandle = -1;
}
