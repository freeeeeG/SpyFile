using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200013E RID: 318
public class UIGameSettingItem_Toggle : AUIGameSettingItem
{
	// Token: 0x06000839 RID: 2105 RVA: 0x0001F2E0 File Offset: 0x0001D4E0
	private void Start()
	{
		this.toggle.isOn = (this.curValue == 1);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0001F2F6 File Offset: 0x0001D4F6
	protected override void OnEnable()
	{
		base.OnEnable();
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleChanged));
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0001F31A File Offset: 0x0001D51A
	protected override void OnDisable()
	{
		base.OnDisable();
		this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleChanged));
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0001F340 File Offset: 0x0001D540
	private void OnToggleChanged(bool isOn)
	{
		this.curValue = (isOn ? 1 : 0);
		if (isOn)
		{
			SoundManager.PlaySound("UI", "Settings_Toggle_On", -1f, -1f, -1f);
		}
		else
		{
			SoundManager.PlaySound("UI", "Settings_Toggle_Off", -1f, -1f, -1f);
		}
		this.ApplySetting();
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0001F3A3 File Offset: 0x0001D5A3
	protected override void ApplySetting()
	{
		base.ApplySetting();
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0001F3AB File Offset: 0x0001D5AB
	protected override void ResetToDefault()
	{
		base.ResetToDefault();
		this.toggle.isOn = (this.curValue == 1);
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0001F3C7 File Offset: 0x0001D5C7
	protected override void UpdateDisplay()
	{
	}

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	protected Toggle toggle;
}
