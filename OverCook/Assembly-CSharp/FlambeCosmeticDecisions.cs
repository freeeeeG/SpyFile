using System;
using UnityEngine;

// Token: 0x020003B3 RID: 947
[RequireComponent(typeof(Renderer))]
public class FlambeCosmeticDecisions : FryingContentsCosmeticDecisions
{
	// Token: 0x04000DC7 RID: 3527
	[SerializeField]
	public float m_gradLimit = 2f;

	// Token: 0x04000DC8 RID: 3528
	[SerializeField]
	public float m_timeToMax = 0.1f;

	// Token: 0x04000DC9 RID: 3529
	[SerializeField]
	public string m_materialFloatName = "FlambeProp";
}
