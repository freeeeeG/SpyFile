using System;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public class SprayingUtensil : MonoBehaviour
{
	// Token: 0x040016DC RID: 5852
	[SerializeField]
	public string m_startSprayTrigger;

	// Token: 0x040016DD RID: 5853
	[SerializeField]
	public string m_stopSprayTrigger;

	// Token: 0x040016DE RID: 5854
	[SerializeField]
	public GameObject m_sprayEffectPrefab;

	// Token: 0x040016DF RID: 5855
	[SerializeField]
	public Transform m_effectAttachPoint;

	// Token: 0x040016E0 RID: 5856
	[SerializeField]
	public float m_sprayAngleInDegrees = 15f;

	// Token: 0x040016E1 RID: 5857
	[SerializeField]
	public float m_sprayDistance = 4f;

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	public GameLoopingAudioTag m_audioTag = GameLoopingAudioTag.ExtinguisherSpray;

	// Token: 0x040016E3 RID: 5859
	[SerializeField]
	public LayerMask m_CollisionLayerMask = -1;

	// Token: 0x040016E4 RID: 5860
	public const float c_itemRadius = 0.6f;
}
