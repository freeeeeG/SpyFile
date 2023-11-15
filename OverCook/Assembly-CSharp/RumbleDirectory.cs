using System;
using UnityEngine;

// Token: 0x0200082B RID: 2091
[CreateAssetMenu]
[Serializable]
public class RumbleDirectory : ScriptableObject
{
	// Token: 0x04001F95 RID: 8085
	public RumbleDirectory.OneShotDirectoryEntry[] OneShot = new RumbleDirectory.OneShotDirectoryEntry[0];

	// Token: 0x04001F96 RID: 8086
	public RumbleDirectory.LoopingDirectoryEntry[] Looping = new RumbleDirectory.LoopingDirectoryEntry[0];

	// Token: 0x0200082C RID: 2092
	[Serializable]
	public class OneShotDirectoryEntry : RumbleDirectoryEntry
	{
		// Token: 0x04001F97 RID: 8087
		public GameOneShotAudioTag Tag;
	}

	// Token: 0x0200082D RID: 2093
	[Serializable]
	public class LoopingDirectoryEntry : RumbleDirectoryEntry
	{
		// Token: 0x04001F98 RID: 8088
		public GameLoopingAudioTag Tag;
	}
}
