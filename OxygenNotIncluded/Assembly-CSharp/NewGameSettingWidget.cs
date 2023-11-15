using System;
using Klei.CustomSettings;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BA1 RID: 2977
public abstract class NewGameSettingWidget : KMonoBehaviour
{
	// Token: 0x06005CD9 RID: 23769 RVA: 0x0022025C File Offset: 0x0021E45C
	protected virtual void Initialize(SettingConfig config, NewGameSettingsPanel panel, string disabledDefault)
	{
		this.config = config;
		this.panel = panel;
		this.disabledDefault = disabledDefault;
	}

	// Token: 0x06005CDA RID: 23770 RVA: 0x00220274 File Offset: 0x0021E474
	public virtual void Refresh()
	{
		bool flag = this.ShouldBeEnabled();
		if (flag == this.widget_enabled)
		{
			return;
		}
		this.widget_enabled = flag;
		if (this.IsEnabled())
		{
			this.BG.color = this.enabledColor;
			CustomGameSettings.Instance.SetQualitySetting(this.config, this.config.GetDefaultLevelId());
			return;
		}
		CustomGameSettings.Instance.SetQualitySetting(this.config, this.disabledDefault);
		this.BG.color = this.disabledColor;
	}

	// Token: 0x06005CDB RID: 23771 RVA: 0x002202F5 File Offset: 0x0021E4F5
	protected void RefreshAll()
	{
		this.panel.Refresh();
	}

	// Token: 0x06005CDC RID: 23772 RVA: 0x00220302 File Offset: 0x0021E502
	protected virtual bool IsEnabled()
	{
		return this.widget_enabled;
	}

	// Token: 0x06005CDD RID: 23773 RVA: 0x0022030A File Offset: 0x0021E50A
	private bool ShouldBeEnabled()
	{
		return true;
	}

	// Token: 0x04003E6F RID: 15983
	[SerializeField]
	private Image BG;

	// Token: 0x04003E70 RID: 15984
	[SerializeField]
	private Color enabledColor;

	// Token: 0x04003E71 RID: 15985
	[SerializeField]
	private Color disabledColor;

	// Token: 0x04003E72 RID: 15986
	private SettingConfig config;

	// Token: 0x04003E73 RID: 15987
	private NewGameSettingsPanel panel;

	// Token: 0x04003E74 RID: 15988
	private string disabledDefault;

	// Token: 0x04003E75 RID: 15989
	private bool widget_enabled = true;
}
