using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000DC RID: 220
[CreateAssetMenu(fileName = "SoundAssetData_", menuName = "SoundAssetData", order = 1)]
[Serializable]
public class SoundAssetData : ScriptableObject
{
	// Token: 0x0600053E RID: 1342 RVA: 0x0001511C File Offset: 0x0001331C
	public void AddSoundEntry(SoundEntry entry)
	{
		SoundEntry[] array = new SoundEntry[this.SoundFile.Length + 1];
		for (int i = 0; i < this.SoundFile.Length; i++)
		{
			array[i] = this.SoundFile[i];
		}
		array[array.Length - 1] = entry;
		this.SoundFile = array;
	}

	// Token: 0x040004E1 RID: 1249
	[Header("自己取的檔案名稱")]
	public string DataName;

	// Token: 0x040004E2 RID: 1250
	[Header("播放時使用的key")]
	public string DataKey;

	// Token: 0x040004E3 RID: 1251
	public AudioMixer AudioMixer;

	// Token: 0x040004E4 RID: 1252
	public SoundEntry[] SoundFile;

	// Token: 0x02000249 RID: 585
	public enum SOUND_TYPE
	{
		// Token: 0x04000B3A RID: 2874
		NONE,
		// Token: 0x04000B3B RID: 2875
		SOUND,
		// Token: 0x04000B3C RID: 2876
		BGM,
		// Token: 0x04000B3D RID: 2877
		VOCAL
	}
}
