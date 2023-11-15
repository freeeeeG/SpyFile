using System;
using UnityEngine;

// Token: 0x02000C4B RID: 3147
public class SingleCheckboxSideScreen : SideScreenContent
{
	// Token: 0x060063C1 RID: 25537 RVA: 0x0024E4CF File Offset: 0x0024C6CF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060063C2 RID: 25538 RVA: 0x0024E4D7 File Offset: 0x0024C6D7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggle.onValueChanged += this.OnValueChanged;
	}

	// Token: 0x060063C3 RID: 25539 RVA: 0x0024E4F6 File Offset: 0x0024C6F6
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ICheckboxControl>() != null || target.GetSMI<ICheckboxControl>() != null;
	}

	// Token: 0x060063C4 RID: 25540 RVA: 0x0024E50C File Offset: 0x0024C70C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.target = target.GetComponent<ICheckboxControl>();
		if (this.target == null)
		{
			this.target = target.GetSMI<ICheckboxControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The target provided does not have an ICheckboxControl component");
			return;
		}
		this.label.text = this.target.CheckboxLabel;
		this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
		this.titleKey = this.target.CheckboxTitleKey;
		this.toggle.isOn = this.target.GetCheckboxValue();
		this.toggleCheckMark.enabled = this.toggle.isOn;
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x0024E5DF File Offset: 0x0024C7DF
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.target = null;
	}

	// Token: 0x060063C6 RID: 25542 RVA: 0x0024E5EE File Offset: 0x0024C7EE
	private void OnValueChanged(bool value)
	{
		this.target.SetCheckboxValue(value);
		this.toggleCheckMark.enabled = value;
	}

	// Token: 0x04004418 RID: 17432
	public KToggle toggle;

	// Token: 0x04004419 RID: 17433
	public KImage toggleCheckMark;

	// Token: 0x0400441A RID: 17434
	public LocText label;

	// Token: 0x0400441B RID: 17435
	private ICheckboxControl target;
}
