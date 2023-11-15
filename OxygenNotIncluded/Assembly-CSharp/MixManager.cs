using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class MixManager : MonoBehaviour
{
	// Token: 0x060018EB RID: 6379 RVA: 0x00082175 File Offset: 0x00080375
	private void Update()
	{
		if (AudioMixer.instance != null && AudioMixer.instance.persistentSnapshotsActive)
		{
			AudioMixer.instance.UpdatePersistentSnapshotParameters();
		}
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x00082194 File Offset: 0x00080394
	private void OnApplicationFocus(bool hasFocus)
	{
		if (AudioMixer.instance == null || AudioMixerSnapshots.Get() == null)
		{
			return;
		}
		if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
			return;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().GameNotFocusedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}
}
