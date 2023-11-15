using System;
using UnityEngine;

// Token: 0x02000BCF RID: 3023
public class TeleportalMapAvatarReceiver : BaseTeleportalReceiver
{
	// Token: 0x06003DD0 RID: 15824 RVA: 0x00126F51 File Offset: 0x00125351
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x00126F6A File Offset: 0x0012536A
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00126F83 File Offset: 0x00125383
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x0400319A RID: 12698
	[SerializeField]
	public bool m_groundPlayer = true;

	// Token: 0x0400319B RID: 12699
	public ManualAnimation m_ReceiverAnimation;

	// Token: 0x0400319C RID: 12700
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};
}
