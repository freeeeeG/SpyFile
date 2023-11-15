using System;
using System.Collections;

// Token: 0x02000BD0 RID: 3024
public class ServerTeleportalMapAvatarSender : ServerBaseTeleportalSender
{
	// Token: 0x06003DD5 RID: 15829 RVA: 0x00126F9B File Offset: 0x0012539B
	protected override void Awake()
	{
		base.Awake();
		this.m_mapAvatarSender = base.gameObject.RequireComponent<TeleportalMapAvatarSender>();
		this.m_mapAvatarSender.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00126FCB File Offset: 0x001253CB
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		return _object is ServerTeleportableMapAvatar;
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00126FD8 File Offset: 0x001253D8
	protected override IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		this.m_teleportAnimationFinished = false;
		while (!this.m_teleportAnimationFinished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00126FF3 File Offset: 0x001253F3
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Teleport")
		{
			this.m_teleportAnimationFinished = true;
		}
	}

	// Token: 0x0400319E RID: 12702
	private TeleportalMapAvatarSender m_mapAvatarSender;

	// Token: 0x0400319F RID: 12703
	private bool m_teleportAnimationFinished;
}
