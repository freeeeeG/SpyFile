using System;
using UnityEngine;

// Token: 0x020005D2 RID: 1490
public class TeleportalPlayerReceiver : BaseTeleportalReceiver
{
	// Token: 0x06001C70 RID: 7280 RVA: 0x0008AF8D File Offset: 0x0008938D
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x0008AFA6 File Offset: 0x000893A6
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x0008AFBF File Offset: 0x000893BF
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x0400163E RID: 5694
	[SerializeField]
	public bool m_groundPlayer = true;

	// Token: 0x0400163F RID: 5695
	public ManualAnimation m_ReceiverAnimation;

	// Token: 0x04001640 RID: 5696
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};
}
