using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class MixingHandler : MonoBehaviour
{
	// Token: 0x06001586 RID: 5510 RVA: 0x0007472D File Offset: 0x00072B2D
	public MixedCompositeOrderNode.MixingProgress GetMixedOrderState(float _mixingProgress)
	{
		if (_mixingProgress > 2f * this.m_mixingTime)
		{
			return MixedCompositeOrderNode.MixingProgress.OverMixed;
		}
		if (_mixingProgress >= this.m_mixingTime)
		{
			return MixedCompositeOrderNode.MixingProgress.Mixed;
		}
		return MixedCompositeOrderNode.MixingProgress.Unmixed;
	}

	// Token: 0x04001065 RID: 4197
	[SerializeField]
	public float m_mixingTime = 10f;

	// Token: 0x04001066 RID: 4198
	[SerializeField]
	public CookingStepData m_mixingType;

	// Token: 0x04001067 RID: 4199
	[SerializeField]
	public CookingUIController m_progressUIPrefab;
}
