using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000918 RID: 2328
	public class PhysicsObjectSynchroniser : MonoBehaviour
	{
		// Token: 0x06002DA2 RID: 11682 RVA: 0x000D8B29 File Offset: 0x000D6F29
		public void SetPhysicalAttachment(PhysicalAttachment _physicalAttachment)
		{
			this.m_PhysicalAttachment = _physicalAttachment;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000D8B32 File Offset: 0x000D6F32
		public PhysicalAttachment GetPhysicalAttachment()
		{
			return this.m_PhysicalAttachment;
		}

		// Token: 0x040024A2 RID: 9378
		private PhysicalAttachment m_PhysicalAttachment;
	}
}
