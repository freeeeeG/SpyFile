using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BDF RID: 3039
[Serializable]
public class SaveConfigurationScreen
{
	// Token: 0x06006022 RID: 24610 RVA: 0x00238B2C File Offset: 0x00236D2C
	public void ToggleDisabledContent(bool enable)
	{
		if (enable)
		{
			this.disabledContentPanel.SetActive(true);
			this.disabledContentWarning.SetActive(false);
			this.perSaveWarning.SetActive(true);
			return;
		}
		this.disabledContentPanel.SetActive(false);
		this.disabledContentWarning.SetActive(true);
		this.perSaveWarning.SetActive(false);
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x00238B88 File Offset: 0x00236D88
	public void Init()
	{
		this.autosaveFrequencySlider.minValue = 0f;
		this.autosaveFrequencySlider.maxValue = (float)(this.sliderValueToCycleCount.Length - 1);
		this.autosaveFrequencySlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnAutosaveValueChanged(Mathf.FloorToInt(val));
		});
		this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
		this.timelapseResolutionSlider.minValue = 0f;
		this.timelapseResolutionSlider.maxValue = (float)(this.sliderValueToResolution.Length - 1);
		this.timelapseResolutionSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnTimelapseValueChanged(Mathf.FloorToInt(val));
		});
		this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
		this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x00238C68 File Offset: 0x00236E68
	public void Show(bool show)
	{
		if (show)
		{
			this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
			this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
			this.OnAutosaveValueChanged(Mathf.FloorToInt(this.autosaveFrequencySlider.value));
			this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
		}
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x00238CDC File Offset: 0x00236EDC
	private void OnTimelapseValueChanged(int sliderValue)
	{
		Vector2I vector2I = this.SliderValueToResolution(sliderValue);
		if (vector2I.x <= 0)
		{
			this.timelapseDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
		}
		else
		{
			this.timelapseDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, vector2I.x, vector2I.y));
		}
		SaveGame.Instance.TimelapseResolution = vector2I;
		Game.Instance.Trigger(75424175, null);
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x00238D5C File Offset: 0x00236F5C
	private void OnAutosaveValueChanged(int sliderValue)
	{
		int num = this.SliderValueToCycleCount(sliderValue);
		if (sliderValue == 0)
		{
			this.autosaveDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
		}
		else
		{
			this.autosaveDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, num));
		}
		SaveGame.Instance.AutoSaveCycleInterval = num;
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x00238DB6 File Offset: 0x00236FB6
	private int SliderValueToCycleCount(int sliderValue)
	{
		return this.sliderValueToCycleCount[sliderValue];
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x00238DC0 File Offset: 0x00236FC0
	private int CycleCountToSlider(int count)
	{
		for (int i = 0; i < this.sliderValueToCycleCount.Length; i++)
		{
			if (this.sliderValueToCycleCount[i] == count)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x00238DEE File Offset: 0x00236FEE
	private Vector2I SliderValueToResolution(int sliderValue)
	{
		return this.sliderValueToResolution[sliderValue];
	}

	// Token: 0x0600602A RID: 24618 RVA: 0x00238DFC File Offset: 0x00236FFC
	private int ResolutionToSliderValue(Vector2I resolution)
	{
		for (int i = 0; i < this.sliderValueToResolution.Length; i++)
		{
			if (this.sliderValueToResolution[i] == resolution)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x0400416D RID: 16749
	[SerializeField]
	private KSlider autosaveFrequencySlider;

	// Token: 0x0400416E RID: 16750
	[SerializeField]
	private LocText timelapseDescriptionLabel;

	// Token: 0x0400416F RID: 16751
	[SerializeField]
	private KSlider timelapseResolutionSlider;

	// Token: 0x04004170 RID: 16752
	[SerializeField]
	private LocText autosaveDescriptionLabel;

	// Token: 0x04004171 RID: 16753
	private int[] sliderValueToCycleCount = new int[]
	{
		-1,
		50,
		20,
		10,
		5,
		2,
		1
	};

	// Token: 0x04004172 RID: 16754
	private Vector2I[] sliderValueToResolution = new Vector2I[]
	{
		new Vector2I(-1, -1),
		new Vector2I(256, 384),
		new Vector2I(512, 768),
		new Vector2I(1024, 1536),
		new Vector2I(2048, 3072),
		new Vector2I(4096, 6144),
		new Vector2I(8192, 12288)
	};

	// Token: 0x04004173 RID: 16755
	[SerializeField]
	private GameObject disabledContentPanel;

	// Token: 0x04004174 RID: 16756
	[SerializeField]
	private GameObject disabledContentWarning;

	// Token: 0x04004175 RID: 16757
	[SerializeField]
	private GameObject perSaveWarning;
}
