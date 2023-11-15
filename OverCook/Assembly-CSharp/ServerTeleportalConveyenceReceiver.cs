using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200059C RID: 1436
public class ServerTeleportalConveyenceReceiver : ServerSynchroniserBase, IConveyenceReceiver
{
	// Token: 0x06001B4D RID: 6989 RVA: 0x00087785 File Offset: 0x00085B85
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_teleportalConveyenceReceiver = (TeleportalConveyenceReceiver)synchronisedObject;
		this.m_portal = base.gameObject.RequireComponent<ServerTeleportal>();
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000877AB File Offset: 0x00085BAB
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000877B3 File Offset: 0x00085BB3
	public void InformStartingConveyToMe()
	{
		this.m_receiving = true;
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000877BC File Offset: 0x00085BBC
	public void InformEndingConveyToMe()
	{
		this.m_receiving = false;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000877C8 File Offset: 0x00085BC8
	public IEnumerator ConveyToMe(ServerConveyorStation _priorConveyor, IAttachment _object)
	{
		ITeleportable teleportable = _object.AccessGameObject().RequestInterface<ITeleportable>();
		ServerAttachStation attachStation = _priorConveyor.gameObject.RequireComponent<ServerAttachStation>();
		Transform attachPoint = attachStation.GetAttachPoint(_object.AccessGameObject());
		Transform teleportPoint = this.m_teleportalConveyenceReceiver.m_attachPoint;
		float progress = 0f;
		float speed = _priorConveyor.GetConveySpeed();
		Vector3 startPos = _object.AccessGameObject().transform.localPosition;
		do
		{
			float dt = TimeManager.GetDeltaTime(base.gameObject);
			progress = Mathf.Clamp01(progress + dt * speed);
			Vector3 endPos = attachPoint.InverseTransformPoint(teleportPoint.position);
			endPos.y = startPos.y;
			_object.AccessGameObject().transform.localPosition = Vector3.Lerp(startPos, endPos, progress);
			yield return null;
		}
		while (progress < 1f);
		_priorConveyor.TakeResponsibilityForItem();
		this.m_portal.Teleport(teleportable);
		yield break;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000877F4 File Offset: 0x00085BF4
	public bool CanConveyTo(IAttachment _itemToConvey)
	{
		if (!this.m_receiving && _itemToConvey != null)
		{
			ITeleportable teleportable = _itemToConvey.AccessGameObject().RequestInterface<ITeleportable>();
			if (teleportable != null)
			{
				return this.m_portal.CanTeleport(teleportable);
			}
		}
		return false;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x00087832 File Offset: 0x00085C32
	public void RegisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x00087834 File Offset: 0x00085C34
	public void UnregisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x00087836 File Offset: 0x00085C36
	public void RefreshConveyTo()
	{
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x00087838 File Offset: 0x00085C38
	public void RegisterAllowConveyToCallback(Generic<bool> _callback)
	{
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x0008783A File Offset: 0x00085C3A
	public void UnregisterAllowConveyToCallback(Generic<bool> _callback)
	{
	}

	// Token: 0x04001571 RID: 5489
	private TeleportalConveyenceReceiver m_teleportalConveyenceReceiver;

	// Token: 0x04001572 RID: 5490
	private ServerTeleportal m_portal;

	// Token: 0x04001573 RID: 5491
	private bool m_receiving;
}
