using System;

// Token: 0x020000DD RID: 221
public static class eSoundTypeExtensions
{
	// Token: 0x06000540 RID: 1344 RVA: 0x0001516E File Offset: 0x0001336E
	public static string ToNameString(this SoundAssetData.SOUND_TYPE type)
	{
		switch (type)
		{
		case SoundAssetData.SOUND_TYPE.NONE:
			return "無";
		case SoundAssetData.SOUND_TYPE.SOUND:
			return "音效";
		case SoundAssetData.SOUND_TYPE.BGM:
			return "音樂";
		case SoundAssetData.SOUND_TYPE.VOCAL:
			return "語音";
		default:
			return "";
		}
	}
}
