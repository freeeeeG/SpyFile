using System;
using System.Collections.Generic;

// Token: 0x02000459 RID: 1113
public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06001842 RID: 6210 RVA: 0x0007E352 File Offset: 0x0007C552
	public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0007E35C File Offset: 0x0007C55C
	public void AddVolume(string animFile, string eventName, EffectorValues vals)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			this.volumeCache.Add(key, vals);
			return;
		}
		this.volumeCache[key] = vals;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0007E3A8 File Offset: 0x0007C5A8
	public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

	// Token: 0x04000D71 RID: 3441
	public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
