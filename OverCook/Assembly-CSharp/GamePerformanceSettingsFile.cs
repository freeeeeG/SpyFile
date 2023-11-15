using System;
using UnityEngine;

// Token: 0x02000798 RID: 1944
public class GamePerformanceSettingsFile : ScriptableObject
{
	// Token: 0x0600258F RID: 9615 RVA: 0x000B1BA0 File Offset: 0x000AFFA0
	public GamePeformanceSettings FindPerformanceSettings()
	{
		int qualityLevel = QualitySettings.GetQualityLevel();
		return this.SettingsForLevel.TryAtIndex(qualityLevel);
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x000B1BBF File Offset: 0x000AFFBF
	public static string GetArrayName(int _index)
	{
		return QualitySettings.names.TryAtIndex(_index);
	}

	// Token: 0x04001D24 RID: 7460
	[ArrayNames("GetArrayName")]
	public GamePeformanceSettings[] SettingsForLevel;
}
