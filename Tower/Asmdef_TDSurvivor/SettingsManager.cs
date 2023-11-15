using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class SettingsManager : Singleton<SettingsManager>
{
	// Token: 0x06000519 RID: 1305 RVA: 0x00014A04 File Offset: 0x00012C04
	private void Start()
	{
		if (!this.isInitialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00014A14 File Offset: 0x00012C14
	private void Initialize()
	{
		GameSettingData[] array = Resources.LoadAll<GameSettingData>("");
		this.list_GameSettingData = new List<GameSettingData>();
		foreach (GameSettingData gameSettingData in array)
		{
			this.list_GameSettingData.Add(gameSettingData);
			gameSettingData.LoadSetting();
		}
	}

	// Token: 0x040004D3 RID: 1235
	[SerializeField]
	[Header("設定資料")]
	private List<GameSettingData> list_GameSettingData;

	// Token: 0x040004D4 RID: 1236
	private bool isInitialized;
}
