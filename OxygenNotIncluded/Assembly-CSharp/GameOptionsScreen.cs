using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class GameOptionsScreen : KModalButtonMenu
{
	// Token: 0x06005700 RID: 22272 RVA: 0x001FCC6F File Offset: 0x001FAE6F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005701 RID: 22273 RVA: 0x001FCC78 File Offset: 0x001FAE78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			this.saveConfiguration.ToggleDisabledContent(true);
			this.saveConfiguration.Init();
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.saveConfiguration.ToggleDisabledContent(false);
		}
		this.resetTutorialButton.onClick += this.OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.controlsButton.gameObject.SetActive(false);
		}
		else
		{
			this.controlsButton.onClick += this.OnKeyBindings;
		}
		this.sandboxButton.onClick += this.OnUnlockSandboxMode;
		this.doneButton.onClick += this.Deactivate;
		this.closeButton.onClick += this.Deactivate;
		if (this.defaultToCloudSaveToggle != null)
		{
			this.RefreshCloudSaveToggle();
			this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += this.OnDefaultToCloudSaveToggle;
		}
		if (this.cloudSavesPanel != null)
		{
			this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		this.cameraSpeedSlider.minValue = 1f;
		this.cameraSpeedSlider.maxValue = 20f;
		this.cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		this.RefreshCameraSliderLabel();
	}

	// Token: 0x06005702 RID: 22274 RVA: 0x001FCE1C File Offset: 0x001FB01C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			this.savePanel.SetActive(true);
			this.saveConfiguration.Show(show);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.savePanel.SetActive(false);
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
	}

	// Token: 0x06005703 RID: 22275 RVA: 0x001FCE84 File Offset: 0x001FB084
	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	// Token: 0x06005704 RID: 22276 RVA: 0x001FCE8D File Offset: 0x001FB08D
	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	// Token: 0x06005705 RID: 22277 RVA: 0x001FCEC4 File Offset: 0x001FB0C4
	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	// Token: 0x06005706 RID: 22278 RVA: 0x001FCF09 File Offset: 0x001FB109
	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x001FCF20 File Offset: 0x001FB120
	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	// Token: 0x06005708 RID: 22280 RVA: 0x001FCF53 File Offset: 0x001FB153
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x001FCF78 File Offset: 0x001FB178
	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		}, null, null, null, null, null, null);
		component.Activate();
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x001FCFF8 File Offset: 0x001FB1F8
	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		string text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
		System.Action on_confirm = delegate()
		{
			SaveGame.Instance.sandboxEnabled = true;
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		System.Action on_cancel = delegate()
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), false, false);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		string confirm_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
		string cancel_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
		component.PopupConfirmDialog(text, on_confirm, on_cancel, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, delegate
		{
		}, null, confirm_text, cancel_text, null);
		component.Activate();
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x001FD08F File Offset: 0x001FB28F
	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x001FD0A4 File Offset: 0x001FB2A4
	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	// Token: 0x04003A9F RID: 15007
	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	// Token: 0x04003AA0 RID: 15008
	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	// Token: 0x04003AA1 RID: 15009
	[SerializeField]
	private KButton resetTutorialButton;

	// Token: 0x04003AA2 RID: 15010
	[SerializeField]
	private KButton controlsButton;

	// Token: 0x04003AA3 RID: 15011
	[SerializeField]
	private KButton sandboxButton;

	// Token: 0x04003AA4 RID: 15012
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04003AA5 RID: 15013
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04003AA6 RID: 15014
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003AA7 RID: 15015
	[SerializeField]
	private GameObject cloudSavesPanel;

	// Token: 0x04003AA8 RID: 15016
	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	// Token: 0x04003AA9 RID: 15017
	[SerializeField]
	private GameObject savePanel;

	// Token: 0x04003AAA RID: 15018
	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	// Token: 0x04003AAB RID: 15019
	[SerializeField]
	private KSlider cameraSpeedSlider;

	// Token: 0x04003AAC RID: 15020
	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	// Token: 0x04003AAD RID: 15021
	private const int cameraSliderNotchScale = 10;

	// Token: 0x04003AAE RID: 15022
	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
