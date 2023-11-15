using System;
using UnityEngine;

// Token: 0x02000688 RID: 1672
[AddComponentMenu("Scripts/Game/Flow/FrontEndFlow")]
public class FrontEndFlow : MonoBehaviour
{
	// Token: 0x04001875 RID: 6261
	[SerializeField]
	private SceneDirectoryData m_sceneDirectory;

	// Token: 0x04001876 RID: 6262
	[SerializeField]
	private float m_kerchunkInterval = 0.12f;

	// Token: 0x04001877 RID: 6263
	[SerializeField]
	private FrontendGUI m_frontendGUI;

	// Token: 0x04001878 RID: 6264
	[SerializeField]
	private GameObject m_gameSessionPrefab;

	// Token: 0x04001879 RID: 6265
	private FrontEndFlow.Screen[] m_screens = new FrontEndFlow.Screen[0];

	// Token: 0x02000689 RID: 1673
	[Serializable]
	public class Screen
	{
		// Token: 0x0400187A RID: 6266
		public string ScreenName = string.Empty;

		// Token: 0x0400187B RID: 6267
		public int PlayerCount = -1;

		// Token: 0x0400187C RID: 6268
		public FrontEndFlow.SceneInfo[] Scenes = new FrontEndFlow.SceneInfo[0];
	}

	// Token: 0x0200068A RID: 1674
	[Serializable]
	public class SceneInfo
	{
		// Token: 0x0400187D RID: 6269
		public SceneDirectoryData.SceneDirectoryEntry SceneDirectoryData;

		// Token: 0x0400187E RID: 6270
		[HideInInspector]
		public bool Completed;

		// Token: 0x0400187F RID: 6271
		[HideInInspector]
		public bool Unlocked;
	}
}
