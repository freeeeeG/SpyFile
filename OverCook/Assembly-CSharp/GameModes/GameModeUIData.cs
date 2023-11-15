using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x0200069E RID: 1694
	[CreateAssetMenu(fileName = "GameModeUIData", menuName = "Team17/Game Mode/Frontend Data")]
	public class GameModeUIData : ScriptableObject
	{
		// Token: 0x040018CB RID: 6347
		[SerializeField]
		public ModeUIData[] m_gameModes = new ModeUIData[3];

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		public ModeSettingUIData[] m_gameModeSettings = new ModeSettingUIData[3];
	}
}
