using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[CreateAssetMenu(menuName = "遊戲設定/DisplayMode", fileName = "GameSettingData_DisplayMode")]
public class GameSettingData_DisplayMode : GameSettingData
{
	// Token: 0x060002C8 RID: 712 RVA: 0x0000B6E3 File Offset: 0x000098E3
	private void Reset()
	{
		this.type = eSettingItemType.VIDEO_DISPLAY_MODE;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000B6EC File Offset: 0x000098EC
	protected override void ApplySettingToGame()
	{
		int settingValue = this.settingValue;
		if (settingValue == 0)
		{
			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
			return;
		}
		if (settingValue != 1)
		{
			return;
		}
		Screen.fullScreenMode = FullScreenMode.Windowed;
	}
}
