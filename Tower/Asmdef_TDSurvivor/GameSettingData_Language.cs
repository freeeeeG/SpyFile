using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[CreateAssetMenu(menuName = "遊戲設定/Language", fileName = "GameSettingData_Language")]
public class GameSettingData_Language : GameSettingData
{
	// Token: 0x060002CB RID: 715 RVA: 0x0000B71D File Offset: 0x0000991D
	private void Reset()
	{
		this.type = eSettingItemType.GAME_LANGUAGE;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000B728 File Offset: 0x00009928
	protected override void ApplySettingToGame()
	{
		switch (this.settingValue)
		{
		case 0:
			LocalizationManager.Instance.SetLanguage(SystemLanguage.English);
			return;
		case 1:
			LocalizationManager.Instance.SetLanguage(SystemLanguage.ChineseTraditional);
			return;
		case 2:
			LocalizationManager.Instance.SetLanguage(SystemLanguage.ChineseSimplified);
			return;
		case 3:
			LocalizationManager.Instance.SetLanguage(SystemLanguage.Japanese);
			return;
		default:
			return;
		}
	}
}
