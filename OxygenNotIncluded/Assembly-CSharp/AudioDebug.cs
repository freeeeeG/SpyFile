using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
	// Token: 0x0600189C RID: 6300 RVA: 0x0007FFE0 File Offset: 0x0007E1E0
	public static AudioDebug Get()
	{
		return AudioDebug.instance;
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x0007FFE7 File Offset: 0x0007E1E7
	protected override void OnPrefabInit()
	{
		AudioDebug.instance = this;
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x0007FFEF File Offset: 0x0007E1EF
	public void ToggleMusic()
	{
		if (Game.Instance != null)
		{
			Game.Instance.SetMusicEnabled(this.musicEnabled);
		}
		this.musicEnabled = !this.musicEnabled;
	}

	// Token: 0x04000D8F RID: 3471
	private static AudioDebug instance;

	// Token: 0x04000D90 RID: 3472
	public bool musicEnabled;

	// Token: 0x04000D91 RID: 3473
	public bool debugSoundEvents;

	// Token: 0x04000D92 RID: 3474
	public bool debugFloorSounds;

	// Token: 0x04000D93 RID: 3475
	public bool debugGameEventSounds;

	// Token: 0x04000D94 RID: 3476
	public bool debugNotificationSounds;

	// Token: 0x04000D95 RID: 3477
	public bool debugVoiceSounds;
}
