using System;
using UnityEngine;

// Token: 0x02000381 RID: 897
public class BellowsCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000D24 RID: 3364
	[SerializeField]
	public BellowsCosmeticDecisions.AnimParams m_animParams;

	// Token: 0x04000D25 RID: 3365
	[SerializeField]
	public RuntimeAnimatorController m_animController;

	// Token: 0x04000D26 RID: 3366
	[SerializeField]
	public Transform m_animTransform;

	// Token: 0x02000382 RID: 898
	[Serializable]
	public struct AnimParams
	{
		// Token: 0x04000D27 RID: 3367
		[SerializeField]
		public string Used;

		// Token: 0x04000D28 RID: 3368
		[SerializeField]
		public string InUse;

		// Token: 0x04000D29 RID: 3369
		[SerializeField]
		public string Stop;
	}
}
