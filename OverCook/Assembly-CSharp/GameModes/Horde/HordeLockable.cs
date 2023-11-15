using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameModes.Horde
{
	// Token: 0x020007D3 RID: 2003
	[RequireComponent(typeof(HordeLockable))]
	public class HordeLockable : MonoBehaviour
	{
		// Token: 0x04001E8B RID: 7819
		[FormerlySerializedAs("m_cost")]
		[SerializeField]
		public int m_unlockCost = 50;

		// Token: 0x04001E8C RID: 7820
		[SerializeField]
		public Collider m_collider;

		// Token: 0x04001E8D RID: 7821
		[SerializeField]
		public HordeLockable[] m_lockables;
	}
}
