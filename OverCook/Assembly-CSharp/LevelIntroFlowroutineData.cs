using System;
using UnityEngine;

// Token: 0x02000681 RID: 1665
[Serializable]
public class LevelIntroFlowroutineData
{
	// Token: 0x04001851 RID: 6225
	public TutorialPopup TutorialPopup = new TutorialPopup();

	// Token: 0x04001852 RID: 6226
	public float ReadyDelay = 1f;

	// Token: 0x04001853 RID: 6227
	public GameObject ReadyUIPrefab;

	// Token: 0x04001854 RID: 6228
	public float ReadyUILifetime = 1f;

	// Token: 0x04001855 RID: 6229
	public GameObject GoUIPrefab;

	// Token: 0x04001856 RID: 6230
	public float GOUILifetime = 1f;
}
