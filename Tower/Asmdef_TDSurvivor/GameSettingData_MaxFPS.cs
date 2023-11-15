using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
[CreateAssetMenu(menuName = "遊戲設定/Max FPS", fileName = "GameSettingData_MaxFPS")]
public class GameSettingData_MaxFPS : GameSettingData
{
	// Token: 0x060002D1 RID: 721 RVA: 0x0000B7B8 File Offset: 0x000099B8
	private void Reset()
	{
		this.type = eSettingItemType.VIDEO_MAX_FPS;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0000B7C4 File Offset: 0x000099C4
	protected override void ApplySettingToGame()
	{
		switch (this.settingValue)
		{
		case 0:
			Application.targetFrameRate = 30;
			return;
		case 1:
			Application.targetFrameRate = 60;
			return;
		case 2:
			Application.targetFrameRate = 120;
			return;
		case 3:
			Application.targetFrameRate = 144;
			return;
		}
		Application.targetFrameRate = 0;
	}
}
