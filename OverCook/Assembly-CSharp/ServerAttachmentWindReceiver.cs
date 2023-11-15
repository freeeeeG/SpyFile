using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
public class ServerAttachmentWindReceiver : ServerSynchroniserBase
{
	// Token: 0x060026E9 RID: 9961 RVA: 0x000B8A00 File Offset: 0x000B6E00
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_receiver = (AttachmentWindReceiver)synchronisedObject;
		this.m_receiver.RegisterVolumeExitedCallback(new VoidGeneric<IWindSource>(this.OnWindVolumeExited));
		this.m_attachment = base.gameObject.RequestComponent<ServerPhysicalAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachChanged));
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x000B8A60 File Offset: 0x000B6E60
	private void FixedUpdate()
	{
		if (this.m_attachment != null && !this.m_attachment.IsAttached())
		{
			Vector3 velocity = this.m_receiver.GetVelocity();
			if (velocity.sqrMagnitude >= 0.05f)
			{
				this.m_attachment.AccessMotion().Movement(velocity);
			}
		}
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x000B8ABC File Offset: 0x000B6EBC
	private void OnAttachChanged(IParentable _parentable)
	{
		base.enabled = (_parentable == null);
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x000B8AC8 File Offset: 0x000B6EC8
	private void OnWindVolumeExited(IWindSource _source)
	{
		if (this.m_attachment != null && this.m_attachment.AccessMotion() != null && this.m_attachment.AccessMotion().gameObject != null)
		{
			Vector3 velocity = this.m_attachment.AccessMotion().GetVelocity() + _source.GetVelocity();
			this.m_attachment.AccessMotion().SetVelocity(velocity);
		}
	}

	// Token: 0x04001EC9 RID: 7881
	private AttachmentWindReceiver m_receiver;

	// Token: 0x04001ECA RID: 7882
	private const float c_MinWindVelocity = 0.05f;

	// Token: 0x04001ECB RID: 7883
	private ServerPhysicalAttachment m_attachment;
}
