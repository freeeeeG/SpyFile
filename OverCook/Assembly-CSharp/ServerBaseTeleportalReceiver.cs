using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200043D RID: 1085
public abstract class ServerBaseTeleportalReceiver : ServerSynchroniserBase, ITeleportalReceiver
{
	// Token: 0x060013F3 RID: 5107 RVA: 0x0006D948 File Offset: 0x0006BD48
	protected virtual void Awake()
	{
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x0006D94A File Offset: 0x0006BD4A
	public bool CanTeleportTo(ITeleportable _object)
	{
		return !this.m_receiving && !this.m_allowTeleportCallbacks.CallForResult(false) && this.CanHandleTeleport(_object);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x0006D972 File Offset: 0x0006BD72
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x060013F6 RID: 5110
	public abstract bool CanHandleTeleport(ITeleportable _object);

	// Token: 0x060013F7 RID: 5111
	protected abstract IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalSender _sender, ITeleportable _object);

	// Token: 0x060013F8 RID: 5112 RVA: 0x0006D97C File Offset: 0x0006BD7C
	public IEnumerator TeleportToMe(ServerTeleportal _entrancePortal, ITeleportalSender _sender, ITeleportable _object)
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

	// Token: 0x060013F9 RID: 5113 RVA: 0x0006D9AC File Offset: 0x0006BDAC
	public void RegisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallbacks.Add(_callback);
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x0006D9BA File Offset: 0x0006BDBA
	public void UnregisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallbacks.Add(_callback);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x0006D9C8 File Offset: 0x0006BDC8
	public void RegisterStartedTeleportCallback(TeleportCallback _callback)
	{
		this.m_teleportStartedCallback = (TeleportCallback)Delegate.Combine(this.m_teleportStartedCallback, _callback);
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x0006D9E1 File Offset: 0x0006BDE1
	public void UnregisterStartedTeleportCallback(TeleportCallback _callback)
	{
		this.m_teleportStartedCallback = (TeleportCallback)Delegate.Remove(this.m_teleportStartedCallback, _callback);
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x0006D9FA File Offset: 0x0006BDFA
	public void RegisterFinishedTeleportCallback(TeleportCallback _callback)
	{
		this.m_teleportFinishedCallback = (TeleportCallback)Delegate.Combine(this.m_teleportFinishedCallback, _callback);
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x0006DA13 File Offset: 0x0006BE13
	public void UnregisterFinishedTeleportCallback(TeleportCallback _callback)
	{
		this.m_teleportFinishedCallback = (TeleportCallback)Delegate.Remove(this.m_teleportFinishedCallback, _callback);
	}

	// Token: 0x04000F73 RID: 3955
	private BaseTeleportalReceiver m_receiver;

	// Token: 0x04000F74 RID: 3956
	protected bool m_receiving;

	// Token: 0x04000F75 RID: 3957
	private List<Generic<bool>> m_allowTeleportCallbacks = new List<Generic<bool>>();

	// Token: 0x04000F76 RID: 3958
	private TeleportCallback m_teleportStartedCallback = delegate(ITeleportable _object)
	{
	};

	// Token: 0x04000F77 RID: 3959
	private TeleportCallback m_teleportFinishedCallback = delegate(ITeleportable _object)
	{
	};
}
