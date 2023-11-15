using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000B9E RID: 2974
public class NewGameSettingList : NewGameSettingWidget
{
	// Token: 0x06005CC4 RID: 23748 RVA: 0x0021FDB1 File Offset: 0x0021DFB1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CycleLeft.onClick += this.DoCycleLeft;
		this.CycleRight.onClick += this.DoCycleRight;
	}

	// Token: 0x06005CC5 RID: 23749 RVA: 0x0021FDE7 File Offset: 0x0021DFE7
	public void Initialize(ListSettingConfig config, NewGameSettingsPanel panel, string disabledDefault)
	{
		base.Initialize(config, panel, disabledDefault);
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
	}

	// Token: 0x06005CC6 RID: 23750 RVA: 0x0021FE1C File Offset: 0x0021E01C
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(this.config);
		this.ValueLabel.text = currentQualitySetting.label;
		this.ValueToolTip.toolTip = currentQualitySetting.tooltip;
		this.CycleLeft.isInteractable = !this.config.IsFirstLevel(currentQualitySetting.id);
		this.CycleRight.isInteractable = !this.config.IsLastLevel(currentQualitySetting.id);
	}

	// Token: 0x06005CC7 RID: 23751 RVA: 0x0021FEA0 File Offset: 0x0021E0A0
	private void DoCycleLeft()
	{
		if (this.IsEnabled())
		{
			CustomGameSettings.Instance.CycleSettingLevel(this.config, -1);
			base.RefreshAll();
		}
	}

	// Token: 0x06005CC8 RID: 23752 RVA: 0x0021FEC2 File Offset: 0x0021E0C2
	private void DoCycleRight()
	{
		if (this.IsEnabled())
		{
			CustomGameSettings.Instance.CycleSettingLevel(this.config, 1);
			base.RefreshAll();
		}
	}

	// Token: 0x04003E5A RID: 15962
	[SerializeField]
	private LocText Label;

	// Token: 0x04003E5B RID: 15963
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04003E5C RID: 15964
	[SerializeField]
	private LocText ValueLabel;

	// Token: 0x04003E5D RID: 15965
	[SerializeField]
	private ToolTip ValueToolTip;

	// Token: 0x04003E5E RID: 15966
	[SerializeField]
	private KButton CycleLeft;

	// Token: 0x04003E5F RID: 15967
	[SerializeField]
	private KButton CycleRight;

	// Token: 0x04003E60 RID: 15968
	private ListSettingConfig config;
}
