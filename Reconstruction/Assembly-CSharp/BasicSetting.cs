using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000285 RID: 645
public class BasicSetting : MonoBehaviour
{
	// Token: 0x06000FF9 RID: 4089 RVA: 0x0002AC03 File Offset: 0x00028E03
	public void Initialize()
	{
		this.InitializeResolution();
		this.ShowSetting();
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0002AC14 File Offset: 0x00028E14
	public void ShowSetting()
	{
		this.fullScreenToggle.isOn = Screen.fullScreen;
		this.musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
		this.effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0002AC68 File Offset: 0x00028E68
	private void InitializeResolution()
	{
		this.resolutions = (from resolution in Screen.resolutions
		select new Resolution
		{
			width = resolution.width,
			height = resolution.height
		}).Distinct<Resolution>().ToList<Resolution>();
		this.resolutionDropdown.ClearOptions();
		Resolution item = default(Resolution);
		item.width = 2560;
		item.height = 1080;
		item.refreshRate = 60;
		this.resolutions.Add(item);
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < this.resolutions.Count; i++)
		{
			string item2 = this.resolutions[i].width.ToString() + "x" + this.resolutions[i].height.ToString();
			list.Add(item2);
			if (this.resolutions[i].width == Screen.currentResolution.width && this.resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		this.resolutionDropdown.AddOptions(list);
		if (PlayerPrefs.GetInt("Resolution", 0) != 0)
		{
			this.resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);
		}
		else
		{
			this.resolutionDropdown.value = value;
		}
		this.resolutionDropdown.RefreshShownValue();
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0002ADF6 File Offset: 0x00028FF6
	public void SetMusicVolume(float volume)
	{
		this.audioMixer.SetFloat("musicVolume", volume);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0002AE0A File Offset: 0x0002900A
	public void SetSoundVolume(float volume)
	{
		this.audioMixer.SetFloat("effectVolume", volume);
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0002AE1E File Offset: 0x0002901E
	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0002AE28 File Offset: 0x00029028
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = this.resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		PlayerPrefs.SetInt("Resolution", resolutionIndex);
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0002AE65 File Offset: 0x00029065
	public void SaveSetting()
	{
		StaticData.SetTipsPos();
		PlayerPrefs.SetFloat("MusicVolume", this.musicSlider.value);
		PlayerPrefs.SetFloat("EffectVolume", this.effectSlider.value);
	}

	// Token: 0x04000846 RID: 2118
	public AudioMixer audioMixer;

	// Token: 0x04000847 RID: 2119
	public Dropdown resolutionDropdown;

	// Token: 0x04000848 RID: 2120
	public Toggle fullScreenToggle;

	// Token: 0x04000849 RID: 2121
	private List<Resolution> resolutions;

	// Token: 0x0400084A RID: 2122
	[SerializeField]
	private Slider musicSlider;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	private Slider effectSlider;
}
