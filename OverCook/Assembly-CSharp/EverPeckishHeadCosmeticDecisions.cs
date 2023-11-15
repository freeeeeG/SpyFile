using System;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
[AddComponentMenu("Scripts/Game/StoryLevel/CoopLevel1/EverPeckishHeadCosmeticDecisions")]
public class EverPeckishHeadCosmeticDecisions : MeshVisibilityBase<EverPeckishHeadCosmeticDecisions.VisState>
{
	// Token: 0x040029DE RID: 10718
	[SerializeField]
	public EverPeckishHeadCosmeticDecisions.VisState m_initialVisState = EverPeckishHeadCosmeticDecisions.VisState.MouthClosed;

	// Token: 0x02000A52 RID: 2642
	public enum VisState
	{
		// Token: 0x040029E0 RID: 10720
		MouthOpen,
		// Token: 0x040029E1 RID: 10721
		MouthClosed,
		// Token: 0x040029E2 RID: 10722
		HeadExploded
	}
}
