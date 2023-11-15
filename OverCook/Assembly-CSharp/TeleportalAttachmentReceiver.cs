using System;
using UnityEngine;

// Token: 0x020005CC RID: 1484
[RequireComponent(typeof(AttachmentThrower))]
public class TeleportalAttachmentReceiver : BaseTeleportalReceiver
{
	// Token: 0x06001C4B RID: 7243 RVA: 0x0008A2C5 File Offset: 0x000886C5
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x0008A2DE File Offset: 0x000886DE
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x0008A2F7 File Offset: 0x000886F7
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x04001621 RID: 5665
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};

	// Token: 0x04001622 RID: 5666
	public ManualAnimation m_ReceiveAnimation;
}
