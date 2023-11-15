using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009CB RID: 2507
[RequireComponent(typeof(PlayerAttachmentCarrier))]
public class ServerAttachmentCatcher : ServerSynchroniserBase, IHandleCatch
{
	// Token: 0x0600311F RID: 12575 RVA: 0x000E6767 File Offset: 0x000E4B67
	public override EntityType GetEntityType()
	{
		return EntityType.AttachCatcher;
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x000E676B File Offset: 0x000E4B6B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_catcher = (AttachmentCatcher)synchronisedObject;
		this.m_carrier = base.gameObject.RequireComponent<ServerPlayerAttachmentCarrier>();
		this.m_controls = base.gameObject.RequireComponent<PlayerControls>();
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x000E67A2 File Offset: 0x000E4BA2
	private void SendTrackingData(GameObject _tracked)
	{
		this.m_data.Initialise(_tracked);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x000E67BC File Offset: 0x000E4BBC
	public bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		if (!_object.AllowCatch(this, _directionXZ))
		{
			return false;
		}
		GameObject gameObject = _object.AccessGameObject();
		if (this.m_carrier.InspectCarriedItem() != null)
		{
			return false;
		}
		IThrower thrower = base.gameObject.RequestInterface<IThrower>();
		if (thrower != null)
		{
			IThrowable throwable = gameObject.RequireInterface<IThrowable>();
			if (throwable.GetThrower() == thrower)
			{
				return false;
			}
		}
		float catchDistance = this.m_catcher.m_catchDistance;
		if ((base.transform.position - gameObject.transform.position).sqrMagnitude > catchDistance * catchDistance)
		{
			return false;
		}
		IAttachment attachment = gameObject.RequireInterface<IAttachment>();
		Vector2 from = attachment.AccessMotion().GetVelocity().XZ();
		Vector2 a = base.transform.forward.XZ();
		float num = Vector2.Angle(from, -a);
		return num <= this.m_catcher.m_catchAngleMax;
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x000E68AC File Offset: 0x000E4CAC
	public void HandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		GameObject @object = _object.AccessGameObject();
		this.m_carrier.CarryItem(@object);
		this.m_trackedThrowable = null;
		this.SendTrackingData(this.m_trackedThrowable);
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x000E68E0 File Offset: 0x000E4CE0
	public void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ)
	{
		if (this.m_controls != null && this.m_controls.GetDirectlyUnderPlayerControl())
		{
			return;
		}
		if (this.m_trackedThrowable != null)
		{
			IThrowable throwable = this.m_trackedThrowable.RequireInterface<IThrowable>();
			if (throwable.IsFlying())
			{
				return;
			}
		}
		if (this.m_carrier.InspectCarriedItem() == null && this.m_controls.GetCurrentlyInteracting() == null)
		{
			this.m_trackedThrowable = _thrown.AccessGameObject();
			this.SendTrackingData(this.m_trackedThrowable);
		}
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x000E697C File Offset: 0x000E4D7C
	public int GetCatchingPriority()
	{
		return 0;
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x000E6980 File Offset: 0x000E4D80
	public override void UpdateSynchronising()
	{
		if (this.m_trackedThrowable != null)
		{
			IThrowable throwable = this.m_trackedThrowable.RequireInterface<IThrowable>();
			if (!throwable.IsFlying() || this.m_controls.GetDirectlyUnderPlayerControl())
			{
				this.m_trackedThrowable = null;
				this.SendTrackingData(this.m_trackedThrowable);
			}
		}
	}

	// Token: 0x0400275F RID: 10079
	private AttachmentCatcher m_catcher;

	// Token: 0x04002760 RID: 10080
	private AttachmentCatcherMessage m_data = new AttachmentCatcherMessage();

	// Token: 0x04002761 RID: 10081
	private GameObject m_trackedThrowable;

	// Token: 0x04002762 RID: 10082
	private ServerPlayerAttachmentCarrier m_carrier;

	// Token: 0x04002763 RID: 10083
	private PlayerControls m_controls;
}
