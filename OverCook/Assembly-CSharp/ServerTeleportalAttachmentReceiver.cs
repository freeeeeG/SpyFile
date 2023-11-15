using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class ServerTeleportalAttachmentReceiver : ServerBaseTeleportalReceiver, IHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001C3C RID: 7228 RVA: 0x00089C35 File Offset: 0x00088035
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachmentReceiver = (TeleportalAttachmentReceiver)synchronisedObject;
		this.m_attachmentReceiver.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
		this.m_thrower = base.gameObject.RequireComponent<ServerAttachmentThrower>();
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x00089C72 File Offset: 0x00088072
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		return _object is ServerTeleportableItem;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x00089C80 File Offset: 0x00088080
	protected override IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalSender _sender, ITeleportable _object)
	{
		GameObject obj = ((MonoBehaviour)_object).gameObject;
		ServerHandlePickupReferral pickupReferral = obj.RequireComponent<ServerHandlePickupReferral>();
		pickupReferral.SetHandlePickupReferree(this);
		ServerLimitedQuantityItem limitedQuantity = obj.RequestComponent<ServerLimitedQuantityItem>();
		if (null != limitedQuantity)
		{
			limitedQuantity.AddInvincibilityCondition(this.m_true);
		}
		IAttachment attachment = obj.RequestInterface<IAttachment>();
		attachment.Attach(this.m_attachmentReceiver);
		this.m_teleportAnimationFinished = false;
		this.m_teleportComplete = false;
		while (!this.m_teleportAnimationFinished)
		{
			yield return null;
		}
		this.m_teleportAnimationFinished = false;
		attachment.Detach();
		pickupReferral.SetHandlePickupReferree(null);
		GameObject root = ((MonoBehaviour)_object).gameObject;
		GroundCast groundCast = root.RequestComponent<GroundCast>();
		if (groundCast != null)
		{
			groundCast.ForceUpdateNow();
		}
		ServerWorldObjectSynchroniser synchroniser = root.RequestComponent<ServerWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			synchroniser.ResumeAllClients(true);
		}
		if (null != limitedQuantity)
		{
			limitedQuantity.RemoveInvincibilityCondition(this.m_true);
			limitedQuantity.Touch();
		}
		while (!this.m_teleportComplete)
		{
			yield return null;
		}
		IThrowable throwable = obj.RequestInterface<IThrowable>();
		if (throwable != null)
		{
			Vector2 normalized = this.m_attachmentReceiver.GetAttachPoint(root).forward.XZ().normalized;
			if (throwable.CanHandleThrow(this.m_thrower, normalized))
			{
				throwable.HandleThrow(this.m_thrower, normalized);
			}
		}
		yield break;
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00089CA2 File Offset: 0x000880A2
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Receive")
		{
			this.m_teleportAnimationFinished = true;
		}
		if (_animName == "TeleportComplete")
		{
			this.m_teleportComplete = true;
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00089CD2 File Offset: 0x000880D2
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return false;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00089CD5 File Offset: 0x000880D5
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x00089CD7 File Offset: 0x000880D7
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x04001617 RID: 5655
	private TeleportalAttachmentReceiver m_attachmentReceiver;

	// Token: 0x04001618 RID: 5656
	private Generic<bool> m_true = () => true;

	// Token: 0x04001619 RID: 5657
	private ServerAttachmentThrower m_thrower;

	// Token: 0x0400161A RID: 5658
	private bool m_teleportAnimationFinished;

	// Token: 0x0400161B RID: 5659
	private bool m_teleportComplete = true;
}
