using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000BA2 RID: 2978
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : KMonoBehaviour
{
	// Token: 0x06005CDF RID: 23775 RVA: 0x0022031C File Offset: 0x0021E51C
	public void SetCloseAction(System.Action onClose)
	{
		if (this.closeButton != null)
		{
			this.closeButton.onClick += onClose;
		}
		if (this.background != null)
		{
			this.background.onClick += onClose;
		}
	}

	// Token: 0x06005CE0 RID: 23776 RVA: 0x00220354 File Offset: 0x0021E554
	public void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		this.settings = CustomGameSettings.Instance;
		this.widgets = new List<NewGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.settings.QualitySettings)
		{
			if ((!keyValuePair.Value.debug_only || DebugHandler.enabled) && (!keyValuePair.Value.editor_only || Application.isEditor) && DlcManager.IsContentActive(keyValuePair.Value.required_content))
			{
				ListSettingConfig listSettingConfig = keyValuePair.Value as ListSettingConfig;
				if (listSettingConfig != null)
				{
					NewGameSettingList newGameSettingList = Util.KInstantiateUI<NewGameSettingList>(this.prefab_cycle_setting, this.content.gameObject, true);
					newGameSettingList.Initialize(listSettingConfig, this, keyValuePair.Value.missing_content_default);
					this.widgets.Add(newGameSettingList);
				}
				else
				{
					ToggleSettingConfig toggleSettingConfig = keyValuePair.Value as ToggleSettingConfig;
					if (toggleSettingConfig != null)
					{
						NewGameSettingToggle newGameSettingToggle = Util.KInstantiateUI<NewGameSettingToggle>(this.prefab_checkbox_setting, this.content.gameObject, true);
						newGameSettingToggle.Initialize(toggleSettingConfig, this, keyValuePair.Value.missing_content_default);
						this.widgets.Add(newGameSettingToggle);
					}
					else
					{
						SeedSettingConfig seedSettingConfig = keyValuePair.Value as SeedSettingConfig;
						if (seedSettingConfig != null)
						{
							NewGameSettingSeed newGameSettingSeed = Util.KInstantiateUI<NewGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, true);
							newGameSettingSeed.Initialize(seedSettingConfig);
							this.widgets.Add(newGameSettingSeed);
						}
					}
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x06005CE1 RID: 23777 RVA: 0x00220518 File Offset: 0x0021E718
	public void Refresh()
	{
		foreach (NewGameSettingWidget newGameSettingWidget in this.widgets)
		{
			newGameSettingWidget.Refresh();
		}
		if (this.OnRefresh != null)
		{
			this.OnRefresh();
		}
	}

	// Token: 0x06005CE2 RID: 23778 RVA: 0x0022057C File Offset: 0x0021E77C
	public void ConsumeSettingsCode(string code)
	{
		this.settings.ParseAndApplySettingsCode(code);
	}

	// Token: 0x06005CE3 RID: 23779 RVA: 0x0022058A File Offset: 0x0021E78A
	public void ConsumeStoryTraitsCode(string code)
	{
		this.settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	// Token: 0x06005CE4 RID: 23780 RVA: 0x00220598 File Offset: 0x0021E798
	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		this.settings.SetQualitySetting(setting, level, notify);
	}

	// Token: 0x06005CE5 RID: 23781 RVA: 0x002205A8 File Offset: 0x0021E7A8
	public string GetSetting(SettingConfig setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x06005CE6 RID: 23782 RVA: 0x002205BB File Offset: 0x0021E7BB
	public string GetSetting(string setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x06005CE7 RID: 23783 RVA: 0x002205CE File Offset: 0x0021E7CE
	public void Cancel()
	{
	}

	// Token: 0x04003E76 RID: 15990
	[SerializeField]
	private Transform content;

	// Token: 0x04003E77 RID: 15991
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003E78 RID: 15992
	[SerializeField]
	private KButton background;

	// Token: 0x04003E79 RID: 15993
	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	// Token: 0x04003E7A RID: 15994
	[SerializeField]
	private GameObject prefab_slider_setting;

	// Token: 0x04003E7B RID: 15995
	[SerializeField]
	private GameObject prefab_checkbox_setting;

	// Token: 0x04003E7C RID: 15996
	[SerializeField]
	private GameObject prefab_seed_input_setting;

	// Token: 0x04003E7D RID: 15997
	private CustomGameSettings settings;

	// Token: 0x04003E7E RID: 15998
	private List<NewGameSettingWidget> widgets;

	// Token: 0x04003E7F RID: 15999
	public System.Action OnRefresh;
}
