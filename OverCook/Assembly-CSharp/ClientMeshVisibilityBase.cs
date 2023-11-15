using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200014A RID: 330
public abstract class ClientMeshVisibilityBase<StateEnum> : ClientSynchroniserBase where StateEnum : struct, IConvertible
{
	// Token: 0x060005E0 RID: 1504 RVA: 0x0002B5ED File Offset: 0x000299ED
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_visibility = (MeshVisibilityBase<StateEnum>)synchronisedObject;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002B602 File Offset: 0x00029A02
	protected void Setup(StateEnum _state)
	{
		if (this.m_visibility != null)
		{
			this.m_visibility.Setup(_state);
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002B621 File Offset: 0x00029A21
	protected void SetState(StateEnum _state)
	{
		if (this.m_visibility != null)
		{
			this.m_visibility.Setup(_state);
		}
	}

	// Token: 0x040004F1 RID: 1265
	public MeshVisibilityBase<StateEnum> m_visibility;
}
