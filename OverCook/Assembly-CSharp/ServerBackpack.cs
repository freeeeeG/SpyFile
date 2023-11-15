using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class ServerBackpack : ServerCarryableItem, IHandleAttachTarget
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x0008DE05 File Offset: 0x0008C205
	public override PlayerAttachTarget PlayerAttachTarget
	{
		get
		{
			return PlayerAttachTarget.Back;
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0008DE08 File Offset: 0x0008C208
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpack = (synchronisedObject as Backpack);
		this.m_pickupReferral = base.gameObject.RequireComponent<ServerHandlePickupReferral>();
		this.m_pickupReferral.RegisterAllowReferralBlock(new Generic<bool, ICarrier>(this.CanBlockReferral));
		this.m_attachment = base.gameObject.RequireInterface<IAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		this.m_backpackDispenser = base.gameObject.RequireComponent<ServerBackpackDispenser>();
		this.m_physicalAttachment = base.gameObject.RequireComponent<ServerPhysicalAttachment>();
		Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0008DEB2 File Offset: 0x0008C2B2
	public override void StopSynchronising()
	{
		base.StopSynchronising();
		Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x0008DED4 File Offset: 0x0008C2D4
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_physicalAttachment.ManualEnable();
		}
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x0008DF00 File Offset: 0x0008C300
	public bool CanBlockReferral(ICarrier _carrier)
	{
		return !this.m_attachment.IsAttached();
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x0008DF10 File Offset: 0x0008C310
	public bool CanHandleDispenserPickup(ICarrier _carrier)
	{
		return this.m_backpack.CanHandleDispenserPickup(_carrier);
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x0008DF20 File Offset: 0x0008C320
	private void OnAttachmentChanged(IParentable _parentable)
	{
		MonoBehaviour monoBehaviour = _parentable as MonoBehaviour;
		if (monoBehaviour != null && monoBehaviour.gameObject.RequestInterface<ICarrier>() != null)
		{
			this.m_pickupReferral.SetHandlePickupReferree(this.m_backpackDispenser);
		}
		else
		{
			this.m_pickupReferral.SetHandlePickupReferree(null);
		}
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x0008DF74 File Offset: 0x0008C374
	public override void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		ServerPlayerAttachmentCarrier serverPlayerAttachmentCarrier = _carrier.AccessGameObject().RequestComponentRecursive<ServerPlayerAttachmentCarrier>();
		if (serverPlayerAttachmentCarrier != null && serverPlayerAttachmentCarrier.InspectCarriedItem(this.PlayerAttachTarget) == null)
		{
			IAttachment component = base.gameObject.GetComponent<IAttachment>();
			if (component.IsAttached())
			{
				component.Detach();
			}
			serverPlayerAttachmentCarrier.CarryItem(component.AccessGameObject());
		}
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x0008DFDC File Offset: 0x0008C3DC
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachment != null)
		{
			this.m_attachment.UnregisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		}
		if (this.m_pickupReferral != null)
		{
			this.m_pickupReferral.UnregisterAllowReferralBlock(new Generic<bool, ICarrier>(this.CanBlockReferral));
		}
	}

	// Token: 0x0400167C RID: 5756
	private Backpack m_backpack;

	// Token: 0x0400167D RID: 5757
	private ServerHandlePickupReferral m_pickupReferral;

	// Token: 0x0400167E RID: 5758
	private IAttachment m_attachment;

	// Token: 0x0400167F RID: 5759
	private ServerBackpackDispenser m_backpackDispenser;

	// Token: 0x04001680 RID: 5760
	private ServerPhysicalAttachment m_physicalAttachment;
}
