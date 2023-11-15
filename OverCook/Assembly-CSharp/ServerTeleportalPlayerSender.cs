using System;
using System.Collections;

// Token: 0x020005D3 RID: 1491
public class ServerTeleportalPlayerSender : ServerBaseTeleportalSender
{
	// Token: 0x06001C75 RID: 7285 RVA: 0x0008AFD7 File Offset: 0x000893D7
	protected override void Awake()
	{
		base.Awake();
		this.m_playerSender = base.gameObject.RequireComponent<TeleportalPlayerSender>();
		this.m_playerSender.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x0008B007 File Offset: 0x00089407
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		return _object is ServerTeleportablePlayer;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x0008B014 File Offset: 0x00089414
	protected override IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		this.m_teleportAnimationFinished = false;
		while (!this.m_teleportAnimationFinished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x0008B02F File Offset: 0x0008942F
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Teleport")
		{
			this.m_teleportAnimationFinished = true;
		}
	}

	// Token: 0x04001642 RID: 5698
	private TeleportalPlayerSender m_playerSender;

	// Token: 0x04001643 RID: 5699
	private bool m_teleportAnimationFinished;
}
