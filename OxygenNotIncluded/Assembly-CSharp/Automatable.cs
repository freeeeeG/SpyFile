using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000483 RID: 1155
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
	// Token: 0x06001962 RID: 6498 RVA: 0x00084E8D File Offset: 0x0008308D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x00084EA8 File Offset: 0x000830A8
	private void OnCopySettings(object data)
	{
		Automatable component = ((GameObject)data).GetComponent<Automatable>();
		if (component != null)
		{
			this.automationOnly = component.automationOnly;
		}
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x00084ED6 File Offset: 0x000830D6
	public bool GetAutomationOnly()
	{
		return this.automationOnly;
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x00084EDE File Offset: 0x000830DE
	public void SetAutomationOnly(bool only)
	{
		this.automationOnly = only;
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x00084EE7 File Offset: 0x000830E7
	public bool AllowedByAutomation(bool is_transfer_arm)
	{
		return !this.GetAutomationOnly() || is_transfer_arm;
	}

	// Token: 0x04000E04 RID: 3588
	[Serialize]
	private bool automationOnly = true;

	// Token: 0x04000E05 RID: 3589
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04000E06 RID: 3590
	private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>(delegate(Automatable component, object data)
	{
		component.OnCopySettings(data);
	});
}
