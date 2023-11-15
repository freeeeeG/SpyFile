using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
[CreateAssetMenu(menuName = "遊戲設定/Resolution", fileName = "GameSettingData_Resolution")]
public class GameSettingData_Resolution : GameSettingData
{
	// Token: 0x060002D7 RID: 727 RVA: 0x0000B85F File Offset: 0x00009A5F
	private void Reset()
	{
		this.type = eSettingItemType.VIDEO_DISPLAY_MODE;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000B868 File Offset: 0x00009A68
	protected override void ApplySettingToGame()
	{
		switch (this.settingValue)
		{
		default:
			return;
		}
	}
}
