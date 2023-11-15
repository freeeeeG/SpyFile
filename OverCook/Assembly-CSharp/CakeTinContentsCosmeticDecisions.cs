using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class CakeTinContentsCosmeticDecisions : AnimationInspectionBase
{
	// Token: 0x04000D46 RID: 3398
	[SerializeField]
	public GameObject m_gameObject;

	// Token: 0x04000D47 RID: 3399
	[SerializeField]
	public GameObject m_contentsObject;

	// Token: 0x04000D48 RID: 3400
	[SerializeField]
	public string m_surfaceMaterialName;

	// Token: 0x04000D49 RID: 3401
	[SerializeField]
	public string m_bubbleMaterialName;

	// Token: 0x04000D4A RID: 3402
	[SerializeField]
	public float m_contentsYPositionWhenFull = 0.2f;

	// Token: 0x04000D4B RID: 3403
	[SerializeField]
	public float m_contentsYPositionWhenEmpty;
}
