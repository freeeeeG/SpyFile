using System;
using System.Collections.Generic;
using System.Linq;
using CutScenes;
using Data;
using GameResources;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x02000428 RID: 1064
	public class Settings : Dialogue
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override bool closeWithPauseKey
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0003DBA9 File Offset: 0x0003BDA9
		private void Awake()
		{
			this.LoadStrings();
			this.InitializeGraphicsOptions();
			this.InitializeAudioOptions();
			this.InitializeDataOptions();
			this.InitializeGameplayOptions();
			this._return.onClick.AddListener(delegate
			{
				this._panel.state = Panel.State.Menu;
			});
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0003DBE8 File Offset: 0x0003BDE8
		protected override void OnEnable()
		{
			base.OnEnable();
			this.LoadStrings();
			switch (Screen.fullScreenMode)
			{
			case FullScreenMode.ExclusiveFullScreen:
				this._screen.value = 1;
				break;
			case FullScreenMode.FullScreenWindow:
				this._screen.value = 0;
				break;
			case FullScreenMode.Windowed:
				this._screen.value = 2;
				break;
			}
			this.UpdateEasyMode();
			this.SetDefaultFocus();
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0003DC53 File Offset: 0x0003BE53
		private void SetDefaultFocus()
		{
			base.Focus(this._resolution);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0003DC64 File Offset: 0x0003BE64
		private void InitializeGraphicsOptions()
		{
			this.InitializeResolutionOption();
			this._light.value = (GameData.Settings.lightEnabled ? 1 : 0);
			this._light.onValueChanged += delegate(int v)
			{
				Light2D.lightEnabled = (GameData.Settings.lightEnabled = (v == 1));
			};
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0003DCB8 File Offset: 0x0003BEB8
		private void InitializeResolutionOption()
		{
			this._resolutionList = new List<Resolution>();
			foreach (Resolution resolution in Screen.resolutions)
			{
				bool flag = false;
				for (int j = 0; j < this._resolutionList.Count; j++)
				{
					if (resolution.width == this._resolutionList[j].width && resolution.height == this._resolutionList[j].height)
					{
						flag = true;
						if (resolution.refreshRate > this._resolutionList[j].refreshRate)
						{
							this._resolutionList[j] = resolution;
						}
					}
				}
				if (!flag)
				{
					this._resolutionList.Add(resolution);
				}
			}
			this._resolution.SetTexts((from r in this._resolutionList
			select string.Format("{0} x {1}", r.width, r.height)).ToArray<string>());
			int num = -1;
			for (int k = 0; k < this._resolutionList.Count; k++)
			{
				if (this._resolutionList[k].width == Screen.width && this._resolutionList[k].height == Screen.height)
				{
					num = k;
				}
			}
			if (num == -1)
			{
				Resolution item = default(Resolution);
				item.width = Screen.width;
				item.height = Screen.height;
				item.refreshRate = Screen.currentResolution.refreshRate;
				this._resolutionList.Add(item);
				num = this._resolutionList.Count - 1;
			}
			this._resolution.value = num;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0003DE84 File Offset: 0x0003C084
		private void InitializeAudioOptions()
		{
			this._master.value = GameData.Settings.masterVolume;
			this._master.onValueChanged.AddListener(delegate(float v)
			{
				GameData.Settings.masterVolume = v;
				PersistentSingleton<SoundManager>.Instance.UpdateMusicVolume();
			});
			this._music.value = GameData.Settings.musicVolume;
			this._music.onValueChanged.AddListener(delegate(float v)
			{
				GameData.Settings.musicVolume = v;
				PersistentSingleton<SoundManager>.Instance.UpdateMusicVolume();
			});
			this._sfx.value = GameData.Settings.sfxVolume;
			this._sfx.onValueChanged.AddListener(delegate(float v)
			{
				GameData.Settings.sfxVolume = v;
			});
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0003DF4E File Offset: 0x0003C14E
		private void InitializeDataOptions()
		{
			this._resetData.onClick.AddListener(delegate
			{
				this._resetDataConfirm.Open(string.Empty, delegate()
				{
					GameData.Generic.ResetAll();
					GameData.Currency.ResetAll();
					GameData.Progress.ResetAll();
					GameData.Gear.ResetAll();
					GameData.Save.instance.ResetAll();
					GameData.HardmodeProgress.ResetAll();
					Singleton<Service>.Instance.levelManager.player.playerComponents.savableAbilityManager.ResetAll();
					this._panel.ReturnToTitleScreen();
					base.Focus(this._resetData);
				}, delegate()
				{
					base.Focus(this._resetData);
				});
			});
			this._resetCutsceneData.onClick.AddListener(delegate
			{
				this._resetCutsceneDataConfirm.Open(string.Empty, delegate()
				{
					foreach (Key key in EnumValues<Key>.Values)
					{
						if (key != Key.DarkMirrorArachne_건강보조장치 && key != Key.darkMirrorCollector_품목순환장치 && key != Key.각인합성장치_Intro && key != Key.행운계측기_Intro && key != Key.darkMirrorBlackMarket)
						{
							GameData.Progress.cutscene.SetData(key, false);
						}
					}
					GameData.Progress.skulstory.ResetAll();
					GameData.Progress.cutscene.SaveAll();
					GameData.Progress.skulstory.SaveAll();
					base.Focus(this._resetCutsceneData);
				}, delegate()
				{
					base.Focus(this._resetCutsceneData);
				});
			});
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0003DF88 File Offset: 0x0003C188
		private void InitializeGameplayOptions()
		{
			this._language.value = GameData.Settings.language;
			this._language.onValueChanged += delegate(int v)
			{
				GameData.Settings.language = v;
			};
			this._cameraShake.value = GameData.Settings.cameraShakeIntensity;
			this._cameraShake.onValueChanged.AddListener(delegate(float v)
			{
				GameData.Settings.cameraShakeIntensity = v;
			});
			this._vibrationPower.value = GameData.Settings.vibrationIntensity;
			this._vibrationPower.onValueChanged.AddListener(delegate(float v)
			{
				GameData.Settings.vibrationIntensity = v;
			});
			this._particleQuality.value = GameData.Settings.particleQuality;
			this._particleQuality.onValueChanged += delegate(int v)
			{
				GameData.Settings.particleQuality = v;
			};
			this._easyMode.value = (GameData.Settings.easyMode ? 1 : 0);
			this._easyMode.onValueChanged += delegate(int v)
			{
				if (GameData.HardmodeProgress.hardmode)
				{
					this._easyMode.SetValueWithoutNotify(0);
					return;
				}
				if (v == 1)
				{
					this._easyModeConfirm.Open(string.Empty, delegate()
					{
						GameData.Settings.easyMode = true;
						Achievement.Type.RookieWelcome.Set();
						base.Focus(this._easyMode);
					}, delegate()
					{
						this._easyMode.SetValueWithoutNotify(0);
						EventSystem.current.SetSelectedGameObject(null);
						base.Focus(this._easyMode);
					});
					return;
				}
				GameData.Settings.easyMode = false;
			};
			this._showTimer.value = (GameData.Settings.showTimer ? 1 : 0);
			this._showTimer.onValueChanged += delegate(int v)
			{
				GameData.Settings.showTimer = (v == 1);
			};
			this._showUI.value = (int)Scene<GameBase>.instance.uiManager.hideOption;
			this._showUI.onValueChanged += delegate(int v)
			{
				Scene<GameBase>.instance.uiManager.SetHideOption((UIManager.HideOption)v);
			};
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0003E138 File Offset: 0x0003C338
		private void LoadStrings()
		{
			this._light.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/off",
				"label/pause/settings/on"
			}));
			string str = "label/pause/settings/graphics/screen";
			this._screen.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				str + "/borderless",
				str + "/fullscreen",
				str + "/windowed"
			}));
			this._language.SetTexts(Localization.nativeNames.ToArray<string>());
			this._particleQuality.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/off",
				"label/pause/settings/low",
				"label/pause/settings/medium",
				"label/pause/settings/high"
			}));
			this._easyMode.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/off",
				"label/pause/settings/on"
			}));
			this._showTimer.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/off",
				"label/pause/settings/on"
			}));
			this._showUI.SetTexts(Localization.GetLocalizedStrings(new string[]
			{
				"label/pause/settings/gamePlay/showUI/all",
				"label/pause/settings/gamePlay/showUI/hideHUD",
				"label/pause/settings/gamePlay/showUI/hideAll"
			}));
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0003E278 File Offset: 0x0003C478
		private void UpdateEasyMode()
		{
			if (GameData.HardmodeProgress.hardmode)
			{
				this._left.enabled = false;
				this._right.enabled = false;
				this._easyMode.interactable = false;
				this._easyMode.value = 0;
				return;
			}
			this._left.enabled = true;
			this._right.enabled = true;
			this._easyMode.interactable = true;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0003E2E1 File Offset: 0x0003C4E1
		protected override void OnDisable()
		{
			base.OnDisable();
			this.ApplyDisplayOptions();
			GameData.Settings.Save();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0003E300 File Offset: 0x0003C500
		private void ApplyDisplayOptions()
		{
			Resolution resolution = this._resolutionList[this._resolution.value];
			FullScreenMode fullscreenMode = FullScreenMode.FullScreenWindow;
			switch (this._screen.value)
			{
			case 0:
				fullscreenMode = FullScreenMode.FullScreenWindow;
				break;
			case 1:
				fullscreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
			case 2:
				fullscreenMode = FullScreenMode.Windowed;
				break;
			}
			Screen.SetResolution(resolution.width, resolution.height, fullscreenMode, resolution.refreshRate);
		}

		// Token: 0x0400111D RID: 4381
		public const string key = "label/pause/settings";

		// Token: 0x0400111E RID: 4382
		[SerializeField]
		private Panel _panel;

		// Token: 0x0400111F RID: 4383
		[SerializeField]
		[Header("Graphics")]
		private Selection _resolution;

		// Token: 0x04001120 RID: 4384
		[SerializeField]
		private Selection _screen;

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private Selection _light;

		// Token: 0x04001122 RID: 4386
		[SerializeField]
		private Selection _particleQuality;

		// Token: 0x04001123 RID: 4387
		[SerializeField]
		private Slider _cameraShake;

		// Token: 0x04001124 RID: 4388
		[SerializeField]
		private Slider _vibrationPower;

		// Token: 0x04001125 RID: 4389
		[Header("Audio")]
		[Space]
		[SerializeField]
		private Slider _master;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private Slider _music;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private Slider _sfx;

		// Token: 0x04001128 RID: 4392
		[SerializeField]
		[Space]
		[Header("Data")]
		private Button _resetData;

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private Confirm _resetDataConfirm;

		// Token: 0x0400112A RID: 4394
		[SerializeField]
		private Button _resetCutsceneData;

		// Token: 0x0400112B RID: 4395
		[SerializeField]
		private Confirm _resetCutsceneDataConfirm;

		// Token: 0x0400112C RID: 4396
		[SerializeField]
		[Space]
		[Header("Game Play")]
		private Selection _language;

		// Token: 0x0400112D RID: 4397
		[SerializeField]
		private Selection _easyMode;

		// Token: 0x0400112E RID: 4398
		[SerializeField]
		private Confirm _easyModeConfirm;

		// Token: 0x0400112F RID: 4399
		[SerializeField]
		private PointerDownHandler _left;

		// Token: 0x04001130 RID: 4400
		[SerializeField]
		private PointerDownHandler _right;

		// Token: 0x04001131 RID: 4401
		[SerializeField]
		private Selection _showTimer;

		// Token: 0x04001132 RID: 4402
		[SerializeField]
		private Selection _showUI;

		// Token: 0x04001133 RID: 4403
		[SerializeField]
		[Space]
		private Button _return;

		// Token: 0x04001134 RID: 4404
		private List<Resolution> _resolutionList;
	}
}
