using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000440 RID: 1088
public abstract class ServerBaseTeleportalSender : ServerSynchroniserBase, ITeleportalSender
{
	// Token: 0x06001413 RID: 5139 RVA: 0x0006DD91 File Offset: 0x0006C191
	protected virtual void Awake()
	{
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0006DD93 File Offset: 0x0006C193
	public bool CanTeleport(ITeleportable _object)
	{
		return !this.m_sending && this.CanHandleTeleport(_object);
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0006DDAA File Offset: 0x0006C1AA
	public bool IsSending()
	{
		return this.m_sending;
	}

	// Token: 0x06001416 RID: 5142
	public abstract bool CanHandleTeleport(ITeleportable _object);

	// Token: 0x06001417 RID: 5143
	protected abstract IEnumerator TeleportRoutine(ServerTeleportal _exitPortal, ITeleportalReceiver _receiver, ITeleportable _object);

	// Token: 0x06001418 RID: 5144 RVA: 0x0006DDB4 File Offset: 0x0006C1B4
	public IEnumerator TeleportFromMe(ServerTeleportal _exitPortal, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		this.m_sending = true;
		IEnumerator routine = this.TeleportRoutine(_exitPortal, _receiver, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_sending = false;
		yield break;
	}

	// Token: 0x04000F82 RID: 3970
	private BaseTeleportalSender m_sender;

	// Token: 0x04000F83 RID: 3971
	protected bool m_sending;
}
