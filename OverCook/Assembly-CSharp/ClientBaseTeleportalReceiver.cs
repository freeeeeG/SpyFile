using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200043E RID: 1086
public abstract class ClientBaseTeleportalReceiver : ClientSynchroniserBase, IClientTeleportalReceiver
{
	// Token: 0x06001402 RID: 5122 RVA: 0x0006DB94 File Offset: 0x0006BF94
	protected virtual void Awake()
	{
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0006DB96 File Offset: 0x0006BF96
	public bool CanTeleportTo(IClientTeleportable _object)
	{
		return !this.m_receiving && !this.m_canTeleportCallbacks.CallForResult(false);
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x0006DBB5 File Offset: 0x0006BFB5
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x06001405 RID: 5125
	protected abstract IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object);

	// Token: 0x06001406 RID: 5126 RVA: 0x0006DBC0 File Offset: 0x0006BFC0
	public IEnumerator TeleportToMe(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object)
	{
		this.m_receiving = true;
		this.m_teleportStartedCallback(_object);
		IEnumerator routine = this.TeleportRoutine(_entrancePortal, _sender, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_teleportFinishedCallback(_object);
		this.m_receiving = false;
		yield break;
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x0006DBF0 File Offset: 0x0006BFF0
	public void RegisterCanTeleportToCallback(Generic<bool> _callback)
	{
		this.m_canTeleportCallbacks.Add(_callback);
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x0006DBFE File Offset: 0x0006BFFE
	public void UnregisterCanTeleportToCallback(Generic<bool> _callback)
	{
		this.m_canTeleportCallbacks.Remove(_callback);
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x0006DC0D File Offset: 0x0006C00D
	public void RegisterStartedTeleportCallback(ClientTeleportCallback _callback)
	{
		this.m_teleportStartedCallback = (ClientTeleportCallback)Delegate.Combine(this.m_teleportStartedCallback, _callback);
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0006DC26 File Offset: 0x0006C026
	public void UnregisterStartedTeleportCallback(ClientTeleportCallback _callback)
	{
		this.m_teleportStartedCallback = (ClientTeleportCallback)Delegate.Remove(this.m_teleportStartedCallback, _callback);
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x0006DC3F File Offset: 0x0006C03F
	public void RegisterFinishedTeleportCallback(ClientTeleportCallback _callback)
	{
		this.m_teleportFinishedCallback = (ClientTeleportCallback)Delegate.Combine(this.m_teleportFinishedCallback, _callback);
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x0006DC58 File Offset: 0x0006C058
	public void UnregisterFinishedTeleportCallback(ClientTeleportCallback _callback)
	{
		this.m_teleportFinishedCallback = (ClientTeleportCallback)Delegate.Remove(this.m_teleportFinishedCallback, _callback);
	}

	// Token: 0x04000F7A RID: 3962
	private TeleportalPlayerReceiver m_receiver;

	// Token: 0x04000F7B RID: 3963
	private bool m_receiving;

	// Token: 0x04000F7C RID: 3964
	private List<Generic<bool>> m_canTeleportCallbacks = new List<Generic<bool>>();

	// Token: 0x04000F7D RID: 3965
	private ClientTeleportCallback m_teleportStartedCallback = delegate(IClientTeleportable _object)
	{
	};

	// Token: 0x04000F7E RID: 3966
	private ClientTeleportCallback m_teleportFinishedCallback = delegate(IClientTeleportable _object)
	{
	};
}
