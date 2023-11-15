using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[CreateAssetMenu(menuName = "遊戲設定/Vsync", fileName = "GameSettingData_VSync")]
public class GameSettingData_VSync : GameSettingData
{
	// Token: 0x060002DD RID: 733 RVA: 0x0000B8CF File Offset: 0x00009ACF
	private void Reset()
	{
		this.type = eSettingItemType.VIDEO_VSYNC;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000B8D8 File Offset: 0x00009AD8
	protected override void ApplySettingToGame()
	{
		QualitySettings.vSyncCount = this.settingValue;
	}
}
