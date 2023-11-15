using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class ServerTeleportalPlayerReceiver : ServerBaseTeleportalReceiver
{
	// Token: 0x06001C64 RID: 7268 RVA: 0x0008A8C4 File Offset: 0x00088CC4
	protected override void Awake()
	{
		base.Awake();
		this.m_playerReceiver = base.gameObject.RequireComponent<TeleportalPlayerReceiver>();
		this.m_playerReceiver.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x0008A8F4 File Offset: 0x00088CF4
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		return _object is ServerTeleportablePlayer;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x0008A900 File Offset: 0x00088D00
	protected override IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalSender _sender, ITeleportable _object)
	{
		this.m_teleportAnimationFinished = false;
		while (!this.m_teleportAnimationFinished)
		{
			yield return null;
		}
		if (_object != null && (MonoBehaviour)_object != null)
		{
			GameObject gameObject = ((MonoBehaviour)_object).gameObject;
			GroundCast groundCast = gameObject.RequestComponent<GroundCast>();
			if (groundCast != null)
			{
				groundCast.ForceUpdateNow();
			}
			ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = gameObject.RequestComponent<ServerWorldObjectSynchroniser>();
			if (serverWorldObjectSynchroniser != null)
			{
				serverWorldObjectSynchroniser.ResumeAllClients(false);
			}
		}
		yield break;
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x0008A922 File Offset: 0x00088D22
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Receive")
		{
			this.m_teleportAnimationFinished = true;
		}
	}

	// Token: 0x0400162F RID: 5679
	private TeleportalPlayerReceiver m_playerReceiver;

	// Token: 0x04001630 RID: 5680
	private bool m_teleportAnimationFinished;

	// Token: 0x04001631 RID: 5681
	private Animator m_animator;
}
