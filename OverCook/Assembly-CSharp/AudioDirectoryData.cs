using System;
using UnityEngine;

// Token: 0x020004EF RID: 1263
[Serializable]
public class AudioDirectoryData : ScriptableObject
{
	// Token: 0x04001143 RID: 4419
	public AudioDirectoryData.OneShotAudioDirectoryEntry[] OneShotAudio = new AudioDirectoryData.OneShotAudioDirectoryEntry[0];

	// Token: 0x04001144 RID: 4420
	public AudioDirectoryData.LoopingAudioDirectoryEntry[] LoopingAudio = new AudioDirectoryData.LoopingAudioDirectoryEntry[0];

	// Token: 0x020004F0 RID: 1264
	[Serializable]
	public class OneShotAudioDirectoryEntry : AudioDirectoryEntry
	{
		// Token: 0x04001145 RID: 4421
		public GameOneShotAudioTag Tag;
	}

	// Token: 0x020004F1 RID: 1265
	[Serializable]
	public class LoopingAudioDirectoryEntry : AudioDirectoryEntry
	{
		// Token: 0x04001146 RID: 4422
		public GameLoopingAudioTag Tag;

		// Token: 0x04001147 RID: 4423
		public AudioClip StartClip;

		// Token: 0x04001148 RID: 4424
		public AudioClip EndClip;
	}
}
