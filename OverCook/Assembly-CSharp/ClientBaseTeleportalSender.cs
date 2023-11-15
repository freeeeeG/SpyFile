using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000441 RID: 1089
public abstract class ClientBaseTeleportalSender : ClientSynchroniserBase, IClientTeleportalSender
{
	// Token: 0x0600141A RID: 5146 RVA: 0x0006DEBE File Offset: 0x0006C2BE
	protected virtual void Awake()
	{
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x0006DEC0 File Offset: 0x0006C2C0
	public bool IsSending()
	{
		return this.m_sending;
	}

	// Token: 0x0600141C RID: 5148
	protected abstract IEnumerator TeleportRoutine(ClientTeleportal _exitPortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object);

	// Token: 0x0600141D RID: 5149 RVA: 0x0006DEC8 File Offset: 0x0006C2C8
	public IEnumerator TeleportFromMe(ClientTeleportal _exitPortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object)
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

	// Token: 0x04000F84 RID: 3972
	private BaseTeleportalSender m_sender;

	// Token: 0x04000F85 RID: 3973
	protected bool m_sending;
}
