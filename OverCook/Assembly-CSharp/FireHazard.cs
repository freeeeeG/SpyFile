using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
[RequireComponent(typeof(Flammable))]
[RequireComponent(typeof(CapsuleCollider))]
public class FireHazard : HazardBase
{
	// Token: 0x04001095 RID: 4245
	[SerializeField]
	public float m_lifetime;

	// Token: 0x04001096 RID: 4246
	[SerializeField]
	public float m_destroyTime;
}
