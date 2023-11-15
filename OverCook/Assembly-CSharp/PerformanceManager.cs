using System;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public class PerformanceManager : MonoBehaviour
{
	// Token: 0x04001382 RID: 4994
	[SerializeField]
	private GamePerformanceSettingsFile m_gamePerformanceSettings;

	// Token: 0x04001383 RID: 4995
	[SerializeField]
	private PerformanceManager.ScenePerformanceType m_sceneType;

	// Token: 0x02000516 RID: 1302
	private enum ScenePerformanceType
	{
		// Token: 0x04001385 RID: 4997
		Kitchen,
		// Token: 0x04001386 RID: 4998
		StartScreen
	}
}
