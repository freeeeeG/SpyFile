using System;
using UnityEngine;

// Token: 0x020005EF RID: 1519
public class ClientBackpack : ClientCarryableItem, IHandleAttachTarget
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x0008E057 File Offset: 0x0008C457
	public override PlayerAttachTarget PlayerAttachTarget
	{
		get
		{
			return PlayerAttachTarget.Back;
		}
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x0008E05C File Offset: 0x0008C45C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpack = (synchronisedObject as Backpack);
		this.m_pickupReferral = base.gameObject.RequireComponent<ClientHandlePickupReferral>();
		this.m_pickupReferral.RegisterAllowReferralBlock(new Generic<bool, ICarrier>(this.CanBlockReferral));
		this.m_attachment = base.gameObject.RequireInterface<IClientAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		this.m_backpackDispenser = base.gameObject.RequireComponent<ClientBackpackDispenser>();
		this.m_layerWhenAttached = LayerMask.NameToLayer("AttachedBackpack");
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x0008E0ED File Offset: 0x0008C4ED
	public bool CanBlockReferral(ICarrier _carrier)
	{
		return !this.m_attachment.IsAttached();
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x0008E0FD File Offset: 0x0008C4FD
	public bool CanHandleDispenserPickup(ICarrier _carrier)
	{
		return this.m_backpack.CanHandleDispenserPickup(_carrier);
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x0008E10C File Offset: 0x0008C50C
	private void OnAttachmentChanged(IParentable _parentable)
	{
		MonoBehaviour monoBehaviour = _parentable as MonoBehaviour;
		if (monoBehaviour != null)
		{
			if (monoBehaviour.gameObject.RequestInterface<ICarrierPlacement>() != null)
			{
				this.m_pickupReferral.SetHandlePickupReferree(this.m_backpackDispenser);
				if (this.m_backpack.m_usesSeparateColliders)
				{
					BoxCollider boxCollider = this.m_backpack.m_collider as BoxCollider;
					boxCollider.center = this.m_backpack.m_carriedColliderCenter;
					boxCollider.size = this.m_backpack.m_carriedColliderSize;
				}
			}
			this.m_previousLayer = base.gameObject.layer;
			base.gameObject.layer = this.m_layerWhenAttached;
		}
		else
		{
			this.m_pickupReferral.SetHandlePickupReferree(null);
			base.gameObject.layer = this.m_previousLayer;
			if (this.m_backpack.m_usesSeparateColliders)
			{
				BoxCollider boxCollider2 = this.m_backpack.m_collider as BoxCollider;
				boxCollider2.center = this.m_backpack.m_restingColliderCenter;
				boxCollider2.size = this.m_backpack.m_restingColliderSize;
			}
		}
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x0008E218 File Offset: 0x0008C618
	protected override void OnDestroy()
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

	// Token: 0x04001681 RID: 5761
	private Backpack m_backpack;

	// Token: 0x04001682 RID: 5762
	private ClientHandlePickupReferral m_pickupReferral;

	// Token: 0x04001683 RID: 5763
	private IClientAttachment m_attachment;

	// Token: 0x04001684 RID: 5764
	private ClientBackpackDispenser m_backpackDispenser;

	// Token: 0x04001685 RID: 5765
	private int m_previousLayer;

	// Token: 0x04001686 RID: 5766
	private int m_layerWhenAttached;
}
