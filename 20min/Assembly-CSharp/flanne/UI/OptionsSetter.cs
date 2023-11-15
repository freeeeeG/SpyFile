using System;
using CameraShake;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200022C RID: 556
	public class OptionsSetter : MonoBehaviour
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x0002D00B File Offset: 0x0002B20B
		private void Start()
		{
			this.AM = AudioManager.Instance;
			this.Refresh();
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002D020 File Offset: 0x0002B220
		public void Refresh()
		{
			this.SetBGM(this.AM.MusicVolume);
			this.SetSFX(this.AM.SFXVolume);
			this._resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
			bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
			this.SetFullscreen(fullscreen);
			this.SetCameraShake(PlayerPrefs.GetInt("CameraShake", 1) == 1);
			this.SetAutoReload(PlayerPrefs.GetInt("AutoReload", 1) == 1);
			this.SetOutline(PlayerPrefs.GetInt("Outline", 0) == 1);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
		public void OnClickBGMVolume()
		{
			float num = this.AM.MusicVolume;
			num += 0.25f;
			if (num > 1f)
			{
				num = 0f;
			}
			this.SetBGM(num);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0002D0FC File Offset: 0x0002B2FC
		public void OnClickSFXVolume()
		{
			float num = this.AM.SFXVolume;
			num += 0.25f;
			if (num > 1f)
			{
				num = 0f;
			}
			this.SetSFX(num);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0002D132 File Offset: 0x0002B332
		public void OnClickFullscreen()
		{
			this.SetFullscreen(!Screen.fullScreen);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0002D144 File Offset: 0x0002B344
		public void OnClickResolution()
		{
			this._resolutionIndex++;
			if (this._resolutionIndex >= this.SupportedResolutions.Length)
			{
				this._resolutionIndex = 0;
			}
			PlayerPrefs.SetInt("ResolutionIndex", this._resolutionIndex);
			this.SetResolution(this._resolutionIndex);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0002D192 File Offset: 0x0002B392
		public void OnClickCameraShake()
		{
			this.SetCameraShake(!CameraShaker.ShakeOn);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0002D1A2 File Offset: 0x0002B3A2
		public void OnClickAutoReload()
		{
			this.SetAutoReload(!OptionsSetter.AutoReloadEnabled);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0002D1B2 File Offset: 0x0002B3B2
		public void OnClickOutline()
		{
			this.SetOutline(!OutlineSetter.isOn);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0002D1C2 File Offset: 0x0002B3C2
		private void SetBGM(float volume)
		{
			this.bgmTMP.text = string.Format(LocalizationSystem.GetLocalizedValue(this.bgmLabel.key) + " {0:P0}.", volume);
			this.AM.MusicVolume = volume;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0002D200 File Offset: 0x0002B400
		private void SetSFX(float volume)
		{
			this.sfxTMP.text = string.Format(LocalizationSystem.GetLocalizedValue(this.sfxLabel.key) + " {0:P0}.", volume);
			this.AM.SFXVolume = volume;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0002D240 File Offset: 0x0002B440
		private void SetFullscreen(bool isFS)
		{
			string text = LocalizationSystem.GetLocalizedValue(this.fullscreenLabel.key) + " ";
			if (isFS)
			{
				text += LocalizationSystem.GetLocalizedValue(this.onString.key);
			}
			else
			{
				text += LocalizationSystem.GetLocalizedValue(this.offString.key);
			}
			this.fullscreenTMP.text = text;
			if (isFS)
			{
				this.DisableResolution();
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
			else
			{
				this.resolutionButton.interactable = true;
				this.SetResolution(this._resolutionIndex);
			}
			PlayerPrefs.SetInt("Fullscreen", isFS ? 1 : 0);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0002D2FC File Offset: 0x0002B4FC
		private void SetResolution(int resolutionIndex)
		{
			Vector2Int vector2Int = this.SupportedResolutions[resolutionIndex];
			Screen.SetResolution(vector2Int.x, vector2Int.y, false);
			string text = LocalizationSystem.GetLocalizedValue(this.resolutionLabel.key) + " ";
			text = string.Concat(new object[]
			{
				text,
				vector2Int.x,
				"x",
				vector2Int.y
			});
			this.resolutionTMP.text = text;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0002D388 File Offset: 0x0002B588
		private void DisableResolution()
		{
			this.resolutionButton.interactable = false;
			string text = LocalizationSystem.GetLocalizedValue(this.resolutionLabel.key) + " ";
			text += "-";
			this.resolutionTMP.text = text;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002D3D4 File Offset: 0x0002B5D4
		private void SetCameraShake(bool isOn)
		{
			string text = LocalizationSystem.GetLocalizedValue(this.cameraShakeLabel.key) + " ";
			if (isOn)
			{
				text += LocalizationSystem.GetLocalizedValue(this.onString.key);
			}
			else
			{
				text += LocalizationSystem.GetLocalizedValue(this.offString.key);
			}
			this.cameraShakeTMP.text = text;
			CameraShaker.ShakeOn = isOn;
			PlayerPrefs.SetInt("CameraShake", isOn ? 1 : 0);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002D454 File Offset: 0x0002B654
		private void SetAutoReload(bool isOn)
		{
			string text = LocalizationSystem.GetLocalizedValue(this.autoReloadLabel.key) + " ";
			if (isOn)
			{
				text += LocalizationSystem.GetLocalizedValue(this.onString.key);
			}
			else
			{
				text += LocalizationSystem.GetLocalizedValue(this.offString.key);
			}
			this.autoReloadTMP.text = text;
			OptionsSetter.AutoReloadEnabled = isOn;
			PlayerPrefs.SetInt("AutoReload", isOn ? 1 : 0);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0002D4D4 File Offset: 0x0002B6D4
		private void SetOutline(bool isOn)
		{
			string text = LocalizationSystem.GetLocalizedValue(this.outlineLabel.key) + " ";
			if (isOn)
			{
				text += LocalizationSystem.GetLocalizedValue(this.onString.key);
			}
			else
			{
				text += LocalizationSystem.GetLocalizedValue(this.offString.key);
			}
			this.outlineTMP.text = text;
			OutlineSetter.isOn = isOn;
			PlayerPrefs.SetInt("Outline", isOn ? 1 : 0);
		}

		// Token: 0x04000887 RID: 2183
		public static bool AutoReloadEnabled;

		// Token: 0x04000888 RID: 2184
		[SerializeField]
		private LocalizedString bgmLabel;

		// Token: 0x04000889 RID: 2185
		[SerializeField]
		private LocalizedString sfxLabel;

		// Token: 0x0400088A RID: 2186
		[SerializeField]
		private LocalizedString fullscreenLabel;

		// Token: 0x0400088B RID: 2187
		[SerializeField]
		private LocalizedString resolutionLabel;

		// Token: 0x0400088C RID: 2188
		[SerializeField]
		private LocalizedString cameraShakeLabel;

		// Token: 0x0400088D RID: 2189
		[SerializeField]
		private LocalizedString autoReloadLabel;

		// Token: 0x0400088E RID: 2190
		[SerializeField]
		private LocalizedString outlineLabel;

		// Token: 0x0400088F RID: 2191
		[SerializeField]
		private LocalizedString onString;

		// Token: 0x04000890 RID: 2192
		[SerializeField]
		private LocalizedString offString;

		// Token: 0x04000891 RID: 2193
		[SerializeField]
		private TMP_Text bgmTMP;

		// Token: 0x04000892 RID: 2194
		[SerializeField]
		private TMP_Text sfxTMP;

		// Token: 0x04000893 RID: 2195
		[SerializeField]
		private TMP_Text fullscreenTMP;

		// Token: 0x04000894 RID: 2196
		[SerializeField]
		private TMP_Text resolutionTMP;

		// Token: 0x04000895 RID: 2197
		[SerializeField]
		private TMP_Text cameraShakeTMP;

		// Token: 0x04000896 RID: 2198
		[SerializeField]
		private TMP_Text autoReloadTMP;

		// Token: 0x04000897 RID: 2199
		[SerializeField]
		private TMP_Text outlineTMP;

		// Token: 0x04000898 RID: 2200
		[SerializeField]
		private Button resolutionButton;

		// Token: 0x04000899 RID: 2201
		private Vector2Int[] SupportedResolutions = new Vector2Int[]
		{
			new Vector2Int(800, 450),
			new Vector2Int(1200, 675),
			new Vector2Int(1600, 900),
			new Vector2Int(1920, 1080)
		};

		// Token: 0x0400089A RID: 2202
		private int _resolutionIndex;

		// Token: 0x0400089B RID: 2203
		private AudioManager AM;
	}
}
