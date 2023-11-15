using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class ClientProjectile : ClientSynchroniserBase
{
	// Token: 0x06001619 RID: 5657 RVA: 0x00075A42 File Offset: 0x00073E42
	public void Start()
	{
		this.m_Rigidbody = base.gameObject.RequestComponentUpwardsRecursive<Rigidbody>();
		this.m_Rigidbody.isKinematic = true;
	}

	// Token: 0x040010B0 RID: 4272
	private Rigidbody m_Rigidbody;
}
