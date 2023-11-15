using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005CD RID: 1485
public class ServerTeleportalAttachmentSender : ServerBaseTeleportalSender, IHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001C50 RID: 7248 RVA: 0x0008A332 File Offset: 0x00088732
	protected override void Awake()
	{
		base.Awake();
		this.m_attachmentSender = base.gameObject.RequireComponent<TeleportalAttachmentSender>();
		this.m_attachmentSender.RegisterAnimationFinishedCallback(new GenericVoid<string>(this.OnAnimationFinished));
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x0008A364 File Offset: 0x00088764
	public override bool CanHandleTeleport(ITeleportable _object)
	{
		if (_object is ServerTeleportableItem)
		{
			ServerTeleportableItem serverTeleportableItem = (ServerTeleportableItem)_object;
			if (serverTeleportableItem != null)
			{
				IAttachment attachment = serverTeleportableItem.gameObject.RequestInterface<IAttachment>();
				return attachment != null && (!attachment.IsAttached() || attachment.AccessGameObject().RequestInterfaceUpwardsRecursive<IGridLocation>() != null);
			}
		}
		return false;
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x0008A3C8 File Offset: 0x000887C8
	protected override IEnumerator TeleportRoutine(ServerTeleportal _entrancePortal, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		GameObject obj = ((MonoBehaviour)_object).gameObject;
		ServerHandlePickupReferral pickupReferral = obj.RequireComponent<ServerHandlePickupReferral>();
		pickupReferral.SetHandlePickupReferree(this);
		ServerLimitedQuantityItem limitedQuantity = obj.RequestComponent<ServerLimitedQuantityItem>();
		if (null != limitedQuantity)
		{
			limitedQuantity.AddInvincibilityCondition(this.m_true);
		}
		Vector3 pos = obj.transform.position;
		Quaternion rot = obj.transform.rotation;
		IAttachment attachment = obj.RequestInterface<IAttachment>();
		attachment.Attach(this.m_attachmentSender);
		obj.transform.SetParent(this.m_attachmentSender.GetAttachPoint(obj), true);
		obj.transform.SetPositionAndRotation(pos, rot);
		this.m_teleportAnimationFinished = false;
		while (!this.m_teleportAnimationFinished)
		{
			yield return null;
		}
		pickupReferral.SetHandlePickupReferree(null);
		attachment.Detach();
		if (null != limitedQuantity)
		{
			limitedQuantity.RemoveInvincibilityCondition(this.m_true);
			limitedQuantity.Touch();
		}
		yield break;
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x0008A3EA File Offset: 0x000887EA
	public void OnAnimationFinished(string _animName)
	{
		if (_animName == "Teleport")
		{
			this.m_teleportAnimationFinished = true;
		}
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x0008A403 File Offset: 0x00088803
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return false;
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x0008A406 File Offset: 0x00088806
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x0008A408 File Offset: 0x00088808
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x04001624 RID: 5668
	private TeleportalAttachmentSender m_attachmentSender;

	// Token: 0x04001625 RID: 5669
	private bool m_teleportAnimationFinished;

	// Token: 0x04001626 RID: 5670
	private Generic<bool> m_true = () => true;
}
