using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
[Serializable]
public class SoundEntry
{
	// Token: 0x06000541 RID: 1345 RVA: 0x000151A8 File Offset: 0x000133A8
	public SoundEntry()
	{
		this.name = "";
		this.clips = new AudioClip[1];
		this.type = SoundAssetData.SOUND_TYPE.SOUND;
		this.doLoop = false;
		this.mod_Volume = 1f;
		this.soundLength = 0f;
		this.doRandomClip = false;
		this.randomClipCount = 0;
		this.haveCooldown = false;
		this.cooldownTime = 0f;
		this.randomPitchRange = new Vector2(1f, 1f);
	}

	// Token: 0x040004E5 RID: 1253
	public string name;

	// Token: 0x040004E6 RID: 1254
	public AudioClip[] clips;

	// Token: 0x040004E7 RID: 1255
	public SoundAssetData.SOUND_TYPE type;

	// Token: 0x040004E8 RID: 1256
	public bool doLoop;

	// Token: 0x040004E9 RID: 1257
	public float mod_Volume;

	// Token: 0x040004EA RID: 1258
	public float soundLength;

	// Token: 0x040004EB RID: 1259
	public bool doRandomClip;

	// Token: 0x040004EC RID: 1260
	public int randomClipCount;

	// Token: 0x040004ED RID: 1261
	public bool haveCooldown;

	// Token: 0x040004EE RID: 1262
	public float cooldownTime;

	// Token: 0x040004EF RID: 1263
	public bool doRandomPitch;

	// Token: 0x040004F0 RID: 1264
	public Vector2 randomPitchRange;
}
