using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
[CreateAssetMenu(menuName = "遊戲設定/MasterVolume", fileName = "GameSettingData_MasterVolume")]
public class GameSettingData_MasterVolume : GameSettingData
{
	// Token: 0x060002CE RID: 718 RVA: 0x0000B78E File Offset: 0x0000998E
	private void Reset()
	{
		this.type = eSettingItemType.AUDIO_MASTER_VOLUME;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000B797 File Offset: 0x00009997
	protected override void ApplySettingToGame()
	{
		SoundManager.SetMasterVolume(Mathf.Clamp01((float)this.settingValue / 100f));
	}
}
