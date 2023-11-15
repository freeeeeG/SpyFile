using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour
{
	// Token: 0x040005E2 RID: 1506
	[SerializeField]
	public string m_onOccupationTrigger;

	// Token: 0x040005E3 RID: 1507
	[SerializeField]
	public string m_onDeoccupationTrigger;

	// Token: 0x040005E4 RID: 1508
	[SerializeField]
	public bool m_fallPad;
}
