using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
[CreateAssetMenu(menuName = "遊戲設定/SoundVolume", fileName = "GameSettingData_SoundVolume")]
public class GameSettingData_SoundVolume : GameSettingData
{
	// Token: 0x060002DA RID: 730 RVA: 0x0000B896 File Offset: 0x00009A96
	private void Reset()
	{
		this.type = eSettingItemType.AUDIO_SOUND_VOLUME;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000B8A0 File Offset: 0x00009AA0
	protected override void ApplySettingToGame()
	{
		float soundLevel = Mathf.Clamp01((float)this.settingValue / 100f);
		SoundManager.SetVolume(SoundAssetData.SOUND_TYPE.SOUND, soundLevel);
	}
}
