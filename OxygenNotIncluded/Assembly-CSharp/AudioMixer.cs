using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class AudioMixer
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060018B2 RID: 6322 RVA: 0x000805B1 File Offset: 0x0007E7B1
	public static AudioMixer instance
	{
		get
		{
			return AudioMixer._instance;
		}
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000805B8 File Offset: 0x0007E7B8
	public static AudioMixer Create()
	{
		AudioMixer._instance = new AudioMixer();
		AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
		if (audioMixerSnapshots != null)
		{
			audioMixerSnapshots.ReloadSnapshots();
		}
		return AudioMixer._instance;
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000805E9 File Offset: 0x0007E7E9
	public static void Destroy()
	{
		AudioMixer._instance.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AudioMixer._instance = null;
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x000805FC File Offset: 0x0007E7FC
	public EventInstance Start(EventReference event_ref)
	{
		string snapshot;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot);
		return this.Start(snapshot);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x00080628 File Offset: 0x0007E828
	public EventInstance Start(string snapshot)
	{
		EventInstance eventInstance;
		if (!this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			if (RuntimeManager.IsInitialized)
			{
				eventInstance = KFMOD.CreateInstance(snapshot);
				this.activeSnapshots[snapshot] = eventInstance;
				eventInstance.start();
				eventInstance.setParameterByName("snapshotActive", 1f, false);
			}
			else
			{
				eventInstance = default(EventInstance);
			}
		}
		AudioMixer.instance.Log("Start Snapshot: " + snapshot);
		return eventInstance;
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x000806A8 File Offset: 0x0007E8A8
	public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.Stop(s, stop_mode);
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000806D8 File Offset: 0x0007E8D8
	public bool Stop(HashedString snapshot, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		bool result = false;
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			eventInstance.setParameterByName("snapshotActive", 0f, false);
			eventInstance.stop(stop_mode);
			eventInstance.release();
			this.activeSnapshots.Remove(snapshot);
			result = true;
			AudioMixer instance = AudioMixer.instance;
			string[] array = new string[5];
			array[0] = "Stop Snapshot: [";
			int num = 1;
			HashedString hashedString = snapshot;
			array[num] = hashedString.ToString();
			array[2] = "] with fadeout mode: [";
			array[3] = stop_mode.ToString();
			array[4] = "]";
			instance.Log(string.Concat(array));
		}
		else
		{
			AudioMixer instance2 = AudioMixer.instance;
			string str = "Tried to stop snapshot: [";
			HashedString hashedString = snapshot;
			instance2.Log(str + hashedString.ToString() + "] but it wasn't active.");
		}
		return result;
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x000807A7 File Offset: 0x0007E9A7
	public void Reset()
	{
		this.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x000807B0 File Offset: 0x0007E9B0
	public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, EventInstance> keyValuePair in this.activeSnapshots)
		{
			if (keyValuePair.Key != AudioMixer.UserVolumeSettingsHash)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Stop(list[i], stop_mode);
		}
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x00080844 File Offset: 0x0007EA44
	public bool SnapshotIsActive(EventReference event_ref)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.SnapshotIsActive(s);
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x00080873 File Offset: 0x0007EA73
	public bool SnapshotIsActive(HashedString snapshot_name)
	{
		return this.activeSnapshots.ContainsKey(snapshot_name);
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x00080888 File Offset: 0x0007EA88
	public void SetSnapshotParameter(EventReference event_ref, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		string snapshot_name;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot_name);
		this.SetSnapshotParameter(snapshot_name, parameter_name, parameter_value, shouldLog);
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000808B8 File Offset: 0x0007EAB8
	public void SetSnapshotParameter(string snapshot_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", snapshot_name, parameter_name, parameter_value));
		}
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot_name, out eventInstance))
		{
			eventInstance.setParameterByName(parameter_name, parameter_value, false);
			return;
		}
		this.Log(string.Concat(new string[]
		{
			"Tried to set [",
			parameter_name,
			"] to [",
			parameter_value.ToString(),
			"] but [",
			snapshot_name,
			"] is not active."
		}));
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x00080948 File Offset: 0x0007EB48
	public void StartPersistentSnapshots()
	{
		this.persistentSnapshotsActive = true;
		this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
		this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
		this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x000809CC File Offset: 0x0007EBCC
	public void StopPersistentSnapshots()
	{
		this.persistentSnapshotsActive = false;
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().PulseSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x00080A4C File Offset: 0x0007EC4C
	private string GetSnapshotName(EventReference event_ref)
	{
		string result;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out result);
		return result;
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x00080A70 File Offset: 0x0007EC70
	public void UpdatePersistentSnapshotParameters()
	{
		this.SetVisibleDuplicants();
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName, out this.duplicantCountMovingInst))
		{
			this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName2 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName2, out this.duplicantCountSleepingInst))
		{
			this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName3 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		if (this.activeSnapshots.TryGetValue(snapshotName3, out this.duplicantCountInst))
		{
			this.duplicantCountInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName4 = this.GetSnapshotName(AudioMixerSnapshots.Get().PulseSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName4, out this.pulseInst))
		{
			float num = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
			int speed = SpeedControlScreen.Instance.GetSpeed();
			if (speed == 1)
			{
				num /= 2f;
			}
			else if (speed == 2)
			{
				num /= 3f;
			}
			float value = Mathf.Abs(Mathf.Sin(Time.time * 3.1415927f * num));
			this.pulseInst.setParameterByName("Pulse", value, false);
		}
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00080C1F File Offset: 0x0007EE1F
	public void UpdateSpaceVisibleSnapshot(float percent)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", percent, false);
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x00080C34 File Offset: 0x0007EE34
	public void UpdateFacilityVisibleSnapshot(float percent)
	{
		this.facilityVisibleInst.setParameterByName("facilityVisible", percent, false);
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x00080C4C File Offset: 0x0007EE4C
	private void SetVisibleDuplicants()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			Vector3 position = Components.LiveMinionIdentities[i].transform.GetPosition();
			if (CameraController.Instance.IsVisiblePos(position))
			{
				num++;
				Navigator component = Components.LiveMinionIdentities[i].GetComponent<Navigator>();
				if (component != null && component.IsMoving())
				{
					num2++;
				}
				else
				{
					StaminaMonitor.Instance smi = Components.LiveMinionIdentities[i].GetComponent<Worker>().GetSMI<StaminaMonitor.Instance>();
					if (smi != null && smi.IsSleeping())
					{
						num3++;
					}
				}
			}
		}
		this.visibleDupes["visible"] = num;
		this.visibleDupes["moving"] = num2;
		this.visibleDupes["sleeping"] = num3;
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x00080D2C File Offset: 0x0007EF2C
	public void StartUserVolumesSnapshot()
	{
		this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			EventDescription eventDescription;
			eventInstance.getDescription(out eventDescription);
			USER_PROPERTY user_PROPERTY;
			eventDescription.getUserProperty("buses", out user_PROPERTY);
			string text = user_PROPERTY.stringValue();
			char c = '-';
			string[] array = text.Split(new char[]
			{
				c
			});
			for (int i = 0; i < array.Length; i++)
			{
				float busLevel = 1f;
				string key = "Volume_" + array[i];
				if (KPlayerPrefs.HasKey(key))
				{
					busLevel = KPlayerPrefs.GetFloat(key);
				}
				AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
				userVolumeBus.busLevel = busLevel;
				userVolumeBus.labelString = Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + array[i].ToUpper());
				this.userVolumeSettings.Add(array[i], userVolumeBus);
				this.SetUserVolume(array[i], userVolumeBus.busLevel);
			}
		}
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x00080E48 File Offset: 0x0007F048
	public void SetUserVolume(string bus, float value)
	{
		if (!this.userVolumeSettings.ContainsKey(bus))
		{
			global::Debug.LogError("The provided bus doesn't exist. Check yo'self fool!");
			return;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		else if (value < 0f)
		{
			value = 0f;
		}
		this.userVolumeSettings[bus].busLevel = value;
		KPlayerPrefs.SetFloat("Volume_" + bus, value);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel, false);
		}
		else
		{
			this.Log(string.Concat(new string[]
			{
				"Tried to set [",
				bus,
				"] to [",
				value.ToString(),
				"] but UserVolumeSettingsSnapshot is not active."
			}));
		}
		if (bus == "Music")
		{
			this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
		}
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x00080F59 File Offset: 0x0007F159
	private void Log(string s)
	{
	}

	// Token: 0x04000D9F RID: 3487
	private static AudioMixer _instance = null;

	// Token: 0x04000DA0 RID: 3488
	private const string DUPLICANT_COUNT_ID = "duplicantCount";

	// Token: 0x04000DA1 RID: 3489
	private const string PULSE_ID = "Pulse";

	// Token: 0x04000DA2 RID: 3490
	private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";

	// Token: 0x04000DA3 RID: 3491
	private const string SPACE_VISIBLE_ID = "spaceVisible";

	// Token: 0x04000DA4 RID: 3492
	private const string FACILITY_VISIBLE_ID = "facilityVisible";

	// Token: 0x04000DA5 RID: 3493
	private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";

	// Token: 0x04000DA6 RID: 3494
	public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();

	// Token: 0x04000DA7 RID: 3495
	public List<HashedString> SnapshotDebugLog = new List<HashedString>();

	// Token: 0x04000DA8 RID: 3496
	public bool activeNIS;

	// Token: 0x04000DA9 RID: 3497
	public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;

	// Token: 0x04000DAA RID: 3498
	public static float PULSE_SNAPSHOT_BPM = 120f;

	// Token: 0x04000DAB RID: 3499
	public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;

	// Token: 0x04000DAC RID: 3500
	private EventInstance duplicantCountInst;

	// Token: 0x04000DAD RID: 3501
	private EventInstance pulseInst;

	// Token: 0x04000DAE RID: 3502
	private EventInstance duplicantCountMovingInst;

	// Token: 0x04000DAF RID: 3503
	private EventInstance duplicantCountSleepingInst;

	// Token: 0x04000DB0 RID: 3504
	private EventInstance spaceVisibleInst;

	// Token: 0x04000DB1 RID: 3505
	private EventInstance facilityVisibleInst;

	// Token: 0x04000DB2 RID: 3506
	private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");

	// Token: 0x04000DB3 RID: 3507
	public bool persistentSnapshotsActive;

	// Token: 0x04000DB4 RID: 3508
	private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();

	// Token: 0x04000DB5 RID: 3509
	public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

	// Token: 0x020010EA RID: 4330
	public class UserVolumeBus
	{
		// Token: 0x04005A95 RID: 23189
		public string labelString;

		// Token: 0x04005A96 RID: 23190
		public float busLevel;
	}
}
