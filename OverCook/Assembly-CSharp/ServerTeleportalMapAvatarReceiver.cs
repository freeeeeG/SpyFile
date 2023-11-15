using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BCD RID: 3021
public class ServerTeleportalMapAvatarReceiver : ServerBaseTeleportalReceiver
{
	// Token: 0x06003DC3 RID: 15811 RVA: 0x001266E5 File Offset: 0x00124AE5
	protected override void Awake()
	{
		base.Awake();
		this.m_mapAvatarReceiver = base.gameObject.RequireComponent<TeleportalMapAvatarReceiver>();
		this.m_mapAvatarReceiver.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x00126715 File Offset: 0x00124B15
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		return _object is ServerTeleportableMapAvatar;
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x00126720 File Offset: 0x00124B20
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
			MapAvatarGroundCast mapAvatarGroundCast = gameObject.RequestComponent<MapAvatarGroundCast>();
			if (mapAvatarGroundCast != null)
			{
				mapAvatarGroundCast.ForceUpdateNow();
			}
			ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = gameObject.RequestComponent<ServerWorldObjectSynchroniser>();
			if (serverWorldObjectSynchroniser != null)
			{
				serverWorldObjectSynchroniser.ResumeAllClients(true);
			}
		}
		yield break;
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x00126742 File Offset: 0x00124B42
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Receive")
		{
			this.m_teleportAnimationFinished = true;
		}
	}

	// Token: 0x0400318B RID: 12683
	private TeleportalMapAvatarReceiver m_mapAvatarReceiver;

	// Token: 0x0400318C RID: 12684
	private bool m_teleportAnimationFinished;

	// Token: 0x0400318D RID: 12685
	private Animator m_animator;
}
