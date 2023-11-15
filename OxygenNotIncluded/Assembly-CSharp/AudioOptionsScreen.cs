using System;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AAA RID: 2730
public class AudioOptionsScreen : KModalScreen
{
	// Token: 0x06005353 RID: 21331 RVA: 0x001DE378 File Offset: 0x001DC578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.doneButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
		foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> keyValuePair in AudioMixer.instance.userVolumeSettings)
		{
			SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
			this.sliderBusMap.Add(newSlider.slider, keyValuePair.Key);
			newSlider.slider.value = keyValuePair.Value.busLevel;
			newSlider.nameLabel.text = keyValuePair.Value.labelString;
			newSlider.UpdateSliderLabel(keyValuePair.Value.busLevel);
			newSlider.slider.ClearReleaseHandleEvent();
			newSlider.slider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnReleaseHandle(newSlider.slider);
			});
			if (keyValuePair.Key == "Master")
			{
				newSlider.transform.SetSiblingIndex(2);
				newSlider.slider.onValueChanged.AddListener(new UnityAction<float>(this.CheckMasterValue));
				this.CheckMasterValue(keyValuePair.Value.busLevel);
			}
		}
		HierarchyReferences component = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
		GameObject gameObject = component.GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
		component.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayMusic();
		};
		component.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, 1);
		}
		HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
		GameObject gameObject2 = component2.GetReference("Button").gameObject;
		gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
		gameObject2.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayAutomation();
		};
		component2.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
		component2.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.MuteOnFocusLost))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, 0);
		}
		HierarchyReferences component3 = this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>();
		GameObject gameObject3 = component3.GetReference("Button").gameObject;
		gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST_TOOLTIP);
		gameObject3.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleMuteOnFocusLost();
		};
		component3.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST);
		component3.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x06005354 RID: 21332 RVA: 0x001DE6F0 File Offset: 0x001DC8F0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005355 RID: 21333 RVA: 0x001DE712 File Offset: 0x001DC912
	private void CheckMasterValue(float value)
	{
		this.jambell.enabled = (value == 0f);
	}

	// Token: 0x06005356 RID: 21334 RVA: 0x001DE727 File Offset: 0x001DC927
	private void OnReleaseHandle(KSlider slider)
	{
		AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
	}

	// Token: 0x06005357 RID: 21335 RVA: 0x001DE748 File Offset: 0x001DC948
	private void ToggleAlwaysPlayMusic()
	{
		MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
		this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayMusicKey, MusicManager.instance.alwaysPlayMusic ? 1 : 0);
	}

	// Token: 0x06005358 RID: 21336 RVA: 0x001DE7B0 File Offset: 0x001DC9B0
	private void ToggleAlwaysPlayAutomation()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1) ? 0 : 1);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
	}

	// Token: 0x06005359 RID: 21337 RVA: 0x001DE808 File Offset: 0x001DCA08
	private void ToggleMuteOnFocusLost()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, (KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1) ? 0 : 1);
		this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x0600535A RID: 21338 RVA: 0x001DE860 File Offset: 0x001DCA60
	private void BuildAudioDeviceList()
	{
		this.audioDevices.Clear();
		this.audioDeviceOptions.Clear();
		int num;
		RuntimeManager.CoreSystem.getNumDrivers(out num);
		for (int i = 0; i < num; i++)
		{
			KFMOD.AudioDevice audioDevice = default(KFMOD.AudioDevice);
			string name;
			RuntimeManager.CoreSystem.getDriverInfo(i, out name, 64, out audioDevice.guid, out audioDevice.systemRate, out audioDevice.speakerMode, out audioDevice.speakerModeChannels);
			audioDevice.name = name;
			audioDevice.fmod_id = i;
			this.audioDevices.Add(audioDevice);
			this.audioDeviceOptions.Add(new Dropdown.OptionData(audioDevice.name));
		}
	}

	// Token: 0x0600535B RID: 21339 RVA: 0x001DE90C File Offset: 0x001DCB0C
	private void OnAudioDeviceChanged(int idx)
	{
		RuntimeManager.CoreSystem.setDriver(idx);
		for (int i = 0; i < this.audioDevices.Count; i++)
		{
			if (idx == this.audioDevices[i].fmod_id)
			{
				KFMOD.currentDevice = this.audioDevices[i];
				KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
				return;
			}
		}
	}

	// Token: 0x0600535C RID: 21340 RVA: 0x001DE983 File Offset: 0x001DCB83
	private void OnClose(GameObject go)
	{
		this.alwaysPlayMusicMetric[AudioOptionsScreen.AlwaysPlayMusicKey] = MusicManager.instance.alwaysPlayMusic;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric, "AudioOptionsScreen");
		UnityEngine.Object.Destroy(go);
	}

	// Token: 0x0400378B RID: 14219
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400378C RID: 14220
	[SerializeField]
	private KButton doneButton;

	// Token: 0x0400378D RID: 14221
	[SerializeField]
	private SliderContainer sliderPrefab;

	// Token: 0x0400378E RID: 14222
	[SerializeField]
	private GameObject sliderGroup;

	// Token: 0x0400378F RID: 14223
	[SerializeField]
	private Image jambell;

	// Token: 0x04003790 RID: 14224
	[SerializeField]
	private GameObject alwaysPlayMusicButton;

	// Token: 0x04003791 RID: 14225
	[SerializeField]
	private GameObject alwaysPlayAutomationButton;

	// Token: 0x04003792 RID: 14226
	[SerializeField]
	private GameObject muteOnFocusLostToggle;

	// Token: 0x04003793 RID: 14227
	[SerializeField]
	private Dropdown deviceDropdown;

	// Token: 0x04003794 RID: 14228
	private UIPool<SliderContainer> sliderPool;

	// Token: 0x04003795 RID: 14229
	private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();

	// Token: 0x04003796 RID: 14230
	public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";

	// Token: 0x04003797 RID: 14231
	public static readonly string AlwaysPlayAutomation = "AlwaysPlayAutomation";

	// Token: 0x04003798 RID: 14232
	public static readonly string MuteOnFocusLost = "MuteOnFocusLost";

	// Token: 0x04003799 RID: 14233
	private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>
	{
		{
			AudioOptionsScreen.AlwaysPlayMusicKey,
			null
		}
	};

	// Token: 0x0400379A RID: 14234
	private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();

	// Token: 0x0400379B RID: 14235
	private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();
}
