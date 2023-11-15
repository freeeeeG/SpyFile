using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B09 RID: 2825
internal class GraphicsOptionsScreen : KModalScreen
{
	// Token: 0x0600571E RID: 22302 RVA: 0x001FD390 File Offset: 0x001FB590
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.TITLE);
		this.originalSettings = this.CaptureSettings();
		this.applyButton.isInteractable = false;
		this.applyButton.onClick += this.OnApply;
		this.applyButton.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.APPLYBUTTON);
		this.doneButton.onClick += this.OnDone;
		this.closeButton.onClick += this.OnDone;
		this.doneButton.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.DONE_BUTTON);
		bool flag = QualitySettings.GetQualityLevel() == 1;
		this.lowResToggle.ChangeState(flag ? 1 : 0);
		MultiToggle multiToggle = this.lowResToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnLowResToggle));
		this.lowResToggle.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.LOWRES);
		this.resolutionDropdown.ClearOptions();
		this.BuildOptions();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(delegate()
		{
			this.BuildOptions();
			this.resolutionDropdown.options = this.options;
		}));
		this.resolutionDropdown.options = this.options;
		this.resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionChanged));
		this.fullscreenToggle.ChangeState(Screen.fullScreen ? 1 : 0);
		MultiToggle multiToggle2 = this.fullscreenToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnFullscreenToggle));
		this.fullscreenToggle.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.FULLSCREEN);
		this.resolutionDropdown.transform.parent.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.RESOLUTION);
		if (this.fullscreenToggle.CurrentState == 1)
		{
			int resolutionIndex = this.GetResolutionIndex(this.originalSettings.resolution);
			if (resolutionIndex != -1)
			{
				this.resolutionDropdown.value = resolutionIndex;
			}
		}
		this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>(true);
		this.UpdateSliderLabel();
		this.uiScaleSlider.onValueChanged.AddListener(delegate(float data)
		{
			this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%";
		});
		this.uiScaleSlider.onReleaseHandle += delegate()
		{
			this.UpdateUIScale(this.uiScaleSlider.value);
		};
		this.BuildColorModeOptions();
		this.colorModeDropdown.options = this.colorModeOptions;
		this.colorModeDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnColorModeChanged));
		int value = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
		{
			value = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
		}
		this.colorModeDropdown.value = value;
		this.RefreshColorExamples(this.originalSettings.colorSetId);
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x001FD662 File Offset: 0x001FB862
	public static void SetSettingsFromPrefs()
	{
		GraphicsOptionsScreen.SetResolutionFromPrefs();
		GraphicsOptionsScreen.SetLowResFromPrefs();
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x001FD670 File Offset: 0x001FB870
	public static void SetLowResFromPrefs()
	{
		int num = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.LowResKey))
		{
			num = KPlayerPrefs.GetInt(GraphicsOptionsScreen.LowResKey);
			QualitySettings.SetQualityLevel(num, true);
		}
		else
		{
			QualitySettings.SetQualityLevel(num, true);
		}
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Low Res Textures? {0}", (num == 1) ? "Yes" : "No")
		});
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x001FD6D0 File Offset: 0x001FB8D0
	public static void SetResolutionFromPrefs()
	{
		int num = Screen.currentResolution.width;
		int num2 = Screen.currentResolution.height;
		int num3 = Screen.currentResolution.refreshRate;
		bool flag = Screen.fullScreen;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionWidthKey) && KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionHeightKey))
		{
			int @int = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionWidthKey);
			int int2 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionHeightKey);
			int int3 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.RefreshRateKey, Screen.currentResolution.refreshRate);
			bool flag2 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.FullScreenKey, Screen.fullScreen ? 1 : 0) == 1;
			if (int2 <= 1 || @int <= 1)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Saved resolution was invalid, ignoring..."
				});
			}
			else
			{
				num = @int;
				num2 = int2;
				num3 = int3;
				flag = flag2;
			}
		}
		if (num <= 1 || num2 <= 1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Detected a degenerate resolution, attempting to fix..."
			});
			foreach (Resolution resolution in Screen.resolutions)
			{
				if (resolution.width == 1920)
				{
					num = resolution.width;
					num2 = resolution.height;
					num3 = 0;
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				foreach (Resolution resolution2 in Screen.resolutions)
				{
					if (resolution2.width == 1280)
					{
						num = resolution2.width;
						num2 = resolution2.height;
						num3 = 0;
					}
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				foreach (Resolution resolution3 in Screen.resolutions)
				{
					if (resolution3.width > 1 && resolution3.height > 1 && resolution3.refreshRate > 0)
					{
						num = resolution3.width;
						num2 = resolution3.height;
						num3 = 0;
					}
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				string text = "Could not find a suitable resolution for this screen! Reported available resolutions are:";
				foreach (Resolution resolution4 in Screen.resolutions)
				{
					text += string.Format("\n{0}x{1} @ {2}hz", resolution4.width, resolution4.height, resolution4.refreshRate);
				}
				global::Debug.LogError(text);
				num = 1280;
				num2 = 720;
				flag = false;
				num3 = 0;
			}
		}
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Applying resolution {0}x{1} @{2}hz (fullscreen: {3})", new object[]
			{
				num,
				num2,
				num3,
				flag
			})
		});
		Screen.SetResolution(num, num2, flag, num3);
	}

	// Token: 0x06005722 RID: 22306 RVA: 0x001FD980 File Offset: 0x001FBB80
	public static void SetColorModeFromPrefs()
	{
		int num = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
		{
			num = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
		}
		GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[num];
	}

	// Token: 0x06005723 RID: 22307 RVA: 0x001FD9BC File Offset: 0x001FBBBC
	public static void OnResize()
	{
		GraphicsOptionsScreen.Settings settings = default(GraphicsOptionsScreen.Settings);
		settings.resolution = Screen.currentResolution;
		settings.resolution.width = Screen.width;
		settings.resolution.height = Screen.height;
		settings.fullscreen = Screen.fullScreen;
		settings.lowRes = QualitySettings.GetQualityLevel();
		settings.colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet);
		GraphicsOptionsScreen.SaveSettingsToPrefs(settings);
	}

	// Token: 0x06005724 RID: 22308 RVA: 0x001FDA40 File Offset: 0x001FBC40
	private static void SaveSettingsToPrefs(GraphicsOptionsScreen.Settings settings)
	{
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.LowResKey, settings.lowRes);
		global::Debug.LogFormat("Screen resolution updated, saving values to prefs: {0}x{1} @ {2}, fullscreen: {3}", new object[]
		{
			settings.resolution.width,
			settings.resolution.height,
			settings.resolution.refreshRate,
			settings.fullscreen
		});
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionWidthKey, settings.resolution.width);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionHeightKey, settings.resolution.height);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.RefreshRateKey, settings.resolution.refreshRate);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.FullScreenKey, settings.fullscreen ? 1 : 0);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ColorModeKey, settings.colorSetId);
	}

	// Token: 0x06005725 RID: 22309 RVA: 0x001FDB20 File Offset: 0x001FBD20
	private void UpdateUIScale(float value)
	{
		this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>(true);
		foreach (KCanvasScaler kcanvasScaler in this.CanvasScalers)
		{
			float userScale = value / 100f;
			kcanvasScaler.SetUserScale(userScale);
			KPlayerPrefs.SetFloat(KCanvasScaler.UIScalePrefKey, value);
		}
		ScreenResize.Instance.TriggerResize();
		this.UpdateSliderLabel();
	}

	// Token: 0x06005726 RID: 22310 RVA: 0x001FDB7C File Offset: 0x001FBD7C
	private void UpdateSliderLabel()
	{
		if (this.CanvasScalers != null && this.CanvasScalers.Length != 0 && this.CanvasScalers[0] != null)
		{
			this.uiScaleSlider.value = this.CanvasScalers[0].GetUserScale() * 100f;
			this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%";
		}
	}

	// Token: 0x06005727 RID: 22311 RVA: 0x001FDBF0 File Offset: 0x001FBDF0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.resolutionDropdown.Hide();
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x001FDC20 File Offset: 0x001FBE20
	private void BuildOptions()
	{
		this.options.Clear();
		this.resolutions.Clear();
		Resolution item = default(Resolution);
		item.width = Screen.width;
		item.height = Screen.height;
		item.refreshRate = Screen.currentResolution.refreshRate;
		this.options.Add(new Dropdown.OptionData(item.ToString()));
		this.resolutions.Add(item);
		foreach (Resolution item2 in Screen.resolutions)
		{
			if (item2.height >= 720)
			{
				this.options.Add(new Dropdown.OptionData(item2.ToString()));
				this.resolutions.Add(item2);
			}
		}
	}

	// Token: 0x06005729 RID: 22313 RVA: 0x001FDCF8 File Offset: 0x001FBEF8
	private void BuildColorModeOptions()
	{
		this.colorModeOptions.Clear();
		for (int i = 0; i < GlobalAssets.Instance.colorSetOptions.Length; i++)
		{
			this.colorModeOptions.Add(new Dropdown.OptionData(Strings.Get(GlobalAssets.Instance.colorSetOptions[i].settingName)));
		}
	}

	// Token: 0x0600572A RID: 22314 RVA: 0x001FDD54 File Offset: 0x001FBF54
	private void RefreshColorExamples(int idx)
	{
		Color32 logicOn = GlobalAssets.Instance.colorSetOptions[idx].logicOn;
		Color32 logicOff = GlobalAssets.Instance.colorSetOptions[idx].logicOff;
		Color32 cropHalted = GlobalAssets.Instance.colorSetOptions[idx].cropHalted;
		Color32 cropGrowing = GlobalAssets.Instance.colorSetOptions[idx].cropGrowing;
		Color32 cropGrown = GlobalAssets.Instance.colorSetOptions[idx].cropGrown;
		logicOn.a = byte.MaxValue;
		logicOff.a = byte.MaxValue;
		cropHalted.a = byte.MaxValue;
		cropGrowing.a = byte.MaxValue;
		cropGrown.a = byte.MaxValue;
		this.colorExampleLogicOn.color = logicOn;
		this.colorExampleLogicOff.color = logicOff;
		this.colorExampleCropHalted.color = cropHalted;
		this.colorExampleCropGrowing.color = cropGrowing;
		this.colorExampleCropGrown.color = cropGrown;
	}

	// Token: 0x0600572B RID: 22315 RVA: 0x001FDE50 File Offset: 0x001FC050
	private int GetResolutionIndex(Resolution resolution)
	{
		int num = -1;
		int result = -1;
		for (int i = 0; i < this.resolutions.Count; i++)
		{
			Resolution resolution2 = this.resolutions[i];
			if (resolution2.width == resolution.width && resolution2.height == resolution.height && resolution2.refreshRate == 0)
			{
				result = i;
			}
			if (resolution2.width == resolution.width && resolution2.height == resolution.height && Math.Abs(resolution2.refreshRate - resolution.refreshRate) <= 1)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			return num;
		}
		return result;
	}

	// Token: 0x0600572C RID: 22316 RVA: 0x001FDEF4 File Offset: 0x001FC0F4
	private GraphicsOptionsScreen.Settings CaptureSettings()
	{
		return new GraphicsOptionsScreen.Settings
		{
			fullscreen = Screen.fullScreen,
			resolution = new Resolution
			{
				width = Screen.width,
				height = Screen.height,
				refreshRate = Screen.currentResolution.refreshRate
			},
			lowRes = QualitySettings.GetQualityLevel(),
			colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet)
		};
	}

	// Token: 0x0600572D RID: 22317 RVA: 0x001FDF80 File Offset: 0x001FC180
	private void OnApply()
	{
		try
		{
			GraphicsOptionsScreen.Settings new_settings = default(GraphicsOptionsScreen.Settings);
			new_settings.resolution = this.resolutions[this.resolutionDropdown.value];
			new_settings.fullscreen = (this.fullscreenToggle.CurrentState != 0);
			new_settings.lowRes = this.lowResToggle.CurrentState;
			new_settings.colorSetId = this.colorModeId;
			if (GlobalAssets.Instance.colorSetOptions[this.colorModeId] != GlobalAssets.Instance.colorSet)
			{
				this.colorModeChanged = true;
			}
			this.ApplyConfirmSettings(new_settings, delegate
			{
				this.applyButton.isInteractable = false;
				if (this.colorModeChanged)
				{
					this.feedbackDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
					this.feedbackDialog.PopupConfirmDialog(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK.text, null, null, UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK_BUTTON.text, delegate
					{
						App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/117325-color-blindness-feedback/");
					}, null, null, null, null);
					this.feedbackDialog.gameObject.SetActive(true);
				}
				this.colorModeChanged = false;
				GraphicsOptionsScreen.SaveSettingsToPrefs(new_settings);
			});
		}
		catch (Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Failed to apply graphics options!\nResolutions:");
			foreach (Resolution resolution in this.resolutions)
			{
				stringBuilder.Append("\t" + resolution.ToString() + "\n");
			}
			stringBuilder.Append("Selected Resolution Idx: " + this.resolutionDropdown.value.ToString());
			stringBuilder.Append("FullScreen: " + this.fullscreenToggle.CurrentState.ToString());
			global::Debug.LogError(stringBuilder.ToString());
			throw ex;
		}
	}

	// Token: 0x0600572E RID: 22318 RVA: 0x001FE12C File Offset: 0x001FC32C
	public void OnDone()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600572F RID: 22319 RVA: 0x001FE13C File Offset: 0x001FC33C
	private void RefreshApplyButton()
	{
		GraphicsOptionsScreen.Settings settings = this.CaptureSettings();
		if (settings.fullscreen && this.fullscreenToggle.CurrentState == 0)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (!settings.fullscreen && this.fullscreenToggle.CurrentState == 1)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (settings.lowRes != this.lowResToggle.CurrentState)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (settings.colorSetId != this.colorModeId)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		int resolutionIndex = this.GetResolutionIndex(settings.resolution);
		this.applyButton.isInteractable = (this.resolutionDropdown.value != resolutionIndex);
	}

	// Token: 0x06005730 RID: 22320 RVA: 0x001FE1F9 File Offset: 0x001FC3F9
	private void OnFullscreenToggle()
	{
		this.fullscreenToggle.ChangeState((this.fullscreenToggle.CurrentState == 0) ? 1 : 0);
		this.RefreshApplyButton();
	}

	// Token: 0x06005731 RID: 22321 RVA: 0x001FE21D File Offset: 0x001FC41D
	private void OnResolutionChanged(int idx)
	{
		this.RefreshApplyButton();
	}

	// Token: 0x06005732 RID: 22322 RVA: 0x001FE225 File Offset: 0x001FC425
	private void OnColorModeChanged(int idx)
	{
		this.colorModeId = idx;
		this.RefreshApplyButton();
		this.RefreshColorExamples(this.colorModeId);
	}

	// Token: 0x06005733 RID: 22323 RVA: 0x001FE240 File Offset: 0x001FC440
	private void OnLowResToggle()
	{
		this.lowResToggle.ChangeState((this.lowResToggle.CurrentState == 0) ? 1 : 0);
		this.RefreshApplyButton();
	}

	// Token: 0x06005734 RID: 22324 RVA: 0x001FE264 File Offset: 0x001FC464
	private void ApplyConfirmSettings(GraphicsOptionsScreen.Settings new_settings, System.Action on_confirm)
	{
		GraphicsOptionsScreen.Settings current_settings = this.CaptureSettings();
		this.ApplySettings(new_settings);
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
		System.Action action = delegate()
		{
			this.ApplySettings(current_settings);
		};
		Coroutine timer = base.StartCoroutine(this.Timer(15f, action));
		this.confirmDialog.onDeactivateCB = delegate()
		{
			this.StopCoroutine(timer);
		};
		this.confirmDialog.PopupConfirmDialog(this.colorModeChanged ? UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES_STRING_COLOR.text : UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES.text, on_confirm, action, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x06005735 RID: 22325 RVA: 0x001FE338 File Offset: 0x001FC538
	private void ApplySettings(GraphicsOptionsScreen.Settings new_settings)
	{
		Resolution resolution = new_settings.resolution;
		Screen.SetResolution(resolution.width, resolution.height, new_settings.fullscreen, resolution.refreshRate);
		Screen.fullScreen = new_settings.fullscreen;
		int resolutionIndex = this.GetResolutionIndex(new_settings.resolution);
		if (resolutionIndex != -1)
		{
			this.resolutionDropdown.value = resolutionIndex;
		}
		GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[new_settings.colorSetId];
		global::Debug.Log("Applying low res settings " + new_settings.lowRes.ToString() + " / existing is " + QualitySettings.GetQualityLevel().ToString());
		if (QualitySettings.GetQualityLevel() != new_settings.lowRes)
		{
			QualitySettings.SetQualityLevel(new_settings.lowRes, true);
		}
	}

	// Token: 0x06005736 RID: 22326 RVA: 0x001FE3F5 File Offset: 0x001FC5F5
	private IEnumerator Timer(float time, System.Action revert)
	{
		yield return SequenceUtil.WaitForSeconds(time);
		if (this.confirmDialog != null)
		{
			this.confirmDialog.Deactivate();
			revert();
		}
		yield break;
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x001FE412 File Offset: 0x001FC612
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04003ABA RID: 15034
	[SerializeField]
	private Dropdown resolutionDropdown;

	// Token: 0x04003ABB RID: 15035
	[SerializeField]
	private MultiToggle lowResToggle;

	// Token: 0x04003ABC RID: 15036
	[SerializeField]
	private MultiToggle fullscreenToggle;

	// Token: 0x04003ABD RID: 15037
	[SerializeField]
	private KButton applyButton;

	// Token: 0x04003ABE RID: 15038
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04003ABF RID: 15039
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003AC0 RID: 15040
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04003AC1 RID: 15041
	[SerializeField]
	private ConfirmDialogScreen feedbackPrefab;

	// Token: 0x04003AC2 RID: 15042
	[SerializeField]
	private KSlider uiScaleSlider;

	// Token: 0x04003AC3 RID: 15043
	[SerializeField]
	private LocText sliderLabel;

	// Token: 0x04003AC4 RID: 15044
	[SerializeField]
	private LocText title;

	// Token: 0x04003AC5 RID: 15045
	[SerializeField]
	private Dropdown colorModeDropdown;

	// Token: 0x04003AC6 RID: 15046
	[SerializeField]
	private KImage colorExampleLogicOn;

	// Token: 0x04003AC7 RID: 15047
	[SerializeField]
	private KImage colorExampleLogicOff;

	// Token: 0x04003AC8 RID: 15048
	[SerializeField]
	private KImage colorExampleCropHalted;

	// Token: 0x04003AC9 RID: 15049
	[SerializeField]
	private KImage colorExampleCropGrowing;

	// Token: 0x04003ACA RID: 15050
	[SerializeField]
	private KImage colorExampleCropGrown;

	// Token: 0x04003ACB RID: 15051
	public static readonly string ResolutionWidthKey = "ResolutionWidth";

	// Token: 0x04003ACC RID: 15052
	public static readonly string ResolutionHeightKey = "ResolutionHeight";

	// Token: 0x04003ACD RID: 15053
	public static readonly string RefreshRateKey = "RefreshRate";

	// Token: 0x04003ACE RID: 15054
	public static readonly string FullScreenKey = "FullScreen";

	// Token: 0x04003ACF RID: 15055
	public static readonly string LowResKey = "LowResTextures";

	// Token: 0x04003AD0 RID: 15056
	public static readonly string ColorModeKey = "ColorModeID";

	// Token: 0x04003AD1 RID: 15057
	private KCanvasScaler[] CanvasScalers;

	// Token: 0x04003AD2 RID: 15058
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x04003AD3 RID: 15059
	private ConfirmDialogScreen feedbackDialog;

	// Token: 0x04003AD4 RID: 15060
	private List<Resolution> resolutions = new List<Resolution>();

	// Token: 0x04003AD5 RID: 15061
	private List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

	// Token: 0x04003AD6 RID: 15062
	private List<Dropdown.OptionData> colorModeOptions = new List<Dropdown.OptionData>();

	// Token: 0x04003AD7 RID: 15063
	private int colorModeId;

	// Token: 0x04003AD8 RID: 15064
	private bool colorModeChanged;

	// Token: 0x04003AD9 RID: 15065
	private GraphicsOptionsScreen.Settings originalSettings;

	// Token: 0x02001A1A RID: 6682
	private struct Settings
	{
		// Token: 0x04007850 RID: 30800
		public bool fullscreen;

		// Token: 0x04007851 RID: 30801
		public Resolution resolution;

		// Token: 0x04007852 RID: 30802
		public int lowRes;

		// Token: 0x04007853 RID: 30803
		public int colorSetId;
	}
}
