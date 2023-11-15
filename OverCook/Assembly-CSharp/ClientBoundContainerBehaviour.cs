using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class ClientBoundContainerBehaviour : ClientSynchroniserBase
{
	// Token: 0x060004A9 RID: 1193 RVA: 0x00027E6D File Offset: 0x0002626D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_boundContainer = (BoundContainer)synchronisedObject;
	}

	// Token: 0x04000417 RID: 1047
	private BoundContainer m_boundContainer;
}
