using System;
using UnityEngine;

// Token: 0x02000525 RID: 1317
[ExecutionDependency(typeof(IFlowController))]
[AddComponentMenu("Scripts/Game/Environment/MixingStation")]
[RequireComponent(typeof(AttachStation))]
[RequireComponent(typeof(Flammable))]
public class MixingStation : MonoBehaviour
{
	// Token: 0x040013CA RID: 5066
	[SerializeField]
	public Collider m_itemBlock;

	// Token: 0x040013CB RID: 5067
	public bool m_bAttachRestrictions = true;
}
