using System;
using TMPro;
using UnityEngine;

// Token: 0x0200013B RID: 315
public abstract class AUIGameSettingItem : MonoBehaviour
{
	// Token: 0x0600081F RID: 2079 RVA: 0x0001EF6C File Offset: 0x0001D16C
	private void Awake()
	{
		this.GetValueFromSettingData();
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0001EF74 File Offset: 0x0001D174
	protected virtual void OnEnable()
	{
		EventMgr.Register(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0001EF8D File Offset: 0x0001D18D
	protected virtual void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0001EFA6 File Offset: 0x0001D1A6
	private void OnLanguageChanged()
	{
		this.UpdateDisplay();
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0001EFAE File Offset: 0x0001D1AE
	private void GetValueFromSettingData()
	{
		this.curValue = this.settingData.LoadSetting();
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0001EFC1 File Offset: 0x0001D1C1
	protected virtual void ApplySetting()
	{
		this.settingData.ApplySetting(this.curValue);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0001EFD4 File Offset: 0x0001D1D4
	protected virtual void ResetToDefault()
	{
		this.settingData.ResetToDefault();
		this.curValue = this.settingData.SettingValue;
	}

	// Token: 0x06000826 RID: 2086
	protected abstract void UpdateDisplay();

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	protected GameSettingData settingData;

	// Token: 0x0400069F RID: 1695
	[SerializeField]
	private TMP_Text text_Name;

	// Token: 0x040006A0 RID: 1696
	[SerializeField]
	[Header("Debug: 目前數值")]
	protected int curValue;
}
