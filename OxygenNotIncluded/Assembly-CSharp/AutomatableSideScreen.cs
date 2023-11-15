using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BFD RID: 3069
public class AutomatableSideScreen : SideScreenContent
{
	// Token: 0x06006126 RID: 24870 RVA: 0x0023E336 File Offset: 0x0023C536
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x0023E340 File Offset: 0x0023C540
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
		this.allowManualToggle.onValueChanged += this.OnAllowManualChanged;
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x0023E38E File Offset: 0x0023C58E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Automatable>() != null;
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x0023E39C File Offset: 0x0023C59C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetAutomatable = target.GetComponent<Automatable>();
		if (this.targetAutomatable == null)
		{
			global::Debug.LogError("The target provided does not have an Automatable component");
			return;
		}
		this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
		this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x0023E418 File Offset: 0x0023C618
	private void OnAllowManualChanged(bool value)
	{
		this.targetAutomatable.SetAutomationOnly(!value);
		this.allowManualToggleCheckMark.enabled = value;
	}

	// Token: 0x0400422F RID: 16943
	public KToggle allowManualToggle;

	// Token: 0x04004230 RID: 16944
	public KImage allowManualToggleCheckMark;

	// Token: 0x04004231 RID: 16945
	public GameObject content;

	// Token: 0x04004232 RID: 16946
	private GameObject target;

	// Token: 0x04004233 RID: 16947
	public LocText DescriptionText;

	// Token: 0x04004234 RID: 16948
	private Automatable targetAutomatable;
}
