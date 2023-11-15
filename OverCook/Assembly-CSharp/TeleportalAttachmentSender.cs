using System;
using UnityEngine;

// Token: 0x020005CF RID: 1487
[RequireComponent(typeof(Teleportal))]
public class TeleportalAttachmentSender : BaseTeleportalSender
{
	// Token: 0x06001C5F RID: 7263 RVA: 0x0008A87A File Offset: 0x00088C7A
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x0008A893 File Offset: 0x00088C93
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x0008A8AC File Offset: 0x00088CAC
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x0400162C RID: 5676
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};

	// Token: 0x0400162D RID: 5677
	public ManualAnimation m_TeleportAnimation;
}
