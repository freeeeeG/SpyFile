using System;
using UnityEngine;

// Token: 0x02000589 RID: 1417
[ExecutionDependency(typeof(IFlowController))]
public class Flammable : MonoBehaviour
{
	// Token: 0x0400154D RID: 5453
	[SerializeField]
	public GameObject m_fireEffectPrefab;

	// Token: 0x0400154E RID: 5454
	[SerializeField]
	public ProgressUIController m_progressUIPrefab;

	// Token: 0x0400154F RID: 5455
	[SerializeField]
	public float m_fireSpreadRadius = 1.5f;

	// Token: 0x04001550 RID: 5456
	[SerializeField]
	public float m_overrideTargetFlammabilityTime;

	// Token: 0x04001551 RID: 5457
	[SerializeField]
	public bool m_overrideTargetFlammability;

	// Token: 0x04001552 RID: 5458
	[SerializeField]
	public bool m_startOnFire;

	// Token: 0x04001553 RID: 5459
	[SerializeField]
	public GameLoopingAudioTag m_audioTag = GameLoopingAudioTag.Flames;
}
