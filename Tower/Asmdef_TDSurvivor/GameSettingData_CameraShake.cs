using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
[CreateAssetMenu(menuName = "遊戲設定/CameraShake", fileName = "GameSettingData_CameraShake")]
public class GameSettingData_CameraShake : GameSettingData
{
	// Token: 0x060002C5 RID: 709 RVA: 0x0000B6D0 File Offset: 0x000098D0
	private void Reset()
	{
		this.type = eSettingItemType.GAME_CAMERA_SHAKE;
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0000B6D9 File Offset: 0x000098D9
	protected override void ApplySettingToGame()
	{
	}
}
