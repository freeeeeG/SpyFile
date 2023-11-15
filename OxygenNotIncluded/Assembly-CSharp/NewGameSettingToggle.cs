using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000BA0 RID: 2976
public class NewGameSettingToggle : NewGameSettingWidget
{
	// Token: 0x06005CD4 RID: 23764 RVA: 0x00220177 File Offset: 0x0021E377
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = this.Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleSetting));
	}

	// Token: 0x06005CD5 RID: 23765 RVA: 0x002201A6 File Offset: 0x0021E3A6
	public void Initialize(ToggleSettingConfig config, NewGameSettingsPanel panel, string disabledDefault)
	{
		base.Initialize(config, panel, disabledDefault);
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
	}

	// Token: 0x06005CD6 RID: 23766 RVA: 0x002201DC File Offset: 0x0021E3DC
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(this.config);
		this.Toggle.ChangeState(this.config.IsOnLevel(currentQualitySetting.id) ? 1 : 0);
		this.ToggleToolTip.toolTip = currentQualitySetting.tooltip;
	}

	// Token: 0x06005CD7 RID: 23767 RVA: 0x00220233 File Offset: 0x0021E433
	public void ToggleSetting()
	{
		if (this.IsEnabled())
		{
			CustomGameSettings.Instance.ToggleSettingLevel(this.config);
			base.RefreshAll();
		}
	}

	// Token: 0x04003E6A RID: 15978
	[SerializeField]
	private LocText Label;

	// Token: 0x04003E6B RID: 15979
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04003E6C RID: 15980
	[SerializeField]
	private MultiToggle Toggle;

	// Token: 0x04003E6D RID: 15981
	[SerializeField]
	private ToolTip ToggleToolTip;

	// Token: 0x04003E6E RID: 15982
	private ToggleSettingConfig config;
}
