using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
[CreateAssetMenu(menuName = "遊戲設定/WindowResolution", fileName = "GameSettingData_WindowResolution")]
public class GameSettingData_WindowResolution : GameSettingData
{
	// Token: 0x060002E0 RID: 736 RVA: 0x0000B8ED File Offset: 0x00009AED
	private void Reset()
	{
		this.type = eSettingItemType.VIDEO_RESOLUTION;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000B8F8 File Offset: 0x00009AF8
	protected override void ApplySettingToGame()
	{
		switch (this.settingValue)
		{
		case 0:
			this.SetResolution(640, 480);
			return;
		case 1:
			this.SetResolution(800, 600);
			return;
		case 2:
			this.SetResolution(1280, 720);
			return;
		case 3:
			this.SetResolution(1920, 1080);
			return;
		case 4:
			this.SetResolution(2560, 1080);
			return;
		case 5:
			this.SetResolution(2560, 1440);
			return;
		case 6:
			this.SetResolution(3440, 1440);
			return;
		default:
			return;
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000B9A5 File Offset: 0x00009BA5
	private void SetResolution(int width, int height)
	{
		Screen.SetResolution(width, height, Screen.fullScreenMode);
	}
}
