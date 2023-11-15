using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
[CreateAssetMenu(menuName = "遊戲設定/MusicVolume", fileName = "GameSettingData_MusicVolume")]
public class GameSettingData_MusicVolume : GameSettingData
{
	// Token: 0x060002D4 RID: 724 RVA: 0x0000B825 File Offset: 0x00009A25
	private void Reset()
	{
		this.type = eSettingItemType.AUDIO_MUSIC_VOLUME;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000B830 File Offset: 0x00009A30
	protected override void ApplySettingToGame()
	{
		float soundLevel = Mathf.Clamp01((float)this.settingValue / 100f);
		SoundManager.SetVolume(SoundAssetData.SOUND_TYPE.BGM, soundLevel);
	}
}
